using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f; // movement speed
    public float jumpForce = 10f; // force applied when jumping
    public float gravity = 20f; // gravity applied to the player
    public bool canWallJump = true; // whether the player can wall jump

    private Vector3 moveDirection = Vector3.zero; // current movement direction
    private CharacterController controller; // reference to the character controller component
    private float wallJumpTimeout = 0f; // timeout for wall jumping

    void Start()
    {
        // get a reference to the character controller component
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // if the character controller is grounded
        if (controller.isGrounded)
        {
            // get input for the horizontal and vertical axes
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // calculate the move direction
            moveDirection = (transform.forward * v) + (transform.right * h);
            moveDirection = moveDirection.normalized * speed;

            // check if the player wants to jump
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }
        // if the character controller is not grounded
        else
        {
            // check if the player can wall jump and has pressed the jump button
            if (canWallJump && Input.GetButtonDown("Jump") && wallJumpTimeout <= 0)
            {
                // get input for the horizontal axis
                float h = Input.GetAxis("Horizontal");

                // check if the player is moving towards a wall
                if (h != 0)
                {
                    // calculate the wall jump direction
                    moveDirection = transform.right * h;
                    moveDirection = moveDirection.normalized * speed;
                    moveDirection.y = jumpForce;

                    // set the wall jump timeout
                    wallJumpTimeout = 0.5f;
                }
            }

            // decrease the wall jump timeout
            wallJumpTimeout -= Time.deltaTime;
        }

        // apply gravity to the character controller
        moveDirection.y -= gravity * Time.deltaTime;

        // move the character controller
        controller.Move(moveDirection * Time.deltaTime);
    }
}
