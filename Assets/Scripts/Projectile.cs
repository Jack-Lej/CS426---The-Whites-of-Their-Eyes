using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    public int damage;
    [SerializeField] string name;

    protected void DealDamage(Collider other)
    {
        HurtBox hurtBox = other.GetComponent<HurtBox>();
        if (hurtBox != null)
        {
            hurtBox.ReceiveDamage(damage);
        }
        else
        {
            Character character = other.GetComponentInParent<Character>();
            if (character != null)
            {
                character.TakeDamage(damage);
            }
        }
    }

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
        if(collision.gameObject.tag != "Weapon" && collision.gameObject.tag != "Projectile" && collision.gameObject.tag != "Activation Sphere")
        {
            // Deals damage to the collided hurbox before destroying the projectile
            DealDamage(collision.collider);
            // Debug.Log(name + " collided with " + collision.gameObject.name);
            Destroy(gameObject);
        }
    }
    // public virtual void OnTriggerEnter(Collider collider)
    // {
    //     if(collider.gameObject.tag != "Weapon" && collider.gameObject.tag != "Projectile")
    //         // Debug.Log(name + " triggered with " + collider.gameObject.name);
    //         Destroy(gameObject);
    // }

}
