using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : Enemy
{
    public override void AlertEnemy()
    {
        throw new System.NotImplementedException();
    }

    public override void onPlayerHit(Weapon wep)
    {
        health -= wep.Damage;
    }

    public override void Shoot()
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        ai.Move(movementSpeed);
        //applyStatusEffect(ScriptableObject.CreateInstance("StatusOnFire") as StatusEffect);
    }
}
