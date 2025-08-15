using UnityEngine;

public class MonsterTest : MonoBehaviour
{
    public float fixedY = 1f;

    void Update()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, fixedY, pos.z);
    }
}
