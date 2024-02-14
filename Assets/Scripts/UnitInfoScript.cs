using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfoScript : MonoBehaviour
{
    
    [SerializeField]
    private int player;
    
    [SerializeField]
    private int lane;

    [SerializeField]
    private HotbarElementObject data;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //setters
    public void SetPlayer(int player)
    {
        this.player = player;
    }

    public void SetLane(int lane)
    {
        this.lane = lane;
    }

    public void SetData(HotbarElementObject data)
    {
        this.data = data;
    }
}
