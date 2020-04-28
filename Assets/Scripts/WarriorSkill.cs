using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill : MonoBehaviour
{
    CharacterMove player;
    Animator playerAnim;
    //public Camera mainCamera;
    public float slash_Cooltime = 0.5f;
    public float slash_Timer;
    public bool slashOff;
    public bool isInCombo;
    public bool comboContinue;
    public float comboTimer;

    public float skill_1_Cooltime = 3.0f;
    public float skill_1_Timer;
    public bool skill_1_Off;

    public float skill_2_Cooltime = 3.0f;
    public float skill_2_Timer;
    public bool skill_2_Off;
    void Start()
    {
        player = gameObject.GetComponent<CharacterMove>();
        playerAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Skill_Slash();
        Skill_1_play();
        Skill_2_play();
        if (slashOff)
        {
            slash_Timer += Time.deltaTime;
            if (slash_Timer >= 1.0f)
                player.isAttacking = false;
            if (slash_Timer >= slash_Cooltime)
            {
                slashOff = false;
                slash_Timer = 0;
            }
        }
        if (comboContinue)
        {
            
            comboTimer += Time.deltaTime;
        }

        if(skill_1_Off)
        {
            skill_1_Timer += Time.deltaTime;
            if (skill_1_Timer >= 1.2f)
                player.isAttacking = false;
            if(skill_1_Timer >= skill_1_Cooltime)
            {
                skill_1_Off = false;
                skill_1_Timer = 0;
            }
        }
        if (skill_2_Off)
        {
            skill_2_Timer += Time.deltaTime;
            if (skill_2_Timer >= 1.0f)
            {
                player.isAttacking = false;
                playerAnim.SetBool("Skill2_2", false);
            }
            if (skill_2_Timer >= skill_2_Cooltime)
            {
                skill_2_Off = false;
                skill_2_Timer = 0;
            }
        }
    }

    void Skill_Slash()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !slashOff)
        {
            player.isAttacking = true;
            slashOff = true;
            comboContinue = true;

            if (comboTimer > 3.0f)
            {
                playerAnim.SetInteger("Combo", 0);
                comboContinue = false;
            }
            comboTimer = 0;

            if (playerAnim.GetInteger("Combo") == 0)
                playerAnim.SetTrigger("FirstAttack");
            else if (playerAnim.GetInteger("Combo") == 1)
                playerAnim.SetTrigger("SecondAttack");
            else if (playerAnim.GetInteger("Combo") == 2)
                playerAnim.SetTrigger("ThirdAttack");



        }
        //else if(playerAnim.GetCurrentAnimatorClipInfo(0))
    }

    void Skill_1_play()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && !skill_1_Off)
        {
            skill_1_Off = true;
            StartCoroutine("Skill_1");
        }

    }

    void Skill_2_play()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2) && !skill_2_Off)
        {
            skill_2_Off = true;
            player.isAttacking = true;
            //playerAnim.SetBool("Skill2", true);
            playerAnim.SetTrigger("Skill2_1");
            playerAnim.SetBool("Skill2_2", true);
        }
    }

    IEnumerator Skill_1()
    {
        //playerAnim.stop
        player.isDashing = true;
        player.isAttacking = true;
        playerAnim.SetTrigger("Dash");
        yield return new WaitForSeconds(0.1f);
        playerAnim.SetTrigger("ThirdAttack");
    }
}
