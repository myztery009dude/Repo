using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;

    private float forwardInputValue;
    private float strafeInputValue;

    private CharacterController characterController;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        forwardInputValue = Input.GetAxisRaw("Vertical");
        strafeInputValue = Input.GetAxisRaw("Horizontal");
        movementSpeed();
    }

    void Movement()
    {
        Vector3 direction = (transform.forward * forwardInputValue + transform.right * strafeInputValue).normalized * movementSpeed * Time.deltaTime;

        characterController.Move(direction);
    }
}
