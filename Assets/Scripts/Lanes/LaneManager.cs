using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    public static LaneManager Instance { get; private set; }

    [SerializeField] 
    private GameObject lanePrefab;
    [SerializeField]
    private int _laneCount = 3;
    [SerializeField]
    private GameObject towerPrefab;
    
    private bool boardInitialized = false;
    public bool BoardInitialized
    {
        get { return boardInitialized; }
    }
    public int laneCount
    {
        get { return _laneCount; }
        set
        {
            _laneCount = value;
        }
    }

    [SerializeField]
    private int _laneLength = 10;

    public int laneLength
    {
        get { return _laneLength; }
        set
        {
            // lane length should be even, supposing we don't have a neutral tile between\
            _laneLength = value;
        }
    }
    [SerializeField]
    private float laneWidth = 1f;
    private List<GameObject> lanes;

    // Start is called before the first frame update
    void Start()
    {
        boardInitialized = false;
        SetupLanes();
    }
    void SetupLanes()
    {
        lanes = new List<GameObject>();
        GenerateLanes();

        // Start a coroutine to place towers after a frame, give time to initialize lanes
        StartCoroutine(PlaceTowersAfterFrame());
    }

    void Awake()
    {
        // Singleton pattern, only one instance of this class should exist
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
            Instance = this;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // Debug.Log("DEBUG: REMOVE THIS, Pressing Space will regenerate all lanes\nKNOWN trivial error associated with regenerating lanes (player will attempt to fetch tiles as tiles are being cleared causing null reference error) this will only happen using this debug feature");
        // // TODO remove this, for testing purposes generate lanes on update, this isn't necessary
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     SetupLanes();
        // }
    }

    /**
    * Clear the lanes
    */
    void ClearLanes() {
        foreach (GameObject lane in lanes)
        {
            Destroy(lane);
        }
        lanes.Clear();
    }

    /**
    * Generate the lanes
    */
    void GenerateLanes()
    {
        // float laneWidth = (screenHeight)/(_laneCount-1);

        for (int i = 0; i < _laneCount; i++)
        {
            // float offset = screenHeight * (-0.5f + (i+1f)/(_laneCount+1)); // this is formula to have gaps and split screen height
            // build lanes from top to bottom so lane index aligns with sorting order
            float offset = (_laneCount/2 - i) * laneWidth; 
            GameObject lane = Instantiate(lanePrefab, Vector3.zero, Quaternion.identity);

            lane.GetComponent<LaneScript>().laneLength = laneLength;
            lane.GetComponent<LaneScript>().laneIndex = i;
            lane.transform.SetParent(this.transform);
            lane.transform.localPosition = new Vector3(0, offset, 0);
            lane.name = "Lane " + i;
            
            lanes.Add(lane);
        }
    }
    
    // get the lane at the index
    public GameObject GetLane(int index)
    {
        if(index < 0 || index >= laneCount)
        {
            Debug.LogError("Invalid lane index");
            return null;
        }
        return lanes[index];
    }

    // get the Tile at a 2d inded
    public GameObject GetTile(int laneIndex, int tileIndex)
    {
        return GetLane(laneIndex).GetComponent<LaneScript>().GetTile(tileIndex);
    }

    // place towers at the start and end of each lane
    void PlaceTowers() {
        foreach (GameObject lane in lanes)
        {
            LaneScript laneScript = lane.GetComponent<LaneScript>();

            GameObject startTile = laneScript.GetTile(0);
            GameObject endTile = laneScript.GetTile(laneScript.laneLength-1);

            Deploy(laneScript.laneIndex, 0, towerPrefab, 0, true);
            Deploy(laneScript.laneIndex, laneScript.laneLength-1, towerPrefab, 1, true);
        }
    }    
    IEnumerator PlaceTowersAfterFrame()
    {
        // Wait for the next frame
        yield return null;

        // Now place the towers
        PlaceTowers();
        boardInitialized = true;
    }

    public GameObject Deploy(int lane, int tile, GameObject prefab, int player, bool isBuilding) {
        GameObject laneObject = GetLane(lane);
        LaneScript laneScript = laneObject.GetComponent<LaneScript>();
        GameObject tileObject = laneScript.GetTile(tile);
        TileScript tileScript = tileObject.GetComponent<TileScript>();

        GameObject newUnit = Instantiate(prefab, tileObject.transform.position - new Vector3(0, 0.4f, 0), Quaternion.identity);
        if(isBuilding)
        {
            newUnit.transform.SetParent(laneScript.buildingParent.transform);
            tileScript.SetBuilding(newUnit);
        }
        else {
            newUnit.transform.SetParent(laneScript.unitParent.transform);
        
        }
        newUnit.GetComponent<UnitInfoScript>().player = player;

        return newUnit;
    }

}
