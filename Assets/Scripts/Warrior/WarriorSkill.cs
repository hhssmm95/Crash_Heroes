using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WarriorSkill : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove player;
    public Animator warriorAnim;
    Camera mainCamera;

    public float attack_Cooltime = 1.0f;
    public float attack_Cost;



    public bool attackOff;
    public float attack_Timer;
    public bool comboContinue;
    public float comboTimer;
    
    public ParticleSystem WarriorVX1_1;
    public ParticleSystem WarriorVX1_2;
    public GameObject WarriorAttack2Pos;
    public ParticleSystem WarriorVX1_3;
    public ParticleSystem WarriorVX2_1;
    public ParticleSystem WarriorVX2_2;

    public ParticleSystem WarriorVX3;
    public GameObject WarriorSkill3Pos;

    public bool isMine;

    void Start()
    {
        //if(CompareTag("Player"))
        if (photonView.IsMine)
        {
            isMine = true;

            player = gameObject.GetComponent<CharacterMove>();
            warriorAnim = gameObject.GetComponent<Animator>();

            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            player.job = Global.Classes.Warrior;
        }

    }

    [PunRPC]
    public void InitStatus()
    {

        player.maxHP = 582.0f;
        player.maxMP = 263.0f;
        player.atk = 64.0f;
        player.def = 39.0f;

        //attack_Cooltime = 1.0f;
        player.skill_1_Cooltime = 3.0f;
        player.skill_1_Cost = 30.0f;
        player.skill_2_Cooltime = 3.0f;
        player.skill_2_Cost = 30.0f;
        player.skill_3_Cooltime = 3.0f;
        player.skill_3_Cost = 30.0f;
        player.skill_4_Cooltime = 3.0f;
        player.skill_4_Cost = 30.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !attackOff)
                photonView.RPC("Warrior_Attack", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha1) && !player.skill_1_Off)
                photonView.RPC("Warrior_Skill1", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha2) && !player.skill_2_Off)
                photonView.RPC("Warrior_Skill2", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha3) && !player.skill_3_Off)
                photonView.RPC("Warrior_Skill3", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha4) && !player.skill_4_Off)
                photonView.RPC("Warrior_Skill4", RpcTarget.All);


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

            //if (skill_1_Off)
            //{
            //    skill_1_Timer += Time.deltaTime;
            //    if (skill_1_Timer >= 1.2f)
            //        player.isAttacking = false;
            //    if (skill_1_Timer >= skill_1_Cooltime)
            //    {
            //        skill_1_Off = false;
            //        skill_1_Timer = 0;
            //    }
            //}
            //if (skill_2_Off)
            //{
            //    skill_2_Timer += Time.deltaTime;
            //    if (skill_2_Timer >= 1.0f)
            //    {
            //        player.isAttacking = false;
            //        if (player.job == Global.Classes.Warrior)
            //            Warrior.SetBool("Skill2_2", false);
            //    }
            //    if (skill_2_Timer >= skill_2_Cooltime)
            //    {
            //        skill_2_Off = false;
            //        skill_2_Timer = 0;
            //    }
            //}
            //if (skill_3_Off)
            //{
            //    skill_3_Timer += Time.deltaTime;
            //    if (skill_3_Timer >= 1.0f && player.job == Global.Classes.Warrior)
            //    {
            //        Warrior.SetBool("Skill3_2", false);
            //    }
            //    if (skill_3_Timer >= skill_3_Cooltime)
            //    {
            //        skill_3_Off = false;
            //        skill_3_Timer = 0;
            //    }
            //}
            //if (skill_4_Off)
            //{
            //    skill_4_Timer += Time.deltaTime;
            //    if (skill_4_Timer >= skill_4_Cooltime)
            //    {
            //        skill_4_Off = false;
            //        skill_4_Timer = 0;
            //    }
            //}

            if (Input.GetKeyDown(KeyCode.Mouse0) && warriorAnim.GetInteger("Combo") == 0)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Vector3 dir = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z);
                    transform.rotation = Quaternion.LookRotation(dir);
                }
            }
        }
    }

    [PunRPC]
    void Warrior_Attack()
    {
        player.isAttacking = true;
        attackOff = true;
        comboContinue = true;

        if (comboTimer > 3.0f)
        {
            warriorAnim.SetInteger("Combo", 0);
            comboContinue = false;
        }
        comboTimer = 0;

        Vector3 dir = player.transform.forward;
        if (warriorAnim.GetInteger("Combo") == 0)
        {
            warriorAnim.SetTrigger("FirstAttack");
            PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack1VX", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * WarriorVX1_1.transform.rotation);

        }
        else if (warriorAnim.GetInteger("Combo") == 1)
        {
            warriorAnim.SetTrigger("SecondAttack");
            PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack2VX", WarriorAttack2Pos.transform.position, Quaternion.LookRotation(dir) * WarriorVX1_2.transform.rotation);
        }
        else if (warriorAnim.GetInteger("Combo") == 2)
        {
            warriorAnim.SetTrigger("ThirdAttack");
            PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack3VX", new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.LookRotation(dir) * WarriorVX1_3.transform.rotation).GetComponent<PhotonView>();

        }


        //transform.rotation = Quaternion.LookRotation(dir);


        //else if(playerAnim.GetCurrentAnimatorClipInfo(0))

    }

    [PunRPC]
    public void Warrior_Skill1()
    {

        if (player.mp >= player.skill_1_Cost)
        {
            player.skill_1_Off = true;
            player.mp -= player.skill_1_Cost;
            StartCoroutine("Warrior_Skill1_Play");

        }


    }

    [PunRPC]
    public void Warrior_Skill2()
    {
        if (player.mp >= player.skill_2_Cost)
        {
            player.skill_2_Off = true;
            player.mp -= player.skill_2_Cost;
            player.isAttacking = true;
            //playerAnim.SetBool("Skill2", true);
            warriorAnim.SetTrigger("Skill2_1");
            warriorAnim.SetBool("Skill2_2", true);
            StartCoroutine("Warrior_Skill2_VFX");
        }

    }

    [PunRPC]
    public void Warrior_Skill3()
    {
        if (player.mp >= player.skill_3_Cost)
        {
            player.skill_3_Off = true;
            player.mp -= player.skill_3_Cost;
            //playerAnim.SetBool("Skill2", true);
            warriorAnim.SetTrigger("Skill3_1");
            warriorAnim.SetBool("Skill3_2", true);
        }

    }

    [PunRPC]
    public void Warrior_Skill4()
    {
        if (player.mp >= player.skill_4_Cost)
        {
            player.skill_4_Off = true;
            player.mp -= player.skill_4_Cost;
            //playerAnim.SetBool("Skill2", true);
            warriorAnim.SetTrigger("Skill4");

        }

    }

    IEnumerator Warrior_Skill1_Play()
    {
        //playerAnim.stop
        player.isDashing = true;
        player.isAttacking = true;
        warriorAnim.SetTrigger("Dash");
        yield return new WaitForSeconds(0.1f);
        warriorAnim.SetTrigger("ThirdAttack");
        Vector3 dir = player.transform.forward;
        PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack3VX", new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.LookRotation(dir) * WarriorVX1_3.transform.rotation);

    }
    
    IEnumerator Warrior_Skill2_VFX()
    {
        Vector3 dir = transform.forward;
        PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill2_1", new Vector3(transform.position.x, transform.position.y + 0.406f, transform.position.z + 0.1F), Quaternion.LookRotation(dir) * WarriorVX2_1.transform.rotation);

        yield return new WaitForSeconds(0.2f);

        PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill2_2", new Vector3(transform.position.x, transform.position.y + 0.656f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * WarriorVX2_2.transform.rotation);

    }

    IEnumerator Warrior_Skill3_VFX()
    {
        //if (stateInfo.normalizedTime >= 0.2f && !trigger)
        //{
        //    trigger = true;
        //    shotPos = GameObject.FindGameObjectWithTag("WarriorSkill3Pos").transform;
        //    player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            
        //    //PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill3VX", new Vector3(shotPos.transform.position.x, shotPos.transform.position.y, shotPos.transform.position.z), Quaternion.LookRotation(dir) * VFX.transform.rotation)
        //    //    .GetComponent<PhotonView>().RPC("SkillEffect", RpcTarget.All);

        //    Instantiate(VFX, new Vector3(shotPos.transform.position.x, shotPos.transform.position.y, shotPos.transform.position.z), Quaternion.LookRotation(dir) * VFX.transform.rotation);
        //}

        Vector3 dir = player.transform.forward;
        yield return new WaitForSeconds(0.4f);
        PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill3VX", WarriorSkill3Pos.transform.position, Quaternion.LookRotation(dir) * WarriorVX3.transform.rotation);

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
