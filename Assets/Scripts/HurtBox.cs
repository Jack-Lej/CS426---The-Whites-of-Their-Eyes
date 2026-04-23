using UnityEngine;

public class HurtBox : MonoBehaviour
{
    internal Character character;

    void Start()
    {
        character = GetComponentInParent<Character>();
    }

    /*
    // For physics-based projectiles (Rigidbody + non-trigger collider)
    private void OnCollisionEnter(Collision collision)
    {

        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        CustomBullet customBullet = collision.gameObject.GetComponent<CustomBullet>();
        if (customBullet != null)
        {
            if(!customBullet.isExplosive) {
                ReceiveDamage(customBullet.directHitDamage);
            }
            
            return;
        }
        if (projectile != null)
        {
            ReceiveDamage(projectile.GetDamage());
            return;
        }
        Debug.Log("Collided with " + collision.gameObject.name);
    } */

    // For trigger-based projectiles and hitboxes
    private void OnTriggerEnter(Collider other)
    {

        CustomBullet customBullet = other.GetComponent<CustomBullet>();
        Projectile projectile = other.GetComponent<Projectile>();
        if (projectile != null)
        {
            ReceiveDamage(projectile.GetDamage());
            return;
        }
        if (customBullet != null)
        {
            if(!customBullet.isExplosive) {
                ReceiveDamage(customBullet.directHitDamage);
            }
            
            return;
        }
    }

    public void ReceiveDamage(int damage)
    {
        if (character != null && character.currentHealth > 0)
            character.TakeDamage(damage);
    }
}