using UnityEngine;

public class BubbleRise : MonoBehaviour
{
    void Update()
    {
        Vector3 position = transform.localPosition;
        position.y += 0.5f * Time.deltaTime;

        if (position.y >= 2.5f)
        {
            position.y = 0.1f;
        }

        transform.localPosition = position;
    }
}