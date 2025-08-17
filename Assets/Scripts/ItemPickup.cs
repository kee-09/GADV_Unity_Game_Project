using UnityEngine;
// Gives access to Unity’s built-in features like MonoBehaviour, GameObject, etc.

public class ItemPickup : MonoBehaviour
{
    // Tracks if the player is close enough to pick up the item
    private bool playerInRange = false;

    // Reference to the player's inventory system
    private PlayerInventory playerInventory;

    // Reference to the item's data (name, value, etc.)
    private ItemValuable valuableData;

    void Update()
    {
        // Runs every frame — constantly checks for player input
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Make sure both inventory and item data are available
            if (playerInventory != null && valuableData != null)
            {
                // Try to add the item to the player's inventory
                bool pickedUp = playerInventory.AddItem(valuableData);

                if (pickedUp)
                {
                    // Show a message in the console with item name and value
                    Debug.Log("Picked up: " + valuableData.itemName + " worth $" + valuableData.currentValue);

                    // Remove the item from the scene — like deleting it from the world
                    Destroy(gameObject);
                }
            }
            else
            {
                // Warn if something’s missing (like inventory or item info)
                Debug.LogWarning("Missing inventory or item data!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // When something enters the item's trigger zone
        if (other.CompareTag("Player"))
        {
            // Mark that the player is close enough to interact
            playerInRange = true;

            // Try to get the PlayerInventory component from the player
            playerInventory = other.GetComponent<PlayerInventory>();

            // Get the item's data from this object
            valuableData = GetComponent<ItemValuable>();

            if (valuableData != null)
            {
                // Show a message prompting the player to press E
                Debug.Log("Press E to pick up " + valuableData.itemName + " worth $" + valuableData.currentValue);
            }
            else
            {
                // Warn if the item doesn’t have its data attached
                Debug.LogWarning("ItemValuable component missing on this item!");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // When the player leaves the item's trigger zone
        if (other.CompareTag("Player"))
        {
            // Reset interaction variables
            playerInRange = false;
            playerInventory = null;
        }
    }
}
