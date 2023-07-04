using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    walk,
    attack
}

//This class is attached to the GameObject that represents the player
//Ensures the player is carried over between levels

public class PlayerController : MonoBehaviour
{
    //implement Singleton pattern for PlayerController --------------------------------------------------------------------------
    private static PlayerController _instance;

    //call this method to access the Singleton instance of this class
    public static PlayerController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);


    }
    //end of Singleton setup ---------------------------------------------------------------------------------------------------

    private bool _exitReached = false;

    public static bool Pause { set; get; } = false;

    // movement variables
    private Rigidbody2D _rb;
    private Vector3 _movement;

    // attack variables
    public PlayerState currState;
    //public Weapon.WeaponType weapState;
    public Animator anim;
    public Transform attackRotation;
    public Transform rangedAttackPoint;
    private Camera _cam;
    public Weapon weapon;
    public Relic[] relics = new Relic[3];
    public SpriteRenderer weaponSprite;

    // stat variables
    [Header("Stats")]
    public float speed = 0.15f;
    public int gold = 10;
    public float maxHealth = 100;
    private float _health;
    public float Health
    {
        get
        {
            return _health;
        }
        set
        {

            _health = value;
            if(_health <= 0)
            {
                //TODO: proper player death
                //Destroy(gameObject);
                StartCoroutine(Dead());
            }
            else if (_health > maxHealth)
            {
                _health = maxHealth;
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
    private HealthBar healthBar = null;

    //control leveling system
    [Header("Leveling")]
    public int[] levelMilestones;//the <<cumulative XP>> values required for each level
    private int _level = 1; //make sure level has default value
    public int Level
    {
        get { return _level; }
        set { 
            if (value > 0) //level must be integer that is >=0
            {
                _level = value;
                _xp = levelMilestones[_level-1];//set the appropriate XP for the new level
            }
        }
    }
    private int _xp=0;
    public int XP
    {
        get
        {
            return _xp;
        }
        set
        {
            _xp = value;
            if (_xp >= levelMilestones[_level - 1] && Level<levelMilestones.Length)
            {
                //print("LEVEL UP" +XP);
                _level++;
                //allow the player to choose a new skill upon levelling up
                _skillPoints++;
            }

        }
    }

    //end of leveling system variables

    //~~~SKILL TREE SYSTEM~~~
    public int _skillPoints = 0;    //this saves the player's number of available skill points that are able to be spent
    public Skill[] equippedSkills = new Skill[3]; //saves the skills that the player currently has selected
  

    // TODO: other equipment effect variables, attack damage currently defined on projectile/meleeweapon classes, have to decide whether to keep them there or move them

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _cam = Camera.main;

        _health = maxHealth;
        healthBar = transform.Find("Health Bar").GetComponent<HealthBar>();
    }

    void Update()
    {
        //dont do anything if player has been frozen
        if (!Pause)
        {

            MoveCharacter();
            MouseInput();
            //if (currState == PlayerState.walk) // freeze mouse movement when melee attacking
            //{
            //MouseInput(); // grab mouse input and rotate weapon
            //}

            // initialize attack based on weapon equipped
            if (Input.GetMouseButton(0) && weapon != null)
            {
                weapon.Attack();
                /*if (weapState == Weapon.WeaponType.melee)
                {
                    StartCoroutine(MeleeAttack());
                    
                }
                else if (weapState == Weapon.WeaponType.ranged) //check for both incase of an error
                {
                    StartCoroutine(RangedAttack());
                }*/
            }

            /*if(Input.GetKey("q")) // swap from ranged to melee or vice versa
            {
                SwapWeapon();
            }*/ //Commented this out. Weapon swapping will be done through the inventory for now.

            /*if (currState == PlayerState.walk) // prevents player from attacking and moving at the same time
            {
                MoveCharacter();
            }*/ //Commented this out for now. Why not allow both?
        }
    }
    
    public void removeRelic(Relic relic) //removes a relic from the array
    {
        for (int i = 0; i < relics.Length; i++)
        {
            if  (relic.Equals(relics[i]))
            {
                relics[i] = null;
                return;
            }
        }
    }

    public void addRelic(Relic relic) //adds a relic to the array
    {
        for (int i = 0; i < relics.Length; i++)
        {
            if (relics[i] == null)
            {
                relics[i] = relic;
                return;
            }
        }
    }

    private void MoveCharacter()
    {
        // grab multidirectional input
        _movement = Vector3.zero;
        _movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);

        if (_movement != Vector3.zero)
        {
            _movement.Normalize(); // prevents speed bonus from moving diagonally
            _rb.MovePosition(transform.position + _movement * speed * Relic.calculatePlayerSpeedMult(relics) * SkillMenuController.getMult(SkillMenuController.SkillMult.Speed));
            anim.SetBool("moving", true); // play walking animation
            anim.SetFloat("moveY", _movement.y);
            anim.SetFloat("moveX", _movement.x);
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }

    /*private void SwapWeapon() // swap to melee or ranged weapon
    {
        if (weapState == Weapon.WeaponType.melee) 
        { 
            weapState = Weapon.WeaponType.ranged; // add code later to disable melee weapon sprite and enable ranged weapon sprite
        }
        else 
        { 
            weapState = Weapon.WeaponType.melee; // add code later to disable ranged weapon sprite and enable melee weapon sprite
        }
    }*/

    private void MouseInput() // grab mouse input
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = _cam.WorldToScreenPoint(transform.localPosition);
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        attackRotation.rotation = Quaternion.Euler(0, 0, angle);

        if (mousePos.x < screenPoint.x) // flip character model when mouse moves to -ve x coords
        {
            //transform.localScale = new Vector3(-1f, 1f, 1f);
            //attackRotation.localScale = new Vector3(-1f, 1f, 1f);
        }
        else // flip back when +ve x coords
        {
            //transform.localScale = Vector3.one;
            //attackRotation.localScale = Vector3.one;
        }
    }
    /*private IEnumerator MeleeAttack()
    {
        currState = PlayerState.attack;
        _anim.SetBool("attacking", true); // swing weapon and activate collider with animation
        yield return null;
        _anim.SetBool("attacking", false);
        yield return new WaitForSeconds(weapon.cooldown);
        currState = PlayerState.walk;
    }

    private IEnumerator RangedAttack()
    {
        currState = PlayerState.attack;
        // prevent walking animation while firing ranged weapon
        yield return null;
        Instantiate(projectile, rangedAttackPoint.position, rangedAttackPoint.rotation);
        yield return new WaitForSeconds(weapon.cooldown);
        _anim.SetBool("moving", true);
        currState = PlayerState.walk;
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Exit")
        {
            _exitReached = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if player touches an object in the game world
        if (collision.gameObject.tag == "Item")
        {
            //call the PLayer Inventory Object to add it to inventory
            PlayerInventoryController.Instance.pickupObject(collision.gameObject);
        }
    }

    public bool atExit()
    {
        if (_exitReached)
        {
            _exitReached = false; //reset exitReached right away sogame doesnt get stuck in a loop of loading the level
            return true;
        }
        return false;
    }

    public void hitPlayer(float damage)
    {
        Health = Health - (damage * Relic.calculateDamageTakenMult(relics) * SkillMenuController.getMult(SkillMenuController.SkillMult.DamageTaken));
       
    }

    IEnumerator Dead()
    {
        // Play death animation, freeze player movement and wait for 5 seconds
        anim.SetBool("isDead", true);
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;
        yield return new WaitForSeconds(5);

        SceneManager.LoadScene("Level 1");
        HUDController.Instance.updateLevelDisplay("Level 1");

        //grant full health and update health bar
        _health = maxHealth;
        healthBar.UpdateHealthBar(_health / maxHealth);

        //reset exp and level
        _xp = 0;
        _level = 1;

        //reset purchased skills, equipped skills and skill points
        equippedSkills = new Skill[3];
        SkillMenuController.Instance.enableAll(false);
        _skillPoints = 0;

        //reset the player's invantory, unequip weapons and relics
        PlayerInventoryController.Instance.reset();
        weapon = null;
        weaponSprite.sprite = null;
        for(int a=0; a < relics.Length; a++)
        {
            relics[a] = null;
        }

        //reset the shop's products and player gold
        ShopController.Instance.populateProducts();
        gold = 10;

        // Unfreeze player movement, transition to idle animation
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        anim.SetBool("isDead", false);
        transform.position = Vector3.zero;

    }

}
