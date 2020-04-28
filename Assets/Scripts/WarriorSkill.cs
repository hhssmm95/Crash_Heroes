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
    void Start()
    {
        player = gameObject.GetComponent<CharacterMove>();
        playerAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Skill_Slash();

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

    void Skill_1()
    {
        
    }
}
