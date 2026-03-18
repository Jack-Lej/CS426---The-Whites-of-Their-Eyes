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
    [SerializeField] protected double reloadTime;
    //How long it takes (in milliseconds) between each shot when holding down the fire button
    [SerializeField] protected double fireDelay;

    //Simple check for reloading; disable firing during reload
    protected bool reloading;


    [SerializeField] protected GameObject projectile;
    [SerializeField] protected TMP_Text ammoText;
    //Where projectiles spawn from
    [SerializeField] protected GameObject firePoint;

    [SerializeField] protected string weaponName;
    //Weight of the weapon for the inventory system
    [SerializeField] protected float weaponWeight;
    //Weight per unit of ammo for the weapon
    [SerializeField] protected float ammoWeight;

    protected DateTime lastShot;
    protected DateTime lastShotEnd;
    protected DateTime reloadStart;
    protected DateTime reloadEnd;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currAmmo = magazineSize;
        reserveAmmo -= magazineSize;
        reloading = false;
        ammoText.text = string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\n", "Reserve Ammo: ", reserveAmmo.ToString());
    }

    void FireWeapon()
    {
        if(currAmmo <= 0)
        {
            //Play dry-fire "click" sfx
            return;
        }
        if(currAmmo > 0 && !reloading && lastShotEnd.CompareTo(DateTime.Now) <= 0)
        {
            currAmmo--;
            lastShot = DateTime.Now;
            lastShotEnd = lastShot.AddMilliseconds(fireDelay);
            GameObject p = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
            p.GetComponent<Rigidbody>().AddForce(p.transform.forward * 100);
            ammoText.text = string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\n", "Reserve Ammo: ", reserveAmmo.ToString());
        }
    }

    void ReloadWeapon()
    {
        if(reserveAmmo > 0 && currAmmo < magazineSize && !reloading)
        {
            reloading = true;
            reloadStart = DateTime.Now;
            reloadEnd = reloadStart.AddMilliseconds(reloadTime);
            ammoText.text = "Reloading . . .";
            //In case the reserve ammo is less than the magazine size
            if(reserveAmmo < magazineSize)
            {
                currAmmo += reserveAmmo;
                reserveAmmo = 0;
            }
            else
            {
                reserveAmmo -= (magazineSize - currAmmo);
                currAmmo = magazineSize;
            }
        }
    }

    //Returns the weight of the weapon and all its ammo
    float GetWeaponWeight()
    {
        return (currAmmo+reserveAmmo)*ammoWeight + weaponWeight;
    }

    // Update is called once per frame
    void Update()
    {
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
        }
    }
}
