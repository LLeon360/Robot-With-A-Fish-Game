using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    //lane manager, need it to snap to lanes and stay in grid
    [SerializeField]
    private GameObject laneManager;
    private int currentLane = 0;
    private int currentTile = 0;

    [SerializeField]
    private bool isPlayerLeft = true;

    //target selector should be pointed to
    private GameObject targetTile;
    [SerializeField]

    //movement delays
    private float moveXDelay = 0.1f;
    [SerializeField]
    private float moveYDelay = 0.3f;
    private float nextXMove = 0;
    private float nextYMove = 0;

    //movement directions

    private float directionX = 0, directionY = 0;

    // Start is called before the first frame update
    void Start()
    {
        laneManager = GameObject.Find("Lane Manager");
        
        currentLane = 0;
        if(isPlayerLeft) {
            currentTile = 0;
        } else {
            currentTile = laneManager.GetComponent<LaneManager>().laneLength -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /**
         * split movements into different directions so that we can have seperate delays 
         * AND 
         * can check if movement is held in one direction but not the other
        */
        if(Mathf.Abs(directionX) > 0.1f && nextXMove < Time.time) {
            nextXMove = Time.time + moveXDelay;
            MoveX(directionX);
        }
        if(Mathf.Abs(directionY) > 0.1f && nextYMove < Time.time) {
            nextYMove = Time.time + moveYDelay;
            MoveY(directionY);
        }

        //update target tile
        targetTile = laneManager.GetComponent<LaneManager>().GetLane(currentLane).GetComponent<LaneScript>().GetTile(currentTile);
        UpdateTransformPosition();
    }

    public void OnMoveX(InputAction.CallbackContext ctx) {
        // can always move if spam button, but cap movespeed if holding
        if(ctx.started) {
            nextXMove = 0;
        }
        directionX = ctx.ReadValue<float>();
    }

    public void OnMoveY(InputAction.CallbackContext ctx) {
        // can always move if spam button, but cap movespeed if holding
        if(ctx.started) {
            nextYMove = 0;
        }
        directionY = ctx.ReadValue<float>();
    }

    void MoveX(float direction) {
        if(direction < 0) {
            if(currentTile > 0) {
                currentTile--;
            }
        } else if(direction > 0) {
            if(currentTile < laneManager.GetComponent<LaneManager>().laneLength - 1) {
                currentTile++;
            }
        }
    }

    void MoveY(float direction) {
        if(direction < 0) {
            if(currentLane > 0) {
                currentLane--;
            }
        } else if(direction > 0) {
            if(currentLane < laneManager.GetComponent<LaneManager>().laneCount - 1) {
                currentLane++;
            }
        }
    }

    void UpdateTransformPosition() {
        Vector3 targetPosition = new Vector3(targetTile.transform.position.x, targetTile.transform.position.y, 0);
        Debug.Log("From " + transform.position + " to " + targetPosition + " with " + Time.deltaTime * 10);
        transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * 15);
    }
}
