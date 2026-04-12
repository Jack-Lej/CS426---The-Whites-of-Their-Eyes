using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class meleeEnemy : MonoBehaviour
{

    private enum CharacterStates {Wandering, Idle, Attacking, Chasing, Dead};
    [SerializeField] private AudioClip IdleSoundClip;
    [SerializeField] private AudioClip[] AttackSoundClips;

    private CharacterStates currentState;
    private AudioSource audioSource;

    [SerializeField] int attackDamage;
    // private bool wandering = true;
    private Vector3 currentDestination;
    public float wanderRadius = 10f;
    public float idleTime = 8f;
    public float interval = 8f;
    public float stopDistance = 2f;
    public float deathCleanupDelay = 5f;
    public float rotationSpeed = 260f; // Speed at which the enemy rotates to face the player
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

    // Used to check if the enemy is facing the player before attacking
    private bool IsFacingTarget(Transform target, float threshold = 0.75f)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, directionToTarget);
        return dot >= threshold;
    }

    private void RotateTowardsPlayer()
    {
        if (playerTransform == null) return;

        Vector3 direction = (playerTransform.position - transform.position);
        direction.y = 0f; // Keep rotation on the horizontal plane only
        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void OnAttackAnimationEvent() {
       if(currentState == CharacterStates.Attacking) {
            playRandAttackSound(AttackSoundClips);
       }
    }

    public void OnIdleAnimationEvent() {
        if(currentState == CharacterStates.Idle) {
            playSound(IdleSoundClip);
        }
    }

    private void playRandAttackSound(AudioClip[] clips) {
        if (clips == null ||clips.Length == 0) return;

        if(audioSource == null) {
            Debug.LogWarning("AudioSource component missing on " + gameObject.name);
            return;
        }

        int index = Random.Range(0, clips.Length);
        audioSource.PlayOneShot(clips[index]);
    }

    private void playSound(AudioClip clip) {
        if (clip == null) return;

        if(audioSource == null) {
            Debug.LogWarning("AudioSource component missing on " + gameObject.name);
            return;
        }

        audioSource.PlayOneShot(clip);
    }

    private void StopSound() {
        if(audioSource != null) {
            audioSource.Stop();
        }
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
                // playSound(IdleSoundClip);
                
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

    private void StepBackFromPlayer()
    {
        if (playerTransform == null) return;

        Vector3 directionAwayFromPlayer = (transform.position - playerTransform.position).normalized;
        Vector3 targetPosition = transform.position + directionAwayFromPlayer * (agent.stoppingDistance + 0.5f);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            Debug.Log("Stepping back from player to position: " + hit.position);
        }
    }



    // Chases the player as long as they remain in the trigger radius
    protected void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentState == CharacterStates.Dead) return; // Don't do anything if dead
            playerTransform = other.transform;
            float distToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            // Makes sure the enemy in distance and is facing the player before attacking
            if (distToPlayer <= agent.stoppingDistance + 0.5f && IsFacingTarget(playerTransform))
            {
                SetState(CharacterStates.Attacking); // Loop attack animation
            }
            // else if (distToPlayer <= agent.stoppingDistance + 0.5f && !IsFacingTarget(playerTransform))
            // {
            //     // Close enough to attack but not facing — rotate in place without moving
            //     SetState(CharacterStates.Chasing);
            //     StepBackFromPlayer();
            //     RotateTowardsPlayer();
            // }
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
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            player.TakeDamage(attackDamage);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        agent.speed = animator.GetFloat("WalkSpeed");
        agent.stoppingDistance = stopDistance;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = IdleSoundClip;

        // if (IdlePlayer != null && IdleSoundClip != null)
        //     IdlePlayer.actionClip = IdleSoundClip;
        

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
            // Helps prevent the bug where the enemy getts stuck near the player
            // not acttacking and not facing the player, but not moving either
            if (IsStopped() && !IsFacingTarget(playerTransform))
            {
                RotateTowardsPlayer();
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
