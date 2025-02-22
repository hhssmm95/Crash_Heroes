﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WarriorSkill : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove player;
    public Animator warriorAnim;
    Camera mainCamera;

    public float attack_Cooltime;
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

    public GameObject WMesh;
    public GameObject WRoot;
    public BoxCollider WColl;
    Vector3 originPos;
    public int Photnet_AnimationSync = 0;
    public bool isMine;
    public int combo = 1;
    GameObject s4Effect;
    void Awake()
    {
        //if(CompareTag("Player"))

        player = gameObject.GetComponent<CharacterMove>();
        warriorAnim = gameObject.GetComponent<Animator>();
        player.job = Global.Classes.Warrior;

        if (photonView.IsMine)
        {
            isMine = true;

            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

    }
    

    [PunRPC]
    public void InitStatus()
    {

        player.maxHP = 582.0f;
        player.maxMP = 263.0f;
        player.atk = 64.0f;
        player.def = 39.0f;
        player.hpRegen = 9.0f;
        player.mpRegen = 6;
        player.stRegen = 7;
        attack_Cooltime = 0.4f;
        player.skill_1_Cooltime = 8.0f;
        player.skill_1_Cost = 32.0f;
        player.skill_2_Cooltime = 28.0f;
        player.skill_2_Cost = 45.0f;
        player.skill_3_Cooltime = 15.0f;
        player.skill_3_Cost = 40.0f;
        player.skill_4_Cooltime = 118.0f;
        player.skill_4_Cost = 126.0f;
        player.skill_5_Cooltime = 60.0f;
        player.skill_5_Cost = 60.0f;
    }

    void Start()
    {
        WColl = GetComponent<BoxCollider>();
    }
    // Update is called once per frame
    void Update()
    {
        //ownerObject = player.gameObject.name;
        //animName = warriorAnim.name;

        if (photonView.IsMine && !player.isDead && !player.isHeavyDamaging && !player.isStun && !player.isExhausting)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !attackOff)
                //photonView.RPC("Warrior_Attack", RpcTarget.All);
                //Warrior_Attack();
                StartCoroutine("Warrior_Attack_Effect");
            if (Input.GetKeyDown(KeyCode.Alpha1) && !player.skill_1_Off)
                //photonView.RPC("Warrior_Skill1", RpcTarget.All);
                Warrior_Skill1();
            if (Input.GetKeyDown(KeyCode.Alpha2) && !player.skill_2_Off)
                //photonView.RPC("Warrior_Skill2", RpcTarget.All);
                Warrior_Skill2();
            if (Input.GetKeyDown(KeyCode.Alpha3) && !player.skill_3_Off)
                //photonView.RPC("Warrior_Skill3", RpcTarget.All);
                Warrior_Skill3();
            if (Input.GetKeyDown(KeyCode.Alpha4) && !player.skill_4_Off)
                //photonView.RPC("Warrior_Skill4", RpcTarget.All);
                Warrior_Skill4();
            if (Input.GetKeyDown(KeyCode.Q) && !player.skill_5_Off)
                //photonView.RPC("Warrior_Skill5", RpcTarget.All);
                Warrior_Skill5();


            //if (attackOff)
            //{
            //    attack_Timer += Time.deltaTime;
            //    //if (attack_Timer >= 1.0f)
            //    //    player.isAttacking = false;
            //    if (attack_Timer >= attack_Cooltime)
            //    {
            //        attackOff = false;
            //        attack_Timer = 0;
            //    }
            //}


            if (comboContinue)
            {

                comboTimer += Time.deltaTime;
            }


            //if (Input.GetKeyDown(KeyCode.Mouse0) && warriorAnim.GetInteger("Combo") == 1 && !player.isDead && !player.isStun)
            //{
            //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //    RaycastHit hit;

            //    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            //    {
            //        Vector3 dir = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z);
            //        transform.rotation = Quaternion.LookRotation(dir);
            //    }
            //}


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

    void ComboCounter(int count)
    {
        combo = count;
    }

    void Warrior_Attack()
    {
        //player.isAttacking = true;
        attackOff = true;
        comboContinue = true;

        if(combo == 1)
        {
            SetLookAtMousePos();
        }

        if (comboTimer > 3.0f)
        {
            //warriorAnim.SetInteger("Combo", 0);
            combo = 1;
            comboContinue = false;
        }
        comboTimer = 0;

        Vector3 dir = player.transform.forward;

        switch(combo)
        {
            case 1:
                warriorAnim.SetTrigger("FirstAttack");
                Photnet_AnimationSync = 1;
                if (photonView.IsMine)
                    Destroy(PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack1VX", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * WarriorVX1_1.transform.rotation),1.0f);
                //StartCoroutine("Skill_Hit");
                SoundManager.Instance.KnightSoundPlay(0);
                break;

            case 2:
                warriorAnim.SetTrigger("SecondAttack");
                Photnet_AnimationSync = 2;
                if (photonView.IsMine)
                    Destroy(PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack2VX", WarriorAttack2Pos.transform.position, Quaternion.LookRotation(dir) * WarriorVX1_2.transform.rotation),1.0f);
                //StartCoroutine("Skill_Hit");
                SoundManager.Instance.KnightSoundPlay(1);
                break;

            case 3:
                warriorAnim.SetTrigger("ThirdAttack");
                Photnet_AnimationSync = 3;
                if (photonView.IsMine)
                    Destroy(PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack3VX", new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.LookRotation(dir) * WarriorVX1_3.transform.rotation),1.0f);
                //StartCoroutine("Skill_Hit");
                SoundManager.Instance.KnightSoundPlay(2);
                break;

        }
        //if (combo == 1/*warriorAnim.GetInteger("Combo") == 0*/)
        //{
        //    warriorAnim.SetTrigger("FirstAttack");
        //    if (photonView.IsMine)
        //        PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack1VX", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * WarriorVX1_1.transform.rotation);
        //    StartCoroutine("Skill_Hit");
        //    SoundManager.Instance.KnightSoundPlay(0);
        //}
        //else if (combo == 2/*warriorAnim.GetInteger("Combo") == 1*/)
        //{
        //    warriorAnim.SetTrigger("SecondAttack");
        //    if (photonView.IsMine)
        //        PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack2VX", WarriorAttack2Pos.transform.position, Quaternion.LookRotation(dir) * WarriorVX1_2.transform.rotation);
        //    StartCoroutine("Skill_Hit");
        //    SoundManager.Instance.KnightSoundPlay(1);
        //}
        //else if (combo == 3/*warriorAnim.GetInteger("Combo") == 2*/)
        //{
        //    warriorAnim.SetTrigger("ThirdAttack");
        //    //if (photonView.IsMine)
        //    //    PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack3VX", new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.LookRotation(dir) * WarriorVX1_3.transform.rotation).GetComponent<PhotonView>();
        //    StartCoroutine("Skill_Hit");
        //    SoundManager.Instance.KnightSoundPlay(2);

        //}

    }
    

    private void OnCollisionEnter(Collision collision)
    {
        if (player.dashAttacking_warrior && collision.gameObject.layer == 9)
        {
            CharacterMove enemy = collision.gameObject.GetComponent<CharacterMove>();

            enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, player.atk * 0.8f, player.transform.forward);
        }
           
    }

    
    public void Warrior_Skill1()
    {

        if (player.mp >= player.skill_1_Cost)
        {
            player.skill_1_Off = true;
            player.mp -= player.skill_1_Cost;
            StartCoroutine("Warrior_Skill1_VFX");

        }


    }

    //[PunRPC]
    public void Warrior_Skill2()
    {
        if (player.mp >= player.skill_2_Cost)
        {
            player.skill_2_Off = true;
            player.mp -= player.skill_2_Cost;
            StartCoroutine("Warrior_Skill2_VFX");
        }
        //if (player.mp >= player.skill_2_Cost)
        //{
        //    player.skill_2_Off = true;
        //    player.mp -= player.skill_2_Cost;
        //    //player.isAttacking = true;
        //    //playerAnim.SetBool("Skill2", true);
        //    warriorAnim.SetTrigger("Skill2");
        //    //warriorAnim.SetBool("Skill2_2", true);
        //    StartCoroutine("Warrior_Skill2_VFX");
        //}

    }

    //[PunRPC]
    public void Warrior_Skill3()
    {
        if (player.mp >= player.skill_3_Cost)
        {
            player.skill_3_Off = true;
            player.mp -= player.skill_3_Cost;
            //playerAnim.SetBool("Skill2", true);
            
            StartCoroutine("Warrior_Skill3_VFX");
        }

    }

    //[PunRPC]
    public void Warrior_Skill4()
    {
        if (player.mp >= player.skill_4_Cost)
        {
            SetLookAtMousePos();
            player.skill_4_Off = true;
            player.mp -= player.skill_4_Cost;
            
           
            StartCoroutine("Warrior_Skill4_1_VFX");
        }

    }

    //[PunRPC]
    public void Warrior_Skill5()
    {
        if(player.mp >= player.skill_5_Cost)
        {
            player.skill_5_Off = true;
            player.mp -= player.skill_5_Cost;
            healing = true;
            //SoundManager.Instance.KnightSoundPlay(6);
            StartCoroutine("Warrior_Skill5_Effect");
        }
    }

    IEnumerator Skill_Hit()
    {
        yield return new WaitForSeconds(0.05f);
        player.isAttacking = true;
        yield return new WaitForSeconds(0.25f);
        player.isAttacking = false;

    }
    
    void SkillHitOn()
    {
        player.isAttacking = true;
    }

    void SkillHitOff()
    {
        player.isAttacking = false;
    }

    IEnumerator Warrior_Attack_Effect()
    {
        attackOff = true;
        comboContinue = true;

        if (combo == 1)
        {
            SetLookAtMousePos();
        }

        if (comboTimer > 3.0f)
        {
            //warriorAnim.SetInteger("Combo", 0);
            combo = 1;
            comboContinue = false;
        }
        comboTimer = 0;

        Vector3 dir = player.transform.forward;

        switch (combo)
        {
            case 1:
                warriorAnim.SetTrigger("FirstAttack");
                Photnet_AnimationSync = 1;

                var effect1 = PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack1VX", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * WarriorVX1_1.transform.rotation);
                //StartCoroutine("Skill_Hit");
                SoundManager.Instance.KnightSoundPlay(0);
                yield return new WaitForSeconds(attack_Cooltime);
                attackOff = false;
                break;

            case 2:
                warriorAnim.SetTrigger("SecondAttack");
                Photnet_AnimationSync = 2;
                PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack2VX", WarriorAttack2Pos.transform.position, Quaternion.LookRotation(dir) * WarriorVX1_2.transform.rotation);
                //StartCoroutine("Skill_Hit");
                SoundManager.Instance.KnightSoundPlay(1);
                yield return new WaitForSeconds(attack_Cooltime);
                attackOff = false;
                break;

            case 3:
                warriorAnim.SetTrigger("ThirdAttack");
                Photnet_AnimationSync = 3;
                PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack3VX", new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.LookRotation(dir) * WarriorVX1_3.transform.rotation);
                //StartCoroutine("Skill_Hit");
                SoundManager.Instance.KnightSoundPlay(2);
                yield return new WaitForSeconds(attack_Cooltime + 0.5f);
                attackOff = false;
                break;

        }
    }

    IEnumerator Warrior_Skill1_VFX()
    {
        if (photonView.IsMine)
        {
            SetLookAtMousePos();
            warriorAnim.SetTrigger("Skill1");
            Photnet_AnimationSync = 4;
            GameObject effect = PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill1VX", new Vector3(transform.position.x, transform.position.y + 0.53f, transform.position.z), transform.rotation);
            effect.transform.parent = gameObject.transform;
            SoundManager.Instance.KnightSoundPlay(3);
            yield return new WaitForSeconds(3.0f);
            PhotonNetwork.Destroy(effect);
        }
        yield return null;
    }
    
    IEnumerator Warrior_Skill2_VFX()
    {
        float originAtk = player.atk;
        float originDef = player.def;
        GameObject buffEffect = PhotonNetwork.Instantiate("Prefebs/VFX/WarriorBuffEffect", new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), transform.rotation);
        buffEffect.transform.parent = gameObject.transform;

        warriorAnim.SetTrigger("Skill2");
        Photnet_AnimationSync = 5;
        SoundManager.Instance.KnightSoundPlay(4);
        player.atk *= 1.5f;
        //player.def *= 1.3f;
        yield return new WaitForSeconds(8.0f);
        Destroy(buffEffect);

        player.atk = originAtk;
        player.def = originDef;

        //Vector3 dir = transform.forward;
        //if (photonView.IsMine)
        //    PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill2_1", new Vector3(transform.position.x, transform.position.y + 0.406f, transform.position.z + 0.1F), Quaternion.LookRotation(dir) * WarriorVX2_1.transform.rotation);
        //StartCoroutine("Skill_Hit");
        //SoundManager.Instance.KnightSoundPlay(3);
        //yield return new WaitForSeconds(0.2f);
        //if (photonView.IsMine)
        //    PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill2_2", new Vector3(transform.position.x, transform.position.y + 0.656f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * WarriorVX2_2.transform.rotation);
        //StartCoroutine("Skill_Hit");
        //SoundManager.Instance.KnightSoundPlay(3);
    }

    IEnumerator Warrior_Skill3_VFX()
    {
        SetLookAtMousePos();
        warriorAnim.SetTrigger("Skill3");
        Photnet_AnimationSync = 6;
        Vector3 dir = player.transform.forward;
        yield return new WaitForSeconds(0.2f);
        var effect = PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill3VX", WarriorSkill3Pos.transform.position, Quaternion.LookRotation(dir) * WarriorVX3.transform.rotation);

        SoundManager.Instance.KnightSoundPlay(5);
        yield return new WaitForSeconds(2.0f);
        PhotonNetwork.Destroy(effect);
    }

    IEnumerator Warrior_Skill4_1_VFX()
    {
        originPos = transform.position;
        Quaternion.LookRotation(transform.right);
        warriorAnim.SetTrigger("Skill4");
        Photnet_AnimationSync = 7;
        player.isAttacking = true;
        player.ControlOff();
        s4Effect = PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill4VX", new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), transform.rotation);
        StartCoroutine("Skill4_Sound");
        yield return new WaitForSeconds(2.3f);
        StartCoroutine("Warrior_Skill4_2_VFX");

        //yield return new WaitForSeconds(3.5f);
        // player.isAttacking = false;
    }
    IEnumerator Warrior_Skill4_2_VFX()
    {
        //yield return new WaitForSeconds(2.1f);
        player.isAttacking = false;
        var myRig = GetComponent<Rigidbody>().useGravity = true;
        WMesh.SetActive(true);
        WRoot.SetActive(true);
        WColl.enabled = true;
        player.ControlOn();
        PhotonNetwork.Destroy(s4Effect);
        yield return null;
    }
    void Invisible()
    {
        WMesh.SetActive(false);
        WRoot.SetActive(false);
        WColl.enabled = false;
        
        transform.position = new Vector3(originPos.x, originPos.y + 1.5f, originPos.z);
        var myRig = GetComponent<Rigidbody>().useGravity = false;
        //StartCoroutine("Warrior_Skill4_2_VFX");
    }

    IEnumerator Skill4_Sound()
    {
        float second = 0;
        SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.knightSoundList[6]);
        yield return new WaitForSeconds(0.2f);
        second += 0.2f;
        SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.knightSoundList[7]);
        yield return new WaitForSeconds(0.2f);
        second += 0.2f;
        while (second< 2.2f)
        {
            SoundManager.Instance.audioSource.PlayOneShot(SoundManager.Instance.knightSoundList[8]);
            yield return new WaitForSeconds(0.2f);
            second += 0.2f;
        }

    }

    IEnumerator Warrior_Skill5_Effect()
    {
        GameObject buffEffect = PhotonNetwork.Instantiate("Prefebs/VFX/WarriorHealEffect", new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), transform.rotation);
        buffEffect.transform.parent = gameObject.transform;
        SoundManager.Instance.KnightSoundPlay(9);
        yield return new WaitForSeconds(1.2f);
        Destroy(buffEffect);
        
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
        if (stream.IsWriting)
        {
            stream.SendNext(Photnet_AnimationSync);
            stream.SendNext(WMesh.activeSelf);
            stream.SendNext(WRoot.activeSelf);
            stream.SendNext(WColl.enabled);

            Photnet_AnimationSync = 0;


        }
        else
        {
            Photnet_AnimationSync = (int)stream.ReceiveNext();
            WMesh.SetActive((bool)stream.ReceiveNext());
            WRoot.SetActive((bool)stream.ReceiveNext());
            WColl.enabled = (bool)stream.ReceiveNext();
            if (!photonView.IsMine)
            {
                switch (Photnet_AnimationSync)
                {
                    case 1:
                        warriorAnim.SetTrigger("FirstAttack");
                        break;

                    case 2:
                        warriorAnim.SetTrigger("SecondAttack");
                        break;

                    case 3:
                        warriorAnim.SetTrigger("ThirdAttack");
                        break;

                    case 4:
                        warriorAnim.SetTrigger("Skill1");
                        break;

                    case 5:
                        warriorAnim.SetTrigger("Skill2");
                        break;

                    case 6:
                        warriorAnim.SetTrigger("Skill3");
                        break;

                    case 7:
                        warriorAnim.SetTrigger("Skill4");
                        break;
                }

            }
            Photnet_AnimationSync = 0;

        }
    }
}
