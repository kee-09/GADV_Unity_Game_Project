using UnityEngine;
using TMPro;

public class ExtractionPoint : MonoBehaviour
{
    public float quota = 30f;
    private float depositedValue = 0f;
    private bool playerInRange = false;
    private bool hasWon = false;

    public TextMeshProUGUI quotaText;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found! Make sure the GameObject is named 'GameManager' and has the GameManager script.");
        }

        UpdateQuotaText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed");

            if (!playerInRange)
            {
                Debug.LogWarning("Player is NOT in range.");
                return;
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player GameObject not found! Make sure it's tagged 'Player'.");
                return;
            }

            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (inventory == null)
            {
                Debug.LogError("PlayerInventory component not found on Player!");
                return;
            }

            Debug.Log($"Depositing ${inventory.currentValue}");

            depositedValue += inventory.currentValue;
            inventory.currentWeight = 0f;
            inventory.currentValue = 0f;
            inventory.UpdateUI();
            UpdateQuotaText();

            Debug.Log($"DepositedValue: {depositedValue}, Quota: {quota}");

            if (!hasWon && depositedValue >= quota)
            {
                hasWon = true;
                Debug.Log("Quota met! Attempting to show win screen...");
                gameManager.ShowWinScreen();
            }
        }
    }

    void UpdateQuotaText()
    {
        if (quotaText != null)
        {
            quotaText.text = $"{depositedValue:F0} / {quota}";
        }
        else
        {
            Debug.LogWarning("QuotaText is not assigned!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered extraction zone");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left extraction zone");
        }
    }
}
