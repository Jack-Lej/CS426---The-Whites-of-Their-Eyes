using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerProjectile : Projectile
{
   
    public override void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Weapon" && collision.gameObject.tag != "Projectile" && collision.gameObject.tag != "Activation Sphere" && collision.gameObject.tag != "Player")
            // Deals damage to the collided hurbox before destroying the projectile
            DealDamage(collision.collider);
            // Debug.Log(name + " collided with " + collision.gameObject.name);
            Destroy(gameObject);
    }
    // public virtual void OnTriggerEnter(Collider collider)
    // {
    //     if(collider.gameObject.tag != "Weapon" && collider.gameObject.tag != "Projectile")
    //         // Debug.Log(name + " triggered with " + collider.gameObject.name);
    //         Destroy(gameObject);
    // }

}
