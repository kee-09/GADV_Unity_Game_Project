using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float movementSpeed = 5f;

    private Vector2 movementInput;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        // Get input from the player
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");
        // Normalize the input vector to ensure consistent speed in all directions
        if (movementInput.magnitude > 1)
        {
            movementInput.Normalize();
        }

    }

    void FixedUpdate()
    {
        // Move the player based on input
        rb.MovePosition(rb.position + movementInput * movementSpeed * Time.fixedDeltaTime);
    }

}
