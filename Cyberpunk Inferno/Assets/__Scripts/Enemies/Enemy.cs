using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// The abstract class for enemies.
/// For new enemies, inherit from this, and override as needed (Make sure to override update!)
/// </summary>
public abstract class Enemy : MonoBehaviour
{
    [Header("AI")]
    public AI ai;

    [Header("Stats")]
    public StatusEffect[] statusImmunities;
    public float movementSpeed = 100f;
    public float armour = 0;
    public int xp = 70;
    public int gold = 10;
    public float maxHealth = 100;
    public float _health = 100;

    private HealthBar healthBar = null;

    public float damage;
    //Hidden in inspector
    [HideInInspector] public Vector3 position;
    [HideInInspector] public bool isStunned = false;
    [HideInInspector] public static bool paused = false;
    [HideInInspector] public List<StatusEffect> statusEffects;

    public float health
    {
        get { return _health; }
        set
        {
            _health = value;
            if (_health <= 0)
            {
                //check that kill() hasn't been called already
                if (gameObject.activeInHierarchy) { 
                    //print("enemy killed");

                    //make sure this enemy instance doesn't call kill() again
                    gameObject.SetActive(false);
                
                    kill(); //kill the enemy when their health hits zero }
                }
            }
            else if (_health > maxHealth)
            {
                _health = maxHealth; //don't let healing enemies get above max health
            }

            //if applicable, update health bar display
            if (healthBar != null)
            {
                //calculate ratio of current health to max health
                float healthFill = _health / maxHealth;
                //use this to adjust health bar display
                healthBar.UpdateHealthBar(healthFill);
            }
        }
    }

    public float _knockbackResist = 0;
    public float knockbackResist 
    {
        get { return _knockbackResist; }

        //capped at 1, for 100% knockback resist. Otherwise, enemies would move towards the players when hit with higher values.
        //Negative values make enemies MORE vulnerable to knockback, so those are fine.

        set
        {
            if (value > 1)
            {
                _knockbackResist = 1;
            }
            else
            {
                _knockbackResist = value;
            }
        }
    }
    protected bool _alert = false;
    public bool Alert;
    public virtual void AlertEnemy()
    {
        _alert = true;
    }

    public virtual void Shoot() //only really needed for ranged enemies, the rest will never call this.
    {

    }

    void Start()
    {
        //setup initial position
        position = gameObject.transform.position;

        //setup initial status effects
        doStatusEffects();

        //setup health bar object
        healthBar = transform.Find("Health Bar").GetComponent<HealthBar>();
        //make sure health bar display starts at initial value
        health = health;
    }
    void Update() { }
    
    protected void doStatusEffects()
    {
        if (!PlayerController.Pause) //status effects will not trigger while paused
        {
            for (int i = 0; i < statusEffects.Count; i++)
            {
                statusEffects[i].doStatusEffect(); //calls each stat effect
            }
        }
        Invoke("doStatusEffects", 0.25f); //repeat this 4 times/second
    }

    public void kill()
    {

        //give player gold
        PlayerController.Instance.gold += gold;

        //give player XP.
        PlayerController.Instance.XP += xp * (int)(SkillMenuController.getMult(SkillMenuController.SkillMult.XPGained));

        //destroy GameObject
        Destroy(gameObject);
    }
    public virtual void onPlayerHit(Weapon wep)
    {
        health -= wep.Damage;
        if (health <= 0)
        {
            kill();
        }
    }

    public bool applyStatusEffect(StatusEffect statEffect) //returns true if it could be applied
    {
        for (int i = 0; i < statusImmunities.Length; i++) //checks if the enemy is immune to this status effect
        {
            if(statusImmunities[i] == statEffect)
            {
                return false; 
            }
        }
        statEffect.boundEnemy = this; //sets the bound enemy
        int existingCounter = 0;
        for(int i = 0; i < statusEffects.Count; i++) //checks how many of this stat effect are already applied
        {
            if(statusEffects[i] == statEffect)
            {
                existingCounter++;
            }
        }

        if (existingCounter < statEffect.maxStacks)
        {
            statusEffects.Add(statEffect);
            return true;
        }
        return false;
    }


    public void knockback(int strength, Vector3 source)
    {
        Vector3 knockbackValue = position - source;
        knockbackValue.z = 0;
        knockbackValue.Normalize(); //get the direction, then normalize it.
        //Knockback effects will always take exactly 0.25 seconds to complete.
        //The knockback movement will be done over the course of 5 movements, meaning the movement update rate is 0.05 seconds.
        //For long-range knockback, this will look messy.
        isStunned = true;
        knockbackValue = ((strength * (1 - knockbackResist)) * knockbackValue) / 5; //modify the knockbackValue by the resistance, then cut it in 5.
        for(int i = 0; i < (0.25 / 0.05); i++)
        {
            position += knockbackValue;
            Invoke("", 0.05f); //creates a delay
        }
        isStunned = false;
        
    }
    
}
