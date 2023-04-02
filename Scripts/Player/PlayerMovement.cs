using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    CharacterController controller;

    [SerializeField]
    float speed = 12f;

    [SerializeField]
    float gravity = -9.81f;

    [SerializeField]
    Transform groundCheck;

    [SerializeField]
    float groundDistance = 0.4f;

    [SerializeField]
    LayerMask groundMask;

    [SerializeField]
    float jumpHeight = 3f;

    [SerializeField]
    FixedJoystick joystick;
    Vector3 velocity;
    bool isGrounded;
    int numberOfJumps = 0;

    //refactor to state machine
    private bool isCartOpen;
    private bool isPDPOpen;
    private bool isCheckoutOpen;


    // Update is called once per frame
    void Start()
    {

    }
    void Update()
    {
        IsGroundedCheck();
        PlayerMove();
        Jump();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    void PlayerMove()
    {

        //maybe bad pratice, refactor in future
        isPDPOpen = PDPController.Instance.GetPDPStatus();
        isCheckoutOpen = CheckoutController.Instance.GetCheckoutStatus();
        isCartOpen = CartController.Instance.GetCartStatus();

        if (!isCartOpen && !isPDPOpen && !isCheckoutOpen)
        {
            // float x = joystick.Horizontal;
            // float z = joystick.Vertical;
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    void IsGroundedCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -10f;
        }
    }

    void Jump()
    {
        int maxJumps = 1;

        if (isGrounded)
            numberOfJumps = 0;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            numberOfJumps++;
        }
        if (Input.GetButtonDown("Jump") && !isGrounded && numberOfJumps < maxJumps)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            numberOfJumps++;
        }
    }
}
