using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UImanager : MonoBehaviour
{
    private static UImanager instance;

    Image coolTime_1;
    Image coolTime_2;
    //Image coolTime_3;
    //Image coolTime_4;
    WarriorSkill warrior;

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
        warrior = GameObject.Find("M05").GetComponent<WarriorSkill>();
        coolTime_1 = GameObject.FindWithTag("CoolTime_Slot_1").GetComponent<Image>();
        coolTime_2 = GameObject.FindWithTag("CoolTime_Slot_2").GetComponent<Image>();
        //coolTime_3 = GameObject.FindWithTag("CoolTime_Slot_3").GetComponent<Image>();
        //coolTime_4 = GameObject.FindWithTag("CoolTime_Slot_4").GetComponent<Image>();
        text_Time = GameObject.FindWithTag("Timer").GetComponent<Text>();
    }
    

    private void Update()
    {
        LimitTime -= Time.deltaTime;
        text_Time.text = "" + Mathf.Round(LimitTime);

        if(LimitTime <= 0)
        {
            text_Time.text = "0";
        }

        if (warrior.skill_1_Off == true)
        {
            float percent = warrior.skill_1_Timer / warrior.skill_1_Cooltime;
            coolTime_1.fillAmount = percent;
        }
        else
        {
            coolTime_1.fillAmount = 1f;
        }

        if (warrior.skill_2_Off == true)
        {
            float percent = warrior.skill_2_Timer / warrior.skill_2_Cooltime;
            coolTime_2.fillAmount = percent;
        }
        else
        {
            coolTime_2.fillAmount = 1f;
        }

        //if (warrior.skill_3_Off == true)
        //{
        //    float percent = warrior.skill_3_Timer / warrior.skill_3_Cooltime;
        //    coolTime_3.fillAmount = percent;
        //}
        //else
        //{
        //    coolTime_3.fillAmount = 1f;
        //}

        //if (warrior.skill_4_Off == true)
        //{
        //    float percent = warrior.skill_4_Timer / warrior.skill_4_Cooltime;
        //    coolTime_4.fillAmount = percent;
        //}
        //else
        //{
        //    coolTime_4.fillAmount = 1f;
        //}
    }
}