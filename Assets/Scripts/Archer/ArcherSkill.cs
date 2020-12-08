using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArcherSkill : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove player;
    public Animator archerAnim;
    Camera mainCamera;

    public float attack_Cooltime = 1.0f;
    public float attack_Cost;



    public bool attackOff;
    public float attack_Timer;
    public bool comboContinue;
    public float comboTimer;
    public bool isSniping;
    public bool snipeTimer;

    public ParticleSystem ArcherVX1; //스킬1
    public GameObject ArcherSkill1Pos;
    public GameObject ArcherAttack2Pos;
    public GameObject ArcherArrow;
    public GameObject BigArrow;
    public ParticleSystem ArcherVX2_1; //평타1
    public ParticleSystem ArcherVX2_2; //평타2
    public GameObject Target;

    //Ray snipeRay;
    //RaycastHit snipeHit;
    public int combo = 1;

    public bool isMine;
    public int Photnet_AnimationSync = 0;
    // Start is called before the first frame update
    void Awake()
    {

        player = gameObject.GetComponent<CharacterMove>();
        archerAnim = gameObject.GetComponent<Animator>();
        player.job = Global.Classes.Archer;
        if (photonView.IsMine)
        {
            isMine = true;

            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    [PunRPC]
    public void InitStatus()
    {

        player.maxHP = 539;
        player.maxMP = 280;
        player.atk = 73;
        player.def = 32;

        player.hpRegen = 6;
        player.mpRegen = 6;
        player.stRegen = 8;
        attack_Cooltime = 1.0f;
        player.skill_1_Cooltime = 7.0f;
        player.skill_1_Cost = 20;
        player.skill_2_Cooltime = 14.0f;
        player.skill_2_Cost = 35;
        player.skill_3_Cooltime = 20.0f;
        player.skill_3_Cost = 50;
        player.skill_4_Cooltime = 30.0f;
        player.skill_4_Cost = 40;
        player.skill_5_Cooltime = 60.0f;
        player.skill_5_Cost = 100;
    }

    // Update is called once per frame
    void Update()
    {
        //ownerObject = player.gameObject.name;
        //animName = archerAnim.name;

        if (photonView.IsMine && !player.isDead && !player.isStun)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !attackOff && !isSniping)
                //photonView.RPC("Archer_Attack", RpcTarget.All);
                Archer_Attack();
            if (Input.GetKeyDown(KeyCode.Alpha1) && !player.skill_1_Off)
                //photonView.RPC("Archer_Skill1", RpcTarget.All);
                Archer_Skill1();
            if (Input.GetKeyDown(KeyCode.Alpha2) && !player.skill_2_Off)
                //photonView.RPC("Archer_Skill2", RpcTarget.All);
                Archer_Skill2();
            if (Input.GetKeyDown(KeyCode.Alpha3) && !player.skill_3_Off)
                //photonView.RPC("Archer_Skill3", RpcTarget.All);
                Archer_Skill3();
            if (Input.GetKeyDown(KeyCode.Alpha4) && !player.skill_4_Off)
                //photonView.RPC("Archer_Skill4", RpcTarget.All);
                Archer_Skill4();
            if (Input.GetKeyDown(KeyCode.Q) && !player.skill_5_Off)
                //photonView.RPC("Archer_Skill5", RpcTarget.All);
                Archer_Skill5();
            if (Input.GetKeyDown(KeyCode.Mouse0) && isSniping)
                SnipeShot();


            if (attackOff)
            {
                attack_Timer += Time.deltaTime;
                if (attack_Timer >= 0.6f)
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
            if (isSniping)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    //Vector3 dir = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z);
                    if (((hit.point.x - transform.position.x) * (hit.point.x - transform.position.x)) + ((hit.point.z - transform.position.z) * (hit.point.z - transform.position.z))
                        > 15.0f)
                    {
                        Target.transform.position = new Vector3(hit.point.x - transform.position.x, 0, hit.point.z - transform.position.z).normalized * 3 + new Vector3(transform.position.x, 0, transform.position.z);
                        Target.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        Target.transform.position = new Vector3(hit.point.x, 0, hit.point.z);
                        Target.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    //transform.rotation = Quaternion.LookRotation(dir);
                }

                //if (Input.GetKeyDown(KeyCode.Mouse0) && archerAnim.GetInteger("Combo") == 0 && !player.isDead && !player.isStun)
                //{
                //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                //    RaycastHit hit;

                //    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                //    {
                //        Vector3 dir = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z);
                //        transform.rotation = Quaternion.LookRotation(dir);
                //    }
                //}
            }
        }

        void ComboCounter(int count)
        {
            combo = count;
        }


        //[PunRPC]
        void Archer_Attack()
        {
            //player.isAttacking = true;
            attackOff = true;
            comboContinue = true;

            if (combo == 1)
            {
                SetLookAtMousePos();
            }


            if (comboTimer > 3.0f)
            {
                //archerAnim.SetInteger("Combo", 0);
                combo = 1;
                comboContinue = false;
            }
            comboTimer = 0;

            Vector3 dir = transform.forward;


            switch (combo)
            {
                case 1:
                    archerAnim.SetTrigger("FirstAttack");
                    if (photonView.IsMine)
                        PhotonNetwork.Instantiate("Prefebs/VFX/ArcherAttack1VX", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * ArcherVX2_1.transform.rotation);
                    //StartCoroutine("Skill_Hit");
                    SoundManager.Instance.ArcherSoundPlay(7);
                    break;

                case 2:
                    archerAnim.SetTrigger("SecondAttack");
                    if (photonView.IsMine)
                        PhotonNetwork.Instantiate("Prefebs/VFX/ArcherAttack2VX", ArcherAttack2Pos.transform.position, Quaternion.LookRotation(dir) * ArcherVX2_2.transform.rotation);
                    //StartCoroutine("Skill_Hit");
                    SoundManager.Instance.ArcherSoundPlay(7);
                    break;

                    //case 3:
                    //    archerAnim.SetTrigger("ThirdAttack");
                    //    if (photonView.IsMine)
                    //        PhotonNetwork.Instantiate("Prefebs/NomralArrow", new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.4f), Quaternion.LookRotation(dir) * ArcherArrow.transform.rotation);
                    //    SoundManager.Instance.ArcherSoundPlay(4);
                    //    SoundManager.Instance.ArcherSoundPlay(0);
                    //    break;

            }

            //if (archerAnim.GetInteger("Combo") == 0)
            //{
            //    archerAnim.SetTrigger("FirstAttack");
            //    if (photonView.IsMine)
            //        PhotonNetwork.Instantiate("Prefebs/VFX/ArcherAttack1VX", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * ArcherVX2_1.transform.rotation);
            //    StartCoroutine("Skill_Hit");
            //    SoundManager.Instance.ArcherSoundPlay(7);
            //}
            //else if (archerAnim.GetInteger("Combo") == 1)
            //{
            //    archerAnim.SetTrigger("SecondAttack");
            //    if (photonView.IsMine)
            //        PhotonNetwork.Instantiate("Prefebs/VFX/ArcherAttack2VX", ArcherAttack2Pos.transform.position, Quaternion.LookRotation(dir) * ArcherVX2_2.transform.rotation);
            //    StartCoroutine("Skill_Hit");
            //    SoundManager.Instance.ArcherSoundPlay(7);
            //}
            //else if (archerAnim.GetInteger("Combo") == 2)
            //{
            //    archerAnim.SetTrigger("ThirdAttack");
            //    if (photonView.IsMine)
            //        PhotonNetwork.Instantiate("Prefebs/NomralArrow", new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.4f), Quaternion.LookRotation(dir) * ArcherArrow.transform.rotation);
            //    SoundManager.Instance.ArcherSoundPlay(4);
            //    SoundManager.Instance.ArcherSoundPlay(0);

            //}




        }

        //[PunRPC]
        void Archer_Skill1()
        {
            if (player.mp >= player.skill_1_Cost && !player.isDead)
            {
                player.skill_1_Off = true;
                player.mp -= player.skill_1_Cost;
                //player.isAttacking = true;
                //playerAnim.SetBool("Skill2", true);
                archerAnim.SetTrigger("Skill1");
                SetLookAtMousePos();
                //Vector3 dir = transform.forward;
                if (photonView.IsMine)
                    Destroy(PhotonNetwork.Instantiate("Prefebs/VFX/PoisonArrow", new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z + 0.4f), transform.rotation/*Quaternion.LookRotation(dir) * ArcherArrow.transform.rotation*/), 4.0f);
                SoundManager.Instance.ArcherSoundPlay(4);
                SoundManager.Instance.ArcherSoundPlay(0);
                //Photnet_AnimationSync = 1;
                //Vector3 dir = player.transform.forward;

                //if (photonView.IsMine)
                //{
                //    PhotonNetwork.Instantiate("Prefebs/VFX/ArcherVX1", ArcherSkill1Pos.transform.position, Quaternion.LookRotation(dir) * ArcherVX1.transform.rotation);
                //}
                ////transform.rotation = Quaternion.LookRotation(dir);
                //StartCoroutine("Skill1_Hit");
                //SoundManager.Instance.ArcherSoundPlay(8);
            }

        }

        //[PunRPC]
        void Archer_Skill2()
        {
            if (player.mp >= player.skill_2_Cost && !player.isDead)
            {
                player.skill_2_Off = true;
                player.mp -= player.skill_2_Cost;
                archerAnim.SetTrigger("Skill2");
                SetLookAtMousePos();
                if (photonView.IsMine)
                    PhotonNetwork.Instantiate("Prefebs/VFX/ArcherSkill2VX", new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.4f), transform.rotation);


                //Photnet_AnimationSync = 2;
                //Vector3 dir = transform.forward;

                ////transform.rotation = Quaternion.LookRotation(dir);
                //Quaternion rot1 = ArcherArrow.transform.rotation * Quaternion.Euler(new Vector3(0, 0, -5.0f));
                //Quaternion rot2 = ArcherArrow.transform.rotation * Quaternion.Euler(new Vector3(0, 0, 5.0f));

                //SoundManager.Instance.ArcherSoundPlay(5);
                //SoundManager.Instance.ArcherSoundPlay(1);
                //if (photonView.IsMine)
                //{
                //    PhotonNetwork.Instantiate("Prefebs/MultiArrow", new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z + 0.4f), Quaternion.LookRotation(dir) * ArcherArrow.transform.rotation);

                //    PhotonNetwork.Instantiate("Prefebs/MultiArrow", new Vector3(transform.position.x + 0.1f, transform.position.y + 0.7f, transform.position.z + 0.4f), Quaternion.LookRotation(dir) * rot1);

                //    PhotonNetwork.Instantiate("Prefebs/MultiArrow", new Vector3(transform.position.x - 0.1f, transform.position.y + 0.7f, transform.position.z + 0.4f), Quaternion.LookRotation(dir) * rot2);
                //}
            }

        }

        //[PunRPC]
        void Archer_Skill3()
        {
            if (player.mp >= player.skill_3_Cost && !player.isDead)
            {
                player.skill_3_Off = true;
                player.mp -= player.skill_3_Cost;
                archerAnim.SetTrigger("Skill3");

                Vector3 dir = player.transform.forward;
                Vector3 pos;

                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    pos = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z);

                    Destroy(PhotonNetwork.Instantiate("Prefebs/VFX/ArcherSkill3VX", new Vector3(transform.position.x, transform.position.y + 15, transform.position.z) + pos, Quaternion.LookRotation(dir)), 5.0f);
                }

            }

        }


        //[PunRPC]
        void Archer_Skill4()
        {
            if (player.mp >= player.skill_4_Cost && !player.isDead)
            {
                player.skill_4_Off = true;
                player.mp -= player.skill_4_Cost;
                //
                Target.SetActive(true);
                player.ControlOff();
                archerAnim.SetBool("Move", false);
                archerAnim.SetBool("Run", false);
                isSniping = true;
                Photnet_AnimationSync = 4;
                //StartCoroutine("Archer_Skill4_Effect");

            }

        }

        void SnipeShot()
        {
            isSniping = false;
            player.ControlOn();
            Target.SetActive(false);
            archerAnim.SetTrigger("Skill4");
            Destroy(PhotonNetwork.Instantiate("Prefebs/VFX/ArcherSkill4VX", new Vector3(Target.transform.position.x, Target.transform.position.y + 0.3f, Target.transform.position.z), Target.transform.rotation), 4.1f);

            SetLookAtMousePos();
        }

        //[PunRPC]
        void Archer_Skill5()
        {
            if (player.mp >= player.skill_5_Cost && !player.isDead)
            {
                player.skill_5_Off = true;
                player.mp -= player.skill_5_Cost;
                StartCoroutine("Archer_Skill5_Effect");

            }

        }


        //IEnumerator Archer_Skill3_Delay()
        //{

        //}


        void SkillHitOn()
        {
            player.isAttacking = true;
        }

        void SkillHitOff()
        {
            player.isAttacking = false;
        }

    }
        //IEnumerator Skill_Hit()
        //{
        //    yield return new WaitForSeconds(0.05f);
        //    player.isAttacking = true;
        //    yield return new WaitForSeconds(0.25f);
        //    player.isAttacking = false;

        //}

        //IEnumerator Skill1_Hit()
        //{
        //    //yield return new WaitForSeconds(0.05f);
        //    player.isAttacking = true;
        //    yield return new WaitForSeconds(0.4f);
        //    player.isAttacking = false;

        //}

        

        IEnumerator Archer_Skill5_Effect()
        {
            float originSpeed = player.speed;

            SoundManager.Instance.ArcherSoundPlay(10);
            player.GetComponent<PhotonView>().RPC("OnHeal", RpcTarget.All, player.maxHP * 0.3f);

            GameObject buffEffect = PhotonNetwork.Instantiate("Prefebs/VFX/WarriorHealEffect", new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), transform.rotation);
            buffEffect.transform.parent = gameObject.transform;

            player.speed *= 1.2f;
            yield return new WaitForSeconds(1.2f);
            Destroy(buffEffect);
            yield return new WaitForSeconds(3.8f);
            player.speed = originSpeed;


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
            //if (stream.IsWriting)
            //{
            //    stream.SendNext(Photnet_AnimationSync);
            //    Photnet_AnimationSync = 0;


            //}
            //else
            //{
            //    Photnet_AnimationSync = (int)stream.ReceiveNext();

            //    switch (Photnet_AnimationSync)
            //    {
            //        case 1:
            //            archerAnim.SetTrigger("Skill1");
            //            Photnet_AnimationSync = 0;
            //            break;

            //        case 2:
            //            archerAnim.SetTrigger("Skill2");
            //            Photnet_AnimationSync = 0;
            //            break;

            //        case 3:
            //            archerAnim.SetTrigger("Skill3");
            //            Photnet_AnimationSync = 0;
            //            break;

            //        case 4:
            //            archerAnim.SetTrigger("Skill4");
            //            Photnet_AnimationSync = 0;
            //            break;
            //    }

            //}
        }
    
}