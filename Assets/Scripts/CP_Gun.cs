using UnityEngine;
using UnityEngine.InputSystem;

// Computer player class for the gun class
public class CP_Gun : Gun
{
    protected enum CharacterStates {Wandering, Idle, Attacking, Chasing, Dead};
    protected CharacterStates currentState;
    protected GameObject targetObj;
    protected Transform playerTransform;
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

    void Start()
    {
        Character character = GetComponent<Character>();
        if (character != null)
        {
            character.onDeath.AddListener(() => SetState(CharacterStates.Dead));
            character.onDamageTaken.AddListener(OnDamageTaken);
        } else
        {
            Debug.LogWarning("No Character component found on " + gameObject.name + ". Death state will not be handled properly.");
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
        playRandomSound(shootClips);

        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(attackPoint.up * upwardForce, ForceMode.Impulse);

        // If there is a muzzle flash animation, instantiate it at the attack point
        if (muzzleFlash != null)
            Instantiate(muzzleFlash, attackPoint.position, directionRotation);

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
        if (other.CompareTag("Player") && currentState != CharacterStates.Dead)
        {
            targetObj = other.gameObject;
            playerTransform = other.transform;

            // Always update the rotate script target so it keeps rotating toward the player
            if (rotateScript != null)
            {
                rotateScript.targetObj = targetObj;
            }

            // Only fire once facing the player
            if (rotateScript == null || rotateScript.IsFacingTarget)
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
            targetObj = null;
            rotateScript.targetObj = null;
            playerTransform = null;
        }
    }

    void SetState(CharacterStates newState)
    {
        if(currentState == newState) return; // No state change, do nothing

        currentState = newState;
        switch (currentState)
        {
            case CharacterStates.Idle:
                Debug.Log("Switching to Idle");
                break;

            case CharacterStates.Attacking:
                Debug.Log("Switching to Attacking");
                break;

            case CharacterStates.Dead:
                Debug.Log("Switching to Dead");
                Destroy(gameObject, 2f);
                break;
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

    protected void OnDamageTaken(int damage)
    {
        if (currentState == CharacterStates.Dead) return; // Don't do anything if dead

    }



}