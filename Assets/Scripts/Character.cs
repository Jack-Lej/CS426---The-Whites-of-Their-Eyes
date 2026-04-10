using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public UnityEvent onDeath;
    public UnityEvent<int> onDamageTaken;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // Already dead, ignore further damage
        currentHealth -= damage;
        onDamageTaken.Invoke(damage);
        Debug.Log(gameObject.name + " took " + damage + ". HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log(gameObject.name + " died");
            onDeath.Invoke();
        }
    }

    public virtual void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            Debug.Log(gameObject.name + " collided.");
            Projectile p = collision.gameObject.GetComponent<Projectile>();
            this.TakeDamage(p.GetDamage());
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            Debug.Log(gameObject.name + " triggered.");
            Projectile p = collision.gameObject.GetComponent<Projectile>();
            this.TakeDamage(p.GetDamage());
        }
    } */
}