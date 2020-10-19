using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MageSkill : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove player;
    Animator mageAnim;
    public Camera mainCamera;

    public float attack_Cooltime;
    public float attack_Cost;

    public bool attackOff;
    public float attack_Timer;
    public bool comboContinue;
    public float comboTimer;


    public GameObject Skill1Pos;
    public GameObject Attack2Pos;

    public ParticleSystem MageVX0_1;
    public ParticleSystem MageVX0_2;
    public ParticleSystem MageVX0_3;

    public ParticleSystem MageVX1;
    public ParticleSystem MageVX2;
    

    private bool isMine;

    void Awake()
    {
        player = gameObject.GetComponent<CharacterMove>();
        mageAnim = gameObject.GetComponent<Animator>();
        player.job = Global.Classes.Mage;
        if (photonView.IsMine)
        {
            isMine = true;

            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

    }

    [PunRPC]
    public void InitStatus()
    {

        player.maxHP = 524;
        player.maxMP = 326;
        player.atk = 65;
        player.def = 29;

        player.hpRegen = 5;
        player.mpRegen = 9;
        player.stRegen = 5;
        attack_Cooltime = 1.0f;
        player.skill_1_Cooltime = 10.0f;
        player.skill_1_Cost = 30;
        player.skill_2_Cooltime = 12.0f;
        player.skill_2_Cost = 40;
        player.skill_3_Cooltime = 18.0f;
        player.skill_3_Cost = 50;
        player.skill_4_Cooltime = 30.0f;
        player.skill_4_Cost = 40;
        player.skill_5_Cooltime = 60.0f;
        player.skill_5_Cost = 100;
    }

    void Update()
    {
        if (photonView.IsMine && !player.isDead && !player.isStun)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !attackOff)
                photonView.RPC("Mage_Attack", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha1) && !player.skill_1_Off)
                photonView.RPC("MageSkill1", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha2) && !player.skill_2_Off)
                photonView.RPC("MageSkill2", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha3) && !player.skill_3_Off)
                photonView.RPC("MageSkill3", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha4) && !player.skill_4_Off)
                photonView.RPC("MageSkill4", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Q) && !player.skill_5_Off)
                photonView.RPC("MageSkill5", RpcTarget.All);

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


            if (Input.GetKeyDown(KeyCode.Mouse0) && mageAnim.GetInteger("Combo") == 0 && !player.isDead && !player.isStun)
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
    void Mage_Attack()
    {
        //player.isAttacking = true;
        attackOff = true;
        comboContinue = true;

        if (comboTimer > 3.0f)
        {
            mageAnim.SetInteger("Combo", 0);
            comboContinue = false;
        }
        comboTimer = 0;

        Vector3 dir = player.transform.forward;
        if (mageAnim.GetInteger("Combo") == 0)
        {
            mageAnim.SetTrigger("FirstAttack");
            if (photonView.IsMine)
                PhotonNetwork.Instantiate("Prefebs/VFX/MageAttack1VX", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * MageVX0_1.transform.rotation);
            StartCoroutine("Skill_Hit");

            //SoundManager.Instance.MageSoundPlay(0);
        }
        else if (mageAnim.GetInteger("Combo") == 1)
        {
            mageAnim.SetTrigger("SecondAttack");
            if (photonView.IsMine)
                PhotonNetwork.Instantiate("Prefebs/VFX/MageAttack2VX", Attack2Pos.transform.position, Quaternion.LookRotation(dir) * MageVX0_2.transform.rotation);
            StartCoroutine("Skill_Hit");
            //Instantiate(MageVX0_2, Attack2Pos.transform.position, Quaternion.LookRotation(dir) * MageVX0_2.transform.rotation);
            //SoundManager.Instance.MageSoundPlay(1);
        }
        else if (mageAnim.GetInteger("Combo") == 2)
        {
            mageAnim.SetTrigger("ThirdAttack");
            if (photonView.IsMine)
                PhotonNetwork.Instantiate("Prefebs/VFX/MageAttack3VX", new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.LookRotation(dir) * MageVX0_3.transform.rotation);
            StartCoroutine("Skill_Hit");
            //Instantiate(MageVX0_3, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.LookRotation(dir) * MageVX0_3.transform.rotation);
            //SoundManager.Instance.MageSoundPlay(2);
        }


        //transform.rotation = Quaternion.LookRotation(dir);




        //else if(playerAnim.GetCurrentAnimatorClipInfo(0))
    }

    [PunRPC]
    void MageSkill1()
    {
        if (player.mp >= player.skill_1_Cost)
        {
            player.skill_1_Off = true;
            player.mp -= player.skill_1_Cost;
            //player.isAttacking = true;
            //playerAnim.SetBool("Skill2", true);
            mageAnim.SetTrigger("Skill1");
            Vector3 dir = player.transform.forward;

            //transform.rotation = Quaternion.LookRotation(dir);
            //Instantiate(MageVX1, Skill1Pos.transform.position, Quaternion.LookRotation(dir) * MageVX1.transform.rotation);
            if (photonView.IsMine)
                PhotonNetwork.Instantiate("Prefebs/VFX/MageSkill1VX", Skill1Pos.transform.position, Quaternion.LookRotation(dir) * MageVX1.transform.rotation);
            StartCoroutine("Skill_Hit");
            //SoundManager.Instance.MageSoundPlay(3);
        }

    }

    [PunRPC]
    void MageSkill2()
    {
        if (player.mp >= player.skill_2_Cost)
        {
            player.skill_2_Off = true;
            player.mp -= player.skill_2_Cost;
            //player.isAttacking = true;
            //playerAnim.SetBool("Skill2", true);
            mageAnim.SetTrigger("Skill2");
            Vector3 dir = player.transform.forward;

            //transform.rotation = Quaternion.LookRotation(dir);
            //Instantiate(MageVX2, Attack2Pos.transform.position, Quaternion.LookRotation(dir) * MageVX2.transform.rotation);
            if (photonView.IsMine)
            {
                PhotonNetwork.Instantiate("Prefebs/VFX/MageSkill2VX", Attack2Pos.transform.position, Quaternion.LookRotation(dir) * MageVX2.transform.rotation);

            }
            StartCoroutine("Skill_Hit");
            //SoundManager.Instance.MageSoundPlay(1);
        }

    }

    [PunRPC]
    void MageSkill3()
    {
        if (player.mp >= player.skill_3_Cost)
        {
            player.skill_3_Off = true;
            player.mp -= player.skill_3_Cost;
            mageAnim.SetTrigger("Skill3");
            Vector3 dir = player.transform.forward;
            //Instantiate(Dragon, new Vector3(DragonSpawn.transform.position.x - 1.95f, DragonSpawn.transform.position.y + 1.3f, DragonSpawn.transform.position.z - 0.16f), Quaternion.LookRotation(dir) * Dragon.transform.rotation);
            
        }

    }

    [PunRPC]
    void MageSkill4()
    {
        if (player.mp >= player.skill_4_Cost)
        {
            player.skill_4_Off = true;
            player.mp -= player.skill_4_Cost;
            StartCoroutine("Mage_Skill4_Effect");
        }

    }

    [PunRPC]
    void MageSkill5()
    {
        if (player.mp >= player.skill_5_Cost)
        {
            player.skill_5_Off = true;
            player.mp -= player.skill_5_Cost;
            StartCoroutine("Mage_Skill5_Effect");
        }

    }



    IEnumerator Mage_Skill4_Effect()
    {
        float originMaxHP = player.maxHP;
        mageAnim.SetTrigger("Skill4");

        player.maxHP += originMaxHP * 1.3f;
        player.hp += originMaxHP * 1.3f;
        //SoundManager.Instance.MageSoundPlay(5);
        yield return new WaitForSeconds(20.0f);
        player.hp = originMaxHP * (player.hp / player.maxHP);
        player.maxHP = originMaxHP;


    }

    IEnumerator Mage_Skill5_Effect()
    {
        float originDef = player.def;
        //SoundManager.Instance.MageSoundPlay(6);
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
