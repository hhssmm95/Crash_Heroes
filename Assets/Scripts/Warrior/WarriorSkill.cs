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
    
    float healTimer;
    float healDurationTimer;
    bool healing;

    public ParticleSystem WarriorVX1_1;
    public ParticleSystem WarriorVX1_2;
    public GameObject WarriorAttack2Pos;
    public ParticleSystem WarriorVX1_3;
    public ParticleSystem WarriorVX2_1;
    public ParticleSystem WarriorVX2_2;

    public ParticleSystem WarriorVX3;
    public GameObject WarriorSkill3Pos;

    public bool isMine;

    void Awake()
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
        player.skill_1_Cooltime = 12.0f;
        player.skill_1_Cost = 30.0f;
        player.skill_2_Cooltime = 8.0f;
        player.skill_2_Cost = 25.0f;
        player.skill_3_Cooltime = 20.0f;
        player.skill_3_Cost = 50.0f;
        player.skill_4_Cooltime = 30.0f;
        player.skill_4_Cost = 40.0f;
        player.skill_5_Cooltime = 60.0f;
        player.skill_5_Cost = 100.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMine && !player.isDead && !player.isStun)
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
            if (Input.GetKeyDown(KeyCode.Q) && !player.skill_5_Off)
                photonView.RPC("Warrior_Skill5", RpcTarget.All);


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

            if(healing && !player.isDead)
            {
                if(healDurationTimer >= 5.0f)
                {
                    healing = false;
                    healDurationTimer = 0;
                    healTimer = 0;
                    return;
                }
                healTimer += Time.deltaTime;
                healDurationTimer += Time.deltaTime;

            }

            if(healing && !player.isDead && healTimer >= 1.0f)
            {
                healTimer = 0;
                player.GetComponent<PhotonView>().RPC("OnHeal", RpcTarget.All, player.maxHP * 0.08f);
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
        
    }

    [PunRPC]
    public void Warrior_Skill1()
    {

        if (player.mp >= player.skill_1_Cost && !player.isDead)
        {
            player.skill_1_Off = true;
            player.mp -= player.skill_1_Cost;
            StartCoroutine("Warrior_Skill1_Play");

        }


    }

    [PunRPC]
    public void Warrior_Skill2()
    {
        if (player.mp >= player.skill_2_Cost && !player.isDead)
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
        if (player.mp >= player.skill_3_Cost && !player.isDead)
        {
            player.skill_3_Off = true;
            player.mp -= player.skill_3_Cost;
            //playerAnim.SetBool("Skill2", true);
            warriorAnim.SetTrigger("Skill3_1");
            warriorAnim.SetBool("Skill3_2", true);
            StartCoroutine("Warrior_Skill3_VFX");
        }

    }

    [PunRPC]
    public void Warrior_Skill4()
    {
        if (player.mp >= player.skill_4_Cost && !player.isDead)
        {
            player.skill_4_Off = true;
            player.mp -= player.skill_4_Cost;
            //playerAnim.SetBool("Skill2", true);
            StartCoroutine("Warrior_Skill4_Effect");
        }

    }

    [PunRPC]
    public void Warrior_Skill5()
    {
        if(player.mp >= player.skill_5_Cost)
        {
            player.skill_5_Off = true;
            player.mp -= player.skill_5_Cost;
            healing = true;
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

        Vector3 dir = player.transform.forward;
        yield return new WaitForSeconds(0.4f);
        PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill3VX", WarriorSkill3Pos.transform.position, Quaternion.LookRotation(dir) * WarriorVX3.transform.rotation);

    }

    IEnumerator Warrior_Skill4_Effect()
    {
        float originAtk = player.atk;
        float originDef = player.def;

        warriorAnim.SetTrigger("Skill4");

        player.atk *= 1.3f;
        player.def *= 1.3f;
        yield return new WaitForSeconds(20.0f);

        player.atk = originAtk;
        player.def = originDef;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
