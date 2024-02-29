using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(UnitInfoScript))]

public class TurretScript : MonoBehaviour
{
    private Animator animator;
    private UnitInfoScript unitInfo;

    [SerializeField] 
    private GameObject projectile;

    [SerializeField]
    private float attackRange;

    [SerializeField]
    private float attackCooldown;
    private float nextAttackTime;

    [SerializeField] //for debug
    private string state;

    void Start() 
    {
        animator = GetComponent<Animator>();
        unitInfo = GetComponent<UnitInfoScript>();
        nextAttackTime = 0f;
    }   

    void Update() 
    {   
        //default idle
        state = "Idle"; 
        //if can fire, check for units and update state if there are enemies in range
        if (Time.time >= nextAttackTime) {
            CheckInFront();
        }

        //update animator
        animator.SetBool("Attack", state == "Attack");
    }

    void CheckInFront() {
        GameObject thisLane = unitInfo.GetLane();
        Transform unitsInLane = thisLane.transform.Find("Units");

        //iterate through children of unitsInLane
        foreach (Transform unit in unitsInLane.transform) 
        {
            if (unit.gameObject != gameObject) {
                //check that it is in front based on player number
                if (unitInfo.player == 0) {
                    if (unit.position.x < transform.position.x) {
                        continue;
                    }
                } else if (unitInfo.player == 1) {
                    if (unit.position.x > transform.position.x) {
                        continue;
                    }
                }

                float distance = Mathf.Abs(transform.position.x - unit.transform.position.x);

                if (distance < attackRange) {
                    state = "Attack";
                    nextAttackTime = Time.time + attackCooldown;
                    //only needs to find one unit to attack
                    break;
                }
            }
        }
    }

    public void FireProjectile() {
        GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        newProjectile.GetComponent<ProjectileScript>().player = unitInfo.player;
        newProjectile.GetComponent<ProjectileScript>().direction = (unitInfo.player == 0 ? Vector3.right : Vector3.left);
        state = "Idle";
    }

}
