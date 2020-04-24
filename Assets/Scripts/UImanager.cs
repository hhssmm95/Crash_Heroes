using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UImanager : MonoBehaviour
{
    private static UImanager instance;

    Image coolTime_1;
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

        if (warrior.slashOff == true)
        {
            float percent = warrior.slash_Timer / warrior.slash_Cooltime;
            coolTime_1.fillAmount = percent;
        }
        else
        {
            coolTime_1.fillAmount = 1f;
        }
    }
}