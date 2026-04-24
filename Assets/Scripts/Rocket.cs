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
        audioSource.clip = explodeSound;
    }

    Collider[] colliders = new Collider[100];
    [SerializeField] LayerMask layerMask;

    void ExplodeNonAlloc()
    {
        Debug.Log("Explosion at " + transform.position);
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, colliders, layerMask);
        Debug.Log("Num colliders:" + numColliders);
        if (numColliders > 0)
        {
            
            for (int i = 0; i < numColliders; i++)
            {
                HurtBox hurtBox = colliders[i].GetComponent<HurtBox>();
                float vec = Vector3.Distance(this.transform.position, colliders[i].transform.position);
                float explosionDamageCalc = explosionDamage*((trueExplosionRadius-vec)/trueExplosionRadius); 
                if(vec <= trueExplosionRadius)
                {
                    if (hurtBox != null)
                    {
                        hurtBox.ReceiveDamage((int)explosionDamageCalc);
                    } 
                    else
                    {
                        Character character = colliders[i].GetComponentInParent<Character>();
                        if (character != null)
                        {
                            character.TakeDamage((int)explosionDamageCalc);
                        }
                    }
                }
                
            }
        }

    }

    public override void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Weapon" && collision.gameObject.tag != "Projectile")
        {
            AudioSource.PlayClipAtPoint(explodeSound, transform.position);
            ExplodeNonAlloc();
            Instantiate(explosionGraphic, transform.position, Quaternion.identity);
            if(collision.gameObject.tag == "Enemy")
            {
                DealDamage(collision.collider);
            }
            Destroy(gameObject);
            //audioSource.PlayOneShot(explodeSound);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
