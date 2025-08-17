using UnityEngine;

// This script controls a monster that roams randomly and chases the player when nearby.
// It also damages the player on contact and shows a visual detection radius in the editor.

public class SimpleMonsterAI : MonoBehaviour
{
    // Speed when roaming randomly
    public float roamSpeed = 2f;

    // Speed when chasing the player
    public float chaseSpeed = 3f;

    // How long the monster roams before changing direction
    public float roamDuration = 3f;

    // How close the player needs to be before the monster starts chasing
    public float detectionRadius = 5f;

    // Reference to the player's position in the game world
    public Transform player;

    // Current direction the monster is roaming in
    private Vector2 roamDirection;

    // Timer to track how long the monster has been roaming
    private float roamTimer;

    // True if the monster is currently chasing the player
    private bool isChasing;

    // Called once when the game starts
    void Start()
    {
        // Pick a random direction to start roaming
        PickNewRoamDirection();
    }

    // Called every frame
    void Update()
    {
        // Measure the distance between the monster and the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // If the player is within detection range, start chasing
        isChasing = distanceToPlayer <= detectionRadius;

        // Choose behavior based on whether the monster is chasing or roaming
        if (isChasing)
            ChasePlayer();
        else
            Roam();
    }

    // Handles roaming behavior when the player is far away
    void Roam()
    {
        // Add time to the roam timer (Time.deltaTime = time since last frame)
        roamTimer += Time.deltaTime;

        // Move the monster in the current roam direction
        transform.Translate(roamDirection * roamSpeed * Time.deltaTime);

        // If roaming time is up, pick a new direction and reset the timer
        if (roamTimer >= roamDuration)
        {
            PickNewRoamDirection();
            roamTimer = 0f;
        }
    }

    // Picks a new random direction for roaming
    void PickNewRoamDirection()
    {
        // Choose a random direction inside a circle and normalize it to keep speed consistent
        // Reference: https://docs.unity3d.com/ScriptReference/Random-insideUnitCircle.html
        roamDirection = Random.insideUnitCircle.normalized;
    }

    // Handles chasing behavior when the player is close
    void ChasePlayer()
    {
        // Calculate direction from monster to player and normalize it
        Vector2 direction = (player.position - transform.position).normalized;

        // Move the monster toward the player
        transform.Translate(direction * chaseSpeed * Time.deltaTime);
    }

    // Called when the monster collides with another object
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object it collided with is tagged "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Try to get the PlayerInventory component from the player
            PlayerInventory inventory = collision.gameObject.GetComponent<PlayerInventory>();

            // If the player has an inventory, apply damage
            if (inventory != null)
            {
                inventory.TakeDamage(1); // Reduce player's health by 1
            }
        }
    }

    // Optional: Draw a red circle in the editor to show detection range
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // Draw a wireframe sphere around the monster to visualize how far it can detect the player
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
