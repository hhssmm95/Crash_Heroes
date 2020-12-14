using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    private static UImanager instance;

    public CharacterMove player;
    public bool playerCheck;
    public bool initComplete;

    public Image slot1;
    public Image slot2;
    public Image slot3;
    public Image slot4;
    public Image slot5;
    public Image dashImage;

    GameObject hpOrb;
    GameObject mpOrb;
    GameObject stBar;
    //WarriorSkill wSkill;
    //ArcherSkill aSkill;
    //DragoonSkill dSkill;
    //MageSkill mSkill;

    

    public static UImanager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<UImanager>();

                if (obj != null)
                {
                    instance = obj;
                }

                else
                {
                    var newSingleton = new GameObject("Singleton Class").AddComponent<UImanager>();

                    instance = newSingleton;
                }
            }
            return instance;
        }

        set
        {
            instance = value;
        }
    }


    private void Awake()
    {

        var objs = FindObjectsOfType<UImanager>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);

            return;
        }
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMove>();

        //warrior = GameObject.Find("M05").GetComponent<WarriorSkill>();
        slot1 = GameObject.FindWithTag("CoolTime_Slot_1").GetComponent<Image>();
        slot2 = GameObject.FindWithTag("CoolTime_Slot_2").GetComponent<Image>();
        slot3 = GameObject.FindWithTag("CoolTime_Slot_3").GetComponent<Image>();
        slot4 = GameObject.FindWithTag("CoolTime_Slot_4").GetComponent<Image>();
        slot5 = GameObject.FindWithTag("CoolTime_Slot_5").GetComponent<Image>();
        dashImage = GameObject.FindWithTag("DashImage").GetComponent<Image>();
        hpOrb = GameObject.FindWithTag("hpOrb");
        mpOrb = GameObject.FindWithTag("mpOrb");
        stBar = GameObject.FindWithTag("stOrb");
        hpOrb.GetComponent<Image>().material.SetFloat("PositionUV_Y_1", -1.6f);
        mpOrb.GetComponent<Image>().material.SetFloat("PositionUV_Y_1", -1.6f);
        stBar.GetComponent<Image>().material.SetFloat("PositionUV_X_1", -1.09f);
        //hpOrb.GetComponent<Renderer>().material.SetFloat("PositionUV_Y_1", -1.6f);
        //mpOrb.GetComponent<Renderer>().material.SetFloat("PositionUV_Y_1", -1.6f);


    }
    
    void hpControl()
    {
        float percent = player.hp / player.maxHP;
        float result = percent * -3.2f + 1.6f;
        hpOrb.GetComponent<Image>().material.SetFloat("PositionUV_Y_1", result);
        //hpOrb.fillAmount = percent * -1.6f;
    }
    void mpControl()
    {
        float percent = player.mp / player.maxMP;
        float result = percent * -3.2f + 1.6f;
        mpOrb.GetComponent<Image>().material.SetFloat("PositionUV_Y_1", result);
        //mpOrb.fillAmount = percent;
    }
    void stControl()
    {
        float percent = player.st / player.maxST;
        float result = percent * -2.18f + 1.09f;
        stBar.GetComponent<Image>().material.SetFloat("PositionUV_X_1", result);
    }

    private void Update()
    {
        if (playerCheck && !initComplete)
        {
            initComplete = true;

            //skill = player.gameObject.GetComponent<SkillControl>();

            switch (player.job)
            {
                case Global.Classes.Warrior:
                    slot1.sprite = Resources.Load("Images/Skill_Icon/Warriorskill_01", typeof(Sprite)) as Sprite;
                    slot2.sprite = Resources.Load("Images/Skill_Icon/Warriorskill_02", typeof(Sprite)) as Sprite;
                    slot3.sprite = Resources.Load("Images/Skill_Icon/Warriorskill_03", typeof(Sprite)) as Sprite;
                    slot4.sprite = Resources.Load("Images/Skill_Icon/Warriorskill_04", typeof(Sprite)) as Sprite;
                    slot5.sprite = Resources.Load("Images/Skill_Icon/Warriorskill_05", typeof(Sprite)) as Sprite;
                    break;

                case Global.Classes.Archer:

                    //aSkill = player.gameObject.GetComponent<ArcherSkill>();
                    slot1.sprite = Resources.Load("Images/Skill_Icon/Archerskill_01", typeof(Sprite)) as Sprite;
                    slot2.sprite = Resources.Load("Images/Skill_Icon/Archerskill_02", typeof(Sprite)) as Sprite;
                    slot3.sprite = Resources.Load("Images/Skill_Icon/Archerskill_03", typeof(Sprite)) as Sprite;
                    slot4.sprite = Resources.Load("Images/Skill_Icon/Archerskill_04", typeof(Sprite)) as Sprite;
                    slot5.sprite = Resources.Load("Images/Skill_Icon/Archerskill_05", typeof(Sprite)) as Sprite;
                    break;

                case Global.Classes.Dragoon:
                    slot1.sprite = Resources.Load("Images/Skill_Icon/Dragoonskill_01", typeof(Sprite)) as Sprite;
                    slot2.sprite = Resources.Load("Images/Skill_Icon/Dragoonskill_02", typeof(Sprite)) as Sprite;
                    slot3.sprite = Resources.Load("Images/Skill_Icon/Dragoonskill_03", typeof(Sprite)) as Sprite;
                    slot4.sprite = Resources.Load("Images/Skill_Icon/Dragoonskill_04", typeof(Sprite)) as Sprite;
                    slot5.sprite = Resources.Load("Images/Skill_Icon/Dragoonskill_05", typeof(Sprite)) as Sprite;
                    break;

                case Global.Classes.Mage:
                    slot1.sprite = Resources.Load("Images/Skill_Icon/Mageskill_01", typeof(Sprite)) as Sprite;
                    slot2.sprite = Resources.Load("Images/Skill_Icon/Mageskill_02", typeof(Sprite)) as Sprite;
                    slot3.sprite = Resources.Load("Images/Skill_Icon/Mageskill_03", typeof(Sprite)) as Sprite;
                    slot4.sprite = Resources.Load("Images/Skill_Icon/Mageskill_04", typeof(Sprite)) as Sprite;
                    slot5.sprite = Resources.Load("Images/Skill_Icon/Mageskill_05", typeof(Sprite)) as Sprite;
                    break;

                case Global.Classes.Lena:
                    slot1.sprite = Resources.Load("Images/Skill_Icon/Lenaskill_01", typeof(Sprite)) as Sprite;
                    slot2.sprite = Resources.Load("Images/Skill_Icon/Lenaskill_02", typeof(Sprite)) as Sprite;
                    slot3.sprite = Resources.Load("Images/Skill_Icon/Lenaskill_03", typeof(Sprite)) as Sprite;
                    slot4.sprite = Resources.Load("Images/Skill_Icon/Lenaskill_04", typeof(Sprite)) as Sprite;
                    slot5.sprite = Resources.Load("Images/Skill_Icon/Lenaskill_05", typeof(Sprite)) as Sprite;
                    break;
            }
        }


        

        if (initComplete)
        {
            if (player.skill_1_Off == true)
            {
                float percent = player.skill_1_Timer / player.skill_1_Cooltime;
                slot1.fillAmount = percent;
            }
            else
            {
                slot1.fillAmount = 1f;
            }

            if (player.skill_2_Off == true)
            {
                float percent = player.skill_2_Timer / player.skill_2_Cooltime;
                slot2.fillAmount = percent;
            }
            else
            {
                slot2.fillAmount = 1f;
            }

            if (player.skill_3_Off == true)
            {
                float percent = player.skill_3_Timer / player.skill_3_Cooltime;
                slot3.fillAmount = percent;
            }
            else
            {
                slot3.fillAmount = 1f;
            }

            if (player.skill_4_Off == true)
            {
                float percent = player.skill_4_Timer / player.skill_4_Cooltime;
                slot4.fillAmount = percent;
            }
            else
            {
                slot4.fillAmount = 1f;
            }

            if (player.skill_5_Off == true)
            {
                float percent = player.skill_5_Timer / player.skill_5_Cooltime;
                slot5.fillAmount = percent;
            }
            else
            {
                slot5.fillAmount = 1f;
            }

            //if(player.dashoff == true)
            //{
            //    dashImage.enabled = true;
            //}
            //else
            //{
            //    dashImage.enabled = false;
            //}

            hpControl();
            mpControl();
            stControl();
        }
    }
}