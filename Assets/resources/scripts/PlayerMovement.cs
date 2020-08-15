using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveState
{
  EMPTY,
  IDLE,
  RUN,
  STRAFE,
  SPRINT,
  CROUCH,
  SLIDE
}

public class PlayerMovement : MonoBehaviour
{
  public Animator animator;
  public CharacterController controller;
  
  // Serializable fields
  [SerializeField]
  private float speed = 5f;
  [SerializeField]
  private Vector3 move;
  
  float gravity = -25f;

  // Movement speed variables
  Vector3 velocity;

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

  Vector3 lastMove;

  MoveState lastState;
  MoveState moveState;

  void Awake() {
    lastMove = move;  
  }

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

    animator.SetFloat("InputY", z, 1f, Time.deltaTime * 20f);
    animator.SetFloat("InputX", x, 1f, Time.deltaTime * 20f);
    animator.SetFloat("Sprint", Input.GetAxisRaw("Sprint"));
    animator.SetFloat("Crouch", Input.GetAxisRaw("Crouch"), 1f , Time.deltaTime * 10f);

    move = transform.right * x + transform.forward * z;

    bool wasGrounded = isGrounded;
    isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

    animator.SetBool("Grounded", isGrounded);

    if (isGrounded && velocity.y < 0)
    {
      velocity.y = gravity;
    } else if (!isGrounded && velocity.y < 0 && wasGrounded) {
      velocity.y = 0;
    }

    if (Input.GetButtonDown("Jump") && isGrounded)
    {
      Jump(2f);
      isJumping = true;
      lastMove = move;
    }
    else if (Input.GetButtonDown("Jump") && !isGrounded && isJumping)
    {
      Jump(3f);
      isJumping = false;
      lastMove = move;
    }

    // Change acceleration to 1/10th + 1/speed of the current state acceleration while midair
    if (!isGrounded) {
      accelMod = Mathf.Clamp(0.1f + 1/speed, 0.1f, 0.2f);
      move = Vector3.Lerp(lastMove, move, 16 * accelMod * Time.deltaTime);
    } 
    else if (moveState != MoveState.SLIDE)
    {
      accelMod = 1.0f;
      if (Input.GetButton("Sprint") && (moveState == MoveState.RUN || moveState == MoveState.SPRINT) && z == 1 && moveState != MoveState.CROUCH)
      {
        Sprint();

        if(Input.GetButtonDown("Slide"))
        {
          animator.Play("Slide");
        }
      }
      else if(Input.GetAxisRaw("Crouch") == 1 )
      {
        Crouch();
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
    }

    if(moveState >= MoveState.RUN && isGrounded && z + x != 0)
    {
      animator.SetBool("Moving", true);
    }
    else
    {
      animator.SetBool("Moving", false);
    }
    // if(animator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
    // {
    //    Slide();
    // }
    // else
    // {
    //   animator.SetBool("Slide", false);
    // }

    //Base Movement/Gravity behaviors
    speed = Mathf.Lerp(speed, maxSpeed, accelSpeed * accelMod * Time.deltaTime);
    controller.Move(move * speed * Time.deltaTime);

    velocity.y += gravity * Time.deltaTime;
    controller.Move(velocity * Time.deltaTime);
    // rigidbody.AddForce(velocity * Time.deltaTime);
    lastMove = move;
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
    maxSpeed = 3f;
    accelSpeed = 50f;
    moveState = MoveState.CROUCH;
  }
  
  public void Slide()
  {
    maxSpeed = 20f;
    accelSpeed = 50f;
    moveState = MoveState.SLIDE;
  }

  void Mantle()
  {

  }

  public void EmptyState() {
    moveState = MoveState.EMPTY;
  }
}
