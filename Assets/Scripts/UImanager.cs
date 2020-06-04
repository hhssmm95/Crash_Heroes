using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    private static UImanager instance;

    public GameObject MenuUI;

    bool isMenuUI = false;

    CharacterMove player;

    Image slot1;
    Image slot2;
    Image slot3;
    Image slot4;
    Image slot5;

    Image hpOrb;
    Image mpOrb;
    SkillControl skill;
    //ClassParent characterClass;

    public Text text_Time;
    public float LimitTime;

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
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMove>();

        skill = player.GetComponent<SkillControl>();
        //warrior = GameObject.Find("M05").GetComponent<WarriorSkill>();
        slot1 = GameObject.FindWithTag("CoolTime_Slot_1").GetComponent<Image>();
        slot2 = GameObject.FindWithTag("CoolTime_Slot_2").GetComponent<Image>();
        slot3 = GameObject.FindWithTag("CoolTime_Slot_3").GetComponent<Image>();
        slot4 = GameObject.FindWithTag("CoolTime_Slot_4").GetComponent<Image>();
        slot5 = GameObject.FindWithTag("CoolTime_Slot_5").GetComponent<Image>();
        text_Time = GameObject.FindWithTag("Timer").GetComponent<Text>();
        hpOrb = GameObject.FindWithTag("hpOrb").GetComponent<Image>();
        mpOrb = GameObject.FindWithTag("mpOrb").GetComponent<Image>();

        switch(player.job)
        {
            case Global.Classes.Warrior:
                slot1.sprite = Resources.Load("500_skill_icons/Skill_standart/Warriorskill_02", typeof(Sprite)) as Sprite;
                slot2.sprite = Resources.Load("500_skill_icons/Skill_standart/Warriorskill_21", typeof(Sprite)) as Sprite;
                slot3.sprite = Resources.Load("500_skill_icons/Skill_standart/Warlock_19", typeof(Sprite)) as Sprite;
                slot4.sprite = Resources.Load("500_skill_icons/Skill_standart/Warriorskill_44", typeof(Sprite)) as Sprite;
                slot5.sprite = Resources.Load("500_skill_icons/Skill_standart/Archerskill_15", typeof(Sprite)) as Sprite;
                break;

            case Global.Classes.Archer:
                slot1.sprite = Resources.Load("500_skill_icons/Skill_standart/Archerskill_33", typeof(Sprite)) as Sprite;
                slot2.sprite = Resources.Load("500_skill_icons/Skill_standart/Archerskill_02", typeof(Sprite)) as Sprite;
                slot3.sprite = Resources.Load("500_skill_icons/Skill_standart/Archerskill_05", typeof(Sprite)) as Sprite;
                slot4.sprite = Resources.Load("500_skill_icons/Skill_standart/Archerskill_03", typeof(Sprite)) as Sprite;
                slot5.sprite = Resources.Load("500_skill_icons/Skill_standart/Archerskill_19", typeof(Sprite)) as Sprite;
                break;

            case Global.Classes.Dragoon:
                slot1.sprite = Resources.Load("500_skill_icons/Skill_standart/Druideskill_44", typeof(Sprite)) as Sprite;
                slot2.sprite = Resources.Load("500_skill_icons/Skill_standart/Druideskill_23", typeof(Sprite)) as Sprite;
                slot3.sprite = Resources.Load("500_skill_icons/Skill_standart/Druideskill_50", typeof(Sprite)) as Sprite;
                slot4.sprite = Resources.Load("500_skill_icons/Skill_standart/Warriorskill_42", typeof(Sprite)) as Sprite;
                slot5.sprite = Resources.Load("500_skill_icons/Skill_standart/Paladinskill_28", typeof(Sprite)) as Sprite;
                break;
        }
    }
    
    void hpControl()
    {
        float percent = player.hp / player.maxHP;
        hpOrb.fillAmount = percent;
    }
    void mpControl()
    {
        float percent = player.mp / player.maxMP;
        mpOrb.fillAmount = percent;
    }

    private void Update()
    {

        LimitTime -= Time.deltaTime;
        text_Time.text = "" + Mathf.Round(LimitTime);

        if(LimitTime <= 0)
        {
            text_Time.text = "0";
        }

        if (skill.skill_1_Off == true)
        {
            float percent = skill.skill_1_Timer / skill.skill_1_Cooltime;
            slot1.fillAmount = percent;
        }
        else
        {
            slot1.fillAmount = 1f;
        }

        if (skill.skill_2_Off == true)
        {
            float percent = skill.skill_2_Timer / skill.skill_2_Cooltime;
            slot2.fillAmount = percent;
        }
        else
        {
            slot2.fillAmount = 1f;
        }

        if (skill.skill_3_Off == true)
        {
            float percent = skill.skill_3_Timer / skill.skill_3_Cooltime;
            slot3.fillAmount = percent;
        }
        else
        {
            slot3.fillAmount = 1f;
        }

        if (skill.skill_4_Off == true)
        {
            float percent = skill.skill_4_Timer / skill.skill_4_Cooltime;
            slot4.fillAmount = percent;
        }
        else
        {
            slot4.fillAmount = 1f;
        }

        if (skill.skill_5_Off == true)
        {
            float percent = skill.skill_5_Timer / skill.skill_5_Cooltime;
            slot5.fillAmount = percent;
        }
        else
        {
            slot5.fillAmount = 1f;
        }

        hpControl();
        mpControl();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (isMenuUI == false)
            {
                MenuUI.SetActive(true);
                isMenuUI = true;
            }

            else
            {
                MenuUI.SetActive(false);
                isMenuUI = false;
            }
        }
    }
}