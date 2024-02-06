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

    private List<GameObject> tiles;
    [SerializeField]
    private List<Sprite> tileSprites;

    // Start is called before the first frame update
    void Start()
    {
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
        for (int i = 0; i < laneLength; i++)
        {
            float offset = i - laneLength / 2f + 0.5f;
            GameObject tile = Instantiate(tilePrefab, Vector3.zero, Quaternion.identity);
            tiles.Add(tile);
            tile.transform.SetParent(this.transform);
            tile.transform.localPosition = new Vector3(offset, 0, 0);
            //randomly pick a tile sprite
            tile.GetComponent<SpriteRenderer>().sprite = tileSprites[Random.Range(0, tileSprites.Count)];
        }
    }
}
