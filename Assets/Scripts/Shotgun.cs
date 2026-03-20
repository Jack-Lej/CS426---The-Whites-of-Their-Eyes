using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class Shotgun : Weapon
{


    bool interruptReload;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interruptReload = false;
        currAmmo = magazineSize;
        reserveAmmo -= magazineSize;
        ammoText.text = string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\nReserve Ammo: ", reserveAmmo.ToString());
    }

    //Over-ridden because the shotgun fires 10 pellets at once
    void FireWeapon()
    {
        if(currAmmo <= 0)
        {
            //Play dry-fire "click" sfx
            return;
        }
        if(reloading)
        {
            interruptReload = true;
        }
        if(currAmmo > 0 && !reloading && lastShotEnd.CompareTo(DateTime.Now) <= 0)
        {
            currAmmo--;
            lastShot = DateTime.Now;
            lastShotEnd = lastShot.AddMilliseconds(fireDelay);
            GameObject pCenter = Instantiate(projectile, firePoint.transform.position, firePoint.transform.rotation);
            pCenter.GetComponent<Rigidbody>().AddForce(pCenter.transform.forward * 200);
            ammoText.text = string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\nReserve Ammo: ", reserveAmmo.ToString());
        }
    }

    //Since the shotgun is reloaded one shell at a time, the player can interrupt reloading to fire
    void ReloadWeapon()
    {
        if(reserveAmmo > 0 && currAmmo < magazineSize && !reloading)
        {
            reloading = true;
            interruptReload = false;
            reloadStart = DateTime.Now;
            reloadEnd = reloadStart.AddMilliseconds(reloadTime);
            currAmmo++;
            reserveAmmo--;
            ammoText.text = string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\nReserve Ammo:", reserveAmmo.ToString());
            
        }
        else
        {
            reloading = false;
        }
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
            if(interruptReload)
            {
                
                FireWeapon();
            }
            else
            {
                ReloadWeapon();
            }
        }
    }
}
