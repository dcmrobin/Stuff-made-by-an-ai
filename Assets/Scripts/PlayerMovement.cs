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
        // Raycast downwards to check if the player is on the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1.1f))
        {
            // get input for the horizontal and vertical axes
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            // get the camera object
            Camera mainCamera = Camera.main;
            // calculate the move direction
            moveDirection = (mainCamera.transform.forward * v) + (mainCamera.transform.right * h);
            moveDirection = moveDirection.normalized * speed;
            // check if the player wants to jump
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }
        // if the player is not on the ground
        else
        {
            // check if the player can wall jump and has pressed the jump button
            if (canWallJump && Input.GetButtonDown("Jump") && wallJumpTimeout <= 0)
            {
                // check if the player is touching a wall
                if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
                {
                    // calculate the wall jump direction
                    moveDirection = (-transform.forward + transform.up).normalized;
                    moveDirection = moveDirection.normalized * speed;
                    moveDirection.y = jumpForce;

                    // set the wall jump timeout
                    wallJumpTimeout = 0.5f;
                }
                else if (Physics.Raycast(transform.position, -transform.forward, out hit, 1f))
                {
                    // calculate the wall jump direction
                    moveDirection = (transform.forward + transform.up).normalized;
                    moveDirection = moveDirection.normalized * speed;
                    moveDirection.y = jumpForce;

                    // set the wall jump timeout
                    wallJumpTimeout = 0.5f;
                }
                else if (Physics.Raycast(transform.position, transform.right, out hit, 1f))
                {
                    // calculate the wall jump direction
                    moveDirection = (-transform.right + transform.up).normalized;
                    moveDirection = moveDirection.normalized * speed;
                    moveDirection.y = jumpForce;

                    // set the wall jump timeout
                    wallJumpTimeout = 0.5f;
                }
                else if (Physics.Raycast(transform.position, -transform.right, out hit, 1f))
                {
                // calculate the wall jump direction
                moveDirection = (transform.right + transform.up).normalized;
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