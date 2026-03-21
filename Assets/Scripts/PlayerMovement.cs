using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// adding namespaces
// because we are using the NetworkBehaviour class
// NewtorkBehaviour class is a part of the Unity.Netcode namespace
// extension of MonoBehaviour that has functions related to multiplayer
public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;
    public float rotationSpeed = 90;
    // create a list of colors
    public List<Color> colors = new List<Color>();

    // getting the reference to the prefab
    [SerializeField]
    private GameObject spawnedPrefab;
    // save the instantiated prefab
    private GameObject instantiatedPrefab;

    public GameObject cannon;
    public GameObject bullet;

    Transform t;

    // reference to the camera audio listener
    [SerializeField] private AudioListener audioListener;
    // reference to the camera
    [SerializeField] private Camera playerCamera;


    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
    }
    // Update is called once per frame
    void Update()
    {

        Vector3 moveDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection.x = -1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection.x = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection.z = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection.z = 1f;
        }
        transform.position += moveDirection * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Q))
            t.rotation *= Quaternion.Euler(0, -rotationSpeed * Time.deltaTime, 0);
        else if (Input.GetKey(KeyCode.E))
            t.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
    }        
       
}