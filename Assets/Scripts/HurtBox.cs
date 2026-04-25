using UnityEngine;

public class HurtBox : MonoBehaviour
{
    internal Character character;
    private bool firstHit = false;

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
        
        if (!firstHit && character.gameObject.CompareTag("Enemy"))
        {
            firstHit = true;

            // Search the character's GameObject and all children for the SphereCollider
            SphereCollider[] colliders = character.GetComponentsInChildren<SphereCollider>();
            foreach (SphereCollider col in colliders)
            {
                if (col.isTrigger)
                {
                    col.radius *= 3f;
                    break; // Only resize the first trigger sphere found
                }
            }
        }
    }
}