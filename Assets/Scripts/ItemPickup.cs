using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private bool playerInRange = false;
    private PlayerInventory playerInventory;
    private ItemValuable valuableData;

    void Update()
    {
        // If player is in range and presses E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory != null && valuableData != null)
            {
                // Try to add the item to the player's inventory
                bool pickedUp = playerInventory.AddItem(valuableData);
                if (pickedUp)
                {
                    Debug.Log("Picked up: " + valuableData.itemName + " worth $" + valuableData.currentValue);
                    Destroy(gameObject); // Remove the item from the world
                }
            }
            else
            {
                Debug.LogWarning("Missing inventory or item data!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerInventory = other.GetComponent<PlayerInventory>();

            // Grab the item's data when the player enters
            valuableData = GetComponent<ItemValuable>();

            if (valuableData != null)
            {
                Debug.Log("Press E to pick up " + valuableData.itemName + " worth $" + valuableData.currentValue);
            }
            else
            {
                Debug.LogWarning("ItemValuable component missing on this item!");
            }
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
