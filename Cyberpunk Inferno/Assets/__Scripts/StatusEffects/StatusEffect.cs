using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for status effects.
/// Do not worry about writing code for max stacks, that code will be in the addStatusEffect function of an enemy.
/// </summary>
public abstract class StatusEffect : ScriptableObject
{
    
    public int maxStacks;
    public string statusName;
    [HideInInspector]
    public Enemy boundEnemy;

    public abstract void doStatusEffect();
    
    //operator overloads, so that status effects can be directly compared. Do NOT override these.
    public static bool operator ==(StatusEffect a, StatusEffect b) => a.statusName == b.statusName;
    public static bool operator !=(StatusEffect a, StatusEffect b) => a.statusName != b.statusName;

    public override bool Equals(object obj)
    {
        var effect = obj as StatusEffect;
        return effect != null && statusName == effect.statusName;
    }

}
