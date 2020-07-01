using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArcherSkill : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove player;
    Animator archerAnim;
    Camera mainCamera;

    public float attack_Cooltime;
    public float attack_Cost;
    


    public bool attackOff;
    public float attack_Timer;
    public bool comboContinue;
    public float comboTimer;

    public ParticleSystem ArcherVX1; //스킬1
    public GameObject ArcherSkill1Pos;
    public GameObject ArcherAttack2Pos;
    public GameObject ArcherArrow;
    public GameObject BigArrow;
    public ParticleSystem ArcherVX2_1; //평타1
    public ParticleSystem ArcherVX2_2; //평타2

    private bool isMine;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            isMine = true;

            player = gameObject.GetComponent<CharacterMove>();
            archerAnim = gameObject.GetComponent<Animator>();

            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            player.job = Global.Classes.Archer;
        }
    }

    public void InitStatus()
    {

        player.maxHP = 539;
        player.maxMP = 280;
        player.atk = 61;
        player.def = 32;

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

    // Update is called once per frame
    void Update()
    {
        if (isMine)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !attackOff)
                photonView.RPC("Archer_Attack", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha1) && !player.skill_1_Off)
                photonView.RPC("Archer_Skill1", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha2) && !player.skill_2_Off)
                photonView.RPC("Archer_Skill2", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha3) && !player.skill_3_Off)
                photonView.RPC("Archer_Skill3", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha4) && !player.skill_4_Off)
                photonView.RPC("Archer_Skill4", RpcTarget.All);


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

            if (Input.GetKeyDown(KeyCode.Mouse0) && archerAnim.GetInteger("Combo") == 0 && gameObject.tag == "Player")
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
    void Archer_Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !attackOff)
        {
            player.isAttacking = true;
            attackOff = true;
            comboContinue = true;

            if (comboTimer > 3.0f)
            {
                archerAnim.SetInteger("Combo", 0);
                comboContinue = false;
            }
            comboTimer = 0;

            Vector3 dir = transform.forward;

            if (archerAnim.GetInteger("Combo") == 0)
            {
                archerAnim.SetTrigger("FirstAttack");

                PhotonNetwork.Instantiate("Prefebs/VFX/ArcherAttack1VX", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * ArcherVX2_1.transform.rotation);

            }
            else if (archerAnim.GetInteger("Combo") == 1)
            {
                archerAnim.SetTrigger("SecondAttack");

                PhotonNetwork.Instantiate("Prefebs/VFX/ArcherAttack2VX", ArcherAttack2Pos.transform.position, Quaternion.LookRotation(dir) * ArcherVX2_2.transform.rotation);
                
            }
            else if (archerAnim.GetInteger("Combo") == 2)
            {
                archerAnim.SetTrigger("ThirdAttack");

                PhotonNetwork.Instantiate("Prefebs/arrow", new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.4f), Quaternion.LookRotation(dir) * ArcherArrow.transform.rotation);

                
            }



        }
    }

    [PunRPC]
    void Archer_Skill1()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !player.skill_1_Off)
        {
            if (player.mp >= player.skill_1_Cost)
            {
                player.skill_1_Off = true;
                player.mp -= player.skill_1_Cost;
                //player.isAttacking = true;
                //playerAnim.SetBool("Skill2", true);
                archerAnim.SetTrigger("Skill1");
                Vector3 dir = player.transform.forward;

                //transform.rotation = Quaternion.LookRotation(dir);
                PhotonNetwork.Instantiate("Prefebs/VFX/ArcherVX1", ArcherSkill1Pos.transform.position, Quaternion.LookRotation(dir) * ArcherVX1.transform.rotation);
                
            }
        }
    }

    [PunRPC]
    void Archer_Skill2()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && !player.skill_2_Off)
        {
            if (player.mp >= player.skill_2_Cost)
            {
                player.skill_2_Off = true;
                player.mp -= player.skill_2_Cost;
                archerAnim.SetTrigger("Skill2");
                Vector3 dir = transform.forward;

                //transform.rotation = Quaternion.LookRotation(dir);
                Quaternion rot1 = ArcherArrow.transform.rotation * Quaternion.Euler(new Vector3(0, 0, -5.0f));
                Quaternion rot2 = ArcherArrow.transform.rotation * Quaternion.Euler(new Vector3(0, 0, 5.0f));


                PhotonNetwork.Instantiate("Prefebs/arrow", new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.4f), Quaternion.LookRotation(dir) * ArcherArrow.transform.rotation);

                PhotonNetwork.Instantiate("Prefebs/arrow", new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z + 0.4f), Quaternion.LookRotation(dir) * rot1);

                PhotonNetwork.Instantiate("Prefebs/arrow", new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z + 0.4f), Quaternion.LookRotation(dir) * rot2);

            }
        }
    }

    [PunRPC]
    void Archer_Skill3()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3) && !player.skill_3_Off)
        {
            if (player.mp >= player.skill_3_Cost)
            {
                player.skill_3_Off = true;
                player.mp -= player.skill_3_Cost;
                archerAnim.SetTrigger("Skill3");
                StartCoroutine("Archer_Skill3_Delay");
            }
        }
    }

    [PunRPC]
    void Archer_Skill4()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4) && !player.skill_4_Off)
        {
            if (player.mp >= player.skill_4_Cost)
            {
                player.skill_4_Off = true;
                player.mp -= player.skill_4_Cost;
                archerAnim.SetTrigger("Skill4");
                StartCoroutine("Archer_Skill4_Effect");

            }
        }
    }


    IEnumerator Archer_Skill3_Delay()
    {
        yield return new WaitForSeconds(1.0f);
        Vector3 dir = transform.forward;

        //transform.rotation = Quaternion.LookRotation(dir);

        PhotonNetwork.Instantiate("Prefebs/BigArrow", new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.8f), Quaternion.LookRotation(dir) * BigArrow.transform.rotation);

    }

    IEnumerator Archer_Skill4_Effect()
    {
        float originSpeed = player.speed;
        player.speed *= 1.5f;
        yield return new WaitForSecondsRealtime(10.0f);
        player.speed = originSpeed;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}