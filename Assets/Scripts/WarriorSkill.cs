using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill : MonoBehaviour
{
    CharacterMove player;
    Animator playerAnim;
    public float slash_Cooltime = 1.5f;
    public float slash_Timer = 0;
    public bool slashOff;
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
            if (slash_Timer >= slash_Cooltime)
            {
                slashOff = false;
                slash_Timer = 0;
            }
        }
    }

    void Skill_Slash()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !slashOff)
        {
            slashOff = true;
            //playerAnim.SetInteger("animation", 2);
            playerAnim.SetTrigger("Attack1");
        }
        //else if(playerAnim.GetCurrentAnimatorClipInfo(0))
    }
}
