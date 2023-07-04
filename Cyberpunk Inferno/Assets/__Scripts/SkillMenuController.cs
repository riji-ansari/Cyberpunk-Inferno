using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SkillMenuController : MonoBehaviour
{
    //implement Singleton pattern for SkillMenu --------------------------------------------------------------------------
    private static SkillMenuController _instance;

    //call this method to access the Singleton instance of this class
    public static SkillMenuController Instance { get { return _instance; } }


    public enum SkillMult
    {
        Speed,
        DamageDealt,
        DamageTaken,
        XPGained,
        BulletVelocity
    }

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

    //gameobjects from the UI that must be referenced in the script
    [Header("Active Elements (DON'T LEAVE NULL)")]
    public GameObject mainCanvas = null;
    public GameObject skillPoints = null;
    public GameObject equippedParent = null;
    public GameObject availableParent = null;
    public GameObject skillSprites = null;

    private int numSkills;
    public LinkedList<Skill> skillList = new LinkedList<Skill>();  //this saves the list of all skills that exist in the game

    void Start()
    {
        getSkills("skillInfo.txt");
        numSkills =skillList.Count;
        updateSkillPoints();
    }

    //reads in the list of skills from the file and saves the data in a linked list
    //  @param fileName         name of file that includes the skill info
    void getSkills(string fileName)
    {
        string path = "Assets/Data/" + fileName;            //path to the file            
        StreamReader fileReader = new StreamReader(path);   //create a file stream to read text from the file
        string line = line = fileReader.ReadLine(); ;       //create a string to save each line read from the file and read the first line
        int numSkills = int.Parse(line);                    //takes the first line of the file which states how many skills are in the file
        Sprite sprite = null;

        //reads each line, saves it into a skill class, and adds the skill to the skill list
        for (int i = 0; i < numSkills; i++)
        {
            line = fileReader.ReadLine();                                       //reads in the next line
            string[] text = line.Split(';');                                    //split the text to separate each variable

            sprite = getSprite(i);
            if (sprite == null)
            {
                sprite = getSprite(0);
            }

            Skill skill = new Skill(text[0], 
                int.Parse(text[1]), 
                text[2], 
                sprite, 
                float.Parse(text[3]), 
                float.Parse(text[4]), 
                float.Parse(text[5]), 
                float.Parse(text[6]), 
                float.Parse(text[7]));//creates the skill with the info
            skillList.AddLast(skill);                                           //add the skill to the list
        }

        fileReader.Close();   //close the file stream
    }

    public void enableAll(bool enable)
    {
        foreach (Skill skill in skillList)
        {
            skill._enabled = enable;
        }
        if (!enable)
        {
            for (int i = 0; i < equippedParent.transform.childCount; i++)
            {
                equippedParent.transform.GetChild(i).GetComponent<SkillSlot>().removeSkill();
            }
        }

    }

    public void setActive(bool active)
    {
        populateAvailable();

        //call method to disable/enable menu
        mainCanvas.SetActive(active);

        updateSkillPoints();
    }

    public void populateAvailable()
    {
        LinkedListNode<Skill> node = skillList.First;
        Skill skill;

        for (int a = 0; a < numSkills; a++)
        {
            skill = node.Value;
            if (!skill._enabled)
            {
                availableParent.transform.GetChild(a).GetComponent<SkillSlot>().addSkill(skill);
            }
            node = node.Next;
        }
    }

    public Sprite getSprite(int index)
    {
        int count = skillSprites.transform.childCount;
        if (index + 1 >= count)
        {
            return null;
        }

        return skillSprites.transform.GetChild(index).GetComponent<SpriteRenderer>().sprite;
    }

    //assume transfer is being done from available to equipped
    public bool transferSkill(SkillSlot slot)
    {
        Skill[] equipped = PlayerController.Instance.equippedSkills;

        for (int a=0; a< equipped.Length; a++)
        {
            if (equipped[a] == slot.storedSkill)
            {
                return false;
            }
        }

        for(int i=0; i< equippedParent.transform.childCount; i++)
        {
            SkillSlot equipSlot= equippedParent.transform.GetChild(i).GetComponent<SkillSlot>();
            if (equipSlot.storedSkill == null)
            {
                equipSlot.addSkill(slot.storedSkill);
                equipped[i] = slot.storedSkill;

                updateSkillPoints();

                return true;
            }

        }
        return false;
    }

    public void updateSkillPoints()
    {
        skillPoints.GetComponent<Text>().text =""+PlayerController.Instance._skillPoints;
    }


    public static float getMult(SkillMult selection) //finds the end multiplier through all equipped skills of a given stat
    {
        Skill[] equipped = PlayerController.Instance.equippedSkills;
        float endMult = 1f;
        for (int i = 0; i < equipped.Length; i++)
        {
            Skill skill = equipped[i];

            if (equipped[i] != null)
            {
                switch (selection)
                {
                    case SkillMult.BulletVelocity:
                        endMult *= equipped[i].playerBulletVelocityMult;
                        break;

                    case SkillMult.DamageDealt:
                        endMult *= equipped[i].playerDamageMult;
                        break;

                    case SkillMult.DamageTaken:
                        endMult *= equipped[i].playerDamageTakenMult;
                        break;

                    case SkillMult.Speed:
                        endMult *= equipped[i].playerMovementSpeedMult;
                        break;

                    case SkillMult.XPGained:
                        endMult *= equipped[i].playerXPGainMult;
                        break;

                }
            }

        }
        return endMult;
    }

}
