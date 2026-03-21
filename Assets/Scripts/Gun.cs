using UnityEngine;
using TMPro;

// YouTube Tutorial by Dave / GameDevelopment
// https://www.youtube.com/watch?v=wZ2UUOC17AY

// Modified to be a parent class to PlayerGun and CP_Gun (Computer Player Gun)

public abstract class Gun : MonoBehaviour
{
    [Header("Bullet Settings")]
    public GameObject bullet;
    public float shootForce, upwardForce;
    public float spread;

    [Header("Fire Rate")]
    public float timeBetweenShooting, timeBetweenShots;
    public int bulletsPerTap;
    public bool allowButtonHold;

    [Header("Magazine")]
    public int magazineSize;
    public float reloadTime;

    [Header("References")]
    public GameObject muzzleFlash;
    public TextMeshProUGUI ammoDisplay;
    public Transform attackPoint;

    protected int bulletsLeft, bulletsShot;
    protected bool readyToShoot, reloading;
    protected bool allowInvoke = true;

    protected virtual void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    protected virtual void Update()
    {
        if (ammoDisplay != null)
        {
            ammoDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
        }
    }

    protected void Shoot(Camera shootCam)
    {
        readyToShoot = false;

        Ray ray = shootCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
        // Rotates bullet so that it is facing the direction it is shooting
        Quaternion directionRotation = Quaternion.LookRotation(directionWithSpread.normalized);
        Quaternion offsetRotation = Quaternion.Euler(90, 0, 0);
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, directionRotation * offsetRotation);

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(shootCam.transform.up * upwardForce, ForceMode.Impulse);
        // If there is a muzzle flash animation, instantiate it at the attack point
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    protected void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}