using System.Collections.Generic;
using UnityEngine;

public class ParabolicBullet : MonoBehaviour
{
    // Assignables
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask Explodable;

    // Stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    // Damage
    public int explosionDamage;
    public int directHitDamage;
    public bool isExplosive = true;
    public float explosionRange;
    public float explosionForce;

    // Lifetime
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    // Parabolic movement
    [Header("Parabolic Movement")]
    public bool useParabolicMovement = false;
    public float bulletSpeed = 30f;
    public float bulletGravity = 10f;

    private Vector3 startPosition;
    private Vector3 startForward;
    private float startTime = -1f;

    private bool hasExploded = false;
    private int collisions;
    private PhysicsMaterial physics_mat;
    private Collider[] ownColliders;
    private bool isTrigger = false;

    public void InitializeParabola(Vector3 origin, Vector3 direction)
    {
        startPosition = origin;
        startForward = direction.normalized;
        startTime = Time.time;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void Start()
    {
        Setup();
        ownColliders = GetComponentsInChildren<Collider>();

        if (useParabolicMovement)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

    }



    private Vector3 FindPointOnParabola(float time)
    {
        Vector3 point = startPosition + (startForward * bulletSpeed * time);
        Vector3 gravityVec = Vector3.down * bulletGravity * time * time;
        return point + gravityVec;
    }

    private bool CastRayBetweenPoints(Vector3 from, Vector3 to, out RaycastHit hit)
    {
        Vector3 direction = to - from;
        RaycastHit[] hits = Physics.RaycastAll(from, direction, direction.magnitude);

        foreach (RaycastHit h in hits)
        {
            // Skip any collider that belongs to this bullet
            bool isOwnCollider = false;
            foreach (Collider col in ownColliders)
            {
                if (h.collider == col)
                {
                    isOwnCollider = true;
                    break;
                }
            }

            if (!isOwnCollider)
            {
                hit = h;
                return true;
            }
        }

        hit = default;
        return false;
    }

    private void onTriggerEnter(Collider other)
    {
        isTrigger = true;
    }

    private void onCollisionEnter(Collision collision)
    {
        isTrigger = false;
    }


    private void FixedUpdate()
    {
        if (!useParabolicMovement || startTime < 0) return;

        float currentTime = Time.time - startTime;
        float prevTime = currentTime - Time.fixedDeltaTime;
        float nextTime = currentTime + Time.fixedDeltaTime;

        Vector3 currentPoint = FindPointOnParabola(currentTime);
        Vector3 nextPoint = FindPointOnParabola(nextTime);

        // Check collision between previous and current point
        if (prevTime > 0)
        {
            Vector3 prevPoint = FindPointOnParabola(prevTime);
            if (CastRayBetweenPoints(prevPoint, currentPoint, out RaycastHit hit) )
            {
                HandleParabolicHit(hit);
                return;
            }
        }

        // Check collision between current and next point
        if (CastRayBetweenPoints(currentPoint, nextPoint, out RaycastHit nextHit))
        {
            HandleParabolicHit(nextHit);
            return;
        }
    }

    private void HandleParabolicHit(RaycastHit hit)
    {
        if (hasExploded) return;

        if (!isExplosive)
        {
            if(isTrigger) return;

            Debug.Log("ParabolicBullet: Direct hit on " + hit.collider.name);
            DealDirectHitDamage(hit.collider);
            Invoke("Delay", 0.05f);
            Destroy(gameObject);
                
            return;
        }

        // Check explodeOnTouch for player tag
        if (hit.collider.CompareTag("Player") && explodeOnTouch)
        {
            Explode();
            return;
        }

        // Explode on any other surface
        Explode();
    }

    private void Update()
    {
        if (useParabolicMovement && startTime >= 0)
        {
            float currentTime = Time.time - startTime;
            transform.position = FindPointOnParabola(currentTime);

            // Face direction of travel
            float nextTime = currentTime + 0.01f;
            Vector3 nextPoint = FindPointOnParabola(nextTime);
            Vector3 direction = nextPoint - transform.position;
            
            if (direction != Vector3.zero) {
                
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                // Rotates 90 degrees so that the bullet model is facing the direction it is moving
                Quaternion meshOffset = Quaternion.Euler(90, 0, 0);
                transform.rotation = lookRotation * meshOffset;; 
            }
            // Only count down lifetime after bullet is initialized
            maxLifetime -= Time.deltaTime;
            if (maxLifetime <= 0) Explode();
        }
        else if (!useParabolicMovement)
        {
            // Non-parabolic bullets count down normally
            maxLifetime -= Time.deltaTime;
            if (maxLifetime <= 0) Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        if (explosion != null)
            Instantiate(explosion, transform.position, Quaternion.identity);

        HashSet<Character> damagedCharacters = new HashSet<Character>();

        Collider[] characters = Physics.OverlapSphere(transform.position, explosionRange, Explodable);
        for (int i = 0; i < characters.Length; i++)
        {
            HurtBox hurtBox = characters[i].GetComponent<HurtBox>();
            Character character = hurtBox != null
                ? hurtBox.character
                : characters[i].GetComponentInParent<Character>();

            if (character != null && damagedCharacters.Add(character))
                character.TakeDamage(explosionDamage);

            Rigidbody crb = characters[i].GetComponent<Rigidbody>();
            if (crb != null)
                crb.AddExplosionForce(explosionForce, transform.position, explosionRange);
        }

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
            return;
        }
        Character character = other.GetComponentInParent<Character>();
        if (character != null)
            character.TakeDamage(directHitDamage);
    }

    // private void OnCollisionEnter(Collision collision)
    // {
    //     if (useParabolicMovement) return; // Parabola uses raycasts instead
    //     if (collision.collider.CompareTag("Bullet")) return;

    //     collisions++;

    //     if (!isExplosive)
    //     {
    //         DealDirectHitDamage(collision.collider);
    //         Invoke("Delay", 0.05f);
    //         return;
    //     }

    //     if (collision.collider.CompareTag("Player") && explodeOnTouch) Explode();
    // }

    // private void onTriggerEnter(Collider other)
    // {
    //     if (useParabolicMovement) return; // Parabola uses raycasts instead
    //     if (other.CompareTag("Bullet")) return;

    //     collisions++;

    //     if (!isExplosive)
    //     {
    //         DealDirectHitDamage(other);
    //         Invoke("Delay", 0.05f);
    //         return;
    //     }

    //     if (other.CompareTag("Player") && explodeOnTouch) Explode();
    // }

    private void Setup()
    {
        physics_mat = new PhysicsMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicsMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicsMaterialCombine.Maximum;
        GetComponent<Collider>().material = physics_mat;
        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}