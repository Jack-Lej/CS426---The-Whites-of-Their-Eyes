using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    private meleeEnemy enemy;
    private float damageCooldown = 0f;
    public float timeBetweenHits;
    public string attackAnimationName;

    void Start()
    {
        enemy = GetComponentInParent<meleeEnemy>();
        foreach(AnimationClip clip in enemy.GetComponent<Animator>().runtimeAnimatorController.animationClips)
        {
            if(clip.name == attackAnimationName)
            {
                timeBetweenHits = clip.length;
                break;
            }
        }
    }

    void Update()
    {
        if (damageCooldown > 0f)
            damageCooldown -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("EnemyHitBox: Enter!");
        // Debug.Log("Player hit for " + enemy.damage + " damage");
        if (other.CompareTag("Player") && damageCooldown <= 0f)
        {
            // enemy.OnHitboxContact(other);
            damageCooldown = timeBetweenHits; // Reset cooldown after each hit
            Debug.Log("Hit!");
        }

        // if(other.CompareTag("Projectile"))
        // {
        //     Projectile p = other.gameObject.GetComponent<Projectile>();
        //     enemy.health -= p.GetDamage();
        //     Debug.Log("Health: " + enemy.health);
        //     if(enemy.health <= 0)
        //     {
        //         Debug.Log("Enemy died");
        //         enemy.setDeathState();
        //         Destroy(gameObject);
        //     }
        // }
    }

    private void OnTriggerExit(Collider other)
    {
        // Debug.Log("EnemyHitBox: Exit!");
    }
}