using UnityEngine;
using System.Collections;

public class Rocket : Projectile
{
    [Header("Damage Numbers")]
    [SerializeField] int explosionDamage;
    [SerializeField] int explosionRadius;

    [SerializeField] protected AudioClip explodeSound;
    [SerializeField] protected AudioSource audioSource;

    public GameObject explosionGraphic;

    float trueExplosionRadius;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trueExplosionRadius = explosionRadius * Mathf.PI;
    }

    Collider[] colliders = new Collider[100];
    [SerializeField] LayerMask layerMask;

    void ExplodeNonAlloc()
    {
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, colliders, layerMask);
        if (numColliders > 0)
        {
            for (int i = 0; i < numColliders; i++)
            {
                Collider col = colliders[i];
                Debug.Log("Collider " + i + ": " + col);
                if(colliders[i].gameObject.tag == "Enemy")
                {
                    Character c = colliders[i].gameObject.GetComponent<Character>();
                    float vec = Vector3.Distance(this.transform.position, col.transform.position);
                    //Explosion damage is determined by distance from the explosion
                    if(vec <= trueExplosionRadius)
                        c.TakeDamage((int) Mathf.Round(explosionDamage * ((trueExplosionRadius-vec)/trueExplosionRadius)));
                }
            }
        }

    }

    public override void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Weapon" && collision.gameObject.tag != "Projectile")
        {
            ExplodeNonAlloc();
            Instantiate(explosionGraphic, transform.position, Quaternion.identity);
            audioSource.PlayOneShot(explodeSound, 1);
            DealDamage(collision.collider);
            //Spawn explosion graphic
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
