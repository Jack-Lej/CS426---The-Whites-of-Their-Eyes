using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class Weapon : MonoBehaviour
{

    
    //Ammo currently read to be used, ammo "in the magazine"
    //Not serialized b/c it updates automatically, and to prevent adding an extra full clip to total ammo
    protected int currAmmo;

    [Header("Basic Info")]
    //Amount of reserve ammo available for reloading, includes the ammo used to load the weapon on game start
    //Important: we need to decide if we keep the reserve ammo for weapons that reload all at once (magazine-based)
    //Ex: A rifle. 30 magazine size, 300 reserve ammo. Fires 3 shots, reloads. Do they lose 3 reserve ammo, or 30? I.e., do they "keep" the partially-emptied magazine and refill it, or discard it entirely
    //Maybe keep track of magazines?
    [SerializeField] protected int reserveAmmo;

    //Amount of shots that can be fired before needing to reload
    [SerializeField] protected int magazineSize;

    //Reload time in milliseconds
    [SerializeField] protected int reloadTime;
    //How long it takes (in milliseconds) between each shot when holding down the fire button
    [SerializeField] protected int fireDelay;

    [SerializeField] protected string weaponName;
    //Weight of the weapon for the inventory system
    [SerializeField] protected float weaponWeight;
    //Weight per unit of ammo for the weapon
    [SerializeField] protected float ammoWeight;

    [SerializeField] protected int projectileVelocity;

    [SerializeField] CanvasGroup crosshair;

    [Header("External References")]

    [SerializeField] protected GameObject manager;
    [SerializeField] protected GameObject projectile;
    //Where projectiles spawn from on the weapon's model
    [SerializeField] protected GameObject firePoint;

    

    [Header("Sounds")]

    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip dryFireSound;
    [SerializeField] protected AudioClip reloadSound;
    [SerializeField] protected AudioClip fireSound;




    Vector3 sleepVector = new Vector3(0, 1000, 0);
    
    void Start()
    {
        currAmmo = magazineSize;
        reserveAmmo -= magazineSize;
    }
    public void SleepWeapon()
    {
        transform.position = sleepVector;
        crosshair.alpha = 0;
    }
    public string WakeWeapon()
    {
        transform.position = manager.transform.position;
        transform.rotation = manager.transform.rotation;
        crosshair.alpha = 1;
        return string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\n", "Reserve Ammo: ", reserveAmmo.ToString());
    }
    public virtual string FireWeapon()
    {
        if(currAmmo <= 0)
        {
            audioSource.PlayOneShot(dryFireSound, 1);
        }
        if(currAmmo > 0)
        {
            audioSource.PlayOneShot(fireSound, 1);
            currAmmo--;
            GameObject p = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
            p.GetComponent<Rigidbody>().AddForce(p.transform.forward * projectileVelocity);
        }
        return string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\n", "Reserve Ammo: ", reserveAmmo.ToString());
    }

    public virtual string ReloadWeapon()
    {
        audioSource.PlayOneShot(reloadSound, 1);
        //Does the weapon need reloading, and can it be reloaded?
        if(currAmmo < magazineSize && reserveAmmo > 0)
        {
            audioSource.PlayOneShot(reloadSound, 1);
            //Is there enough ammo to reload the entire capacity?
            if(reserveAmmo > (magazineSize-currAmmo))
            {
                reserveAmmo -= (magazineSize - currAmmo);
                currAmmo = magazineSize;
            }
            else
            {
                currAmmo += reserveAmmo;
                reserveAmmo = 0;
            }
            return "Reloading . . .";
        }
        //Otherwise just return status as usual
        return string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\nReserve Ammo: ", reserveAmmo.ToString());
    }

    //Returns the weight of the weapon and all its ammo
    public float GetWeaponWeight()
    {
        return (currAmmo+reserveAmmo)*ammoWeight + weaponWeight;
    }

    public virtual string GetWeaponText()
    {
        return string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\nReserve Ammo: ", reserveAmmo.ToString());
    }

    public string GetWeaponName()
        {return weaponName;}

    public virtual int GetWeaponFireDelay()
        {return fireDelay;}
    public virtual int GetReloadDelay()
        {return reloadTime;}   

    public void DropAmmo(int amount)
    {
        reserveAmmo -= amount;
        if(reserveAmmo < 0)
            reserveAmmo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
