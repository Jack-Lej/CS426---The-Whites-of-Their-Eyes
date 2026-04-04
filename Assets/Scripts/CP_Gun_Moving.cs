using UnityEngine;
using UnityEngine.AI;

public class CP_Gun_Moving : CP_Gun
{
    private enum CharacterStates {Wandering, Idle, Attacking, Chasing, Dead};
    private CharacterStates currentState;
    // private bool shooting;
    public UnityEngine.AI.NavMeshAgent agent;
    // public GameObject player;
    public float stopDistance = 5f;
    private Vector3 currentDestination;
    private Transform playerTransform;
    private GameObject targetObj;
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
            targetObj = other.gameObject;
            rotateScript.targetObj = targetObj;
            playerTransform = other.transform;
            
            managefiring();
        }
    }

    protected void onTriggerExit(Collider other)
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

        // if(activeCoroutine != null)
        // {
        //     StopCoroutine(activeCoroutine); // Stop any existing coroutine when changing state
        //     activeCoroutine = null;
        // } 

        currentState = newState;
        switch (currentState)
        {
            case CharacterStates.Wandering:
                Debug.Log("Switching to Wandering");

                // agent.speed = animator.GetFloat("WalkSpeed");
                // activeCoroutine = StartCoroutine(WanderCoroutine());
                break;

            case CharacterStates.Idle:
                Debug.Log("Switching to Idle");

                // agent.speed = animator.GetFloat("WalkSpeed");
                // agent.ResetPath();
                // activeCoroutine = StartCoroutine(IdleCoroutine());
                break;

            case CharacterStates.Attacking:
                Debug.Log("Switching to Attacking");
                // agent.ResetPath();
                break;

            case CharacterStates.Chasing:
                Debug.Log("Switching to Chasing");
                // agent.speed = animator.GetFloat("RunSpeed");
                break;


            case CharacterStates.Dead:
                Debug.Log("Switching to Dead");
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                agent.enabled = false; // Disable NavMeshAgent on death
                Destroy(gameObject, 0.5f);
                // activeCoroutine = StartCoroutine(DeathCleanup());
                break;
        }
    }

    void Start() {
        agent.stoppingDistance = stopDistance;
        // rotateScript.targetObj = player;
        originalAttackPoint = attackPoint.localPosition; // store the original local position of the attack point
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

    // Update is called once per frame

    override protected void Update()
    {   
        if(playerTransform != null) {
            agent.SetDestination(playerTransform.position);
        }

        Vector3 pos = transform.position;
        pos.y = Mathf.Round(pos.y * 1000f) / 1000f; // snap Y to prevent float drift
        transform.position = pos;
        
        if(IsStopped())
        {
            attackPoint.localPosition = originalAttackPoint; // reset the attack point position when stopped
        } else
        {
            // extends the attack point forward when moving towards the player to prevent bullets from colliding with the NPC itself
            attackPoint.localPosition = new Vector3(originalAttackPoint.x, originalAttackPoint.y, extendedBarrelDistance);
        }
        // Debug.Log(IsStopped());
    }

    void OnDamageTaken(int damage)
    {
        if (currentState == CharacterStates.Dead) return; // Don't do anything if dead

    }

    

    
}