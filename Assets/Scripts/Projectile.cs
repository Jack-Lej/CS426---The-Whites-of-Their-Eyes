using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    public float velocity;
    //Not necessary now, maybe not in future
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

    public void OnCollisionEnter()
    {
        Destroy(gameObject);
    }


    void Update()
    {

    }
}
