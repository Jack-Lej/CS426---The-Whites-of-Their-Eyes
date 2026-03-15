using UnityEngine;

public class Target : MonoBehaviour
{

    public int health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile")
        {
            Debug.Log("Health: " + health);
            int damage = collision.gameObject.GetComponent<int>();
            health -= damage;

            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }


}
