using UnityEngine;
using TMPro; // Allows use of TextMeshProUGUI for better-looking UI text

public class PlayerInventory : MonoBehaviour
{
    public float maxWeight = 30f;                      // Max weight player can carry
    public float currentWeight = 0f;                   // Current weight of carried items
    public float currentValue = 0f;                    // Total value of carried items

    public TMP_Text weightText;                        // UI text for weight display
    public TMP_Text valueText;                         // UI text for value display
    public TMP_Text livesText;                         // UI text for lives display

    public int maxLives = 5;                           // Starting number of lives
    private int currentLives;                          // Current number of lives

    void Start()
    {
        currentLives = maxLives;                       // Reset lives to full
        UpdateUI();                                    // Refresh UI

        Debug.Log("Player starting lives: " + currentLives); // Debug check
    }

    public bool AddItem(ItemValuable item)
    {
        if (currentWeight + item.currentWeight > maxWeight) // Check weight limit
        {
            Debug.Log("Too heavy to pick up " + item.itemName);
            return false;
        }

        currentWeight += item.currentWeight;           // Add item weight
        currentValue += item.currentValue;             // Add item value
        UpdateUI();                                    // Refresh UI
        return true;
    }

    public void TakeDamage(int amount)
    {
        currentLives -= amount;                        // Subtract lives
        currentLives = Mathf.Clamp(currentLives, 0, maxLives); // Clamp between 0 and max
        UpdateUI();                                    // Update UI

        if (currentLives <= 0)                         // Only trigger if lives are gone
        {
            Debug.Log("Player is dead!");

            GameManager gm = Object.FindFirstObjectByType<GameManager>(); // Find GameManager
            if (gm != null)
                gm.ShowLoseScreen();                   // Show lose screen
        }
    }

    public void Heal(int amount)
    {
        currentLives += amount;                        // Add lives
        currentLives = Mathf.Clamp(currentLives, 0, maxLives); // Clamp to max
        UpdateUI();                                    // Update lives UI
    }

    public void UpdateUI()
    {
        if (weightText != null)
            weightText.text = $"Weight: {currentWeight:F1} / {maxWeight}";

        if (valueText != null)
            valueText.text = $"Value: ${currentValue:F0}";

        if (livesText != null)
            livesText.text = $"Lives: {currentLives}";
    }
}
