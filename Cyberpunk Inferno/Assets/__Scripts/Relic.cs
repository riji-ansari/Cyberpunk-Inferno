using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : MonoBehaviour
{
    //All the stats that can be affected by relics
    public float damageTakenMult = 1f; //applied in PlayerController on taking damage
    public float damageDealtMult = 1f; //aplied in abstract Weapon class
    public float healthRegen = 1f; //Unimplemented; we don't even have healing in the game right now, so until that happens, this is useless anyway.
    public float playerSpeedMult = 1f; //Applied in PlayerController
    public float enemySpeedMult = 1f; //Applied to each enemy 
    public float timeBetweenShotsMult = 1f; //Applied to each ranged weapon (Make sure this one is < 1! Otherwise you increase the time between shots)
    public float dashCooldownMult = 1f; //Unimplemented (Dash script doesn't seem to work?)

    //A status effect to apply to enemies when hit
    public StatusEffect onHitEffect;

    //static functions for calculating multipliers
    public static float calculateDamageTakenMult(Relic[] relics)
    {
        float endMult = 1;
        foreach (Relic r in relics)
        {
            if (r != null) endMult *= r.damageTakenMult;
        }
        return endMult;
    }

    public static float calculateDamageDealtMult(Relic[] relics)
    {
        float endMult = 1;
        foreach (Relic r in relics)
        {
            if (r != null) endMult *= r.damageTakenMult;
        }
        return endMult;
    }

    public static float calculateHealthRegenMult(Relic[] relics)
    {
        float endMult = 1;
        foreach (Relic r in relics)
        {
            if (r != null) endMult *= r.healthRegen;
        }
        return endMult;
    }

    public static float calculatePlayerSpeedMult(Relic[] relics)
    {
        float endMult = 1;
        foreach (Relic r in relics)
        {
            if (r != null) endMult *= r.playerSpeedMult;
        }
        return endMult;
    }

    public static float calculateEnemySpeedMult(Relic[] relics)
    {
        float endMult = 1;
        foreach (Relic r in relics)
        {
            if (r != null) endMult *= r.enemySpeedMult;
        }
        return endMult;
    }

    public static float calculateTimeBetweenShotsMult(Relic[] relics)
    {
        float endMult = 1;
        foreach (Relic r in relics)
        {
            if (r != null) endMult *= r.timeBetweenShotsMult;
        }
        return endMult;
    }

    public static float calculateDashCooldownMult(Relic[] relics)
    {
        float endMult = 1;
        foreach (Relic r in relics)
        {
            if (r != null) endMult *= r.dashCooldownMult;
        }
        return endMult;
    }

    //Static function for finding all onhit effects from equipped relics
    public static List<StatusEffect> getOnhitEffects(Relic[] relics)
    {
        List<StatusEffect> stats = new List<StatusEffect>();

        foreach (Relic r in relics)
        {
            if (r != null)
            {
                if(r.onHitEffect != null)
                {
                    stats.Add(r.onHitEffect);
                }
            }
        }

        return stats;
    }

    public override bool Equals(object obj)
    {
        var relic = obj as Relic;
        return relic != null &&
               base.Equals(obj) &&
               damageTakenMult == relic.damageTakenMult &&
               damageDealtMult == relic.damageDealtMult &&
               healthRegen == relic.healthRegen &&
               playerSpeedMult == relic.playerSpeedMult &&
               enemySpeedMult == relic.enemySpeedMult &&
               timeBetweenShotsMult == relic.timeBetweenShotsMult &&
               dashCooldownMult == relic.dashCooldownMult &&
               EqualityComparer<StatusEffect>.Default.Equals(onHitEffect, relic.onHitEffect);
    }
}
