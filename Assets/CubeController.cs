using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpSpeed = 8f;
    private CharacterController controller;
    private Vector3 movement;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded){
            movement = new Vector3(0f, jumpSpeed, 0f);
        }
        else {
            movement = new Vector3(horizontalInput, 0f, verticalInput);
        }
        controller.Move(movement * moveSpeed * Time.deltaTime);
    }

    // private void Jump()
    // {
    //     Vector3 jump = new Vector3();
    //     controller.Move(jump * Time.deltaTime);
    // }
}
