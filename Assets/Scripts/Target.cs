using UnityEngine;
using TMPro;

public class Target : MonoBehaviour
{
    [SerializeField] TMP_Text healthText;
    public int health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthText.text = string.Concat("Heatlh: ", health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            Projectile p = collision.gameObject.GetComponent<Projectile>();
            health -= p.GetDamage();
            Debug.Log(string.Concat("Heatlh: ", health));
            healthText.text = string.Concat("Heatlh: ", health);
            if(health <= 0)
            {
                Destroy(gameObject);
            }
            
        }
    }


}
