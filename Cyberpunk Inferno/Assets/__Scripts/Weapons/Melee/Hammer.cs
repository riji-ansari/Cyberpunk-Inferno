using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MeleeWeapon
{
    void Awake()
    {
        cooldown = 0.5f;
        this.Damage = 20;
    }
}
