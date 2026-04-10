using UnityEngine;

public class SphereofDoom : Projectile
{

    [Serializable] int numBounces;
    int startNumBounces;
    [Serializable] Material mat;
    Color color;
    float alpha;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        alpha = 1f;
        startNumBounces = numBounces;
    }
    public override void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Weapon" && collision.gameObject.tag != "Projectile")
        {
            // Deals damage to the collided hurbox before destroying the projectile
            DealDamage(collision.collider);
            if(collision.GameObject.tag == "Enemy")
                numBounces -= 2;
            else
                numBounces--;    

            alpha = numBounces/startNumBounces;
            color = new Color(color.r, color.g, color.b, alpha);
            mat.SetColor("_Color", color); 
            // Debug.Log(name + " collided with " + collision.gameObject.name);
            if(numBounces <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
