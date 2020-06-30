using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    private static UImanager instance;

    public GameObject MenuUI;

    bool isMenuUI = false;

    public CharacterMove player;
    public bool playerCheck;
    bool initComplete;

    Image slot1;
    Image slot2;
    Image slot3;
    Image slot4;
    Image slot5;

    Image hpOrb;
    Image mpOrb;
    WarriorSkill wSkill;
    //ArcherSkill aSkill;
    //DragoonSkill dSkill;
    //MageSkill mSkill;
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
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMove>();

        //warrior = GameObject.Find("M05").GetComponent<WarriorSkill>();
        slot1 = GameObject.FindWithTag("CoolTime_Slot_1").GetComponent<Image>();
        slot2 = GameObject.FindWithTag("CoolTime_Slot_2").GetComponent<Image>();
        slot3 = GameObject.FindWithTag("CoolTime_Slot_3").GetComponent<Image>();
        slot4 = GameObject.FindWithTag("CoolTime_Slot_4").GetComponent<Image>();
        slot5 = GameObject.FindWithTag("CoolTime_Slot_5").GetComponent<Image>();
        text_Time = GameObject.FindWithTag("Timer").GetComponent<Text>();
        hpOrb = GameObject.FindWithTag("hpOrb").GetComponent<Image>();
        mpOrb = GameObject.FindWithTag("mpOrb").GetComponent<Image>();

        
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
        if (playerCheck && !initComplete)
        {
            initComplete = true;

            //skill = player.gameObject.GetComponent<SkillControl>();

            switch (player.job)
            {
                case Global.Classes.Warrior:
                    wSkill = player.gameObject.GetComponent<WarriorSkill>();
                    slot1.sprite = Resources.Load("500_skill_icons/Skill_standart/Warriorskill_02", typeof(Sprite)) as Sprite;
                    slot2.sprite = Resources.Load("500_skill_icons/Skill_standart/Warriorskill_21", typeof(Sprite)) as Sprite;
                    slot3.sprite = Resources.Load("500_skill_icons/Skill_standart/Warlock_19", typeof(Sprite)) as Sprite;
                    slot4.sprite = Resources.Load("500_skill_icons/Skill_standart/Warriorskill_44", typeof(Sprite)) as Sprite;
                    slot5.sprite = Resources.Load("500_skill_icons/Skill_standart/Archerskill_15", typeof(Sprite)) as Sprite;
                    break;

                case Global.Classes.Archer:

                    //aSkill = player.gameObject.GetComponent<ArcherSkill>();
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


        LimitTime -= Time.deltaTime;
        text_Time.text = "" + Mathf.Round(LimitTime);

        if(LimitTime <= 0)
        {
            text_Time.text = "0";
        }
        switch(player.job)
        {
            case Global.Classes.Warrior:
                if (wSkill.skill_1_Off == true)
                {
                    float percent = wSkill.skill_1_Timer / wSkill.skill_1_Cooltime;
                    slot1.fillAmount = percent;
                }
                else
                {
                    slot1.fillAmount = 1f;
                }

                if (wSkill.skill_2_Off == true)
                {
                    float percent = wSkill.skill_2_Timer / wSkill.skill_2_Cooltime;
                    slot2.fillAmount = percent;
                }
                else
                {
                    slot2.fillAmount = 1f;
                }

                if (wSkill.skill_3_Off == true)
                {
                    float percent = wSkill.skill_3_Timer / wSkill.skill_3_Cooltime;
                    slot3.fillAmount = percent;
                }
                else
                {
                    slot3.fillAmount = 1f;
                }

                if (wSkill.skill_4_Off == true)
                {
                    float percent = wSkill.skill_4_Timer / wSkill.skill_4_Cooltime;
                    slot4.fillAmount = percent;
                }
                else
                {
                    slot4.fillAmount = 1f;
                }

                if (wSkill.skill_5_Off == true)
                {
                    float percent = wSkill.skill_5_Timer / wSkill.skill_5_Cooltime;
                    slot5.fillAmount = percent;
                }
                else
                {
                    slot5.fillAmount = 1f;
                }
                break;

            case Global.Classes.Archer:
            //if (aSkill.skill_1_Off == true)
            //{
            //    float percent = aSkill.skill_1_Timer / aSkill.skill_1_Cooltime;
            //    slot1.fillAmount = percent;
            //}
            //else
            //{
            //    slot1.fillAmount = 1f;
            //}

            //if (aSkill.skill_2_Off == true)
            //{
            //    float percent = aSkill.skill_2_Timer / aSkill.skill_2_Cooltime;
            //    slot2.fillAmount = percent;
            //}
            //else
            //{
            //    slot2.fillAmount = 1f;
            //}

            //if (aSkill.skill_3_Off == true)
            //{
            //    float percent = aSkill.skill_3_Timer / aSkill.skill_3_Cooltime;
            //    slot3.fillAmount = percent;
            //}
            //else
            //{
            //    slot3.fillAmount = 1f;
            //}

            //if (aSkill.skill_4_Off == true)
            //{
            //    float percent = aSkill.skill_4_Timer / aSkill.skill_4_Cooltime;
            //    slot4.fillAmount = percent;
            //}
            //else
            //{
            //    slot4.fillAmount = 1f;
            //}

            //if (aSkill.skill_5_Off == true)
            //{
            //    float percent = aSkill.skill_5_Timer / aSkill.skill_5_Cooltime;
            //    slot5.fillAmount = percent;
            //}
            //else
            //{
            //    slot5.fillAmount = 1f;
            //}
            break;

            case Global.Classes.Dragoon:
                //if (dSkill.skill_1_Off == true)
                //{
                //    float percent = dSkill.skill_1_Timer / dSkill.skill_1_Cooltime;
                //    slot1.fillAmount = percent;
                //}
                //else
                //{
                //    slot1.fillAmount = 1f;
                //}

                //if (dSkill.skill_2_Off == true)
                //{
                //    float percent = dSkill.skill_2_Timer / dSkill.skill_2_Cooltime;
                //    slot2.fillAmount = percent;
                //}
                //else
                //{
                //    slot2.fillAmount = 1f;
                //}

                //if (dSkill.skill_3_Off == true)
                //{
                //    float percent = dSkill.skill_3_Timer / dSkill.skill_3_Cooltime;
                //    slot3.fillAmount = percent;
                //}
                //else
                //{
                //    slot3.fillAmount = 1f;
                //}

                //if (dSkill.skill_4_Off == true)
                //{
                //    float percent = dSkill.skill_4_Timer / dSkill.skill_4_Cooltime;
                //    slot4.fillAmount = percent;
                //}
                //else
                //{
                //    slot4.fillAmount = 1f;
                //}

                //if (dSkill.skill_5_Off == true)
                //{
                //    float percent = dSkill.skill_5_Timer / dSkill.skill_5_Cooltime;
                //    slot5.fillAmount = percent;
                //}
                //else
                //{
                //    slot5.fillAmount = 1f;
                //}
                break;

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