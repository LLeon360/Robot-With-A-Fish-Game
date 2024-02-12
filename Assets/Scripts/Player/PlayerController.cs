using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    //lane manager, need it to snap to lanes and stay in grid
    [SerializeField]
    private LaneManager laneManager;
    private int currentLane = 0;
    private int currentTile = 0;

    [SerializeField]
    private int playerNum = 0;

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

    private HotbarManager hotbarManager;

    // Start is called before the first frame update
    void Start()
    {
        laneManager = LaneManager.Instance;
        
        currentLane = 0;
        if(playerNum == 0) {
            currentTile = 0;
        } else {
            currentTile = laneManager.laneLength -1;
        }
        hotbarManager = GameObject.Find("Hotbar "+(playerNum+1)).GetComponent<HotbarManager>();
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
        FetchTargetTile();
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
        int laneLength = laneManager.laneLength;
        if(direction < 0) {
            currentTile = (currentTile - 1 + laneLength) % laneLength;
        } else if(direction > 0) {
            currentTile = (currentTile + 1) % laneLength;
        }
    }

    void MoveY(float direction) {
        int laneCount = laneManager.laneCount;
        if(direction < 0) {
            currentLane = (currentLane - 1 + laneCount) % laneCount; //strictly positive
        } else if(direction > 0) {
            currentLane = (currentLane + 1) % laneCount;
        }
    }

    void FetchTargetTile() {
        targetTile = laneManager.GetTile(currentLane, currentTile);
        if(targetTile == null){
            Debug.LogError("Target Tile is null, failed to fetch");
            Debug.LogError("Forcing " + gameObject.name + " to back to (0, 0)");
            currentLane = 0;
            currentTile = 0;
            return;
        }
    }

    void UpdateTransformPosition() {
        if(!targetTile) {
            return;
        }
        Vector3 targetPosition = new Vector3(targetTile.transform.position.x, targetTile.transform.position.y, 0);
        transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * 15);
    }
}
