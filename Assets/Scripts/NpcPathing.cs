using UnityEngine;
using UnityEngine.AI;

public class NpcPathing : CP_Gun
{
    // private bool shooting;
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject player;
    public float stopDistance = 5f;

    // protected void Shoot()
    // {
    //     readyToShoot = false;

    //     Ray ray = new Ray(attackPoint.position, attackPoint.forward);
    //     RaycastHit hit;
    //     Vector3 targetPoint;
    //     if (Physics.Raycast(ray, out hit))
    //         targetPoint = hit.point;
    //     else
    //         targetPoint = ray.GetPoint(75);

    //     Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

    //     float x = Random.Range(-spread, spread);
    //     float y = Random.Range(-spread, spread);

    //     Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
    //     // Rotates bullet so that it is facing the direction it is shooting
    //     Quaternion directionRotation = Quaternion.LookRotation(directionWithSpread.normalized);
    //     Quaternion offsetRotation = Quaternion.Euler(90, 0, 0);
    //     GameObject currentBullet = Instantiate(bullet, attackPoint.position, directionRotation * offsetRotation);

    //     currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
    //     currentBullet.GetComponent<Rigidbody>().AddForce(attackPoint.up * upwardForce, ForceMode.Impulse);

    //     // If there is a muzzle flash animation, instantiate it at the attack point
    //     if (muzzleFlash != null)
    //         Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

    //     bulletsLeft--;
    //     bulletsShot++;

    //     if (allowInvoke)
    //     {
    //         Invoke("ResetShot", timeBetweenShooting);
    //         allowInvoke = false;
    //     }

    //     if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
    //         Invoke("Shoot", timeBetweenShots);
    // }

    bool IsStopped() {
        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f) {
            if(agent.velocity.sqrMagnitude < 0.1f) {
                return true;
            }
        }

        return false;
    }

    protected new void OnTriggerStay(Collider other)
    {   
        // Debug.Log("Is faceing target: " + rotateScript.IsFacingTarget);
        if (other.CompareTag("Player"))
        {
            managefiring();
        }
    }
    // // stops firing when player is no longer within the trigger collider
    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         shooting = false;
    //     }
    // }

    void Start() {
        agent.stoppingDistance = stopDistance;
        rotateScript.targetObj = player;
    }

    // Update is called once per frame

    override protected void Update()
    {
        agent.SetDestination(player.transform.position);
        if(IsStopped())
        {
            rotateScript.Update();
        }
        // Debug.Log(IsStopped());
    }

    

    
}


