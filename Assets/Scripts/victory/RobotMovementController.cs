using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovementController : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // This is where you put the condition for when the robot should walk.
        // For example, this could be input from the player:
        if (Input.GetKey(KeyCode.RightArrow)) // If the right arrow key is held down
        {
            animator.SetBool("isMovingRight", true); // Tell the Animator to start the walking animation
        }
        else
        {
            animator.SetBool("isMovingRight", false); // Tell the Animator to stop the walking animation
        }
    }
}
