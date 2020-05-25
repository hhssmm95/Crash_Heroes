using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillControl : MonoBehaviour
{
    CharacterMove player;
    Animator playerAnim;
    Camera mainCamera;

    public float attack_Cooltime;
    public float attack_Cost;

    public float skill_1_Cooltime;
    public float skill_1_Cost;

    public float skill_2_Cooltime;
    public float skill_2_Cost;

    public float skill_3_Cooltime;
    public float skill_3_Cost;

    public float skill_4_Cooltime;
    public float skill_4_Cost;

    public float skill_5_Cooltime;
    public float skill_5_Cost;


    public bool attackOff;
    public float attack_Timer;
    public bool comboContinue;
    public float comboTimer;
    public bool skill_1_Off;
    public bool skill_2_Off;
    public bool skill_3_Off;
    public bool skill_4_Off;
    public bool skill_5_Off;
    public float skill_1_Timer;
    public float skill_2_Timer;
    public float skill_3_Timer;
    public float skill_4_Timer;
    public float skill_5_Timer;

    public ParticleSystem ArcherVX1;
    public GameObject ArcherSkill1Pos;
    public ParticleSystem WarriorVX1_1;
    public ParticleSystem WarriorVX1_2;
    public GameObject WarriorAttack2Pos;
    public ParticleSystem WarriorVX1_3;
    public ParticleSystem WarriorVX2_1;
    public ParticleSystem WarriorVX2_2;

    void Start()
    {
        player = gameObject.GetComponent<CharacterMove>();
        playerAnim = gameObject.GetComponent<Animator>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //ArcherVX1 = GameObject.FindGameObjectWithTag("ArcherVX1").GetComponent<ParticleSystem>();
    }

    //public void Init(float attack_cool, float attack_cost, float skill_1_cool, float skill_1_cost, float skill_2_cool, float skill_2_cost,
    //    float skill_3_cool, float skill_3_cost, float skill_4_cool, float skill_4_cost)
    //{
    //    InitParent(attack_cool, attack_cost, skill_1_cool, skill_1_cost, skill_2_cool, skill_2_cost,
    //    skill_3_cool, skill_3_cost, skill_4_cool, skill_4_cost);
    //}

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (player.job == Global.Classes.Warrior)
            {
                Warrior_Attack();
                Warrior_Skill1();
                Warrior_Skill2();
                Warrior_Skill3();
                Warrior_Skill4();
            }
            if (player.job == Global.Classes.Archer)
            {
                Archer_Attack();
                Archer_Skill1();
            }
        }
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

        if(skill_1_Off)
        {
            skill_1_Timer += Time.deltaTime;
            if (skill_1_Timer >= 1.2f)
                player.isAttacking = false;
            if(skill_1_Timer >= skill_1_Cooltime)
            {
                skill_1_Off = false;
                skill_1_Timer = 0;
            }
        }
        if (skill_2_Off)
        {
            skill_2_Timer += Time.deltaTime;
            if (skill_2_Timer >= 1.0f)
            {
                player.isAttacking = false;
                playerAnim.SetBool("Skill2_2", false);
            }
            if (skill_2_Timer >= skill_2_Cooltime)
            {
                skill_2_Off = false;
                skill_2_Timer = 0;
            }
        }
        if (skill_3_Off)
        {
            skill_3_Timer += Time.deltaTime;
            if (skill_3_Timer >= 1.0f)
            {
                playerAnim.SetBool("Skill3_2", false);
            }
            if (skill_3_Timer >= skill_3_Cooltime)
            {
                skill_3_Off = false;
                skill_3_Timer = 0;
            }
        }
        if (skill_4_Off)
        {
            skill_4_Timer += Time.deltaTime;
            if (skill_4_Timer >= skill_4_Cooltime)
            {
                skill_4_Off = false;
                skill_4_Timer = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && playerAnim.GetInteger("Combo") == 0 && gameObject.tag == "Player")
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

    void Warrior_Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !attackOff)
        {
            player.isAttacking = true;
            attackOff = true;
            comboContinue = true;

            if (comboTimer > 3.0f)
            {
                playerAnim.SetInteger("Combo", 0);
                comboContinue = false;
            }
            comboTimer = 0;

            Vector3 dir = player.transform.forward;
            if (playerAnim.GetInteger("Combo") == 0)
            {
                playerAnim.SetTrigger("FirstAttack");
                Instantiate(WarriorVX1_1, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.1f), Quaternion.LookRotation(dir) * WarriorVX1_1.transform.rotation);

            }
            else if (playerAnim.GetInteger("Combo") == 1)
            {
                playerAnim.SetTrigger("SecondAttack");
                Instantiate(WarriorVX1_2, WarriorAttack2Pos.transform.position, Quaternion.LookRotation(dir) * WarriorVX1_2.transform.rotation);

            }
            else if (playerAnim.GetInteger("Combo") == 2)
            {
                playerAnim.SetTrigger("ThirdAttack");
                Instantiate(WarriorVX1_3, new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), Quaternion.LookRotation(dir) * WarriorVX1_3.transform.rotation);

            }
            

            //transform.rotation = Quaternion.LookRotation(dir);

        }
        //else if(playerAnim.GetCurrentAnimatorClipInfo(0))
    }

    void Warrior_Skill1()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && !skill_1_Off)
        {
            if (player.mp >= skill_1_Cost)
            {
                skill_1_Off = true;
                player.mp -= skill_1_Cost;
                StartCoroutine("Warrior_Skill1_Play");
            }
        }

    }

    void Warrior_Skill2()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2) && !skill_2_Off)
        {
            if (player.mp >= skill_2_Cost)
            {
                skill_2_Off = true;
                player.mp -= skill_2_Cost;
                player.isAttacking = true;
                //playerAnim.SetBool("Skill2", true);
                playerAnim.SetTrigger("Skill2_1");
                playerAnim.SetBool("Skill2_2", true);
                StartCoroutine("Warrior_Skill2_VFX");
            }
        }
    }

    void Warrior_Skill3()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3) && !skill_3_Off)
        {
            if (player.mp >= skill_3_Cost)
            {
                skill_3_Off = true;
                player.mp -= skill_3_Cost;
                //playerAnim.SetBool("Skill2", true);
                playerAnim.SetTrigger("Skill3_1");
                playerAnim.SetBool("Skill3_2", true);
            }
        }
    }

    void Warrior_Skill4()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4) && !skill_4_Off)
        {
            if (player.mp >= skill_4_Cost)
            {
                skill_4_Off = true;
                player.mp -= skill_4_Cost;
                //playerAnim.SetBool("Skill2", true);
                playerAnim.SetTrigger("Skill4");
               
            }
        }
    }

    IEnumerator Warrior_Skill1_Play()
    {
        //playerAnim.stop
        player.isDashing = true;
        player.isAttacking = true;
        playerAnim.SetTrigger("Dash");
        yield return new WaitForSeconds(0.1f);
        playerAnim.SetTrigger("ThirdAttack");
        Vector3 dir = player.transform.forward;
        Instantiate(WarriorVX1_3, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), Quaternion.LookRotation(dir) * WarriorVX1_3.transform.rotation);

    }

    IEnumerator Warrior_Skill2_VFX()
    {
        Vector3 dir = transform.forward;
        Instantiate(WarriorVX2_1, new Vector3(transform.position.x, transform.position.y + 0.406f, transform.position.z+0.1F), Quaternion.LookRotation(dir) * WarriorVX2_1.transform.rotation);

        yield return new WaitForSeconds(0.2f);

        Instantiate(WarriorVX2_2, new Vector3(transform.position.x, transform.position.y + 0.656f, transform.position.z+0.1f), Quaternion.LookRotation(dir) * WarriorVX2_2.transform.rotation);

    }

    void Archer_Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !attackOff)
        {
            player.isAttacking = true;
            attackOff = true;
            comboContinue = true;

            if (comboTimer > 3.0f)
            {
                playerAnim.SetInteger("Combo", 0);
                comboContinue = false;
            }
            comboTimer = 0;

            if (playerAnim.GetInteger("Combo") == 0)
                playerAnim.SetTrigger("FirstAttack");
            else if (playerAnim.GetInteger("Combo") == 1)
                playerAnim.SetTrigger("SecondAttack");
            else if (playerAnim.GetInteger("Combo") == 2)
                playerAnim.SetTrigger("ThirdAttack");



        }
    }
    void Archer_Skill1()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !skill_1_Off)
        {
            if (player.mp >= skill_1_Cost)
            {
                skill_1_Off = true;
                player.mp -= skill_1_Cost;
                //player.isAttacking = true;
                //playerAnim.SetBool("Skill2", true);
                playerAnim.SetTrigger("Skill1");
                Vector3 dir = player.transform.forward;
                
                //transform.rotation = Quaternion.LookRotation(dir);
                Instantiate(ArcherVX1, ArcherSkill1Pos.transform.position, Quaternion.LookRotation(dir)*ArcherVX1.transform.rotation);
            }
        }
    }
}
