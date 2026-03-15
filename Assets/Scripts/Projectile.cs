using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    public float velocity;
    public float deceleration;
    public int damage;

    Rigidbody rb;
    Transform t;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
    }

    public int GetDamage()
    {
        return damage;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDirection = new Vector3(0, 0, velocity);
        t.position += moveDirection;
        if(velocity > 0)
            velocity -= deceleration;
    }
}
