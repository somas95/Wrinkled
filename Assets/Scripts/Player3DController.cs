using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3DController : MonoBehaviour
{

    private float horizontalMov;
    private float verticalMov;

    private Vector3 playerInput;
    private Vector3 movePlayer;

    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;


    public CharacterController player;
    public float speed;
    public float gravity = -9.8f;
    public float jumpForce;
    private float y_velocity = -1;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMov = Input.GetAxis("Horizontal");
        verticalMov = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMov, 0, verticalMov);   //normalizamos
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        camDirection();

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;
        movePlayer *= speed;
        //player.transform.LookAt(player.transform.position + movePlayer);

        setGravity();

        advancedMovement();

        player.Move(movePlayer * Time.deltaTime);
        
        
    }

    void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }

    void advancedMovement()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            y_velocity = jumpForce;
            movePlayer.y = y_velocity;
        }
    }

    void setGravity()
    {
        if (player.isGrounded)
        {
            movePlayer.y = -1;
        } else {
            y_velocity += gravity * Time.deltaTime;
            movePlayer.y = y_velocity; 
        }
    }


}
