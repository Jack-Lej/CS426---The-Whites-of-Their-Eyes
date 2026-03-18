using UnityEngine;

// Timer to destroy VFX after a certain amount of time
// This is used to delte VFX of the bullet collision to simulate a short, small explosion
public class VFX_timeout : MonoBehaviour
{
    public float duration = 1.5f;

    void Start()
    {
        Destroy(gameObject, duration);
    }


}
