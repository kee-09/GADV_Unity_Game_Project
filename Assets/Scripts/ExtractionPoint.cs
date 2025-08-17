using UnityEngine; // Gives access to Unity engine features like GameObjects, components, and input
using TMPro;        // Allows use of TextMeshPro for advanced UI text elements

// This script controls the extraction zone where the player deposits loot.
// If the player deposits enough value to meet the quota, they win the game.
public class ExtractionPoint : MonoBehaviour
{
    public float quota = 30f; // The target value the player must deposit to win (e.g., like collecting $30 worth of loot)

    private float depositedValue = 0f; // Tracks how much value the player has deposited so far
    private bool playerInRange = false; // True when the player is standing inside the extraction zone
    private bool hasWon = false; // Prevents the win screen from showing more than once

    public TextMeshProUGUI quotaText; // UI element that displays progress (e.g., "15 / 30")
    private GameManager gameManager; // Reference to the GameManager script that controls win/lose screens

    // Called once when the game starts
    void Start()
    {
        // Try to find the GameManager object in the scene and get its script
        gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();

        // If GameManager is missing, show an error in the console to help with debugging
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found! Make sure the GameObject is named 'GameManager' and has the GameManager script.");
        }

        // Update the UI to show the starting progress (e.g., "0 / 30")
        UpdateQuotaText();
    }

    // Called every frame — used to check for player input
    void Update()
    {
        // If the player presses the E key
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed");

            // If the player is NOT in range of the extraction zone, stop here
            if (!playerInRange)
            {
                Debug.LogWarning("Player is NOT in range.");
                return; // Exit the method early
            }

            // Find the player GameObject using its tag
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player GameObject not found! Make sure it's tagged 'Player'.");
                return;
            }

            // Get the PlayerInventory script from the player
            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (inventory == null)
            {
                Debug.LogError("PlayerInventory component not found on Player!");
                return;
            }

            // Log how much value is being deposited
            Debug.Log($"Depositing ${inventory.currentValue}");

            // Add the player's loot value to the total deposited
            depositedValue += inventory.currentValue;

            // Reset the player's inventory after depositing
            inventory.currentWeight = 0f;
            inventory.currentValue = 0f;

            // Update the player's inventory UI and the quota progress UI
            inventory.UpdateUI();
            UpdateQuotaText();

            // Log the updated totals for debugging
            Debug.Log($"DepositedValue: {depositedValue}, Quota: {quota}");

            // If the player has met the quota and hasn't already won
            if (!hasWon && depositedValue >= quota)
            {
                hasWon = true; // Mark the game as won
                Debug.Log("Quota met! Attempting to show win screen...");
                gameManager.ShowWinScreen(); // Trigger the win screen
            }
        }
    }

    // Updates the quota UI text to show progress like "15 / 30"
    void UpdateQuotaText()
    {
        if (quotaText != null)
        {
            // Format the deposited value with no decimal places (e.g., 15 instead of 15.0)
            quotaText.text = $"{depositedValue:F0} / {quota}";
        }
        else
        {
            // Warn if the UI text wasn't assigned in the inspector
            Debug.LogWarning("QuotaText is not assigned!");
        }
    }

    // Called when another object enters the trigger zone
    void OnTriggerEnter2D(Collider2D other)
    {
        // If the object is the player, mark them as in range
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered extraction zone");
        }
    }

    // Called when another object exits the trigger zone
    void OnTriggerExit2D(Collider2D other)
    {
        // If the object is the player, mark them as out of range
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left extraction zone");
        }
    }
}
