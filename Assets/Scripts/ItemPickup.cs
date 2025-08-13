using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private bool playerInRange = false;
    private PlayerInventory playerInventory;
    private ItemValuable valuableData;

    void Start()
    {
        valuableData = GetComponent<ItemValuable>(); // Get the item's data
    }

    void Update()
    {
        // If player is in range and presses E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory != null)
            {
                // Try to add the item to the player's inventory
                bool pickedUp = playerInventory.AddItem(valuableData);
                if (pickedUp)
                {
                    Destroy(gameObject); // Remove the item from the world
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponent<PlayerInventory>();
            Debug.Log("Press E to pick up " + valuableData.itemName);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
        }
    }
}
