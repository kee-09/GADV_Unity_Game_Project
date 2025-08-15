using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float baseSpeed = 5f;
    public float minSpeed = 1f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 5f;

    private Vector2 movementInput;
    private Vector2 lastInput;
    private Rigidbody2D rb;
    private PlayerInventory inventory;

    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inventory = GetComponent<PlayerInventory>();
    }

    void Update()
    {
        Vector2 currentInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (currentInput.magnitude > 1)
            currentInput.Normalize();

        if (currentInput != Vector2.zero)
            lastInput = currentInput;

        movementInput = currentInput;

        // Dash input with cooldown
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
            return;

        float weightFactor = inventory != null ? Mathf.Clamp01(inventory.currentWeight / inventory.maxWeight) : 0f;
        float rawSpeed = baseSpeed * Mathf.Pow(1f - weightFactor, 2f);
        float currentSpeed = Mathf.Max(rawSpeed, minSpeed);

        rb.MovePosition(rb.position + movementInput * currentSpeed * Time.fixedDeltaTime);

        Debug.Log($"Speed: {currentSpeed:F2}, Weight: {inventory?.currentWeight:F1}");
    }

    System.Collections.IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        rb.linearVelocity = lastInput * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;
        isDashing = false;
    }
}
