/*
Code From YouTube Video : Easy Enemy Health Bar in Unity
By: BMo
https://www.youtube.com/watch?v=_lREXfAMUcE
*/

using UnityEngine;
using UnityEngine.UI;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    private new Camera camera;
    public void updateHealthBar(float currentVal, float maxVal)
    {
        slider.value = currentVal / maxVal;
    }

    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = camera.transform.rotation;
        transform.position = target.position + offset;
    }
}
