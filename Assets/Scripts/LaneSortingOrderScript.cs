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
        
    }
}
