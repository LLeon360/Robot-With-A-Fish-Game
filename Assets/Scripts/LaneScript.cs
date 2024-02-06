using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneScript : MonoBehaviour
{

    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private int laneLength;
    [SerializeField]
    private List<GameObject> tiles;

    // Start is called before the first frame update
    void Start()
    {
        GenerateLane();
    }

    // Update is called once per frame
    void Update()
    {   
        
    }

    /**
    * Clear the lane 
    */
    void clearLane() {
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
            float offset = i-laneLength/2f;
            GameObject tile = Instantiate(tilePrefab, new Vector3(offset, 0, 0), Quaternion.identity);
            tiles.Add(tile);
            tile.transform.SetParent(this.transform);
        }
    }
}
