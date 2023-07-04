using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserRifle : RangedWeapon
{
    
    /*public LaserRifle(float damage, float cooldown, GameObject projectile) : base(damage, cooldown, projectile)
    {
       //this should not be instantiable through code 
    }*/

    private void Awake()
    {
        cooldown = 0.15f;
        this.damage = 2f;
    }

}
