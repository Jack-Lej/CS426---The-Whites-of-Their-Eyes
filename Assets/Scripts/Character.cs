using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public UnityEvent onDeath;
    private float playSoundInterval = 2f; // Minimum time between damage sounds
    private float lastDamageSoundTime = -Mathf.Infinity; // Initialize to negative infinity
    public UnityEvent<int> onDamageTaken;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] damageClips;

    [SerializeField] protected HealthBar healthBar;

    void Awake()
    {
        if (healthBar == null)
        {
            healthBar = GetComponent<HealthBar>();
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.updateHealthBar(currentHealth, maxHealth);
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    // Plays a random damage sound from the array, with rate limiting to prevent spamming
    private void PlayRandomDamageSound()
    {
        if (Time.time - lastDamageSoundTime < playSoundInterval) return;

        if (damageClips == null || damageClips.Length == 0) return;
        lastDamageSoundTime = Time.time;
        if(audioSource == null) {
            Debug.LogWarning("AudioSource component missing on " + gameObject.name);
            return;
        }

        AudioClip clip = damageClips[Random.Range(0, damageClips.Length)];
        audioSource.PlayOneShot(clip);
    }

    public virtual void TakeDamage(int damage)
    {
        if(damageClips != null && damageClips.Length > 0)
            PlayRandomDamageSound();
        if (currentHealth <= 0) return; // Already dead, ignore further damage
        currentHealth -= damage;
        onDamageTaken.Invoke(damage);
        healthBar.updateHealthBar(currentHealth, maxHealth);
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
        if (healthBar != null)
        {
            healthBar.updateHealthBar(currentHealth, maxHealth);
        }
    }
}