using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveState
{
  IDLE,
  WALK,
  RUN,
  STRAFE,
  SPRINT
}

public class PlayerMovement : MonoBehaviour
{
  public Animator animator;
  public CharacterController controller;
  
  float gravity = -25f;

  // Movement speed variables
  Vector3 velocity;
  float speed = 5f;
  float maxSpeed = 0f;
  float sprintSpeed = 4f;
  float accelSpeed = 2f;

  // Speed modifiers
  float accelMod = 1.0f;

  public Transform groundCheck;
  public LayerMask groundMask;
  public float groundDist = 0.4f;

  // Conditional variables
  bool isGrounded;
  bool isMoving = false;
  bool isBack = false;
  bool isJumping = false;

  MoveState lastState;
  MoveState moveState;

  void Update()
  {
    // If we have changed state, print both states
    if (lastState != moveState){
      print("Moving from state " + lastState + " to " + moveState);
    }
    BaseMovement();
  }

  void BaseMovement()
  {
    lastState = moveState;

    float x = Input.GetAxisRaw("Horizontal");
    float z = Input.GetAxisRaw("Vertical");
    Vector3 move = transform.right * x + transform.forward * z;

    isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

    if (isGrounded && velocity.y < 0)
    {
      velocity.y = -4f;
    }

    if (Input.GetButtonDown("Jump") && isGrounded)
    {
      Jump(2f);
      isJumping = true;
    }
    else if (Input.GetButtonDown("Jump") && !isGrounded && isJumping)
    {
      Jump(3f);
      isJumping = false;
    }

    // Change acceleration to 1/100th of the current state while midair
    if (!isGrounded) {
      accelMod = 0.01f;
    } else {
      accelMod = 1.0f;
    }

    if (Input.GetButton("Sprint") && (moveState == MoveState.RUN || moveState == MoveState.SPRINT) && z == 1 && !Input.GetButtonDown("Walk") && isGrounded)
    {
      Sprint();
    }
    else if (Input.GetButton("Walk") && isGrounded)
    {
      Walk();
    }
    else if (z != 0)
    {
      Run();
    }
    else if (x != 0)
    {
      Strafe();
    }
    else
    {
      maxSpeed = 0f;
      accelSpeed = 2f;
      moveState = MoveState.IDLE;

    }

    //Base Movement/Gravity behaviors
    speed = Mathf.Lerp(speed, maxSpeed, accelSpeed * accelMod * Time.deltaTime);
    controller.Move(move * speed * Time.deltaTime);

    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
  }

  void Jump(float JumpHeight)
  {
    velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
  }

  void Run()
  {
    maxSpeed = 5f;
    accelSpeed = 100f;
    moveState = MoveState.RUN;
  }

  void Sprint()
  {
    maxSpeed = 10f;
    accelSpeed = 50f;
    moveState = MoveState.SPRINT;
  }

  void Walk()
  {
    maxSpeed = 2f;
    accelSpeed = 75f;
    moveState = MoveState.WALK;
  }

  void Wallrun()
  {

  }

  void Strafe()
  {
    maxSpeed = 5f;
    accelSpeed = 100f;
    moveState = MoveState.STRAFE;
  }

  void Crouch()
  {

  }

  void Slide()
  {

  }

  void Mantle()
  {

  }

}
