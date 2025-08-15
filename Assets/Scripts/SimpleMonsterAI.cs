using UnityEngine;

public class SimpleMonsterAI : MonoBehaviour
{
    public float roamSpeed = 2f;
    public float chaseSpeed = 3f;
    public float roamDuration = 3f;
    public float detectionRadius = 5f;
    public Transform player;

    private Vector2 roamDirection;
    private float roamTimer;
    private bool isChasing;

    void Start()
    {
        PickNewRoamDirection();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        isChasing = distanceToPlayer <= detectionRadius;

        if (isChasing)
            ChasePlayer();
        else
            Roam();
    }

    void Roam()
    {
        roamTimer += Time.deltaTime;
        transform.Translate(roamDirection * roamSpeed * Time.deltaTime);

        if (roamTimer >= roamDuration)
        {
            PickNewRoamDirection();
            roamTimer = 0f;
        }
    }

    void PickNewRoamDirection()
    {
        roamDirection = Random.insideUnitCircle.normalized;
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * chaseSpeed * Time.deltaTime);
    }

    //  Damage player on collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInventory inventory = collision.gameObject.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.TakeDamage(1);
            }
        }
    }

    // Optional: visualize detection radius
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
