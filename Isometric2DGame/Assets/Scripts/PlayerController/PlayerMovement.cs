using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerStats playerStats;
    private InputSystem_Actions playerInputActions; // Input System actions for handling player input
    private Vector2 input; // Stores the player's movement input
    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    // Rotation matrix to align input with isometric perspective
    private readonly Vector2 isometricUp = new Vector2(1, 1).normalized; // Northeast
    private readonly Vector2 isometricRight = new Vector2(1, -1).normalized; // Southeast

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        // Initialize input actions and get the Rigidbody2D component
        playerInputActions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // Enable the input action map
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        // Disable the input action map
        playerInputActions.Player.Disable();
    }

    private void Update()
    {
        // Gather player input every frame
        GatherInput();
    }

    private void FixedUpdate()
    {
        // Move the player using physics calculations
        Move();
    }

    private void Move()
    {
        // Transform input to isometric space
        Vector2 isometricInput = input.x * isometricRight + input.y * isometricUp;

        // Calculate the new position based on transformed input
        Vector2 targetPosition = rb.position + isometricInput * playerStats.MoveSpeed * Time.fixedDeltaTime;

        // Move the Rigidbody2D to the calculated target position
        rb.MovePosition(targetPosition);
    }

    private void GatherInput()
    {
        // Read movement input from the Input System
        input = playerInputActions.Player.Move.ReadValue<Vector2>();
    }
}
