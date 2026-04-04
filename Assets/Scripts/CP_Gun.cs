using UnityEngine;
using UnityEngine.InputSystem;

// Computer player class for the gun class
public class CP_Gun : Gun
{
    protected bool shooting;
    private bool isShooting = false;
    [Header("Rotation")]
    public RotateAroundObj rotateScript; // implment rotation within CP_Gun instead of attaching two scripts to the turret

    protected override void Awake()
    {
        base.Awake();
        // Set up the RotateAroundObj script with the appropriate references and values
        if(rotateScript != null)
        {
            rotateScript = GetComponent<RotateAroundObj>();
        }
    }
    
    protected void managefiring()
    {
        shooting = true;
        if (!isShooting && readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            isShooting = true;
            bulletsShot = 0;
            Shoot();
        }
        if(readyToShoot && shooting && !reloading && bulletsLeft <= 0)
        {
            Reload();
        }
    }

    override protected void Update()
    {
       rotateScript.Update(); // Call the Update function of the RotateAroundObj script to rotate the turret towards the target object
    }

    // Modified Shoot function without camera needed
    protected void Shoot()
    {
        readyToShoot = false;

        Ray ray = new Ray(attackPoint.position, attackPoint.forward);
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
        currentBullet.GetComponent<Rigidbody>().AddForce(attackPoint.up * upwardForce, ForceMode.Impulse);

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
    // When a player is within the trigger collider, the turret keeps firing
    protected void OnTriggerStay(Collider other)
    {   
        // Debug.Log("Is faceing target: " + rotateScript.IsFacingTarget);
        if(rotateScript == null)
        {
            if (other.CompareTag("Player"))
            {
                managefiring();
            }
        } else
        {
            if (rotateScript.IsFacingTarget && other.CompareTag("Player"))
            {
                managefiring();
            }
        }
        
    }
    // stops firing when player is no longer within the trigger collider
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shooting = false;
        }
    }

    override protected void ResetShot()  // override this from Gun
    {
        readyToShoot = true;
        allowInvoke = true;
        isShooting = false;  // allow a new chain to start
    }

    override protected void ReloadFinished()  // override in CP_Gun
    {
        bulletsLeft = magazineSize;
        reloading = false;
        isShooting = false;  // allow firing again after reload
    }



}