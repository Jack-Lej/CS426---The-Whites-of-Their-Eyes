using UnityEngine;
using TMPro;

public class Credits : MonoBehaviour
{
   public float speed = 50f; // Units per second

    void Update()
    {
        // Moves the object upward relative to its current position
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
