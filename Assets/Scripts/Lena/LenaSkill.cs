﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LenaSkill : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove player;
    public Animator lenaAnim;
    Camera mainCamera;
    public TrailRenderer trailRenderer;

    public float attack_Cooltime;
    public float attack_Cost;

    public bool attackOff;
    public float attack_Timer;
    public bool comboContinue;
    public float comboTimer;
    public int combo = 1;

    float healTimer;
    float healDurationTimer;
    bool healing;

    public ParticleSystem LenaAttack1VX;
    public ParticleSystem LenaAttack2VX;
    public ParticleSystem LenaAttack3VX;
    public GameObject LenaAttack2Pos;
    public ParticleSystem LenaVX2_1;
    public ParticleSystem LenaVX2_2;

    public ParticleSystem LenaVX3;
    public Transform Skill_01_Pos;
    public GameObject LenaSkill3Pos;


    public bool isMine;
    public string ownerObject;
    public string animName;

    void Awake()
    {
        //if(CompareTag("Player"))

        player = gameObject.GetComponent<CharacterMove>();
        lenaAnim = gameObject.GetComponent<Animator>();
        player.job = Global.Classes.Lena;

        if (photonView.IsMine)
        {
            isMine = true;

            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            Skill_01_Pos = GameObject.Find("SkillPos").GetComponent<Transform>();
        }

    }

    [PunRPC]
    public void InitStatus()
    {

        player.maxHP = 550.0f;
        player.maxMP = 300.0f;
        player.atk = 66.0f;
        player.def = 33.0f;
        player.hpRegen = 7.0f;
        player.mpRegen = 8.0f;
        player.stRegen = 5.0f;
        attack_Cooltime = 1.0f;
        player.skill_1_Cooltime = 6.0f;
        player.skill_1_Cost = 20.0f;
        player.skill_2_Cooltime = 9.0f;
        player.skill_2_Cost = 25.0f;
        player.skill_3_Cooltime = 18.0f;
        player.skill_3_Cost = 45.0f;
        player.skill_4_Cooltime = 95.0f;
        player.skill_4_Cost = 125.0f;
        player.skill_5_Cooltime = 30.0f;
        player.skill_5_Cost = 100.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //ownerObject = player.gameObject.name;
        //animName = warriorAnim.name;

        if (photonView.IsMine && !player.isDead && !player.isStun)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !attackOff)
                //photonView.RPC("Lena_Attack", RpcTarget.All);
                Lena_Attack();
            if (Input.GetKeyDown(KeyCode.Alpha1) && !player.skill_1_Off)
                //photonView.RPC("Lena_Skill1", RpcTarget.All);
                Lena_Skill1();
            if (Input.GetKeyDown(KeyCode.Alpha2) && !player.skill_2_Off)
                photonView.RPC("Lena_Skill2", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha3) && !player.skill_3_Off)
                photonView.RPC("Lena_Skill3", RpcTarget.All);
            if (Input.GetKeyDown(KeyCode.Alpha4) && !player.skill_4_Off)
                //photonView.RPC("Lena_Skill4", RpcTarget.All);
                Lena_Skill4();
            if (Input.GetKeyDown(KeyCode.Q) && !player.skill_5_Off)
                photonView.RPC("Lena_Skill5", RpcTarget.All);


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

            if (Input.GetKeyDown(KeyCode.Mouse0) && lenaAnim.GetInteger("Combo") == 0 && !player.isDead && !player.isStun)
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Vector3 dir = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z);
                    transform.rotation = Quaternion.LookRotation(dir);
                }
            }

            if (healing && !player.isDead)
            {
                if (healDurationTimer >= 5.0f)
                {
                    healing = false;
                    healDurationTimer = 0;
                    healTimer = 0;
                    return;
                }
                healTimer += Time.deltaTime;
                healDurationTimer += Time.deltaTime;

            }

            if (healing && !player.isDead && healTimer >= 1.0f)
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

    void Lena_Attack()
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
                lenaAnim.SetTrigger("FirstAttack");
                StartCoroutine("Attack");
                if (photonView.IsMine)
                    Destroy(PhotonNetwork.Instantiate("Prefebs/VFX/LenaAttack1VX", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * LenaAttack1VX.transform.rotation), 1.0f);
                SoundManager.Instance.KnightSoundPlay(0);
                break;

            case 2:
                lenaAnim.SetTrigger("SecondAttack");
                StartCoroutine("Attack");
                if (photonView.IsMine)
                    Destroy(PhotonNetwork.Instantiate("Prefebs/VFX/LenaAttack2VX", LenaAttack2Pos.transform.position, Quaternion.LookRotation(dir) * LenaAttack2VX.transform.rotation), 1.0f);
                SoundManager.Instance.KnightSoundPlay(1);
                break;

            case 3:
                lenaAnim.SetTrigger("ThirdAttack");
                StartCoroutine("Attack");
                if (photonView.IsMine)
                    Destroy(PhotonNetwork.Instantiate("Prefebs/VFX/LenaAttack3VX", new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.LookRotation(dir) * LenaAttack3VX.transform.rotation), 1.0f);
                SoundManager.Instance.KnightSoundPlay(2);
                break;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (player.dashAttacking_warrior && collision.gameObject.layer == 9 && player.isAttacking)
        //{
        //    CharacterMove enemy = collision.gameObject.GetComponent<CharacterMove>();

        //    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, player.atk * 0.8f, player.transform.forward);
        //}

    }

    public void Lena_Skill1()
    {
        if (player.mp >= player.skill_1_Cost)
        {
            player.skill_1_Off = true;
            player.mp -= player.skill_1_Cost;

            lenaAnim.SetTrigger("Skill1");
            Vector3 dir = player.transform.forward;
            PhotonNetwork.Instantiate("Prefebs/VFX/LenaSkill_01", Skill_01_Pos.position, Skill_01_Pos.rotation);
        }
    }

    [PunRPC]
    public void Lena_Skill2()
    {
        if (player.mp >= player.skill_2_Cost)
        {
            player.skill_2_Off = true;
            player.mp -= player.skill_2_Cost;
            //player.isAttacking = true;
            //playerAnim.SetBool("Skill2", true);
            lenaAnim.SetTrigger("Skill2_1");
            lenaAnim.SetBool("Skill2_2", true);
            StartCoroutine("Lena_Skill2_VFX");
        }

    }

    [PunRPC]
    public void Lena_Skill3()
    {
        if (player.mp >= player.skill_3_Cost)
        {
            player.skill_3_Off = true;
            player.mp -= player.skill_3_Cost;
            //playerAnim.SetBool("Skill2", true);
            lenaAnim.SetTrigger("Skill3_1");
            lenaAnim.SetBool("Skill3_2", true);
            StartCoroutine("Lena_Skill3_VFX");
        }

    }

    //[PunRPC]
    public void Lena_Skill4()
    {
        if (player.mp >= player.skill_4_Cost)
        {
            player.skill_4_Off = true;
            player.mp -= player.skill_4_Cost;
            //playerAnim.SetBool("Skill2", true);
            StartCoroutine("Lena_Skill4_Effect");
        }

    }

    //[PunRPC]
    public void Lena_Skill5()
    {
        if (player.mp >= player.skill_5_Cost)
        {
            player.skill_5_Off = true;
            player.mp -= player.skill_5_Cost;
            healing = true;
            SoundManager.Instance.KnightSoundPlay(6);
            StartCoroutine("Lena_Skill5_Effect");
        }
    }

    IEnumerator Skill_Hit()
    {
        yield return new WaitForSeconds(0.05f);
        player.isAttacking = true;
        yield return new WaitForSeconds(0.25f);
        player.isAttacking = false;

    }

    IEnumerator Lena_Skill1_Play()
    {
        //playerAnim.stop
        player.isDashing = true;
        lenaAnim.SetTrigger("Dash");
        player.isAttacking = true;
        player.dashAttacking_warrior = true;
        yield return new WaitForSeconds(0.1f);
        player.dashAttacking_warrior = false;
        lenaAnim.SetTrigger("ThirdAttack");
        player.isAttacking = false;
        Vector3 dir = player.transform.forward;
        if (photonView.IsMine)
            PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack3VX", new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.LookRotation(dir) * LenaAttack3VX.transform.rotation);
        StartCoroutine("Skill_Hit");
        SoundManager.Instance.KnightSoundPlay(2);
    }

    IEnumerator Lena_Skill2_VFX()
    {
        Vector3 dir = transform.forward;
        if (photonView.IsMine)
            PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill2_1", new Vector3(transform.position.x, transform.position.y + 0.406f, transform.position.z + 0.1F), Quaternion.LookRotation(dir) * LenaVX2_1.transform.rotation);
        StartCoroutine("Skill_Hit");
        SoundManager.Instance.KnightSoundPlay(3);
        yield return new WaitForSeconds(0.2f);
        if (photonView.IsMine)
            PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill2_2", new Vector3(transform.position.x, transform.position.y + 0.656f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * LenaVX2_2.transform.rotation);
        StartCoroutine("Skill_Hit");
        SoundManager.Instance.KnightSoundPlay(3);
    }

    IEnumerator Lena_Skill3_VFX()
    {

        Vector3 dir = player.transform.forward;
        yield return new WaitForSeconds(0.4f);
        if (photonView.IsMine)
            PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill3VX", LenaSkill3Pos.transform.position, Quaternion.LookRotation(dir) * LenaVX3.transform.rotation);
        SoundManager.Instance.KnightSoundPlay(5);
    }

    IEnumerator Lena_Skill4_Effect()
    {
        float originAtk = player.atk;
        float originDef = player.def;
        GameObject buffEffect = PhotonNetwork.Instantiate("Prefebs/VFX/WarriorBuffEffect", new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), transform.rotation);
        buffEffect.transform.parent = gameObject.transform;

        lenaAnim.SetTrigger("Skill4");
        SoundManager.Instance.KnightSoundPlay(4);
        player.atk *= 1.3f;
        player.def *= 1.3f;
        yield return new WaitForSeconds(20.0f);
        Destroy(buffEffect);

        player.atk = originAtk;
        player.def = originDef;
    }

    IEnumerator Lena_Skill5_Effect()
    {
        GameObject buffEffect = PhotonNetwork.Instantiate("Prefebs/VFX/WarriorHealEffect", new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), transform.rotation);
        buffEffect.transform.parent = gameObject.transform;

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

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.1f);
        trailRenderer.enabled = true;
        yield return new WaitForSeconds(0.3f);
        trailRenderer.enabled = false;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
