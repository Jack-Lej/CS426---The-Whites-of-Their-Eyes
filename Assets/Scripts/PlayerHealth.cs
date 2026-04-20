using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

//Only difference for child class is to include the health UI text and 
public class PlayerHealth : Character
{   
    [SerializeField] TMP_Text healthText;
    [SerializeField] BasicFPCC playerController;

    [SerializeField] PlayerHealthBar pHealthBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }
    public override void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return; // Already dead, ignore further damage

        currentHealth -= damage;
        onDamageTaken.Invoke(damage);
        Debug.Log(gameObject.name + " took " + damage + ". HP: " + currentHealth);
        healthText.text = string.Concat(currentHealth, "/", maxHealth);
        pHealthBar.updateHealthBar(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log(gameObject.name + " died");
            playerController.ToggleLockCursor();
            SceneManager.LoadScene("Death Screen");
        }
    }

    public override void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        healthText.text = string.Concat(currentHealth, "/", maxHealth);
        pHealthBar.updateHealthBar(currentHealth, maxHealth);
    }
    // Update is called once per frame
    void Update()
    {
 
    }
}
