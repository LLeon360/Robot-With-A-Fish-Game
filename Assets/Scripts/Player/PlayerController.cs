using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private int currentLane = 0;
    private int currentTile = 0;

    [SerializeField]
    private int playerNum = 0;

    //target selector should be pointed to
    private GameObject targetTile;

    //movement delays
    [SerializeField]
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
        currentLane = 0;
        if(playerNum == 0) {
            currentTile = 0;
        } else {
            currentTile = LaneManager.Instance.laneLength -1;
        }
        hotbarManager = GameObject.Find("Hotbar"+(playerNum+1)).GetComponent<HotbarManager>();
        if(hotbarManager == null) {
            Debug.LogError("Hotbar"+(playerNum+1)+ " not found, could not retrieve HotbarManager");
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
        FetchTargetTile();
        UpdateTransformPosition();
    }

    /**
     * Activates when the player presses the action button, usually to place the current selection
     */
    public void OnAction(InputAction.CallbackContext ctx) {
        if(ctx.started) {
            if(!LaneManager.Instance.BoardInitialized) {
                return;
            }
            if(targetTile == null) {
                Debug.Log("Target Tile is null, cannot place");
                return;
            }
            HotbarElement currentSelection = hotbarManager.GetSelectedElement();
            HotbarElementObject currentSelectionObject = currentSelection.GetHotbarElementObject();
            TileScript targetTileScript = targetTile.GetComponent<TileScript>();
            LaneScript laneScript = LaneManager.Instance.GetLane(currentLane).GetComponent<LaneScript>();

            //check for nulls
            if(currentSelection == null) {
                Debug.LogError("Player " + playerNum + " tried to place a null selection");
                return;
            }
            if(currentSelectionObject == null) {
                Debug.LogError("Player " + playerNum + " tried to place a null selection object");
                return;
            }
            if(targetTile == null) {
                Debug.LogError("Player " + playerNum + " tried to place on a null tile");
                return;
            }
            if(targetTileScript == null) {
                Debug.LogError("Player " + playerNum + " tried to place on a tile with a null TileScript");
                return;
            }

            //check if player is on their half of the map
            if(playerNum == 0 && currentTile >= LaneManager.Instance.laneLength/2) {
                return;
            }
            if(playerNum == 1 && currentTile < LaneManager.Instance.laneLength/2) {
                return;
            }

            //check if nextUse is ready
            if(currentSelection.IsUsable()) {
                //check if player can afford
                if(ResourceManager.Instance.CanAfford(currentSelectionObject.cost, playerNum)) {
                    //if is building
                    if(currentSelectionObject.slotType == "Building") {
                        if(!targetTileScript.IsEmpty()) {
                            // Debug.Log("Player " + playerNum + " tried to build on a non-empty tile at Lane " + currentLane + " Tile " + currentTile);
                            return;
                        }
                    }

                    LaneManager.Instance.Deploy(currentLane, currentTile, currentSelectionObject.deployPrefab, playerNum, currentSelectionObject.slotType == "Building");

                    //set next use
                    currentSelection.UpdateNextUsableTime();
                    //deduct cost
                    ResourceManager.Instance.RemoveMoney(playerNum, currentSelectionObject.cost);
                }
            }
        }
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
        int laneLength = LaneManager.Instance.laneLength;
        if(direction < 0) {
            currentTile = (currentTile - 1 + laneLength) % laneLength;
        } else if(direction > 0) {
            currentTile = (currentTile + 1) % laneLength;
        }
    }

    void MoveY(float direction) {
        int laneCount = LaneManager.Instance.laneCount;
        // lanes count down to align with sorting order, realize this might be not intuitive for movement
        if(direction < 0) {
            currentLane = (currentLane + 1) % laneCount;
        } else if(direction > 0) {
            currentLane = (currentLane - 1 + laneCount) % laneCount; //strictly positive
        }
    }

    void FetchTargetTile() {
        if(!LaneManager.Instance.BoardInitialized) {
            Debug.Log("Board not initialized, cannot fetch target tile");
            return;
        }
        targetTile = LaneManager.Instance.GetTile(currentLane, currentTile);
        if(targetTile == null){
            Debug.Log("Target Tile is null, failed to fetch");
            Debug.Log("Forcing " + gameObject.name + " to back to (0, 0)");
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
