using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DragoonSkill : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove player;
    Animator playerAnim;
    Camera mainCamera;

    public float attack_Cooltime;
    public float attack_Cost;

    public float skill_1_Cooltime;
    public float skill_1_Cost;

    public float skill_2_Cooltime;
    public float skill_2_Cost;

    public float skill_3_Cooltime;
    public float skill_3_Cost;

    public float skill_4_Cooltime;
    public float skill_4_Cost;

    public float skill_5_Cooltime;
    public float skill_5_Cost;



    public bool attackOff;
    public float attack_Timer;
    public bool comboContinue;
    public float comboTimer;
    public bool skill_1_Off;
    public bool skill_2_Off;
    public bool skill_3_Off;
    public bool skill_4_Off;
    public bool skill_5_Off;
    public float skill_1_Timer;
    public float skill_2_Timer;
    public float skill_3_Timer;
    public float skill_4_Timer;
    public float skill_5_Timer;


    public GameObject ArcherSkill1Pos;
    public GameObject WarriorAttack2Pos;

    public ParticleSystem DragoonVX0_1;
    public ParticleSystem DragoonVX0_2;
    public ParticleSystem DragoonVX0_3;

    public ParticleSystem DragoonVX1;
    public ParticleSystem DragoonVX2;

    public GameObject Dragon;
    public GameObject DragonSpawn;

    private bool isMine;

    void Start()
    {
        if (photonView.IsMine)
        {
            isMine = true;

            player = gameObject.GetComponent<CharacterMove>();
            playerAnim = gameObject.GetComponent<Animator>();

            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            player.job = Global.Classes.Dragoon;
        }

    }

    public void InitStatus()
    {

        player.maxHP = 580;
        player.maxMP = 274;
        player.atk = 66;
        player.def = 36;
        attack_Cooltime = 1.0f;
        skill_1_Cooltime = 3.0f;
        skill_1_Cost = 30;
        skill_2_Cooltime = 3.0f;
        skill_2_Cost = 30;
        skill_3_Cooltime = 3.0f;
        skill_3_Cost = 30;
        skill_4_Cooltime = 3.0f;
        skill_4_Cost = 30;
    }

    void Update()
    {
        if(isMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !attackOff)
                photonView.RPC("Dragoon_Attack", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha1) && !skill_1_Off)
                photonView.RPC("Dragoon_Skill1", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha2) && !skill_2_Off)
                photonView.RPC("Dragoon_Skill2", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha3) && !skill_3_Off)
                photonView.RPC("Dragoon_Skill3", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha4) && !skill_4_Off)
                photonView.RPC("Dragoon_Skill4", RpcTarget.All);

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

            if (skill_1_Off)
            {
                skill_1_Timer += Time.deltaTime;
                if (skill_1_Timer >= 1.2f)
                    player.isAttacking = false;
                if (skill_1_Timer >= skill_1_Cooltime)
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
                    if (player.job == Global.Classes.Warrior)
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
                if (skill_3_Timer >= 1.0f && player.job == Global.Classes.Warrior)
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

            if (Input.GetKeyDown(KeyCode.Mouse0) && playerAnim.GetInteger("Combo") == 0 && gameObject.tag == "Player")
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
    void Dragoon_Attack()
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

            Vector3 dir = player.transform.forward;
            if (playerAnim.GetInteger("Combo") == 0)
            {
                playerAnim.SetTrigger("FirstAttack");
                Instantiate(DragoonVX0_1, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * DragoonVX0_1.transform.rotation);

            }
            else if (playerAnim.GetInteger("Combo") == 1)
            {
                playerAnim.SetTrigger("SecondAttack");
                Instantiate(DragoonVX0_2, WarriorAttack2Pos.transform.position, Quaternion.LookRotation(dir) * DragoonVX0_2.transform.rotation);

            }
            else if (playerAnim.GetInteger("Combo") == 2)
            {
                playerAnim.SetTrigger("ThirdAttack");
                Instantiate(DragoonVX0_3, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.LookRotation(dir) * DragoonVX0_3.transform.rotation);

            }


            //transform.rotation = Quaternion.LookRotation(dir);

        }


        //else if(playerAnim.GetCurrentAnimatorClipInfo(0))
    }

    [PunRPC]
    void DragoonSkill1()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !skill_1_Off)
        {
            if (player.mp >= skill_1_Cost)
            {
                skill_1_Off = true;
                player.mp -= skill_1_Cost;
                //player.isAttacking = true;
                //playerAnim.SetBool("Skill2", true);
                playerAnim.SetTrigger("Skill1");
                Vector3 dir = player.transform.forward;

                //transform.rotation = Quaternion.LookRotation(dir);
                Instantiate(DragoonVX1, ArcherSkill1Pos.transform.position, Quaternion.LookRotation(dir) * DragoonVX1.transform.rotation);
            }
        }
    }

    [PunRPC]
    void DragoonSkill2()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && !skill_2_Off)
        {
            if (player.mp >= skill_2_Cost)
            {
                skill_2_Off = true;
                player.mp -= skill_2_Cost;
                //player.isAttacking = true;
                //playerAnim.SetBool("Skill2", true);
                playerAnim.SetTrigger("Skill2");
                Vector3 dir = player.transform.forward;

                //transform.rotation = Quaternion.LookRotation(dir);
                Instantiate(DragoonVX2, WarriorAttack2Pos.transform.position, Quaternion.LookRotation(dir) * DragoonVX2.transform.rotation);
            }
        }
    }

    [PunRPC]
    void DragoonSkill3()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3) && !skill_3_Off)
        {
            if (player.mp >= skill_3_Cost)
            {
                skill_3_Off = true;
                player.mp -= skill_3_Cost;
                playerAnim.SetTrigger("Skill3");
                Vector3 dir = player.transform.forward;
                Instantiate(Dragon, new Vector3(DragonSpawn.transform.position.x - 1.95f, DragonSpawn.transform.position.y + 1.3f, DragonSpawn.transform.position.z - 0.16f), Quaternion.LookRotation(dir) * Dragon.transform.rotation);

            }
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
