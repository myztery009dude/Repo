using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float sprintMultiplier = 2.5f;
    public float JumpHeight = 2f;
    public float fallGravityMultiplier = 2f;
    public float mouseSensitivity = 2.0f;
    public float pitchRange = 60.0f;

    private float forwardInputValue;
    private float strafeInputValue;
    private bool jumpInput;
    private bool sprintInput;

    private float terminalVelocity = 53f;
    private float verticalVelocity;

    private float rotateCameraPitch;
    private float currentMoveSpeed;

    private Camera firstPersonCam;
    private CharacterController characterController;
    private Animator animator;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        firstPersonCam = GetComponentInChildren<Camera>();
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        forwardInputValue = Input.GetAxisRaw("Vertical");
        strafeInputValue = Input.GetAxisRaw("Horizontal");
        jumpInput = Input.GetButtonDown("Jump");
        sprintInput = Input.GetButton("Sprint");

        Movement();
        JumpAndGravity();
        CameraMovement();
    }

    void Movement()
    {
        if (sprintInput)
        {
            currentMoveSpeed = movementSpeed * sprintMultiplier;
            Debug.Log("sprinting");
        } else
        {
            currentMoveSpeed = movementSpeed;
        }


        if (forwardInputValue != 0 || strafeInputValue != 0)
        {
            animator.Play("Walk Animation", 0);
        } else
        {
            animator.Play("Walk Animation", 0, 0f);
        }


            Vector3 direction = (transform.forward * forwardInputValue + transform.right * strafeInputValue).normalized * currentMoveSpeed * Time.deltaTime;

        direction += Vector3.up * verticalVelocity * Time.deltaTime;

        characterController.Move(direction);
    }

    void JumpAndGravity()
    {
        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0.0f)
            {
                verticalVelocity = -2f;
            }

            if(jumpInput)
            {
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);
            }
        }
        else
        {
            if(verticalVelocity < terminalVelocity)
            {
                float gravityMultiplier = 1;
                if(characterController.velocity.y < -1)
                {
                    gravityMultiplier = fallGravityMultiplier;
                }
                verticalVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
            }
        }
    }

    void CameraMovement()
    {
        float rotateYaw = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotateYaw, 0);

        rotateCameraPitch += -Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotateCameraPitch = Mathf.Clamp(rotateCameraPitch, -pitchRange, pitchRange);
        firstPersonCam.transform.localRotation = Quaternion.Euler(rotateCameraPitch, 0, 0);

    }
}
