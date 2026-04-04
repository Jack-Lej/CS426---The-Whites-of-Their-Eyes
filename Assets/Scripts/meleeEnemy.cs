using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class meleeEnemy : MonoBehaviour
{
    private enum CharacterStates {Wandering, Idle, Attacking, Chasing, Dead};
    private CharacterStates currentState;
    // private bool wandering = true;
    private Vector3 currentDestination;
    public float wanderRadius = 10f;
    public float idleTime = 8f;
    public float interval = 8f;
    public float stopDistance = 2f;
    public float deathCleanupDelay = 5f;
    public UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;
    private Coroutine activeCoroutine;

    private Transform playerTransform;

    bool IsStopped() {
        if(!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f) {
            if(agent.velocity.sqrMagnitude < 0.1f) {
                return true;
            }
        }

        return false;
    }

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
                Debug.Log("Switching to Wandering");
                animator.SetBool("isWalking", true);
                animator.SetBool("isIdle", false);
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isDead", false);

                agent.speed = animator.GetFloat("WalkSpeed");
                activeCoroutine = StartCoroutine(WanderCoroutine());
                break;

            case CharacterStates.Idle:
                Debug.Log("Switching to Idle");
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", true);
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isDead", false);

                agent.speed = animator.GetFloat("WalkSpeed");
                agent.ResetPath();
                activeCoroutine = StartCoroutine(IdleCoroutine());
                break;

            case CharacterStates.Attacking:
                Debug.Log("Switching to Attacking");
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", false);
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", true);
                animator.SetBool("isDead", false);
                agent.ResetPath();
                // agent.speed = 0f;
                agent.velocity = Vector3.zero; // Stop the agent's movement immediately
                break;

            case CharacterStates.Chasing:
                Debug.Log("Switching to Chasing");
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", false);
                animator.SetBool("isChasing", true);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isDead", false);

                agent.speed = animator.GetFloat("RunSpeed");
                break;


            case CharacterStates.Dead:
                Debug.Log("Switching to Dead");
                animator.SetBool("isWalking", false);
                animator.SetBool("isIdle", false);
                animator.SetBool("isChasing", false);
                animator.SetBool("isAttacking", false);
                animator.SetBool("isDead", true);
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                agent.enabled = false; // Disable NavMeshAgent on death
                activeCoroutine = StartCoroutine(DeathCleanup());
                break;
        }
    }

    // public void OnHitboxContact(Collider playerCollider)
    // {
    //     // playerCollider.GetComponent<PlayerHealth>().TakeDamage(damage);
    //     Debug.Log("Player hit for " + damage + " damage");
    // }



    // Chases the player as long as they remain in the trigger radius
    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentState == CharacterStates.Dead) return; // Don't do anything if dead
            playerTransform = other.transform;
            float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distToPlayer <= agent.stoppingDistance + 0.5f)
            {
                SetState(CharacterStates.Attacking); // Loop attack animation
            }
            else
            {
                SetState(CharacterStates.Chasing);
                agent.SetDestination(other.transform.position);
            }
        }
    }

    // Switches back to idle state when the player moves a certain distance away from the enemy
    protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentState == CharacterStates.Dead) return; // Don't do anything if dead

            playerTransform = null; // Clear the player reference when they leave
            SetState(CharacterStates.Idle);
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Implement damage to player here
            // Debug.Log("Player hit for " + damage + " damage");
            // Debug.Log("Hit!");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        agent.speed = animator.GetFloat("WalkSpeed");
        agent.stoppingDistance = stopDistance;

        Character character = GetComponent<Character>();
        if (character != null)
        {
            character.onDeath.AddListener(() => SetState(CharacterStates.Dead));
            character.onDamageTaken.AddListener(OnDamageTaken);
        } else
        {
            Debug.LogWarning("meleeEnemy: No Character component found on " + gameObject.name);
        }
        SetState(CharacterStates.Wandering);
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

        if(currentState == CharacterStates.Chasing && playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
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

    void OnDamageTaken(int damage)
    {
        if (currentState == CharacterStates.Dead) return; // Don't do anything if dead


        // Need to implement this later....
        // If currently wandering or idle, switch to chasing when damaged
        // if (currentState == CharacterStates.Wandering || currentState == CharacterStates.Idle)
        // {
        //     SetState(CharacterStates.Chasing);
        // }
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
    
    private IEnumerator DeathCleanup()
    {
        yield return new WaitForSeconds(deathCleanupDelay);
        Destroy(gameObject);
    }
}
