using UnityEngine;
using TMPro;

public class ExtractionPoint : MonoBehaviour
{
    public float quota = 100f;
    private float depositedValue = 0f;
    private bool playerInRange = false;

    public TextMeshProUGUI quotaText;

    void Start()
    {
        UpdateQuotaText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed");

            if (playerInRange)
            {
                Debug.Log("Player is in range");

                PlayerInventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();

                Debug.Log("Depositing $" + inventory.currentValue);

                depositedValue += inventory.currentValue;
                inventory.currentWeight = 0f;
                inventory.currentValue = 0f;
                inventory.UpdateUI();
                UpdateQuotaText();

                if (depositedValue >= quota)
                {
                    Debug.Log("Quota met!");
                }
            }
        }
    }

    void UpdateQuotaText()
    {
        if (quotaText != null)
        {
            quotaText.text = $"{depositedValue:F0} / {quota}";
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
