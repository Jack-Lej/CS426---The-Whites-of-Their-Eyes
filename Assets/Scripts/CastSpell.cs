using UnityEngine;

public class CastSpell : MonoBehaviour
{
    public enum SpellType {Attack, Heal, Summon}

    [System.Serializable]
    public class Spell
    {
        public string spellName;
        public SpellType spellType;
        public int damage;
        public int healAmount;
        public GameObject summonedObject;
        public float cooldown;
        public float range;
        public ParticleSystem spell_vfx;
        public Vector3 summonSpawnPoint;
        public Transform spellAttackPoint;
        
    }

    [SerializeField] public Spell[] spells;
    private float[] cooldownTimers;

    private void PlaySpellVFX(Spell spell, Vector3 position)
    {
        if(spell.spell_vfx == null) return;

        Vector3 particlePosition;
        if(spell.spellType == SpellType.Attack)
        {
            particlePosition = spell.spellAttackPoint.position;
        } else
        {
            particlePosition = position + (Vector3.up * 1.0f);
        }

        Vector3 directionWithSpread = transform.forward;
        Quaternion directionRotation = Quaternion.LookRotation(directionWithSpread.normalized);

        // if(spell.spellType == SpellType.Attack) {
        //     directionWithSpread = transform.forward;
        //     // directionWithSpread = (position - spell.spellAttackPoint.position).normalized;
        //     // directionRotation = Quaternion.LookRotation(directionWithSpread);
        // } else if(spell.spellType == SpellType.Heal)
        // {
        //     directionWithSpread = Vector3.up; // Heal VFX faces upwards
        //     directionRotation = Quaternion.LookRotation(directionWithSpread);
        // }
        // ParticleSystem effect = Instantiate(spell.spell_vfx, position + (Vector3.up * 1.0f), directionRotation);
        ParticleSystem effect = Instantiate(spell.spell_vfx, particlePosition, directionRotation);
        effect.Play();
        Destroy(effect.gameObject, effect.main.duration);
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        cooldownTimers = new float[spells.Length];
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < cooldownTimers.Length; i++)
        {
            if(cooldownTimers[i] > 0)
            {
                cooldownTimers[i] -= Time.deltaTime;
            }
        }
    }

    public bool IsSpellReady(int spellIdx)
    {
        if (spellIdx < 0 || spellIdx >= spells.Length) return false;
        return cooldownTimers[spellIdx] <= 0;
    }


    public void Cast(int spellIdx, GameObject target)
    {
        if (spellIdx < 0 || spellIdx >= spells.Length)
        {
            Debug.LogError("Invalid spell index: " + spellIdx);
            return;
        }

        Spell spell = spells[spellIdx];

        if (cooldownTimers[spellIdx] > 0)
        {
            // Debug.Log("Spell " + spell.spellName + " is on cooldown for " + cooldownTimers[spellIdx] + " more seconds.");
            return;
        }

        switch (spell.spellType)
        {
            case SpellType.Attack:
                CastAttack(spell, target);
                break;
            case SpellType.Heal:
                CastHeal(spell, target);
                break;
            case SpellType.Summon:
                CastSummon(spell, spell.summonSpawnPoint);
                break;
        }

        cooldownTimers[spellIdx] = spell.cooldown; // Start cooldown timer
    }

    private void CastAttack(Spell spell, GameObject target)
    {
        if (target == null)
        {
            Debug.LogError("No target specified for attack spell: " + spell.spellName);
            return;
        }

        Character character = target.GetComponent<Character>();

        if(character != null) {
            character.TakeDamage(spell.damage);
            if(spell.spellType == SpellType.Attack)
                PlaySpellVFX(spell, this.transform.position);
            else
                PlaySpellVFX(spell, target.transform.position);
            Debug.Log("Cast attack spell: " + spell.spellName + " on target: " + target.name);
        }
    }

    private void CastHeal(Spell spell, GameObject target)
    {
        if (target == null)
        {
            Debug.LogError("No target specified for heal spell: " + spell.spellName);
            return;
        }

        Character character = target.GetComponent<Character>();

        if(character != null) {
            character.Heal(spell.healAmount);
            PlaySpellVFX(spell, target.transform.position);
        }
    }

    private void CastSummon(Spell spell, Vector3 summonLocation)
    {
        if (summonLocation == null)
        {
            Debug.LogError("No summon location specified for summon spell: " + spell.spellName);
            return;
        }

        if (spell.summonedObject == null)
        {
            Debug.LogError("No summoned object specified for summon spell: " + spell.spellName);
            return;
        }

        
        Instantiate(spell.summonedObject, summonLocation, Quaternion.identity);
        PlaySpellVFX(spell, summonLocation);
        // Instantiate(spell.summonedObject, summonLocation.position, summonLocation.rotation);
        // PlaySpellVFX(spell, summonLocation.position);
        
    }

    public void CastSummonAtPosition(int spellIdx, Vector3 spawnPosition)
    {
        if (spellIdx < 0 || spellIdx >= spells.Length) return;

        Spell spell = spells[spellIdx];

        if (cooldownTimers[spellIdx] > 0) return;

        if (spell.spellType != SpellType.Summon)
        {
            Debug.LogError("Spell at index " + spellIdx + " is not a summon spell.");
            return;
        }
        Debug.Log("CASTING SUMMON!!!!");
        CastSummon(spell, spawnPosition);
        cooldownTimers[spellIdx] = spell.cooldown;
    }
}
