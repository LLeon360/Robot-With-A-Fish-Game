using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneScript : MonoBehaviour
{

    [SerializeField]
    private GameObject tilePrefab;

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
    // keep track of the lane index
    [SerializeField]
    public int laneIndex { get; set; }

    [SerializeField] //for testing purposes, shouldn't need to assign through inspector
    private List<GameObject> tiles;
    [SerializeField]
    private List<Sprite> tileSprites;

    //child references for tiles, buildings, and units
    [SerializeField]
    public GameObject tileParent {get; private set;}
    [SerializeField]
    public GameObject buildingParent {get; private set;}
    [SerializeField]
    public GameObject unitParent {get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        //get child references
        tileParent = transform.Find("Tiles").gameObject;
        buildingParent = transform.Find("Buildings").gameObject;
        unitParent = transform.Find("Units").gameObject;

        tiles = new List<GameObject>();
        GenerateLane();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /**
    * Clear the lane 
    */
    void ClearLane()
    {
        foreach (GameObject tile in tiles)
        {
            Destroy(tile);
        }
        tiles.Clear();
    }

    /**
    * Generate a new lane
    */
    void GenerateLane()
    {
        ClearLane();
        for (int i = 0; i < laneLength; i++)
        {
            float offset = i - laneLength / 2f + 0.5f;
            GameObject tile = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
            tile.transform.SetParent(tileParent.transform);
            tile.transform.localPosition = new Vector3(offset, 0, 0);
            tile.name = "Tile " + i;

            //randomly pick a tile sprite
            tile.GetComponent<SpriteRenderer>().sprite = tileSprites[(i+(laneIndex%4))%4];
            //set sorting layer to tiles
            tile.GetComponent<SpriteRenderer>().sortingLayerName = "Tiles";    

            tiles.Add(tile);
        }
    }

    // get the tile at the index
    public GameObject GetTile(int index) {
        if(index < 0 || index >= tiles.Count) {
            Debug.LogError("Index out of range " + index + " for lane " + laneIndex + " with " + tiles.Count + " tiles.");
            return null;
        }
        return tiles[index];
    }
}
