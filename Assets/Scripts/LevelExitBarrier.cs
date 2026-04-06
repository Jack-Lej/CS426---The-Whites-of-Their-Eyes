using UnityEngine;

public class LevelExitBarrier : MonoBehaviour
{
    [SerializeField] WeaponManager manager;
    [SerializeField] float weightLimit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.GetTotalWeight() <= weightLimit)
            Destroy(gameObject);
    }
}
