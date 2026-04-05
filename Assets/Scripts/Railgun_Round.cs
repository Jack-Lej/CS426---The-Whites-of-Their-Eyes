using UnityEngine;

public class Railgun_Round : Projectile
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    //Do nothing on collision, only check on triggers
    public override void OnCollisionEnter(Collision collision)
    {
        
    }
    //Destroys self if hits non-enemy, otherwise divide damage in half.
    public void OnTriggerEnter(Collider collision)
    {
        
        if(collision.gameObject.tag != "Enemy")
            Destroy(gameObject);

        damage /= 2;    
    }

}
