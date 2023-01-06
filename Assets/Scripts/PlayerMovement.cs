using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f; // movement speed
    public float jumpForce = 10f; // force applied when jumping
    public float gravity = 20f; // gravity applied to the player

    private Vector3 moveDirection = Vector3.zero; // current movement direction
    private CharacterController controller; // reference to the character controller component

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

        // apply gravity to the character controller
        moveDirection.y -= gravity * Time.deltaTime;

        // move the character controller
        controller.Move(moveDirection * Time.deltaTime);
    }
}
