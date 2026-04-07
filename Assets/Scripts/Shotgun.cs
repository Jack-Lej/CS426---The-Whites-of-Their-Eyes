using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class Shotgun : Weapon
{
    int numShellsToReload;
    DateTime lastTimeReloaded;

    //Spread (in degrees) of randomness for the pellets to move in
    [SerializeField] float spread;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currAmmo = magazineSize;
        reserveAmmo -= magazineSize;
        spread /= 360;
        lastTimeReloaded = DateTime.Now;
    }

    //Over-ridden because the shotgun fires 9 pellets at once
    public override string FireWeapon()
    {
        if(currAmmo == 0)
        {
            //Play dry-fire "click" sfx
        }
        else if(currAmmo > 0)
        {
            audioSource.PlayOneShot(fireSound, 1);
            currAmmo--;
            //Spawn the guaranteed center pellet
            Vector3 center = firePoint.transform.position;

            GameObject pCenter = Instantiate(projectile, center, firePoint.transform.rotation);
            pCenter.GetComponent<Rigidbody>().AddForce(pCenter.transform.forward * projectileVelocity * -1);
            
            //Position vectors for each of the 8 "random" pellets
            //If they all spawned on the same point, they would clip together & explode all over the place
            Vector3[] vecs = {new Vector3(0, -0.04f, -0.04f), new Vector3(0, -0.04f, 0), new Vector3(0, -0.04f, 0.04f),
            new Vector3(0, 0, -0.04f), new Vector3(0, 0, 0.04f),
            new Vector3(0, 0.04f, -0.04f), new Vector3(0, 0.04f, 0), new Vector3(0, 0.04f, 0.04f)};

            
            //Spawn the random 8 pellets within a [spread * 360]-degree cone, and each slightly away from the center
            //Vector3 rotation uses a 0-1 scale, so [spread] is a fraction of the actual degrees (provided by the serialized field) over 360
            for(int i = 0; i < 8; i++)
            {
                GameObject pRand = Instantiate(projectile, center+vecs[i], firePoint.transform.rotation);
                Vector3 rotat = new Vector3(UnityEngine.Random.Range(-spread, spread), UnityEngine.Random.Range(-spread, spread), UnityEngine.Random.Range(-spread, spread));
                pRand.GetComponent<Rigidbody>().AddForce((pRand.transform.forward + rotat) * projectileVelocity * -1);
            }
        }
        return string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\nReserve Ammo: ", reserveAmmo.ToString());
    }

    public override string ReloadWeapon()
    {
        //Does the weapon need reloading, and can it be reloaded?
        if(currAmmo < magazineSize && reserveAmmo > 0)
        {
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
            return string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\nReserve Ammo: ", reserveAmmo.ToString());
        }
        //Otherwise just return status as usual
        return string.Concat(weaponName, " ammo: ", currAmmo.ToString(), "/", magazineSize.ToString(), "\nReserve Ammo: ", reserveAmmo.ToString());
    }


    //Special reload delay depends on how many shells need to be reloaded
    public override int GetReloadDelay()
    {
        return reloadTime*(magazineSize-currAmmo);
    }

    // Update is called once per frame
    void Update()
    {
        //Used to repeatedly play reload sound; one per shell loaded
        if(numShellsToReload > 0 && lastTimeReloaded.AddMilliseconds(reloadTime).CompareTo(DateTime.Now) <= 0)
        {
            lastTimeReloaded = DateTime.Now;
            numShellsToReload--;
            audioSource.PlayOneShot(reloadSound, 1);
        }
    }
}
