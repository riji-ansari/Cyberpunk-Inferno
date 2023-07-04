using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    /*public Weapon(float damage, float cooldown, WeaponType type = WeaponType.melee)
    {
        this.damage = damage;
        this.cooldown = cooldown;
        this.weaponType = type;
    }*/

    public float Damage //automatically factors in relic damage for every weapon this way
    {
        get
        {
            return damage * Relic.calculateDamageDealtMult(PlayerController.Instance.relics) * SkillMenuController.getMult(SkillMenuController.SkillMult.DamageDealt);
        }
        set
        {
            damage = value;
        }
    }
    protected float damage = 10f;
    public float cooldown { get; set; } = 0.75f;
    protected bool canAttack { get; set; } = true;
    public WeaponType weaponType { get; set; } = WeaponType.melee;
    public enum WeaponType
    {
        melee,
        ranged
    }

    public abstract void Attack();
    public abstract void MeleeHit(Collider2D other);

    private void ActivateCooldown()
    {
        canAttack = true;
    }
}
