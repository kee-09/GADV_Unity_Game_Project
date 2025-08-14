using UnityEngine;
using TMPro; // For using TextMeshProUGUI

public class PlayerInventory : MonoBehaviour
{
    public float maxWeight = 30f;        // Maximum weight the player can carry
    public float currentWeight = 0f;     // Current total weight of items
    public float currentValue = 0f;      // Current total value of items

    public TMP_Text weightText;          // UI text showing weight
    public TMP_Text valueText;           // UI text showing value

    // Try to add an item to the inventory
    public bool AddItem(ItemValuable item)
    {
        // Check if the new item would exceed the weight limit
        if (currentWeight + item.currentWeight > maxWeight)
        {
            Debug.Log("Too heavy to pick up " + item.itemName);
            return false; // Item not added
        }

        // Add the item's weight and value
        currentWeight += item.currentWeight;
        currentValue += item.currentValue;

        UpdateUI(); // Refresh the UI
        return true; // Item successfully added
    }

    // Public method to update the inventory UI
    public void UpdateUI()
    {
        if (weightText != null)
            weightText.text = $"Weight: {currentWeight:F1} / {maxWeight}";

        if (valueText != null)
            valueText.text = $"Value: ${currentValue:F0}";
    }
}
