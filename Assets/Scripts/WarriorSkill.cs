using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill : MonoBehaviour
{
    CharacterMove player;
    Animator playerAnim;
    //public Camera mainCamera;
    public float slash_Cooltime = 1.5f;
    public float slash_Timer;
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
            if (slash_Timer >= 1.0f)
                player.isAttacking = false;
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
            //Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;

            player.isAttacking = true;
            slashOff = true;
            //playerAnim.SetInteger("animation", 2);
            playerAnim.SetTrigger("Attack1");

            //if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            //{
            //    //player.transform.LookAt(hit.transform.position);
            //    Vector3 dir = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z);
            //    transform.rotation = Quaternion.LookRotation(dir);
                
            //}


        }
        //else if(playerAnim.GetCurrentAnimatorClipInfo(0))
    }
    
}
