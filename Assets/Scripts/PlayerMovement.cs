using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    //private bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    public UnityEngine.GameObject Player;

    public static Rigidbody rb;

    public Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Set initial player position to the rigid body and log it
        rb.position = Player.transform.position;
        Debug.Log("Initial Player Position: " + Player.transform.position);

        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        // Rotate player to face the same direction as the camera
        //if (moveDirection.magnitude > 0.1f)
        //{
        //    transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * 10f);
        //}
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        //if (Input.GetKey(jumpKey) && readyToJump && grounded)
        //{
        //    readyToJump = false;

        //    Jump();

        //    Invoke(nameof(ResetJump), jumpCooldown);
        //}
    }

    private void MovePlayer()
    {
        // Get camera's forward and right vectors
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // Project the vectors onto the horizontal plane (y=0)
        forward.y = 0f;
        right.y = 0f;

        // Normalize vectors
        forward.Normalize();
        right.Normalize();

        // Calculate the movement direction
        moveDirection = forward * verticalInput + right * horizontalInput;

        // Apply the force based on whether the player is on the ground or in the air
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce * 100, ForceMode.Force);
    }

    private void ResetJump()
    {
        //readyToJump = true;
    }
}
