using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfoScript : MonoBehaviour
{
    
    [SerializeField]
    public int player;
    
    // [SerializeField]
    // public int lane;
    
    [SerializeField]
    [StringInList("Building", "Unit")]
    public string type;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //scale based on player
        if(player == 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if(player == 1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public int GetLaneIndex(){
        GameObject lane = GetLane();
        return lane.GetComponent<LaneScript>().laneIndex;
    }

    public GameObject GetLane() {
        return transform.parent.parent.gameObject;
    }

    public int GetTileIndex() {
        GetLane();
        // get offset from the lanetransform
        float offset = transform.position.x - GetLane().transform.position.x;
        // get tile based on offset 
        int tileIndex = Mathf.RoundToInt((offset - 0.5f) + GetLane().GetComponent<LaneScript>().laneLength / 2); 
        return tileIndex;
    }
}
