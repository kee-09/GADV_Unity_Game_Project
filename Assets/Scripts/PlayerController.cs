using UnityEngine; // Gives access to Unity-specific features like Rigidbody2D, Input, MonoBehaviour

// This script controls player movement, dashing, and speed affected by inventory weight
public class PlayerController : MonoBehaviour
{
    // Movement and dash settings (can be adjusted in the Unity Inspector)
    public float baseSpeed = 5f;        // Normal movement speed when not carrying weight
    public float minSpeed = 1f;         // Minimum speed to prevent player from becoming too slow
    public float dashSpeed = 20f;       // Speed during a dash
    public float dashDuration = 0.2f;   // How long the dash lasts (in seconds)
    public float dashCooldown = 5f;     // Time before the player can dash again

    // Internal variables to track movement and physics
    private Vector2 movementInput;      // Current movement direction based on player input
    private Vector2 lastInput;          // Last non-zero input direction (used for dashing)
    private Rigidbody2D rb;             // Reference to the player's Rigidbody2D for movement
    private PlayerInventory inventory;  // Reference to the player's inventory to check weight

    // Dash control variables
    private bool isDashing = false;     // Whether the player is currently dashing
    private float lastDashTime = -Mathf.Infinity; // Time when the last dash occurred (starts at negative infinity)

    // Called once when the game starts
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();       // Get the Rigidbody2D attached to the player
        inventory = GetComponent<PlayerInventory>(); // Get the PlayerInventory script (if attached)
    }

    // Called every frame — handles input and dash logic
    void Update()
    {
        // Get horizontal and vertical input from keyboard (WASD or arrow keys)
        Vector2 currentInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        // Prevent diagonal movement from being faster than straight movement
        if (currentInput.magnitude > 1)
            currentInput.Normalize(); // Normalize keeps direction but limits magnitude to 1

        // If there's movement input, store it as the last direction (used for dashing)
        if (currentInput != Vector2.zero)
            lastInput = currentInput;

        // Save current input for movement
        movementInput = currentInput;

        // If Space is pressed, and player is not already dashing, and cooldown has passed
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash()); // Start the dash coroutine
        }
    }

    // Called at fixed intervals — used for physics-based movement
    void FixedUpdate()
    {
        // If currently dashing, skip normal movement
        if (isDashing)
            return;

        // Calculate how heavy the player is (0 = no weight, 1 = full weight)
        float weightFactor = inventory != null ? Mathf.Clamp01(inventory.currentWeight / inventory.maxWeight) : 0f;

        // Reduce speed based on weight using a curve (heavier = slower)
        float rawSpeed = baseSpeed * Mathf.Pow(1f - weightFactor, 2f);

        // Ensure speed doesn't drop below the minimum
        float currentSpeed = Mathf.Max(rawSpeed, minSpeed);

        // Move the player using physics (position += direction × speed × time)
        rb.MovePosition(rb.position + movementInput * currentSpeed * Time.fixedDeltaTime);

        // Print current speed and weight to the console for debugging
        Debug.Log($"Speed: {currentSpeed:F2}, Weight: {inventory?.currentWeight:F1}");
    }

    // Coroutine that handles the dash behavior
    System.Collections.IEnumerator Dash()
    {
        isDashing = true;               // Mark player as dashing
        lastDashTime = Time.time;       // Record the time of this dash

        // Apply fast movement in the last direction the player moved
        rb.linearVelocity = lastInput * dashSpeed;

        // Wait for the dash duration to finish
        yield return new WaitForSeconds(dashDuration);

        // Stop the dash by resetting velocity and flag
        rb.linearVelocity = Vector2.zero;
        isDashing = false;
    }
}
