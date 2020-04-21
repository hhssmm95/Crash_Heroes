using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UImanager : MonoBehaviour
{
    private static UImanager instance;

    Image image;
    Image coolTime_1;
    WarriorSkill warrior;
    public GameObject player;
    

    bool cooltime = false;


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
        warrior = GameObject.Find("lp_guy").GetComponent<WarriorSkill>();
        coolTime_1 = GameObject.FindWithTag("CoolTime_Slot_1").GetComponent<Image>();
    }
    

    private void Update()
    {
        if (warrior.slashOff)
        {
            cooltime = true;
        }
        Skil_1_CoolTime_On();
    }

    void Skil_1_CoolTime_On()
    {
        
        float time = 0f;
        //coolTime_1.color = Mathf.Lerp(1f, 0f, time);

        if (cooltime)
        {
            coolTime_1.color = new Color(coolTime_1.color.r, coolTime_1.color.g, coolTime_1.color.b, 255f); //쿨타임 이미지 알파값 설정
        }

        Color fadecolor = coolTime_1.color;

        if (fadecolor.a >= 255f)
        {
            time += Time.deltaTime / warrior.slash_Cooltime;
            fadecolor.a = Mathf.Lerp(warrior.slash_Cooltime, 0f, time);
            coolTime_1.color = fadecolor;
        }
        cooltime = false;
    }

    // Skil_1_CoolTime_On()
    //{

    //    float time = 0f;
    //    //coolTime_1.color = Mathf.Lerp(1f, 0f, time);

    //    if (cooltime)
    //    {
    //        coolTime_1.color = new Color(coolTime_1.color.r, coolTime_1.color.g, coolTime_1.color.b, 255f); //쿨타임 이미지 알파값 설정
    //    }

    //    Color fadecolor = coolTime_1.color;

    //    if (fadecolor.a >= 255f)
    //    {
    //        time += Time.deltaTime / warrior.slash_Cooltime;
    //        fadecolor.a = Mathf.Lerp(warrior.slash_Cooltime, 0f, time);
    //        coolTime_1.color = fadecolor;
    //    }
    //    cooltime = false;
    //}
}
