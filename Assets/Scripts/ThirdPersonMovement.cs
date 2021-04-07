using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    // Motor that drives our player
    public CharacterController charCon;
    public Transform cam;

    public float moveSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float doubleJumpMultiplier = 0.5f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    bool canDoubleJump = false;
    bool inWater = false;
    Vector3 velocity;

    void Update()
    {       
        if(charCon)
        {
            handleJump();
            handleMovement();
        }
               
    }

    /// <summary>
    /// Handles character movement.
    /// Gathers user input and then calculates movement.
    /// Also applies gravity.
    /// </summary>
    void handleMovement()
    {
        // Gather Input
        // GetAxisRaw: No axis input smoothing
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        // normalized: to make sure if we move diagonally (press two keys) we don't go faster!!

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            charCon.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }        

        if (inWater)
        {
            // float upwards
            Debug.Log("inWater");
            Vector3 up = new Vector3(0f, 2f, 0f);
            charCon.Move(up * Time.deltaTime);
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            charCon.Move(velocity * Time.deltaTime);
        }
    }

    /// <summary>
    /// Handles the jump of the character.
    /// Checks if they are grounded and therefore can jump,
    /// or if they're on the air and can double jump.
    /// </summary>
    void handleJump()
    {
        if (charCon.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            canDoubleJump = true;
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        else
        {
            if (Input.GetButtonDown("Jump") && canDoubleJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity) * doubleJumpMultiplier;
                canDoubleJump = false;
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
        {
            //Debug.Log("Collided with object that has no rigid body or is kinematic.");
            return;
        }

        // we don't want to push objects below us
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        if (hit.gameObject.tag == "Water")
        {
            //moveSpeed = 3f;
            Debug.Log("Collided with Water");
        }
    }

    public void EnterWater()
    {
        Debug.Log("Enter water");
        moveSpeed = 3f;
        inWater = true;
    }

    public void ExitWater()
    {
        Debug.Log("Exit water");
        moveSpeed = 6f;
        inWater = false;
    }
}
