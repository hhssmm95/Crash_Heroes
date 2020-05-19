using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassParent : MonoBehaviour
{
    public CharacterMove player;
    public Animator playerAnim;
    //public Camera mainCamera;



    public float attack_Cooltime;
    public float attack_Timer;
    public bool attackOff;
    public bool isInCombo;
    public bool comboContinue;
    public float comboTimer;

    public float skill_1_Cooltime;
    public float skill_1_Timer;
    public bool skill_1_Off;
    public float skill_1_cost;

    public float skill_2_Cooltime;
    public float skill_2_Timer;
    public bool skill_2_Off;
    public float skill_2_cost;

    public float skill_3_Cooltime;
    public float skill_3_Timer;
    public bool skill_3_Off;
    public float skill_3_cost;

    public float skill_4_Cooltime;
    public float skill_4_Timer;
    public bool skill_4_Off;
    public float skill_4_cost;
}
