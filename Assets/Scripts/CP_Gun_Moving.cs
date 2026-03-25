using UnityEngine;
using UnityEngine.AI;

public class CP_Gun_Moving : CP_Gun
{
    // private bool shooting;
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject player;
    public float stopDistance = 5f;
    private float extendedBarrelDistance = 1f; // distance to extend the attack point when the NPC is moving towards the player
    Vector3 originalAttackPoint; // to store the original local position of the attack point

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

    void Start() {
        agent.stoppingDistance = stopDistance;
        rotateScript.targetObj = player;
        originalAttackPoint = attackPoint.localPosition; // store the original local position of the attack point
    }

    // Update is called once per frame

    override protected void Update()
    {
        agent.SetDestination(player.transform.position);
        if(IsStopped())
        {
            // continues to rotate when stopped
            rotateScript.Update();
            attackPoint.localPosition = originalAttackPoint; // reset the attack point position when stopped
        } else
        {
            // extends the attack point forward when moving towards the player to prevent bullets from colliding with the NPC itself
            attackPoint.localPosition = new Vector3(originalAttackPoint.x, originalAttackPoint.y, extendedBarrelDistance);
        }
        // Debug.Log(IsStopped());
    }

    

    
}


