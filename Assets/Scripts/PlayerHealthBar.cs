using UnityEngine;
using UnityEngine.UI;

//Just duplicating the health bar code without the rotational update so it doesn't spin on the player's screen
public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    public void updateHealthBar(float currentVal, float maxVal)
    {
        slider.value = currentVal / maxVal;
        Debug.Log(slider.value + " " + currentVal + " " + maxVal);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
