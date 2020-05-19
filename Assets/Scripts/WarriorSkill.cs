using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill : ClassParent
{
    

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
        Skill_3_play();
        Skill_4_play();
        if (attackOff)
        {
            attack_Timer += Time.deltaTime;
            if (attack_Timer >= 1.0f)
                player.isAttacking = false;
            if (attack_Timer >= attack_Cooltime)
            {
                attackOff = false;
                attack_Timer = 0;
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
        if (skill_3_Off)
        {
            skill_3_Timer += Time.deltaTime;
            if (skill_3_Timer >= 1.0f)
            {
                playerAnim.SetBool("Skill3_2", false);
            }
            if (skill_3_Timer >= skill_3_Cooltime)
            {
                skill_3_Off = false;
                skill_3_Timer = 0;
            }
        }
        if (skill_4_Off)
        {
            skill_4_Timer += Time.deltaTime;
            if (skill_4_Timer >= skill_4_Cooltime)
            {
                skill_4_Off = false;
                skill_4_Timer = 0;
            }
        }
    }

    void Skill_Slash()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !attackOff)
        {
            player.isAttacking = true;
            attackOff = true;
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

    void Skill_3_play()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3) && !skill_3_Off)
        {
            skill_3_Off = true;
            //playerAnim.SetBool("Skill2", true);
            playerAnim.SetTrigger("Skill3_1");
            playerAnim.SetBool("Skill3_2", true);
        }
    }

    void Skill_4_play()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4) && !skill_4_Off)
        {
            skill_4_Off = true;
            //playerAnim.SetBool("Skill2", true);
            playerAnim.SetTrigger("Skill4");
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
