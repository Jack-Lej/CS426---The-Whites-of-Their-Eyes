using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class meleeEnemy : MonoBehaviour
{
    private enum CharacterStates {Wandering, Idle, Attacking, Dead};
    private CharacterStates currentState;
    // private bool wandering = true;
    private Vector3 currentDestination;
    public float wanderRadius = 10f;
    public float idleTime = 8f;
    public float interval = 8f;
    public UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;

    private Coroutine activeCoroutine;


    protected Vector3 GetRandPoint(Vector3 origin, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += origin;
        NavMeshHit hit;
        Vector3 finalPosition = origin;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            finalPosition = hit.position;
        }
        

        return finalPosition;
    }

    void SetState(CharacterStates newState)
    {
        if(currentState == newState) return; // No state change, do nothing

        if(activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine); // Stop any existing coroutine when changing state
            activeCoroutine = null;
        } 

        currentState = newState;
        switch (currentState)
        {
            case CharacterStates.Wandering:
                // Debug.Log("Switching to Wandering");
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdle", false);
                activeCoroutine = StartCoroutine(WanderCoroutine());
                break;
            case CharacterStates.Idle:
                // Debug.Log("Switching to Idle");
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", true);
                agent.ResetPath();
                activeCoroutine = StartCoroutine(IdleCoroutine());
                break;
            case CharacterStates.Attacking:
                // Implement attacking behavior here
                break;
            case CharacterStates.Dead:
                // Implement death behavior here
                break;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        SetState(CharacterStates.Wandering);
        agent.speed = animator.GetFloat("WalkSpeed");
        // currentState = CharacterStates.Idle;
        // starts the wandering loop
        // StartCoroutine(Wander());
        // currentDestination = GetRandPoint(transform.position, 10f);
        // agent.SetDestination(currentDestination);
    }

    // Update is called once per frame
    void Update()
    {
        // While wandering, check if we've reached our destination
        if (currentState == CharacterStates.Wandering)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance
                && agent.velocity.sqrMagnitude < 0.1f)
            {
                SetState(CharacterStates.Idle);
            }
        }
    }
    // Grabs a random point within the wander radius and sets it as the destination
    private IEnumerator Wander()
    {
        SetState(CharacterStates.Wandering);
        while (currentState == CharacterStates.Wandering)
        {
            currentDestination = GetRandPoint(transform.position, wanderRadius);
            agent.SetDestination(currentDestination);
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator WanderCoroutine()
    {
        while (currentState == CharacterStates.Wandering)
        {
            currentDestination = GetRandPoint(transform.position, wanderRadius);
            agent.SetDestination(currentDestination);
            yield return new WaitForSeconds(interval); // Pick a new point every interval
        }
    }

    private IEnumerator IdleCoroutine()
    {
        yield return new WaitForSeconds(idleTime);
        SetState(CharacterStates.Wandering); // Resume wandering after idle time
    }

    
    
}
