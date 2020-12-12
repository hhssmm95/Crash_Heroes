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
    

    public GameObject AttackPos1, AttackPos2, AttackPos3;
    public GameObject skillPos;
    public GameObject BuffEff;
    public GameObject ShiledEff;

    public ParticleSystem MageVX0_1;
    public ParticleSystem MageVX0_2;
    public ParticleSystem MageVX0_3;

    public ParticleSystem MageVX1;
    public ParticleSystem MageVX2;

    Ray rayn;
    
    private bool isMine;
    private bool skill;
    int combo = 1;

    void ComboCounter(int count)
    {
        combo = count;
    }

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
        skill = false;
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

            if (!(player.hp <= 0) && gameObject.GetComponent<CharacterMove>().stopWhileAttack==false)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    //Vector3 dir = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z);
                    if (((hit.point.x - transform.position.x) * (hit.point.x - transform.position.x)) + ((hit.point.z - transform.position.z) * (hit.point.z - transform.position.z))
                        > 9.0f)
                    {
                        skillPos.transform.position = new Vector3(hit.point.x - transform.position.x, 0, hit.point.z - transform.position.z).normalized * 3 + new Vector3(transform.position.x,0,transform.position.z);
                        skillPos.transform.rotation = Quaternion.Euler(0,0,0);
                    }
                    else
                    {
                        skillPos.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
                        skillPos.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    //transform.rotation = Quaternion.LookRotation(dir);
                }
            }

                if (Input.GetKeyDown(KeyCode.Mouse0) && mageAnim.GetInteger("Combo") == 0 && !player.isDead && !player.isStun)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                

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

        switch (combo)
        {
            case 1:
                mageAnim.SetTrigger("FirstAttack");
                if (photonView.IsMine)
                    PhotonNetwork.Instantiate("Prefebs/VFX/MageAttack1VX", AttackPos1.transform.position, Quaternion.LookRotation(dir) * MageVX0_1.transform.rotation);

                StartCoroutine("Skill_Hit");
                break;

            case 2:
                mageAnim.SetTrigger("SecondAttack");
                if (photonView.IsMine)
                    PhotonNetwork.Instantiate("Prefebs/VFX/MageAttack2VX", AttackPos2.transform.position, Quaternion.LookRotation(dir) * MageVX0_2.transform.rotation);
                StartCoroutine("Skill_Hit");
                break;

            case 3:
                mageAnim.SetTrigger("ThirdAttack");
                if (photonView.IsMine)
                    PhotonNetwork.Instantiate("Prefebs/VFX/MageAttack3VX", AttackPos3.transform.position, Quaternion.LookRotation(dir) * MageVX0_3.transform.rotation);
                StartCoroutine("Skill_Hit");
                SoundManager.Instance.DragoonSoundPlay(2);
                break;

        }


        //transform.rotation = Quaternion.LookRotation(dir);




        //else if(playerAnim.GetCurrentAnimatorClipInfo(0))
    }

    [PunRPC]
    void MageSkill1()
    {
        if (player.mp >= player.skill_1_Cost)
        {
            SetLookAtMousePos();
            player.skill_1_Off = true;
            player.mp -= player.skill_1_Cost;
            //player.isAttacking = true;
            //playerAnim.SetBool("Skill2", true);
            mageAnim.SetTrigger("ThirdAttack");
            Vector3 dir = player.transform.forward;
            SoundManager.Instance.MageSoundPlay(0);
            //transform.rotation = Quaternion.LookRotation(dir);
            //Instantiate(MageVX1, Skill1Pos.transform.position, Quaternion.LookRotation(dir) * MageVX1.transform.rotation);
            if (photonView.IsMine)
                PhotonNetwork.Instantiate("Prefebs/VFX/MageSkill1VX", transform.position, Quaternion.LookRotation(dir) * MageVX1.transform.rotation);
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
            SoundManager.Instance.MageSoundPlay(1);
            //transform.rotation = Quaternion.LookRotation(dir);
            //Instantiate(MageVX2, Attack2Pos.transform.position, Quaternion.LookRotation(dir) * MageVX2.transform.rotation);
            if (photonView.IsMine)
            {
                PhotonNetwork.Instantiate("Prefebs/VFX/MageSkill2VX", skillPos.transform.position, Quaternion.LookRotation(dir) * MageVX2.transform.rotation);
                SetLookAtMousePos();
            }
            //StartCoroutine("Spell");
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
            SoundManager.Instance.MageSoundPlay(2);
            //Instantiate(Dragon, new Vector3(DragonSpawn.transform.position.x - 1.95f, DragonSpawn.transform.position.y + 1.3f, DragonSpawn.transform.position.z - 0.16f), Quaternion.LookRotation(dir) * Dragon.transform.rotation);
            if (photonView.IsMine)
            {
                PhotonNetwork.Instantiate("Prefebs/VFX/MageSkill3VX", skillPos.transform.position, Quaternion.Euler(0, 0, 0));//Quaternion.LookRotation(dir) * MageVX2.transform.rotation);
                SetLookAtMousePos();
            }
        }

    }

    [PunRPC]
    void MageSkill4()
    {
        if (player.mp >= player.skill_4_Cost)
        {
            player.skill_4_Off = true;
            mageAnim.SetTrigger("Skill4");
            player.mp -= player.skill_4_Cost;
            SoundManager.Instance.MageSoundPlay(3);
            StartCoroutine("Mage_Skill4_Effect");
            player.skill_1_Timer = 0;
            player.skill_2_Timer = 0;
            player.skill_3_Timer = 0;
            player.skill_1_Off = false;
            player.skill_2_Off = false;
            player.skill_3_Off = false;

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
            SoundManager.Instance.MageSoundPlay(4);
        }

    }


    [PunRPC]
    void BarriorDestroy()
    {
        Destroy(GameObject.FindWithTag("Shiled"));
    }


    IEnumerator Mage_Skill4_Effect()
    {
        BuffEff.SetActive(true);
        //player.skill_1_Cooltime *= 0.1f;
        //player.skill_2_Cooltime *= 0.1f;
        //player.skill_3_Cooltime *= 0.1f;
        yield return new WaitForSeconds(1.0f);
        //player.skill_1_Cooltime *= 10.0f;
        //player.skill_2_Cooltime *= 10.0f;
        //player.skill_3_Cooltime *= 10.0f;
        BuffEff.SetActive(false);
    }

    IEnumerator Mage_Skill5_Effect()
    {
        GameObject shld = Instantiate(ShiledEff, gameObject.transform.position + ShiledEff.transform.position, Quaternion.EulerAngles(0, 0, 0));
        shld.transform.parent = gameObject.transform;
        player.BarriorFill(player.maxHP * 0.4f);
        yield return new WaitForSeconds(30.0f);
        Destroy(shld);
    }

    IEnumerator Skill_Hit()
    {
        //yield return new WaitForSeconds(0.01f);
        player.isAttacking = true;
        yield return new WaitForSeconds(0.4f);
        player.isAttacking = false;


    }

    void SetLookAtMousePos()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 dir = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z);
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
