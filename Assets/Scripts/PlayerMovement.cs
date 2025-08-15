using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 5f;
    public float minSpeed = 1f; // Never go below this
    public PlayerInventory inventory;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (inventory == null)
            inventory = GetComponent<PlayerInventory>();
    }

    void FixedUpdate()
    {
        if (inventory == null) return;

        float weightFactor = Mathf.Clamp01(inventory.currentWeight / inventory.maxWeight);

        // Exponential slowdown + minimum speed clamp
        float rawSpeed = baseSpeed * Mathf.Pow(1f - weightFactor, 2f);
        float currentSpeed = Mathf.Max(rawSpeed, minSpeed);

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rb.MovePosition(rb.position + input * currentSpeed * Time.fixedDeltaTime);

        Debug.Log($"Speed: {currentSpeed:F2}, Weight: {inventory.currentWeight:F1}");
    }
}
