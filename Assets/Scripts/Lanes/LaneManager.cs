using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject lanePrefab;

    [SerializeField]
    private int laneCount = 3;
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
        for (int i = 0; i < laneCount; i++)
        {
            float offset = i-laneCount/2f+0.5f;
            GameObject lane = Instantiate(lanePrefab, new Vector3(0, offset, 0), Quaternion.identity);
            lanes.Add(lane);
            lane.transform.SetParent(this.transform);
        }
    }
}
