using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthScript))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(UnitInfoScript))]
public class UnitScript : MonoBehaviour
{
    [SerializeField]
    private HealthScript healthScript;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Rigidbody2D rb;
    
    [SerializeField]
    private UnitInfoScript unitInfo;

    [SerializeField]
    private float speed;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float attackRange;

    [SerializeField]
    private bool targetBaseOnly;
    [SerializeField]
    private bool targetBuildingsOnly;

    [SerializeField]
    private string state;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private Vector3 direction;

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

        rb = GetComponent<Rigidbody2D>();

        unitInfo = GetComponent<UnitInfoScript>();
    }

    void Awake()
    {
        state = "Walk";
    }

    // Update is called once per frame
    void Update()
    {   
        direction = ( unitInfo.player == 0 ? Vector3.right : Vector3.left );
        target = CheckInFront();

        if(target == null)
        {
            state = "Walk";
        }
        else {
            state = "Attack";
        }

        if(state == "Walk")
        {
            animator.SetBool("Idle", false);

            if (Mathf.Abs(rb.velocity.x) < speed)
            {
                rb.velocity = direction * speed;
            }
        }
        else
        {
            if(Mathf.Abs(rb.velocity.x) <= speed + 0.05f) { //continue if getting propelled by trampoline
                rb.velocity = Vector2.zero;
            }
        }

        if(state == "Attack")
        {
            animator.SetBool("Attack", true);

            //handle damage as key event in attack anim
        }
        else
        {
            animator.SetBool("Attack", false);
        }

        if(state == "Idle") { //perhaps on stun
            animator.SetBool("Idle", true);
        }


        //kill units that are beyond 100 
        if(transform.position.x > 100 || transform.position.x < -100)
        {
            Destroy(gameObject);
        }
    }

    GameObject CheckInFront() {
        float closestDistance = Mathf.Infinity;
        GameObject closestUnit = null;        
        GameObject thisLane = unitInfo.GetLane();
        Transform unitsInLane = thisLane.transform.Find("Units");
        Transform buildingsInLane = thisLane.transform.Find("Buildings");

        //iterate through children of unitsInLane
        if(!targetBuildingsOnly && !targetBaseOnly) {
            foreach (Transform otherUnit in unitsInLane.transform) 
            {
                UnitInfoScript otherUnitInfo = otherUnit.GetComponent<UnitInfoScript>();
                //null check, should not happen, only units should go in "Units" object
                if (otherUnitInfo != null)
                {
                    float distance = Mathf.Abs(transform.position.x - otherUnit.transform.position.x);
                    //check if it's within range
                    if(distance > attackRange)
                    {
                        continue;
                    }
                    //check that it is in front based on player number
                    if (unitInfo.player == 0) {
                        if (otherUnit.position.x < transform.position.x) {
                            continue;
                        }
                    } else if (unitInfo.player == 1) {
                        if (otherUnit.position.x > transform.position.x) {
                            continue;
                        }
                    }

                    // if unit info is different player
                    if(unitInfo.player != otherUnitInfo.player)
                    {
                        if(distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestUnit = otherUnitInfo.gameObject;
                        }
                    }
                }
            }
        }

        //iterate through children of buildingsInLane
        foreach (Transform building in buildingsInLane.transform) 
            {
                // Check if the collider belongs to a unit.
                UnitInfoScript buildingInfo = building.GetComponent<UnitInfoScript>();
                
                //null check, should not happen, only units should go in "Units" object
                if (buildingInfo != null)
                {
                    if(targetBaseOnly && buildingInfo.tag != "Power Tower")
                    {
                        continue;
                    }
                    if(buildingInfo.tag == "Trap") {
                        continue;
                    }

                    float distance = Mathf.Abs(transform.position.x - building.transform.position.x);
                    //check if it's within range
                    if(distance > attackRange)
                    {
                        continue;
                    }

                    float margin = 0.5f;
                    //check that it is in front based on player number
                    if (unitInfo.player == 0) {
                        if (building.position.x < transform.position.x - margin) {
                            continue;
                        }
                    } else if (unitInfo.player == 1) {
                        if (building.position.x > transform.position.x + margin) {
                            continue;
                        }
                    }

                    // if unit info is different player
                    if(unitInfo.player != buildingInfo.player)
                    {
                        if(distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestUnit = buildingInfo.gameObject;
                        }
                    }
                }
            }

        return closestUnit;
    }

    // to be called by animation event
    public void AttackTarget()
    {
        //it's possible that the target has been destroyed inbetween the start of the attack and dealing dmg
        if(target == null)
        {
            return;
        }

        //grab enemy health script and damage
        HealthScript enemyHealthScript = target.GetComponent<HealthScript>();
        enemyHealthScript.Damage(damage, gameObject);
    }
}
