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

    public int GetLane(){
        GameObject lane = transform.parent.parent.gameObject;
        return lane.GetComponent<LaneScript>().laneIndex;
    }
}
