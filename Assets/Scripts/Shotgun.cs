using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class Shotgun : Weapon
{

    [SerializeField] float spread;
    bool interruptReload;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interruptReload = false;
        currAmmo = magazineSize;
        reserveAmmo -= magazineSize;
        spread /= 360;
    }

    //Over-ridden because the shotgun fires 9 pellets at once
    string FireWeapon()
    {
        if(currAmmo == 0)
        {
            //Play dry-fire "click" sfx
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
            //Spawn the guaranteed center pellet
            Vector3 center = firePoint.transform.position;

            GameObject pCenter = Instantiate(projectile, center, firePoint.transform.rotation);
            pCenter.GetComponent<Rigidbody>().AddForce(pCenter.transform.forward * projectileVelocity);
            
            //Position vectors for each of the 8 "random" pellets
            //If they all spawned on the same point, they would clip together & explode all over the place
            Vector3[] vecs = {new Vector3(0, -0.05f, -0.05f), new Vector3(0, -0.05f, 0), new Vector3(0, -0.05f, 0.05f),
            new Vector3(0, 0, -0.05f), new Vector3(0, 0, 0.05f),
            new Vector3(0, 0.05f, -0.05f), new Vector3(0, 0.05f, 0), new Vector3(0, 0.05f, 0.05f)};

            
            //Spawn the random 8 pellets within a [spread * 360]-degree cone, and each slightly away from the center
            //Vector3 rotation uses a 0-1 scale, so [spread] is a fraction of the actual degrees over 360
            for(int i = 0; i < 8; i++)
            {
                GameObject pRand = Instantiate(projectile, center+vecs[i], firePoint.transform.rotation);
                Vector3 rotat = new Vector3(0, UnityEngine.Random.Range(-spread, spread), UnityEngine.Random.Range(-spread, spread));
                pRand.GetComponent<Rigidbody>().AddForce((pRand.transform.forward + rotat) * projectileVelocity);
            }
        }
        return string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\nReserve Ammo: ", reserveAmmo.ToString());
    }

    //Since the shotgun is reloaded one shell at a time, the player can interrupt reloading to fire
    string ReloadWeapon()
    {
        if(reserveAmmo > 0 && currAmmo < magazineSize && !reloading)
        {
            reloading = true;
            interruptReload = false;
            reloadStart = DateTime.Now;
            reloadEnd = reloadStart.AddMilliseconds(reloadTime);
            currAmmo++;
            reserveAmmo--;
            
        }
        else
        {
            reloading = false;
        }
        return string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\nReserve Ammo: ", reserveAmmo.ToString());
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
            if(interruptReload)
            {
                
                FireWeapon();
            }
            else
            {
                ReloadWeapon();
            }
        } */
    }
}
