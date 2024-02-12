using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    [SerializeField] //just for debug, TODO remove
    private GameObject building;
    [SerializeField] //just for debug, TODO remove
    private GameObject tileModifier;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetBuilding()
    {
        return building;
    }
    
    public GameObject GetTileModifier()
    {
        return tileModifier;
    }

    public void SetBuilding(GameObject building)
    {
        this.building = building;
    }

    public void SetTileModifier(GameObject tileModifier)
    {
        this.tileModifier = tileModifier;
    }

    public void RemoveBuilding()
    {
        Destroy(building);
    }

    public void RemoveTileModifier()
    {
        Destroy(tileModifier);
    }

    public bool IsEmpty()
    {
        return building == null;
    }
}
