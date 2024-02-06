using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject laneManager;
    private int currentLane = 0;
    private int currentTile = 0;

    [SerializeField]
    private bool isPlayerLeft = true;

    //target cursor should be pointed to
    private GameObject targetTile;

    // Start is called before the first frame update
    void Start()
    {
        laneManager = GameObject.Find("Lane Manager");

        if(isPlayerLeft) {
            currentLane = 0;
        } else {
            currentLane = laneManager.GetComponent<LaneManager>().laneCount - 1;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //update target tile
        targetTile = laneManager.GetComponent<LaneManager>().GetLane(currentLane).GetComponent<LaneScript>().GetTile(currentTile);
        Debug.Log("Target tile: " + targetTile);
        UpdateTransformPosition();
    }

    public void OnMove(InputAction.CallbackContext ctx) {
        if(ctx.started) {
            Move(ctx.ReadValue<Vector2>());
        }
    }

    void Move(Vector2 direction) {
        if(direction.x < 0) {
            if(currentTile > 0) {
                currentTile--;
            }
        } else if(direction.x > 0) {
            if(currentTile < laneManager.GetComponent<LaneManager>().laneLength - 1) {
                currentTile++;
            }
        }

        if(direction.y < 0) {
            if(currentLane > 0) {
                currentLane--;
            }
        } else if(direction.y > 0) {
            if(currentLane < laneManager.GetComponent<LaneManager>().laneCount - 1) {
                currentLane++;
            }
        }
    }

    void UpdateTransformPosition() {
        Vector3 targetPosition = new Vector3(targetTile.transform.position.x, targetTile.transform.position.y, 0);
        Debug.Log("From " + transform.position + " to " + targetPosition + " with " + Time.deltaTime * 10);
        transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * 10);
    }
}
