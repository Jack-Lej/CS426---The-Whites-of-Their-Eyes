using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerHitbox : MonoBehaviour
{
    [SerializeField] private int damagePerSecond = 20;
    [SerializeField] private ParticleSystem flameParticles;

    private Dictionary<GameObject, float> damageTimers = new Dictionary<GameObject, float>();
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void Start()
    {
        if (flameParticles == null)
            flameParticles = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Flamethrower collided with " + other.name);
        // Get collision events for more detail if needed
        ParticlePhysicsExtensions.GetCollisionEvents(flameParticles, other, collisionEvents);

        if (!other.CompareTag("Player") && !other.CompareTag("Enemy")) return;

        // Rate limit damage to once per second per target
        if (damageTimers.ContainsKey(other) && Time.time < damageTimers[other]) return;

        damageTimers[other] = Time.time + 1f; // Reset timer

        // Try HurtBox first, then Character directly
        HurtBox hurtBox = other.GetComponent<HurtBox>();
        if (hurtBox != null)
        {
            hurtBox.ReceiveDamage(damagePerSecond);
            Debug.Log("Flamethrower hit " + other.name + " for " + damagePerSecond + " damage via HurtBox.");
            return;
        }

        Character character = other.GetComponentInParent<Character>();
        if (character != null)
            character.TakeDamage(damagePerSecond);
    }

    // Clean up stale entries to prevent memory leak
    private void Update()
    {
        List<GameObject> toRemove = new List<GameObject>();
        foreach (var key in damageTimers.Keys)
        {
            if (key == null || Time.time > damageTimers[key] + 2f)
                toRemove.Add(key);
        }
        foreach (var key in toRemove)
            damageTimers.Remove(key);
    }
}