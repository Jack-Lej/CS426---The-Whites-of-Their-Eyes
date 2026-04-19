using UnityEngine;

public class SphereofDoom : Projectile
{

    [SerializeField] int numBounces;
    [SerializeField] Rigidbody rb;
    [SerializeField] int speed;
    int startNumBounces;
    Color color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startNumBounces = numBounces;
    }
    public override void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Weapon" && collision.gameObject.tag != "Projectile" && collision.gameObject.tag != "Player")
        {
            // Deals damage to the collided hurbox before destroying the projectile
            DealDamage(collision.collider);
            if(collision.gameObject.tag == "Enemy")
                numBounces -= 2;
            else
                numBounces--;    
            Debug.Log("Num bounces left: " + numBounces);
            // Debug.Log(name + " collided with " + collision.gameObject.name);
            if(numBounces <= 0)
            {
                Destroy(gameObject);
            }
        }
        rb.linearVelocity = (Vector3.Normalize(rb.linearVelocity) * speed);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
