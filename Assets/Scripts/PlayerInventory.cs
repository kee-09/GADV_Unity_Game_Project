using UnityEngine; // Gives access to Unity's core features like MonoBehaviour and GameObjects
using TMPro; // Allows use of TextMeshProUGUI for better-looking UI text: https://docs.unity3d.com/Packages/com.unity.textmeshpro@4.0/manual/index.html

public class PlayerInventory : MonoBehaviour
{
    // Maximum weight the player can carry before being overloaded
    public float maxWeight = 30f;

    // Tracks how much weight the player is currently carrying
    public float currentWeight = 0f;

    // Tracks the total value of all items the player has collected
    public float currentValue = 0f;

    // UI elements to show weight, value, and lives on screen
    public TMP_Text weightText;
    public TMP_Text valueText;
    public TMP_Text livesText;

    // Maximum number of lives the player starts with
    public int maxLives = 5;

    // Current number of lives the player has left
    private int currentLives;

    void Start()
    {
        // When the game starts, give the player full lives
        currentLives = maxLives;

        // Update the UI so the player sees the correct starting values
        UpdateUI();

        // Print the starting lives to the console for debugging
        Debug.Log("Player starting lives: " + currentLives);
    }

    // Tries to add an item to the player's inventory
    public bool AddItem(ItemValuable item)
    {
        // If adding the item would exceed the weight limit, reject it
        if (currentWeight + item.currentWeight > maxWeight)
        {
            Debug.Log("Too heavy to pick up " + item.itemName); // Show message in console
            return false; // Item not added
        }

        // Add the item's weight and value to the player's totals
        currentWeight += item.currentWeight;
        currentValue += item.currentValue;

        // Update the UI to reflect the new totals
        UpdateUI();

        return true; // Item successfully added
    }

    // Reduces the player's lives when they take damage
    public void TakeDamage(int amount)
    {
        currentLives -= amount; // Subtract damage from current lives

        // Clamp lives so they never go below 0 or above maxLives
        currentLives = Mathf.Clamp(currentLives, 0, maxLives); // https://docs.unity3d.com/ScriptReference/Mathf.Clamp.html

        UpdateUI(); // Refresh the lives display

        // If lives reach 0, trigger the lose condition
        if (currentLives <= 0)
        {
            Debug.Log("Player is dead!");

            // Find the GameManager in the scene to handle game over
            GameManager gm = Object.FindFirstObjectByType<GameManager>(); // https://docs.unity3d.com/ScriptReference/Object.FindFirstObjectByType.html

            if (gm != null)
                gm.ShowLoseScreen(); // Show the lose screen
        }
    }

    // Heals the player by adding lives
    public void Heal(int amount)
    {
        currentLives += amount; // Add healing amount to current lives

        // Clamp lives so they don’t go above maxLives
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        UpdateUI(); // Refresh the lives display
    }

    // Updates all UI elements to show current inventory and lives
    public void UpdateUI()
    {
        if (weightText != null)
            weightText.text = $"Weight: {currentWeight:F1} / {maxWeight}"; // Show weight with 1 decimal place

        if (valueText != null)
            valueText.text = $"Value: ${currentValue:F0}"; // Show value with no decimals

        if (livesText != null)
            livesText.text = $"Lives: {currentLives}"; // Show current lives
    }
}
