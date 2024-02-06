using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    [SerializeField] 
    private GameObject lanePrefab;
    [SerializeField]
    private int laneCount = 3;
    [SerializeField]
    private int laneLength = 10;
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
        float laneWidth = (screenHeight)/(laneCount-1);

        for (int i = 0; i < laneCount; i++)
        {
            float offset = screenHeight * (-0.5f + (i+1f)/(laneCount+1));
            GameObject lane = Instantiate(lanePrefab, Vector3.zero, Quaternion.identity);

            lanes.Add(lane);

            lane.GetComponent<LaneScript>().laneLength = laneLength;
            lane.transform.SetParent(this.transform);
            lane.transform.localPosition = new Vector3(0, offset, 0);
        }
    }
    
}
