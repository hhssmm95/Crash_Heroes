using System.Collections;
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
    

    public bool isMine;
    public int combo = 1;
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
        player.mpRegen = 7;
        player.stRegen = 7;
        attack_Cooltime = 1.0f;
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
        //ownerObject = player.gameObject.name;
        //animName = warriorAnim.name;

        if (photonView.IsMine && !player.isDead && !player.isStun)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !attackOff)
                //photonView.RPC("Warrior_Attack", RpcTarget.All);
                Warrior_Attack();
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


            if (Input.GetKeyDown(KeyCode.Mouse0) && warriorAnim.GetInteger("Combo") == 1 && !player.isDead && !player.isStun)
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

    void ComboCounter(int count)
    {
        combo = count;
    }

    void Warrior_Attack()
    {
        //player.isAttacking = true;
        attackOff = true;
        comboContinue = true;

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
                if (photonView.IsMine)
                    PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack1VX", new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * WarriorVX1_1.transform.rotation);
                StartCoroutine("Skill_Hit");
                SoundManager.Instance.KnightSoundPlay(0);
                break;

            case 2:
                warriorAnim.SetTrigger("SecondAttack");
                if (photonView.IsMine)
                    PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack2VX", WarriorAttack2Pos.transform.position, Quaternion.LookRotation(dir) * WarriorVX1_2.transform.rotation);
                StartCoroutine("Skill_Hit");
                SoundManager.Instance.KnightSoundPlay(1);
                break;

            case 3:
                warriorAnim.SetTrigger("ThirdAttack");
                if (photonView.IsMine)
                    PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack3VX", new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.LookRotation(dir) * WarriorVX1_3.transform.rotation).GetComponent<PhotonView>();
                StartCoroutine("Skill_Hit");
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
            //StartCoroutine("Warrior_Skill1_Play");

            warriorAnim.SetTrigger("Skill1");
        }


    }

    [PunRPC]
    public void Warrior_Skill2()
    {
        if (player.mp >= player.skill_2_Cost)
        {
            player.skill_2_Off = true;
            player.mp -= player.skill_2_Cost;
            //player.isAttacking = true;
            //playerAnim.SetBool("Skill2", true);
            warriorAnim.SetTrigger("Skill2");
            //warriorAnim.SetBool("Skill2_2", true);
            StartCoroutine("Warrior_Skill2_VFX");
        }

    }

    [PunRPC]
    public void Warrior_Skill3()
    {
        if (player.mp >= player.skill_3_Cost)
        {
            player.skill_3_Off = true;
            player.mp -= player.skill_3_Cost;
            //playerAnim.SetBool("Skill2", true);
            warriorAnim.SetTrigger("Skill3_1");
            warriorAnim.SetBool("Skill3_2", true);
            StartCoroutine("Warrior_Skill3_VFX");
        }

    }

    //[PunRPC]
    public void Warrior_Skill4()
    {
        if (player.mp >= player.skill_4_Cost)
        {
            player.skill_4_Off = true;
            player.mp -= player.skill_4_Cost;
            //playerAnim.SetBool("Skill2", true);
            StartCoroutine("Warrior_Skill4_Effect");
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
            SoundManager.Instance.KnightSoundPlay(6);
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

    void DashAttackOn()
    {
        player.isDashing = true;
        player.dashAttacking_warrior = true;
    }
    void DashAttackOff()
    {
        player.dashAttacking_warrior = false;
        Vector3 dir = player.transform.forward;
        if (photonView.IsMine)
            PhotonNetwork.Instantiate("Prefebs/VFX/WarriorAttack3VX", new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.LookRotation(dir) * WarriorVX1_3.transform.rotation);
        StartCoroutine("Skill_Hit");
        SoundManager.Instance.KnightSoundPlay(2);
    }

    //IEnumerator Warrior_Skill1_Play()
    //{
    //    //playerAnim.stop
    //    //player.isDashing = true;
    //    //warriorAnim.SetTrigger("Dash");
    //    //player.isAttacking = true;
    //    //yield return new WaitForSeconds(0.1f);
    //    //player.isAttacking = false;
    //    yield return null;
    //}
    
    IEnumerator Warrior_Skill2_VFX()
    {
        Vector3 dir = transform.forward;
        if (photonView.IsMine)
            PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill2_1", new Vector3(transform.position.x, transform.position.y + 0.406f, transform.position.z + 0.1F), Quaternion.LookRotation(dir) * WarriorVX2_1.transform.rotation);
        StartCoroutine("Skill_Hit");
        SoundManager.Instance.KnightSoundPlay(3);
        yield return new WaitForSeconds(0.2f);
        if (photonView.IsMine)
            PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill2_2", new Vector3(transform.position.x, transform.position.y + 0.656f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * WarriorVX2_2.transform.rotation);
        StartCoroutine("Skill_Hit");
        SoundManager.Instance.KnightSoundPlay(3);
    }

    IEnumerator Warrior_Skill3_VFX()
    {

        Vector3 dir = player.transform.forward;
        yield return new WaitForSeconds(0.4f);
        if (photonView.IsMine)
            PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill3VX", WarriorSkill3Pos.transform.position, Quaternion.LookRotation(dir) * WarriorVX3.transform.rotation);
        SoundManager.Instance.KnightSoundPlay(5);
    }

    IEnumerator Warrior_Skill4_Effect()
    {
        float originAtk = player.atk;
        float originDef = player.def;
        GameObject buffEffect = PhotonNetwork.Instantiate("Prefebs/VFX/WarriorBuffEffect", new Vector3(transform.position.x, transform.position.y+0.1f, transform.position.z), transform.rotation);
        buffEffect.transform.parent = gameObject.transform;

        warriorAnim.SetTrigger("Skill4");
        SoundManager.Instance.KnightSoundPlay(4);
        player.atk *= 1.3f;
        player.def *= 1.3f;
        yield return new WaitForSeconds(20.0f);
        Destroy(buffEffect);

        player.atk = originAtk;
        player.def = originDef;
    }

    IEnumerator Warrior_Skill5_Effect()
    {
        GameObject buffEffect = PhotonNetwork.Instantiate("Prefebs/VFX/WarriorHealEffect", new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), transform.rotation);
        buffEffect.transform.parent = gameObject.transform;
        
        yield return new WaitForSeconds(1.2f);
        Destroy(buffEffect);
        
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
