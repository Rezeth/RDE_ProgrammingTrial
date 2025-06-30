using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player movement and visual feedback (color) based on movement direction.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    private PlayerStats playerStats;
    private InputSystem_Actions playerInputActions; // Handles player input actions
    private Vector2 input; // Raw input from player
    private Rigidbody2D rb; // Rigidbody2D for movement
    private SpriteRenderer spriteRenderer; // SpriteRenderer for color changes

    // Isometric axes for diagonal movement
    private static readonly Vector2 IsometricUp = new Vector2(1, 1).normalized;    // Northeast
    private static readonly Vector2 IsometricRight = new Vector2(1, -1).normalized; // Southeast

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerInputActions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    private void Update()
    {
        GatherInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// Moves the player and updates the sprite color based on movement direction.
    /// </summary>
    private void Move()
    {
        // Convert input to isometric movement vector
        Vector2 isometricInput = input.x * IsometricRight + input.y * IsometricUp;

        // Move the player
        Vector2 targetPosition = rb.position + isometricInput * playerStats.MoveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);

        // Update color based on movement direction
        SetColorByDirection(isometricInput);
    }

    /// <summary>
    /// Sets the player's color based on the isometric movement direction.
    /// </summary>
    /// <param name="moveDir">Isometric movement vector.</param>
    private void SetColorByDirection(Vector2 moveDir)
    {
        if (spriteRenderer == null)
            return;

        if (moveDir.sqrMagnitude < 0.001f)
        {
            spriteRenderer.color = Color.white; // Idle
            return;
        }

        float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;

        // Define color for each isometric direction
        // Right/Southeast: -45 to 45
        // Up/Northeast: 45 to 135
        // Down/Southwest: -135 to -45
        // Left/Northwest: otherwise
        if (angle >= -45 && angle < 45)
            spriteRenderer.color = Color.red;      // Right/Southeast
        else if (angle >= 45 && angle < 135)
            spriteRenderer.color = Color.blue;     // Up/Northeast
        else if (angle >= -135 && angle < -45)
            spriteRenderer.color = Color.yellow;   // Down/Southwest
        else
            spriteRenderer.color = Color.green;    // Left/Northwest
    }

    /// <summary>
    /// Reads movement input from the Input System.
    /// </summary>
    private void GatherInput()
    {
        input = playerInputActions.Player.Move.ReadValue<Vector2>();
    }
}
