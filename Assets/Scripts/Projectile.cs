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
    [SerializeField] string name;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public int GetDamage()
    {
        return damage;
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
    public virtual void OnTriggerEnter(Collider collider)
    {
        Destroy(gameObject);
    }

}
