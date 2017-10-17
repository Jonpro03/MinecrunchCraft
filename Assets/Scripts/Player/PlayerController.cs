using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

/// <summary>
/// Player controller.
/// This is the main class for the physics based 3rd person player controller.
/// This class handels everything that requires Update(), LateUpdate(), and all physics methods.
/// This class calls PlayerAnim() to handle the animation states required for walking/running.
/// This class calls PlayerLocomotion() to handle all the applied force to let the player move.
/// </summary>

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 8.0F;
    public float jumpSpeed = 8.0F;
    public float runSpeed = 9.0F;
    public float gravity = 20.0F;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection *= walkSpeed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

}