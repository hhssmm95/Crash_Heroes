using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DragoonSkill : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove player;
    Animator dragoonAnim;
    Camera mainCamera;

    public float attack_Cooltime;
    public float attack_Cost;
    
    public bool attackOff;
    public float attack_Timer;
    public bool comboContinue;
    public float comboTimer;


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

    void Awake()
    {
        if (photonView.IsMine)
        {
            isMine = true;

            player = gameObject.GetComponent<CharacterMove>();
            dragoonAnim = gameObject.GetComponent<Animator>();

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
        player.skill_1_Cooltime = 3.0f;
        player.skill_1_Cost = 30;
        player.skill_2_Cooltime = 3.0f;
        player.skill_2_Cost = 30;
        player.skill_3_Cooltime = 3.0f;
        player.skill_3_Cost = 30;
        player.skill_4_Cooltime = 3.0f;
        player.skill_4_Cost = 30;
    }

    void Update()
    {
        if(isMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !attackOff)
                photonView.RPC("Dragoon_Attack", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha1) && !player.skill_1_Off)
                photonView.RPC("Dragoon_Skill1", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha2) && !player.skill_2_Off)
                photonView.RPC("Dragoon_Skill2", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha3) && !player.skill_3_Off)
                photonView.RPC("Dragoon_Skill3", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha4) && !player.skill_4_Off)
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


            if (Input.GetKeyDown(KeyCode.Mouse0) && dragoonAnim.GetInteger("Combo") == 0 && gameObject.tag == "Player")
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
                dragoonAnim.SetInteger("Combo", 0);
                comboContinue = false;
            }
            comboTimer = 0;

            Vector3 dir = player.transform.forward;
            if (dragoonAnim.GetInteger("Combo") == 0)
            {
                dragoonAnim.SetTrigger("FirstAttack");
                Instantiate(DragoonVX0_1, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * DragoonVX0_1.transform.rotation);

            }
            else if (dragoonAnim.GetInteger("Combo") == 1)
            {
                dragoonAnim.SetTrigger("SecondAttack");
                Instantiate(DragoonVX0_2, WarriorAttack2Pos.transform.position, Quaternion.LookRotation(dir) * DragoonVX0_2.transform.rotation);

            }
            else if (dragoonAnim.GetInteger("Combo") == 2)
            {
                dragoonAnim.SetTrigger("ThirdAttack");
                Instantiate(DragoonVX0_3, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.LookRotation(dir) * DragoonVX0_3.transform.rotation);

            }


            //transform.rotation = Quaternion.LookRotation(dir);

        }


        //else if(playerAnim.GetCurrentAnimatorClipInfo(0))
    }

    [PunRPC]
    void DragoonSkill1()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !player.skill_1_Off)
        {
            if (player.mp >= player.skill_1_Cost)
            {
                player.skill_1_Off = true;
                player.mp -= player.skill_1_Cost;
                //player.isAttacking = true;
                //playerAnim.SetBool("Skill2", true);
                dragoonAnim.SetTrigger("Skill1");
                Vector3 dir = player.transform.forward;

                //transform.rotation = Quaternion.LookRotation(dir);
                Instantiate(DragoonVX1, ArcherSkill1Pos.transform.position, Quaternion.LookRotation(dir) * DragoonVX1.transform.rotation);
            }
        }
    }

    [PunRPC]
    void DragoonSkill2()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && !player.skill_2_Off)
        {
            if (player.mp >= player.skill_2_Cost)
            {
                player.skill_2_Off = true;
                player.mp -= player.skill_2_Cost;
                //player.isAttacking = true;
                //playerAnim.SetBool("Skill2", true);
                dragoonAnim.SetTrigger("Skill2");
                Vector3 dir = player.transform.forward;

                //transform.rotation = Quaternion.LookRotation(dir);
                Instantiate(DragoonVX2, WarriorAttack2Pos.transform.position, Quaternion.LookRotation(dir) * DragoonVX2.transform.rotation);
            }
        }
    }

    [PunRPC]
    void DragoonSkill3()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3) && !player.skill_3_Off)
        {
            if (player.mp >= player.skill_3_Cost)
            {
                player.skill_3_Off = true;
                player.mp -= player.skill_3_Cost;
                dragoonAnim.SetTrigger("Skill3");
                Vector3 dir = player.transform.forward;
                Instantiate(Dragon, new Vector3(DragonSpawn.transform.position.x - 1.95f, DragonSpawn.transform.position.y + 1.3f, DragonSpawn.transform.position.z - 0.16f), Quaternion.LookRotation(dir) * Dragon.transform.rotation);

            }
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
