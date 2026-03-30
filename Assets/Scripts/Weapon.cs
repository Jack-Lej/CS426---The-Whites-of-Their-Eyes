using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class Weapon : MonoBehaviour
{

    //Amount of shots that can be fired before needing to reload
    [SerializeField] protected int magazineSize;
    //Ammo currently read to be used, ammo "in the magazine"
    [SerializeField] protected int currAmmo;

    //Amount of reserve ammo available for reloading, includes the ammo used to load the weapon on game start
    //Important: we need to decide if we keep the reserve ammo for weapons that reload all at once (magazine-based)
    //Ex: A rifle. 30 magazine size, 300 reserve ammo. Fires 3 shots, reloads. Do they lose 3 reserve ammo, or 30? I.e., do they "keep" the partially-emptied magazine and refill it, or discard it entirely
    //Maybe keep track of magazines?
    [SerializeField] protected int reserveAmmo;
    //Reload time in milliseconds
    [SerializeField] protected int reloadTime;
    //How long it takes (in milliseconds) between each shot when holding down the fire button
    [SerializeField] protected int fireDelay;

    //Simple check for reloading; disable firing during reload

    [SerializeField] protected int projectileVelocity;

    [SerializeField] protected GameObject manager;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected TMP_Text ammoText;
    //Where projectiles spawn from on the weapon's model
    [SerializeField] protected GameObject firePoint;

    [SerializeField] protected string weaponName;
    //Weight of the weapon for the inventory system
    [SerializeField] protected float weaponWeight;
    //Weight per unit of ammo for the weapon
    [SerializeField] protected float ammoWeight;

    Transform t;
    Vector3 sleepVector = new Vector3(0, 1000, 0);
    Vector3 readyPosition = new Vector3(0.5f, 0, 1);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currAmmo = magazineSize;
        reserveAmmo -= magazineSize;
    }
    public void SleepWeapon()
    {
        transform.position = sleepVector;
    }
    public string WakeWeapon()
    {
        transform.position = manager.transform.position;
        transform.rotation = manager.transform.rotation;
        return string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\n", "Reserve Ammo: ", reserveAmmo.ToString());
    }
    public virtual string FireWeapon()
    {
        if(currAmmo <= 0)
        {
            //Play dry-fire "click" sfx
        }
        if(currAmmo > 0)
        {
            currAmmo--;
            GameObject p = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
            p.GetComponent<Rigidbody>().AddForce(p.transform.forward * projectileVelocity);
        }
        return string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\n", "Reserve Ammo: ", reserveAmmo.ToString());
    }

    public virtual string ReloadWeapon()
    {
        //Does the weapon need reloading, and can it be reloaded?
        if(currAmmo < magazineSize && reserveAmmo > 0)
        {
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
    float GetWeaponWeight()
    {
        return (currAmmo+reserveAmmo)*ammoWeight + weaponWeight;
    }

    public string GetWeaponText()
    {
        return string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\nReserve Ammo: ", reserveAmmo.ToString());
    }

    public virtual int GetWeaponFireDelay()
        {return fireDelay;}
    public virtual int GetReloadDelay()
        {return reloadTime;}   

    public void RemoveAmmo(int amount)
    {
        reserveAmmo -= amount;
        if(reserveAmmo < 0)
            reserveAmmo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetButtonDown("Fire1"))
        {
            FireWeapon();
        }
        if(Input.GetButtonDown("Reload"))
        {
            ReloadWeapon();
        }
        if(reloading && reloadEnd.CompareTo(DateTime.Now) <= 0)
        {
            reloading = false;
            ammoText.text = string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\n", "Reserve Ammo: ", reserveAmmo.ToString());
        } */
    }
}
