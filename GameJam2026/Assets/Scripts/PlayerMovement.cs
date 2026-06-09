using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    ///vars
    /// 
    /// 
    /// 
    ///Input Stuff 
    [SerializeField] private InputActionAsset InputActions; //put InputSystem_Actions in here

    //Input Actions
    private InputAction moveInput;
    private InputAction turnInput;
    private InputAction jumpInput;


    /// Player movement force variables
    public float playerWalkSpeed;
    public float playerTurnSpeed;
    public float jumpForce;
    private Vector2 groundMoveValue; ///how much the player moves forward and back
    private Vector2 turnMoveValue; ///how much the player rotates looking

    //Rigidbody
    [SerializeField] private Rigidbody rb; //drag the PlayerObj rigidbody into this field

    ///Check to see if you're touching the ground while jumping
    public LayerMask groundMask;
    private bool isGrounded;
    private bool isJumping;


    private bool altTurnAndMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnEnable()
    {
        //go into your InputActions asset and enable the
        //"Player" action map
        InputActions.FindActionMap("Player").Enable();
        moveInput = InputSystem.actions.FindAction("Move");
        jumpInput = InputSystem.actions.FindAction("Jump");
        turnInput = InputSystem.actions.FindAction("Look");

        altTurnAndMove = true;


        ///Lock the Cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;




    }

    private void OnDisable()
    {
        //Disable the Dialogue actionmap when you're not in Dialogue
        InputActions.FindActionMap("Player").Disable();                //point the action variables to the correct actions in the actionmap

        ///Unock the Cursor from the center of the screen and make it visible
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;


    }

    // Update is called once per frame
    void Update()//based on renderer, always slightly off time with fixed update
                 //in sync with what the player sees
    {
        //Read the value off the input and assign it to the groundMove vector2
        groundMoveValue = moveInput.ReadValue<Vector2>();//between -1 and  +1
        turnMoveValue = turnInput.ReadValue<Vector2>();//same range, but for left and right
        isJumping = jumpInput.IsPressed();

    }

    private void FixedUpdate()//also every frame, but evenly split with = time between frames, not based on renderer
                              //physics stuff goes here otherwise if you put it in update then it will be jittery
    {
        ///Add force to the rigidbody in the direction and amount 
        /// of groundMoveValue * the variable playerWalkSpeed
        /// to X and Z
        if (altTurnAndMove && (groundMoveValue.x != 0 || groundMoveValue.y != 0))
        {
            rb.AddRelativeForce(groundMoveValue.x * playerWalkSpeed, 0, groundMoveValue.y * playerWalkSpeed);
        }
        //relative is important for which direction its applied, this is for moving in a direction
        else if (turnMoveValue.x != 0)
        {

            //force mode can be diff for us
            rb.AddRelativeTorque(0,turnMoveValue.x * playerTurnSpeed,0, ForceMode.VelocityChange);
            //this is for turning
        }
        altTurnAndMove = !altTurnAndMove;
        if (isJumping)//&& isGrounded
        {
            rb.AddRelativeForce(0, jumpForce, 0);
            isJumping = false;
        }

    }

}
