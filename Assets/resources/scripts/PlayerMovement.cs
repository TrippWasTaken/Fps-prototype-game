using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum MoveState{
        IDLE,
        RUN,
        STRAFE,
        SPRINT,
        WALK
    }
public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;
    float speed = 5f;
    float gravity = -15f;
    float maxSpeed = 0f;
    public Transform groundCheck;
    public float groundDist = 0.4f;
    public LayerMask groundMask;
    float sprintSpeed = 4f;
    float accelSpeed = 2f;
    Vector3 velocity;
    bool isGrounded;
    bool isMoving = false;
    bool isBack = false;
    bool isJumping = false;

    MoveState moveState;


    void Update()
    {
        BaseMovement();
    }

    void BaseMovement(){


        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -4f;
        }

        // if(Input.GetAxis("Vertical") < 0){
        //     isBack = true;
        // }
        // else
        // {
        //     isBack = false;
        // }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump(2f);
            isJumping = true;
        }
        else if(Input.GetButtonDown("Jump") && !isGrounded && isJumping)
        {
            Jump(3f);
            isJumping = false;

        }

        if(Input.GetButton("Sprint") && moveState == MoveState.RUN && z == 1)
        {
            Sprint();
        }
        else if(Input.GetButton("Walk"))
        {
            //Walk();
        }
        else if(z != 0){
            Run();
        }
        else if (x != 0){
            Strafe();
        }
        else
        {
            maxSpeed = 0f;
            accelSpeed = 2f;
            moveState = MoveState.IDLE;
            
        }

        //Base Movement/Gravity behaviors
        controller.Move(move * speed * Time.deltaTime);
        speed = Mathf.Lerp(speed, maxSpeed , accelSpeed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


    }

    void Jump(float JumpHeight){
        velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
    }

    void Run(){
        maxSpeed = 5f;
        accelSpeed = 50f;
        moveState = MoveState.RUN;
    }

    void Sprint(){
        maxSpeed = 10f;
        accelSpeed = 50f;
        moveState = MoveState.RUN;
    }

    void Walk(){
        float SpeedMax = 0f;
    }

    void Wallrun(){

    }

    void Strafe(){
        maxSpeed = 4f;
        accelSpeed = 100f;
        moveState = MoveState.STRAFE;
    }

    void Crouch(){

    }

    void Slide(){
        float SpeedModifier = 0f;
        print("SLIDING BRO");
    }

    void Mantle(){

    }

}
