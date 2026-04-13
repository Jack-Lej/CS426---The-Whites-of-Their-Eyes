using UnityEngine;

public class Minigun : Weapon
{

    [SerializeField] GameObject firePoint1;
    [SerializeField] GameObject firePoint2;
    [SerializeField] GameObject firePoint3;
    [SerializeField] GameObject firePoint4;

    GameObject[] firePoints; 
    int firePointPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firePointPos = 0;
        currAmmo = reserveAmmo;
        magazineSize = reserveAmmo;
        firePoints = new[] {firePoint1, firePoint2, firePoint3, firePoint4};
    }

    public override string FireWeapon()
    {
        if(currAmmo <= 0)
        {
            audioSource.PlayOneShot(dryFireSound, 1);
        }
        if(currAmmo > 0)
        {
            audioSource.PlayOneShot(fireSound, 1);
            currAmmo--;
            GameObject p = Instantiate(projectile, firePoints[firePointPos].transform.position, firePoints[firePointPos].transform.rotation);
            p.GetComponent<Rigidbody>().AddForce(p.transform.forward * projectileVelocity);
            if(firePointPos < 3)
                firePointPos++;
            else
                firePointPos = 0;    
        }
        return string.Concat(weaponName, " ammo: ", currAmmo.ToString());
    }

    //Weapon doesn't reload, ReloadWeapon does nothing but still has to account for r being pressed
    public override string ReloadWeapon()
    {
        return string.Concat(weaponName, " ammo: ", currAmmo.ToString());
    }

    public override string GetWeaponText()
    {
        return string.Concat(weaponName, " ammo: ", currAmmo.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
