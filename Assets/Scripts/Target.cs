using UnityEngine;
using TMPro;

public class Target : MonoBehaviour
{
    public int health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
            Debug.Log(string.Concat("Target health: ", health));
            if(health <= 0)
            {
                Destroy(gameObject);
            }
            
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            Projectile p = collision.gameObject.GetComponent<Projectile>();
            health -= p.GetDamage();
            Debug.Log(string.Concat("Heatlh: ", health));
            if(health <= 0)
            {
                Destroy(gameObject);
            }
            
        }
    }


}
