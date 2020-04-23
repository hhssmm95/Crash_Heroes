using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTime : MonoBehaviour
{
    Image image;
    WarriorSkill warrior;

    bool cooltime = false;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        warrior = GameObject.Find("M05").GetComponent<WarriorSkill>();
    }

    // Update is called once per frame
    void Update()
    {
        if (warrior.slashOff == true)
        {
            float percent = warrior.slash_Timer / warrior.slash_Cooltime;
            image.fillAmount = percent;
        }
        else
        {
            image.fillAmount = 1f;
        }
    }

}
