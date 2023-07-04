using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    //class that defines a skill

        public string _name;       //the name of the skill that the player will see
        public int _reqPoints;     //the required number of points needed to unlock the skill
        public string _effect;     //description of the skill's effect
        public bool _enabled;      //states if the player has bought the skill

        public Sprite sprite;

    //Multipliers for stat boost skills
    public float playerMovementSpeedMult = 1f;
    public float playerBulletVelocityMult = 1f;
    public float playerDamageMult = 1f;
    public float playerDamageTakenMult = 1f;
    public float playerXPGainMult = 1f;

    //constructor for a skill
    //@param name       name of skill that player sees
    //@param points     number of points needed to unlock
    //@param effect     description of the skill's effect
    public Skill(string name, int points, string effect, Sprite sprite, float speedMult = 1f, float bulletVelocity = 1f, float damageMult = 1f, float damageTakenMult = 1f, float XPGainMult = 1f)
    {
        _name = name;
        _reqPoints = points;
        _effect = effect;
        _enabled = false;       //skills begin as not enabled until player has bought them

        this.sprite = sprite;

        playerMovementSpeedMult = speedMult;
        playerBulletVelocityMult = bulletVelocity;
        playerDamageMult = damageMult;
        playerDamageTakenMult = damageTakenMult;
        playerXPGainMult = XPGainMult;
    }

    //enables a skill by checking if it is available to be bought, and checks if the player has enough skill points to buy
    public bool unlock()
        {
            if (PlayerController.Instance._skillPoints >= _reqPoints)
            {
                PlayerController.Instance._skillPoints -= _reqPoints;
                _enabled = true;
            }
            return _enabled;
        }
    
}
