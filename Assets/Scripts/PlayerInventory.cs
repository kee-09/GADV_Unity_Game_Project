using UnityEngine;
using TMPro; // Allows use of TextMeshProUGUI for better-looking UI text

public class PlayerInventory : MonoBehaviour
{
    // Maximum weight the player can carry
    public float maxWeight = 30f;

    // Current total weight of items the player is carrying
    public float currentWeight = 0f;

    // Current total value of items the player is carrying
    public float currentValue = 0f;

    // UI text element that displays the current weight
    public TMP_Text weightText;

    // UI text element that displays the current value
    public TMP_Text valueText;

    //  UI text element that displays the player's remaining lives
    public TMP_Text livesText;

    //  Maximum number of lives the player starts with
    public int maxLives = 5;

    //  Current number of lives the player has
    private int currentLives;

    // Called when the game starts
    void Start()
    {
        // Set current lives to the maximum at the beginning
        currentLives = maxLives;

        // Update all UI elements to show starting values
        UpdateUI();
    }

    // Try to add an item to the inventory
    public bool AddItem(ItemValuable item)
    {
        // Check if adding this item would exceed the weight limit
        if (currentWeight + item.currentWeight > maxWeight)
        {
            Debug.Log("Too heavy to pick up " + item.itemName);
            return false; // Item not added
        }

        // Add the item's weight and value to the player's totals
        currentWeight += item.currentWeight;
        currentValue += item.currentValue;

        // Refresh the UI to show new totals
        UpdateUI();
        return true; // Item successfully added
    }

    //  Call this method when the player takes damage
    public void TakeDamage(int amount)
    {
        // Reduce current lives by the damage amount
        currentLives -= amount;

        // Clamp lives so it doesn't go below 0 or above max
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        // Update the UI to show new lives count
        UpdateUI();

        // Check if the player has run out of lives
        if (currentLives <= 0)
        {
            Debug.Log("Player is dead!");
            // You can add game over or respawn logic here
        }
    }

    //  Call this method to heal the player and restore lives
    public void Heal(int amount)
    {
        // Increase current lives by the healing amount
        currentLives += amount;

        // Clamp lives so it doesn't go above max
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        // Update the UI to show new lives count
        UpdateUI();
    }

    // Update all UI elements (weight, value, lives)
    public void UpdateUI()
    {
        // Update weight display
        if (weightText != null)
            weightText.text = $"Weight: {currentWeight:F1} / {maxWeight}";

        // Update value display
        if (valueText != null)
            valueText.text = $"Value: ${currentValue:F0}";

        //  Update lives display
        if (livesText != null)
            livesText.text = $"Lives: {currentLives}";
    }
}
