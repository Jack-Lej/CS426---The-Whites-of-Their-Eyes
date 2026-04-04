using UnityEngine;

public class HurtBox : MonoBehaviour
{
    private Character character;

    void Start()
    {
        character = GetComponentInParent<Character>();
    }

    // For physics-based projectiles (Rigidbody + non-trigger collider)
    private void OnCollisionEnter(Collision collision)
    {
        // Handle player's Projectile
        Projectile projectile = collision.gameObject.GetComponent<Projectile>();
        if (projectile != null)
        {
            ReceiveDamage(projectile.GetDamage());
            return;
        }
    }

    // For trigger-based projectiles and hitboxes
    private void OnTriggerEnter(Collider other)
    {
        // Handle CustomBullet explosion overlap
        CustomBullet bullet = other.GetComponent<CustomBullet>();
        if (bullet != null)
        {
            ReceiveDamage(bullet.explosionDamage);
            return;
        }
    }

    public void ReceiveDamage(int damage)
    {
        if (character != null && character.currentHealth > 0)
            character.TakeDamage(damage);
    }
}