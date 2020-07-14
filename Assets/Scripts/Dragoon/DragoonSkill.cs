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


    public GameObject Skill1Pos;
    public GameObject Attack2Pos;

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
        player = gameObject.GetComponent<CharacterMove>();
        dragoonAnim = gameObject.GetComponent<Animator>();
        player.job = Global.Classes.Dragoon;

        if (photonView.IsMine)
        {
            isMine = true;

            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

    }

    [PunRPC]
    public void InitStatus()
    {

        player.maxHP = 532;
        player.maxMP = 274;
        player.atk = 68;
        player.def = 36;

        player.hpRegen = 7;
        player.mpRegen = 6;
        player.stRegen = 6;

        attack_Cooltime = 1.0f;
        player.skill_1_Cooltime = 9.0f;
        player.skill_1_Cost = 25;
        player.skill_2_Cooltime = 15.0f;
        player.skill_2_Cost = 35;
        player.skill_3_Cooltime = 10.0f;
        player.skill_3_Cost = 70;
        player.skill_4_Cooltime = 30.0f;
        player.skill_4_Cost = 40;
        player.skill_5_Cooltime = 60.0f;
        player.skill_5_Cost = 100;
    }

    void Update()
    {
        if(photonView.IsMine && !player.isDead && !player.isStun)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !attackOff)
                photonView.RPC("Dragoon_Attack", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha1) && !player.skill_1_Off)
                photonView.RPC("DragoonSkill1", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha2) && !player.skill_2_Off)
                photonView.RPC("DragoonSkill2", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha3) && !player.skill_3_Off)
                photonView.RPC("DragoonSkill3", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha4) && !player.skill_4_Off)
                photonView.RPC("DragoonSkill4", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Q) && !player.skill_5_Off)
                photonView.RPC("DragoonSkill5", RpcTarget.All);

            if (attackOff)
            {
                attack_Timer += Time.deltaTime;
                //if (attack_Timer >= 1.0f)
                //    player.isAttacking = false;
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


            if (Input.GetKeyDown(KeyCode.Mouse0) && dragoonAnim.GetInteger("Combo") == 0 && !player.isDead && !player.isStun)
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
        //player.isAttacking = true;
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
            if (photonView.IsMine)
                PhotonNetwork.Instantiate("Prefebs/VFX/DragoonAttack1VX", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * DragoonVX0_1.transform.rotation);
            StartCoroutine("Skill_Hit");

            SoundManager.Instance.DragoonSoundPlay(0);
        }
        else if (dragoonAnim.GetInteger("Combo") == 1)
        {
            dragoonAnim.SetTrigger("SecondAttack");
            if (photonView.IsMine)
                PhotonNetwork.Instantiate("Prefebs/VFX/DragoonAttack2VX", Attack2Pos.transform.position, Quaternion.LookRotation(dir) * DragoonVX0_2.transform.rotation);
            StartCoroutine("Skill_Hit");
            //Instantiate(DragoonVX0_2, Attack2Pos.transform.position, Quaternion.LookRotation(dir) * DragoonVX0_2.transform.rotation);
            SoundManager.Instance.DragoonSoundPlay(1);
        }
        else if (dragoonAnim.GetInteger("Combo") == 2)
        {
            dragoonAnim.SetTrigger("ThirdAttack");
            if (photonView.IsMine)
                PhotonNetwork.Instantiate("Prefebs/VFX/DragoonAttack3VX", new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.LookRotation(dir) * DragoonVX0_3.transform.rotation);
            StartCoroutine("Skill_Hit");
            //Instantiate(DragoonVX0_3, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.LookRotation(dir) * DragoonVX0_3.transform.rotation);
            SoundManager.Instance.DragoonSoundPlay(2);
        }


        //transform.rotation = Quaternion.LookRotation(dir);




        //else if(playerAnim.GetCurrentAnimatorClipInfo(0))
    }

    [PunRPC]
    void DragoonSkill1()
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
            //Instantiate(DragoonVX1, Skill1Pos.transform.position, Quaternion.LookRotation(dir) * DragoonVX1.transform.rotation);
            if (photonView.IsMine)
                PhotonNetwork.Instantiate("Prefebs/VFX/DragoonSkill1VX", Skill1Pos.transform.position, Quaternion.LookRotation(dir) * DragoonVX1.transform.rotation);
            StartCoroutine("Skill_Hit");
            SoundManager.Instance.DragoonSoundPlay(3);
        }

    }

    [PunRPC]
    void DragoonSkill2()
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
            //Instantiate(DragoonVX2, Attack2Pos.transform.position, Quaternion.LookRotation(dir) * DragoonVX2.transform.rotation);
            if (photonView.IsMine)
            {
                PhotonNetwork.Instantiate("Prefebs/VFX/DragoonSkill2VX", Attack2Pos.transform.position, Quaternion.LookRotation(dir) * DragoonVX2.transform.rotation);

            }
            StartCoroutine("Skill_Hit");
            SoundManager.Instance.DragoonSoundPlay(1);
        }

    }

    [PunRPC]
    void DragoonSkill3()
    {
        if (player.mp >= player.skill_3_Cost)
        {
            player.skill_3_Off = true;
            player.mp -= player.skill_3_Cost;
            dragoonAnim.SetTrigger("Skill3");
            Vector3 dir = player.transform.forward;
            //Instantiate(Dragon, new Vector3(DragonSpawn.transform.position.x - 1.95f, DragonSpawn.transform.position.y + 1.3f, DragonSpawn.transform.position.z - 0.16f), Quaternion.LookRotation(dir) * Dragon.transform.rotation);
            if (photonView.IsMine)
            {
                PhotonNetwork.Instantiate("Prefebs/FireDragon", new Vector3(DragonSpawn.transform.position.x - 1.95f, DragonSpawn.transform.position.y + 1.3f, DragonSpawn.transform.position.z - 0.16f), Quaternion.LookRotation(dir) * Dragon.transform.rotation);
                PhotonNetwork.Instantiate("Prefebs/DragonDestination", new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z), transform.rotation);
                SoundManager.Instance.DragoonSoundPlay(4);
            }
            //StartCoroutine("Skill_Hit");
        }

    }

    [PunRPC]
    void DragoonSkill4()
    {
        if (player.mp >= player.skill_4_Cost)
        {
            player.skill_4_Off = true;
            player.mp -= player.skill_4_Cost;
            StartCoroutine("Dragoon_Skill4_Effect");
        }

    }

    [PunRPC]
    void DragoonSkill5()
    {
        if (player.mp >= player.skill_5_Cost)
        {
            player.skill_5_Off = true;
            player.mp -= player.skill_5_Cost;
            StartCoroutine("Dragoon_Skill5_Effect");
        }

    }



    IEnumerator Dragoon_Skill4_Effect()
    {
        float originMaxHP = player.maxHP;
        dragoonAnim.SetTrigger("Skill4");

        player.maxHP += originMaxHP * 1.3f;
        player.hp += originMaxHP * 1.3f;
        SoundManager.Instance.DragoonSoundPlay(5);
        yield return new WaitForSeconds(20.0f);
        player.hp = originMaxHP * (player.hp / player.maxHP);
        player.maxHP = originMaxHP;

        
    }

    IEnumerator Dragoon_Skill5_Effect()
    {
        float originDef = player.def;
        SoundManager.Instance.DragoonSoundPlay(6);
        player.GetComponent<PhotonView>().RPC("OnHeal", RpcTarget.All, player.maxHP * 0.5f);
        player.def *= 0.8f;
        yield return new WaitForSeconds(20.0f);
        player.def = originDef;
        
    }

    IEnumerator Skill_Hit()
    {
        //yield return new WaitForSeconds(0.01f);
        player.isAttacking = true;
        yield return new WaitForSeconds(0.4f);
        player.isAttacking = false;

    }
    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
