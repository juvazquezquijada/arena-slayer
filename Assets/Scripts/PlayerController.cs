using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float jumpForce = 5.0f;
    public float gravity = 9.81f;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Get the input axes
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculate the movement direction
        Vector3 forward = transform.forward * vertical;
        Vector3 right = transform.right * horizontal;
        moveDirection = forward + right;

        // Apply the movement direction and speed
        moveDirection *= moveSpeed;

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Check for jump input
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
        {
            moveDirection.y = jumpForce;
        }

        // Move the character controller
        controller.Move(moveDirection * Time.deltaTime);
    }
}

