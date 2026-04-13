using UnityEngine;

// YouTube Tutorial by Dave / GameDevelopment
// https://www.youtube.com/watch?v=0jGL5_DFIo8

public class CustomBullet : MonoBehaviour
{
    //Assignables
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask Explodable;

    //Stats
    [Range(0f,1f)]
    public float bounciness;
    public bool useGravity;

    //Damage
    public int explosionDamage;
    public int directHitDamage;
    public bool isExplosive = true;
    public float explosionRange;
    public float explosionForce;

    //Lifetime
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    // Check Explosion
    private bool hasExploded = false;

    int collisions;
    PhysicsMaterial physics_mat;

    private void Start()
    {
        Setup();
    }

    private void HandleImpact()
    {
        if (isExplosive)
        {
            Explode();
        }
        else
        {
            Invoke("Delay", 0.05f); // Just clean up, damage was already dealt in OnCollisionEnter
        }
    }

    private void Update()
    {
        //When to explode:
        if (collisions > maxCollisions) Explode();

        // If the bullet doesn't collide with anything, it will still explode after a certain amount of time
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Explode();
    }

    private void Explode()
    {
        // if (hasExploded) return; // Prevent multiple explosions
        //Instantiate explosion
        if (explosion != null && !hasExploded)
        {
            // prevent multiple explosion VFX from spawning
            Instantiate(explosion, transform.position, Quaternion.identity);
        } 

        hasExploded = true;

        //Check for characters within the explosion range
        Collider[] chatacters = Physics.OverlapSphere(transform.position, explosionRange, Explodable);
        for (int i = 0; i < chatacters.Length; i++)
        {
            HurtBox hurtBox = chatacters[i].GetComponent<HurtBox>();
            if (hurtBox != null)
            {
                hurtBox.ReceiveDamage(explosionDamage);
            } 
            else
            {
                Character character = chatacters[i].GetComponentInParent<Character>();
                if (character != null)
                {
                    character.TakeDamage(explosionDamage);
                }
            }

            //Add explosion force (if enemy has a rigidbody)
            if (chatacters[i].GetComponent<Rigidbody>())
                chatacters[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
        }

        //Add a little delay, just to make sure everything works fine
        Invoke("Delay", 0.05f);
    }
    private void Delay()
    {
        Destroy(gameObject);
    }

    private void DealDirectHitDamage(Collider other)
    {
        HurtBox hurtBox = other.GetComponent<HurtBox>();
        if (hurtBox != null)
        {
            hurtBox.ReceiveDamage(directHitDamage);
        }
        else
        {
            Character character = other.GetComponentInParent<Character>();
            if (character != null)
            {
                character.TakeDamage(directHitDamage);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Don't count collisions with other bullets
        if (collision.collider.CompareTag("Bullet")) return;

        //Count up collisions
        collisions++;

        if(!isExplosive)
        {
            DealDirectHitDamage(collision.collider);
            Invoke("Delay", 0.05f);
            return;
        }

        // //Explode if bullet hits an enemy directly and explodeOnTouch is activated
        // if (collision.collider.CompareTag("Enemy") && explodeOnTouch) Explode();

        //Explode if bullet hits an Player directly and explodeOnTouch is activated
        if (collision.collider.CompareTag("Player") && explodeOnTouch) Explode();
    }

    private void Setup()
    {
        //Create a new Physic material
        physics_mat = new PhysicsMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicsMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicsMaterialCombine.Maximum;
        //Assign material to collider
        GetComponent<Collider>().material = physics_mat;

        //Set gravity
        rb.useGravity = useGravity;
    }

    /// Used to visualize the explosion range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
