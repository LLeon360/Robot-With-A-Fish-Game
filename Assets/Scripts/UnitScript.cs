using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{
    [SerializeField]
    private HealthScript healthScript;

    [SerializeField]
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        healthScript = GetComponent<HealthScript>();
        if(healthScript == null)
        {
            Debug.LogError("HealthScript not found in UnitScript of " + gameObject.name);
        }
        
        animator = GetComponent<Animator>();
        if(animator == null)
        {
            Debug.LogError("Animator not found in UnitScript of " + gameObject.name);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
