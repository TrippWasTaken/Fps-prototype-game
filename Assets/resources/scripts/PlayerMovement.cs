using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;
    float speed = 5f;
    float gravity = -11f;

    public Transform groundCheck;
    public float groundDist = 0.4f;
    public LayerMask groundMask;
    float sprintSpeed = 4f;
    Vector3 velocity;
    bool isGrounded;
    bool isMoving = false;
    bool isBack = false;

    void Update()
    {
        BaseMoevement();
    }

    void BaseMoevement(){
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        animator.SetBool("Backwards", isBack);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            animator.SetBool("Jump", false);
        }

        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") > 0){
            isMoving = true;
            animator.SetBool("Run", true);
        }
        else
        {
            isMoving = false;
            animator.SetBool("Run", false);
        }

        if(Input.GetAxis("Vertical") < 0){
            isBack = true;
        }
        else
        {
            isBack = false;
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
            animator.SetBool("Jump", true);
        }

        if(Input.GetButtonDown("Sprint"))
        {
            print("Sprinting");
            speed += sprintSpeed;
            animator.SetBool("Sprint", true);
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            print("running");
            speed -= sprintSpeed;
            animator.SetBool("Sprint", false);
        }

        if(Input.GetButtonDown("Walk"))
        {
            print("Walking");
            speed -= 2.5f;
            animator.SetBool("Walk", true);
        }
        else if (Input.GetButtonUp("Walk"))
        {
            print("running");
            speed += 2.5f;
            animator.SetBool("Walk", false);
        }
    }

    void Jump(){
        float JumpHeight = 2f;
        velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);

    }
}
