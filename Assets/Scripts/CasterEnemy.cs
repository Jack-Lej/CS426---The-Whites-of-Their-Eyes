using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CasterEnemy : meleeEnemy
{
    private enum CasterState { 
        Wandering,
        Idle,
        AttackIdle,
        CastingAttack,
        CastingHeal,
        CastingSummon,
        ChasingPlayer,
        ChasingAlly,
        Dead 
    };

    [Header("Caster Settings")]
    [SerializeField] private CastSpell spellCaster;
    [SerializeField] private float preferredCastingDist = 8f;
    [SerializeField] private float minCastingDist = 4f;
    // [SerializeField] private int attackSpellIdx = 0;
    // [SerializeField] private int healSpellIdx = 1;
    // [SerializeField] private int summonSpellIdx = 2;
    [Header("Flamethrower")]
    [SerializeField] private ParticleSystem flamethrower;
    // [SerializeField] private float flamethrowerDuration = 3f;
    [Header("Healing Spell")]
    [SerializeField] private float allyHealRadius = 20f;
    [SerializeField] private float healTriggerDist = 5f;
    [SerializeField] private ParticleSystem healingSpell;
    [SerializeField] private AudioClip[] healingSounds;
    // [SerializeField] private float healingSpellDuration = 1.5f;
    [Header("Summoning Spell")]
    [SerializeField] private AudioClip[] summoningSounds;
    [SerializeField] private ParticleSystem summoningSpell;
    [SerializeField] private float summonRadius = 10f;

    private CasterState casterState;
    private Transform injuredAllyTransform;
    private Coroutine scanCoroutine;
    private Coroutine casterActiveCoroutine;

    // private bool HasAttackSpell => spellCaster != null &&
    //     attackSpellIdx >= 0 &&
    //     attackSpellIdx < spellCaster.spells.Length &&
    //     spellCaster.spells[attackSpellIdx].spellType == CastSpell.SpellType.Attack;

    // private bool HasHealSpell => spellCaster != null &&
    //     healSpellIdx >= 0 &&
    //     healSpellIdx < spellCaster.spells.Length &&
    //     spellCaster.spells[healSpellIdx].spellType == CastSpell.SpellType.Heal;

    // private bool HasSummonSpell => spellCaster != null &&
    //     summonSpellIdx >= 0 &&
    //     summonSpellIdx < spellCaster.spells.Length &&
    //     spellCaster.spells[summonSpellIdx].spellType == CastSpell.SpellType.Summon;

    private bool HasAttackSpell = false;
    private bool HasHealSpell = false;
    private bool HasSummonSpell = false;

    private int GetSpellIndex(CastSpell.SpellType type)
    {
        if (spellCaster == null) return -1;
        for (int i = 0; i < spellCaster.spells.Length; i++)
        {
            if (spellCaster.spells[i].spellType == type)
                return i;
        }
        return -1; // No spell of that type found
    }

    new void Start()
    {
        base.Start(); // Initializes animator, agent, audioSource
        spellCaster = GetComponent<CastSpell>();
        // Derive spell availability from what's actually assigned
        HasAttackSpell = GetSpellIndex(CastSpell.SpellType.Attack) >= 0;
        HasHealSpell = GetSpellIndex(CastSpell.SpellType.Heal) >= 0;
        HasSummonSpell = GetSpellIndex(CastSpell.SpellType.Summon) >= 0;

        scanCoroutine = StartCoroutine(ScanForInjuredAllies());
        SetCasterState(CasterState.Wandering);


        // Debug.Log("SpellCaster is NOT NULL: " + (spellCaster != null));
        // Debug.Log("Summon Spell IDX: " + (summonSpellIdx));
        // Debug.Log("Is Summon Spell Type: " + (spellCaster.spells[summonSpellIdx].spellType == CastSpell.SpellType.Summon));
    }

    new void Update()
    {
        // Do NOT call base.Update() - meleeEnemy's state machine would conflict

        // Handle wandering destination reached
        if (casterState == CasterState.Wandering)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance
                && agent.velocity.sqrMagnitude < 0.1f)
            {
                SetCasterState(CasterState.Idle);
            }
        }

        // Rotate towards player when chasing and stopped
        if ((casterState == CasterState.ChasingPlayer || 
             casterState == CasterState.AttackIdle ||
             casterState == CasterState.CastingAttack) && 
             playerTransform != null)
        {
            if (IsStopped() && !IsFacingTarget(playerTransform))
            {
                RotateTowardsPlayer();
            }
        }

        switch (casterState)
        {
            case CasterState.ChasingPlayer:
                HandleChasingPlayer();
                break;
            case CasterState.ChasingAlly:
                HandleChasingAlly();
                break;
        }
    }
    

    void SetCasterState(CasterState newState)
    {
        if (casterState == newState || animator.GetBool("isDead")) return;
        casterState = newState;

        // Stop any running caster coroutine on state change
        if (casterActiveCoroutine != null)
        {
            StopCoroutine(casterActiveCoroutine);
            casterActiveCoroutine = null;
        }

        // Reset all bools first
        animator.SetBool("isIdle", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isChasing", false);
        // animator.SetBool("isHealing", false);
        animator.SetBool("isAttackIdle", false);
        animator.SetBool("isCastingFire", false);
        animator.SetBool("isCastingHeal", false);
        animator.SetBool("isCastingSummon", false);
        // animator.SetBool("isDead", false);

        switch (casterState)
        {
            case CasterState.Wandering:
                animator.SetBool("isWalking", true);
                agent.speed = animator.GetFloat("WalkSpeed");
                casterActiveCoroutine = StartCoroutine(CasterWanderCoroutine());
                break;

            case CasterState.Idle:
                animator.SetBool("isIdle", true);
                agent.ResetPath();
                casterActiveCoroutine = StartCoroutine(CasterIdleCoroutine());
                break;

            case CasterState.AttackIdle:
                animator.SetBool("isAttackIdle", true);
                agent.ResetPath();
                break;

            case CasterState.ChasingPlayer:
            case CasterState.ChasingAlly:
                animator.SetBool("isChasing", true);
                agent.speed = animator.GetFloat("RunSpeed");
                break;

            case CasterState.CastingAttack:
                // animator.SetBool("isCastingFire", true);
                animator.SetTrigger("castFire");
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                casterActiveCoroutine = StartCoroutine(FireFlamethrower());
                break;

            case CasterState.CastingHeal:
                // animator.SetBool("isCastingHeal", true);
                animator.SetTrigger("castHeal");
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                casterActiveCoroutine = StartCoroutine(HealAlly());
                break;

            case CasterState.CastingSummon:
                // Debug.Log("SOME MONSTER ENTERED CASTING SUMMON STATE!!!!");
                // animator.SetBool("isCastingSummon", true);
                animator.SetTrigger("castSummon");
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                casterActiveCoroutine = StartCoroutine(SummonMinion());
                break;

            case CasterState.Dead:
                // animator.SetBool("isDead", true);
                animator.SetTrigger("die");
                agent.ResetPath();
                agent.velocity = Vector3.zero;
                agent.enabled = false;
                if (scanCoroutine != null) StopCoroutine(scanCoroutine);
                // activeCoroutine = StartCoroutine(DeathCleanup());
                break;
        }
    }


    private IEnumerator CasterWanderCoroutine()
    {
        while (casterState == CasterState.Wandering)
        {
            Vector3 destination = GetRandPoint(transform.position, wanderRadius);
            agent.SetDestination(destination);
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator CasterIdleCoroutine()
    {
        yield return new WaitForSeconds(idleTime);
        if (casterState == CasterState.Idle) // Only wander if still idle
            SetCasterState(CasterState.Wandering);
    }

    new protected void OnTriggerStay(Collider other)
    {
        if (casterState == CasterState.Dead ||
            casterState == CasterState.CastingHeal ||
            casterState == CasterState.ChasingAlly ||
            casterState == CasterState.CastingSummon ||
            casterState == CasterState.CastingAttack) return;
        // if (casterState == CasterState.ChasingAlly || casterState == CasterState.CastingHeal) return;

        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            float dist = Vector3.Distance(transform.position, playerTransform.position);

            if (dist > preferredCastingDist)
            {
                SetCasterState(CasterState.ChasingPlayer);
            }
            else if (dist < minCastingDist)
            {
                SetCasterState(CasterState.ChasingPlayer);
                MaintainCastDistance();
            }
            else
            {
                SetCasterState(CasterState.AttackIdle);
                int attackIdx = GetSpellIndex(CastSpell.SpellType.Attack);
                int summonIdx = GetSpellIndex(CastSpell.SpellType.Summon);

                if (IsFacingTarget(playerTransform) && HasAttackSpell && spellCaster.IsSpellReady(attackIdx))
                {
                    SetCasterState(CasterState.CastingAttack);
                    // spellCaster.Cast(attackSpellIdx, playerTransform.gameObject);
                }
                else if (HasSummonSpell && spellCaster.IsSpellReady(summonIdx))
                {
                    SetCasterState(CasterState.CastingSummon);
                }
            }
        }
    }

    new protected void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (casterState != CasterState.ChasingAlly && casterState != CasterState.CastingHeal)
            {
                playerTransform = null;
                SetCasterState(injuredAllyTransform != null ? CasterState.AttackIdle : CasterState.Idle);
            }
        }
    }

    private void HandleChasingPlayer()
    {
        if (playerTransform == null)
        {
            SetCasterState(CasterState.Idle);
            return;
        }

        float dist = Vector3.Distance(transform.position, playerTransform.position);

        if (dist < minCastingDist)
            MaintainCastDistance();
        else if (dist <= preferredCastingDist)
            SetCasterState(CasterState.AttackIdle);
        else
            agent.SetDestination(playerTransform.position);
    }

    private void HandleChasingAlly()
    {
        if (injuredAllyTransform == null)
        {
            SetCasterState(playerTransform != null ? CasterState.ChasingPlayer : CasterState.Idle);
            return;
        }

        float dist = Vector3.Distance(transform.position, injuredAllyTransform.position);
        agent.SetDestination(injuredAllyTransform.position);

        if (dist <= healTriggerDist)
        {
            int healIdx = GetSpellIndex(CastSpell.SpellType.Heal);
            GameObject allyObj = injuredAllyTransform.gameObject;

            if (HasHealSpell &&
                spellCaster.IsSpellReady(healIdx) &&
                allyObj.GetComponent<Character>().currentHealth > 0)
            {
                // casterActiveCoroutine = StartCoroutine(CastingCoroutine(CasterState.CastingHeal));
                // spellCaster.Cast(healSpellIdx, injuredAllyTransform.gameObject);
                
                // GameObject healTarget = injuredAllyTransform.gameObject;

                SetCasterState(CasterState.CastingHeal);
                // spellCaster.Cast(healSpellIdx, healTarget);
                // Do NOT call SetCasterState again here — HealAlly coroutine handles the exit
                return;
            }
            // Heal still in cooldown
            if(!spellCaster.IsSpellReady(healIdx))
            {
                agent.ResetPath();
                return;
            }

            injuredAllyTransform = null;
            SetCasterState(playerTransform != null ? CasterState.ChasingPlayer : CasterState.Idle);
        }
    }

    private IEnumerator CastingCoroutine(CasterState castingState)
    {
        yield return null;

        float castDuration = GetCurrentAnimationDuration();
        yield return new WaitForSeconds(castDuration);

        if(casterState == castingState)
        {
            SetCasterState(playerTransform != null ? CasterState.ChasingPlayer : CasterState.Idle);
        }

    } 

    private void MaintainCastDistance()
    {
        if (playerTransform == null) return;

        Vector3 dirAway = (transform.position - playerTransform.position).normalized;
        Vector3 targetPos = transform.position + dirAway * (preferredCastingDist - minCastingDist);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 3f, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }

    private IEnumerator ScanForInjuredAllies()
    {
        while (casterState != CasterState.Dead)
        {
            yield return new WaitForSeconds(1f);
           

            if (casterState == CasterState.CastingHeal ||
                casterState == CasterState.CastingAttack ||
                casterState == CasterState.CastingSummon) continue;

            Collider[] nearbyAllies = Physics.OverlapSphere(transform.position, allyHealRadius);
            Transform closestInjuredAlly = null;
            float closestDist = Mathf.Infinity;

            foreach (Collider col in nearbyAllies)
            {
                if (col.transform == transform) continue;
                if (col.CompareTag("Player")) continue;

                Character ally = col.GetComponent<Character>();
                if (ally == null) continue;

                if (ally.currentHealth <= ally.maxHealth * 0.75f)
                {
                    float dist = Vector3.Distance(transform.position, col.transform.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        closestInjuredAlly = col.transform;
                    }
                }
            }

            if (closestInjuredAlly != null)
            {
                injuredAllyTransform = closestInjuredAlly;
                if (HasHealSpell)
                {
                    SetCasterState(CasterState.ChasingAlly);
                }
            }
            else
            {
                injuredAllyTransform = null;
                if (casterState == CasterState.AttackIdle && playerTransform == null)
                    SetCasterState(CasterState.Idle);
            }
        }
    }

    public void OnAttackSoundAnimationEvent()
    {
        if (casterState == CasterState.CastingAttack && !animator.GetBool("isDead"))
        {
            playRandAttackSound(AttackSoundClips);
        }
    }

    public void OnAttackAnimationEvent()
    {
        if (casterState == CasterState.CastingAttack && !animator.GetBool("isDead"))
        {
            
            int idx = GetSpellIndex(CastSpell.SpellType.Attack);
            if (idx >= 0)
            {
                // playRandAttackSound(AttackSoundClips);
                spellCaster.Cast(idx, playerTransform.gameObject);
            }
        }
        // StartCoroutine(FireFlamethrower());
        // spellCaster
    }

    public void onHealAnimationEvent()
    {
        if (casterState == CasterState.CastingHeal && !animator.GetBool("isDead"))
        {
            if(injuredAllyTransform != null)
            {
                int idx = GetSpellIndex(CastSpell.SpellType.Heal);
                if (idx >= 0 && injuredAllyTransform != null)
                    spellCaster.Cast(idx, injuredAllyTransform.gameObject);
            }
            
        }
    }

    public void onHealSoundAnimationEvent()
    {
        if (casterState == CasterState.CastingHeal && !animator.GetBool("isDead"))
        {
            playRandAttackSound(healingSounds);
        }
    }

    public void onSummonAnimationEvent()
    {
        if (casterState == CasterState.CastingSummon && !animator.GetBool("isDead"))
        {
            int idx = GetSpellIndex(CastSpell.SpellType.Summon);
            if (idx >= 0)
            {
                Vector3 randSpawnPoint = GetRandPoint(transform.position, summonRadius);
                spellCaster.CastSummonAtPosition(idx, randSpawnPoint);
            }
        }
    }

    public void onSummonSoundAnimationEvent()
    {
        if (casterState == CasterState.CastingSummon && !animator.GetBool("isDead"))
        {
            playRandAttackSound(summoningSounds);
        }
    }



    private IEnumerator FireFlamethrower()
    {
        if (flamethrower == null) yield break;

        yield return null;
        float animDuration = GetCurrentAnimationDuration();

        // flamethrower.Play();
        // Debug.Log("Starting flamethrower for animation duration: " + animDuration);
        yield return new WaitForSeconds(4.3f);
        // Debug.Log("Flamethrower finished");
        // flamethrower.Stop();
        if (casterState == CasterState.CastingAttack)
            SetCasterState(playerTransform != null ? CasterState.AttackIdle : CasterState.Idle);
    }

    private IEnumerator HealAlly()
    {
        if (healingSpell == null) yield break;

        yield return null;
        float animDuration = GetCurrentAnimationDuration();

        // healingSpell.Play();
        yield return new WaitForSeconds(2.267f);
        // healingSpell.Stop();
        if (casterState == CasterState.CastingHeal)
            SetCasterState(playerTransform != null ? CasterState.ChasingPlayer : CasterState.Idle);
    }

    private IEnumerator SummonMinion()
    {
        if (summoningSpell == null) yield break;

        yield return null;
        
        yield return new WaitForSeconds(2.167f);
        if(casterState == CasterState.CastingSummon)
            SetCasterState(playerTransform != null ? CasterState.AttackIdle : CasterState.Idle);
    }
    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, preferredCastingDist);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minCastingDist);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, allyHealRadius);
    }
}