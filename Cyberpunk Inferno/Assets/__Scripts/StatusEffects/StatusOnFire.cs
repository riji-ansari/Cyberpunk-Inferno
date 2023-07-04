using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusOnFire : StatusEffect
{
    void Awake()
    {
        statusName = "On Fire";
        maxStacks = 1;
    }
    public override void doStatusEffect()
    {
        boundEnemy.health -= 5; //effective 20dps
    }

    /*
     * Make sure to use 'Awake()' to define the name and the maximum stacks, otherwise lots of status effect code may break, as the operator overrides are based on names.
     * boundEnemy will be set when an enemy has applyStatusEffect() called, so do not worry about that, just access their values through this.
     * Status effects are called 4 times/second, so use that to come up with timers for temporary status effects.
     * Remember, if a status effect is temporary, you must revert the effect at the end! (ex. if it reduces movement speed by 5, make sure to increase by 5 again at the end!)
     */
}
