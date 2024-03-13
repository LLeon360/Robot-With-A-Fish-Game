using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(UnitInfoScript))]

public class TrampolineScript : MonoBehaviour
{
    private Animator animator;
    private UnitInfoScript unitInfo;

    [SerializeField]
    private float range;

    [SerializeField]
    private float abilityCooldown;
    [SerializeField]
    private float nextAbilityTime;

    [SerializeField] //for debug
    private string state;   
    [SerializeField]
    private float speed;

    void Start() 
    {
        animator = GetComponent<Animator>();
        unitInfo = GetComponent<UnitInfoScript>();
        nextAbilityTime = 0f;
    }   

    void Update() 
    {   
        //default idle
        state = "Idle"; 
        //if can fire, check for units and update state if there are enemies in range
        if (Time.time >= nextAbilityTime) {
            CheckInFront();
        }

        //update animator
        animator.SetBool("Active", state == "Active");
    }

    void CheckInFront() {
        GameObject thisLane = unitInfo.GetLane();
        Transform unitsInLane = thisLane.transform.Find("Units");

        //iterate through children of unitsInLane
        foreach (Transform unit in unitsInLane.transform) 
        {
            //if it is friendly
            if (unit.GetComponent<UnitInfoScript>().player == unitInfo.player) {
                float distance = Mathf.Abs(transform.position.x - unit.transform.position.x);
                if (distance < range) {
                    state = "Active";
                    nextAbilityTime = Time.time + abilityCooldown;
                    //only needs to find one unit to attack
                    break;
                }
            }
        }
    }

    public void ShoveUnitsForward() {
        GameObject thisLane = unitInfo.GetLane();
        Transform unitsInLane = thisLane.transform.Find("Units");

        //get all units within range
        foreach (Transform unit in unitsInLane.transform) 
        {
             //if it is friendly
            if (unit.GetComponent<UnitInfoScript>().player == unitInfo.player) {
                float distance = Mathf.Abs(transform.position.x - unit.transform.position.x);
                if (distance < range) {
                    //get rigidbody and apply force
                    Rigidbody2D rb = unit.GetComponent<Rigidbody2D>();
                    if (rb != null) {
                        rb.velocity = (unitInfo.player == 0 ? Vector3.right : Vector3.left) * speed;
                    }
                }
            }
        }
        state = "Idle";
    }

    public void DestroyBuilding() {
        Destroy(gameObject);
    }
}
