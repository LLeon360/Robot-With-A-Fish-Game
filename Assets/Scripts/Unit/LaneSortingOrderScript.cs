using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(UnitInfoScript))]
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
        int laneIndex = unitInfoScript.GetLane();
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
            default:
                Debug.LogError("Unknown type: " + unitInfoScript.type + " in LaneSortingOrderScript of " + gameObject.name);
                break;
        }

        sortingGroup.sortingOrder = 2 * laneIndex + offset;
    }
}
