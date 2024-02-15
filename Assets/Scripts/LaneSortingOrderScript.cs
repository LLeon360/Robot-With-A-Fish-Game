using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LaneSortingOrderScript : MonoBehaviour
{
    [SerializeField]
    private SortingGroup sortingGroup;
    [SerializeField]
    private UnitInfoScript unitInfoScript;
    
    // Start is called before the first frame update
    void Start()
    {
        sortingGroup = GetComponent<SortingGroup>();
        if(sortingGroup == null)
        {
            Debug.LogError("SortingGroup not found in LaneSortingOrderScript of " + gameObject.name);
        }
        
        unitInfoScript = GetComponent<UnitInfoScript>();
        if(unitInfoScript == null)
        {
            Debug.LogError("UnitInfoScript not found in LaneSortingOrderScript of " + gameObject.name);
        }    
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSortingOrder();
    }

    void UpdateSortingOrder()
    {
        int laneIndex = transform.parent.parent.GetComponent<LaneScript>().laneIndex;
        //if is not a building, increment sorting order by 1
        int offset = 0;
        switch(unitInfoScript.type)
        {
            case "Building":
                offset = 0;
                break;
            case "Unit":
                offset = 1;
                break;
        }

        if(unitInfoScript != null)
        {
            sortingGroup.sortingOrder = 5 * laneIndex + offset;
        }
    }
}
