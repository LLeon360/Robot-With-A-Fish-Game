using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject lanePrefab;
    [SerializeField]
    private int _laneCount = 3;
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
    private float screenHeight = 10f;
    private List<GameObject> lanes;

    // Start is called before the first frame update
    void Start()
    {
        lanes = new List<GameObject>();
        GenerateLanes();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("DEBUG: REMOVE THIS, Pressing Space will regenerate all lanes\nKNOWN trivial error associated with regenerating lanes (player will attempt to fetch tiles as tiles are being cleared causing null reference error) this will only happen using this debug feature");
        // TODO remove this, for testing purposes generate lanes on update, this isn't necessary
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ClearLanes();
            GenerateLanes();
        }
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
        float laneWidth = (screenHeight)/(_laneCount-1);

        for (int i = 0; i < _laneCount; i++)
        {
            float offset = screenHeight * (-0.5f + (i+1f)/(_laneCount+1));
            GameObject lane = Instantiate(lanePrefab, Vector3.zero, Quaternion.identity);

            lanes.Add(lane);

            lane.GetComponent<LaneScript>().laneLength = laneLength;
            lane.transform.SetParent(this.transform);
            lane.transform.localPosition = new Vector3(0, offset, 0);
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

}
