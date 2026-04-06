using UnityEngine;
//Simple class that is used to test player health
public class DamageBox : MonoBehaviour
{

    [SerializeField] int damage;

    public int GetDamage()
    {
        return damage;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
