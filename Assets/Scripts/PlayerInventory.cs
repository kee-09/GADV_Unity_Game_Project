using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public float maxWeight = 30f;
    public float currentWeight = 0f;
    public float currentValue = 0f;

    public TMP_Text weightText;
    public TMP_Text valueText;

    public bool AddItem(ItemValuable item)
    {
        // Check if the new item would exceed weight limit
        if (currentWeight + item.currentWeight > maxWeight)
        {
            Debug.Log("Too heavy to pick up " + item.itemName);
            return false;
        }

        // Add the item stats
        currentWeight += item.currentWeight;
        currentValue += item.currentValue;

        UpdateUI();
        return true;
    }

    void UpdateUI()
    {
        if (weightText != null)
            weightText.text = $"Weight: {currentWeight:F1} / {maxWeight}";

        if (valueText != null)
            valueText.text = $"Value: ${currentValue:F0}";
    }
}
