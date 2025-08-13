using UnityEngine;

public class ItemValuable : MonoBehaviour
{
    public string itemName;

    public float minWeight;  // minimum possible weight
    public float maxWeight;  // maximum possible weight

    public float minValue;   // minimum possible price
    public float maxValue;   // maximum possible price

    [HideInInspector]
    public float currentWeight;  // actual weight assigned at spawn

    [HideInInspector]
    public float currentValue;   // actual price assigned at spawn

    // Call this to randomly set currentWeight and currentValue
    public void GenerateRandomStats()
    {
        currentWeight = Random.Range(minWeight, maxWeight);
        currentValue = Random.Range(minValue, maxValue);
    }
}
