using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

public class DraftSelector : MonoBehaviour
{
    private int currentIndex = 0;

    [SerializeField]
    private int playerNum = 0;

    //target selector should be pointed to
    private GameObject targetSelection;

    //movement delays
    [SerializeField]
    private float moveXDelay = 0.1f;
    [SerializeField]
    private float moveYDelay = 0.3f;
    private float nextXMove = 0;
    private float nextYMove = 0;

    //movement directions

    private float directionX = 0, directionY = 0;

    private bool isMyTurn = false;

    private GameObject targetSlot;

    // Start is called before the first frame update
    void Start()
    {        
        currentIndex = (playerNum == 0) ? 0 : DraftManager.Instance.RowSize - 1;
        isMyTurn = playerNum == 0;
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
        FetchTargetSlot();
        UpdateTransformPosition();
    }

    /**
     * Activates when the player presses the action button, usually to place the current selection
     */
    public void OnAction(InputAction.CallbackContext ctx) {
        if(!isMyTurn) {
            return;
        }
        if(ctx.started) {
            DraftManager.Instance.SelectElement(playerNum, currentIndex);
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
        int selectionsSize = DraftManager.Instance.selectionElements.Count;
        if(direction < 0) {
            currentIndex = (currentIndex - 1 + selectionsSize) % selectionsSize;
        } else if(direction > 0) {
            currentIndex = (currentIndex + 1) % selectionsSize;
        }
    }

    void MoveY(float direction) {
        int selectionsSize = DraftManager.Instance.selectionElements.Count;
        int rowSize = DraftManager.Instance.RowSize;
        if(direction < 0) {
            currentIndex = (currentIndex - rowSize + selectionsSize) % selectionsSize;
        } else if(direction > 0) {
            currentIndex = (currentIndex + rowSize) % selectionsSize;
        }
    }

    void FetchTargetSlot() {
        if(!DraftManager.initialized) {
            return;
        }
        targetSlot = DraftManager.Instance.GetSelectionSlot(currentIndex);
        if(targetSlot == null){
            Debug.Log("Target Slot is null, failed to fetch");
            Debug.Log("Forcing " + gameObject.name + " to back to (0, 0)");
            currentIndex = 0;
            return;
        }
    }

    void UpdateTransformPosition() {
        if(!targetSlot) {
            return;
        }
        Vector3 targetPosition = new Vector3(targetSlot.transform.position.x, targetSlot.transform.position.y, 0);
        transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * 15);
    }

    public void EndTurn() {
        isMyTurn = false;
    }

    public void StartTurn() {
        isMyTurn = true;
    }
}
