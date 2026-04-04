using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Character ownerCharacter;  // The character this hitbox belongs to
    public int damage = 10;
    public string hitReceiverTag;     // e.g. "Player" or "Enemy"
    public string attackAnimationName; // Optional, leave blank if no animation
    private float damageCooldown = 0f;
    private float timeBetweenHits = 1f;

    void Start()
    {
        // Only try to get animation length if an animation name was provided
        // and the owner actually has an Animator
        if (!string.IsNullOrEmpty(attackAnimationName))
        {
            Animator animator = ownerCharacter != null
                ? ownerCharacter.GetComponent<Animator>()
                : GetComponentInParent<Animator>(); // Fallback to parent

            if (animator != null)
            {
                foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
                {
                    if (clip.name == attackAnimationName)
                    {
                        timeBetweenHits = clip.length;
                        break;
                    }
                }
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
        if (!other.CompareTag(hitReceiverTag) || damageCooldown > 0f) return;

        // First check for a HurtBox — preferred path
        HurtBox hurtBox = other.GetComponent<HurtBox>();
        if (hurtBox != null)
        {
            Debug.Log("Dealt damage" + damage + " to " + other.name);
            hurtBox.ReceiveDamage(damage);
            damageCooldown = timeBetweenHits;
            return;
        }

        // Fallback: try to find Character directly on the hit object
        Character character = other.GetComponentInParent<Character>();
        if (character != null)
        {
            character.TakeDamage(damage);
            damageCooldown = timeBetweenHits;
        }
    }
}