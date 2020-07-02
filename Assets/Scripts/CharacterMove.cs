using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CharacterMove : MonoBehaviourPunCallbacks, IPunObservable //캐릭터의 전반적인 입력들과 애니메이션, 상태 처리 클래스
{
    public HealthBar hpBar;
    private Animator myAnim;
    //private Animator myAnim2;
    private Rigidbody myRig;

    private Camera mainCamera;
    private CameraLocator cameraLoc;
    public Global.Classes job;

    //private float delay = 1.0f; //점프 딜레이를 위한 카운터
    private bool jumpCooltime; //점프 후 아직 쿨타임 중일경우 true
    public static bool dying; //캐릭터 사망 애니메이션 중복 재생 방지용 변수
    public bool isGround; //캐릭터의 발이 땅에 붙어있을때 true
    public float maxHP;
    public float hp;
    public float maxMP;
    public float mp;
    public float atk;
    public float def;
    public int potion;
    public bool isDamaging;
    public bool isDashing;
    float mpTimer;
    float damageTimer;

    public float speed = 2.0f; // 캐릭터 이동속도
    public float jumpPower = 5.0f;
    public float rotateSpeed = 10.0f;
    public static bool isDead; // ※전역변수, true일때 즉시 사망 애니메이션 진행
    //public GameObject GameOverPanel;
    public Vector3 moveDirection;
    public bool isAttacking;
    float dashTimer;
    private float h;
    private float v;

    public bool isMine;

    private UImanager ui;

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

    //private int score;
    public WarriorSkill wSkill;
    public ArcherSkill aSkill;
    public DragoonSkill dSkill;
    //MageSkill mSkill;
    void Start()
    {

        myAnim = gameObject.GetComponent<Animator>();
        myRig = gameObject.GetComponent<Rigidbody>();

        wSkill = gameObject.GetComponent<WarriorSkill>();
        aSkill = gameObject.GetComponent<ArcherSkill>();
        dSkill = gameObject.GetComponent<DragoonSkill>();
        //mSkill = gameObject.GetComponent<MageSkill>();


        if (wSkill != null)
            job = Global.Classes.Warrior;
        else if (aSkill != null)
            job = Global.Classes.Archer;
        else if (dSkill != null)
            job = Global.Classes.Dragoon;
        //else if (mSkill != null)
        //    job = Global.Classes.Mage;

            switch (job)
        {
            case Global.Classes.Warrior:
                wSkill.photonView.RPC("InitStatus", RpcTarget.All);
                //photonView.RPC("Warrior_Attack", RpcTarget.All);
                break;

            case Global.Classes.Archer:
                aSkill.photonView.RPC("InitStatus", RpcTarget.All);

                break;

            case Global.Classes.Dragoon:
                dSkill.photonView.RPC("InitStatus", RpcTarget.All);

                break;

            case Global.Classes.Mage:
                //maxHP = 524;
                //maxMP = 326;
                //atk = 65;
                //def = 29;
                //skill.attack_Cooltime = 0.5f;
                //skill.skill_1_Cooltime = 3.0f;
                //skill.skill_2_Cooltime = 3.0f;
                //skill.skill_3_Cooltime = 3.0f;
                //skill.skill_4_Cooltime = 3.0f;

                break;
        }

        if (photonView.IsMine)
        {
            isMine = true;

            hpBar.SetMaxHealth(maxHP);
            //SkillControl skill = gameObject.GetComponent<SkillControl>();
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            cameraLoc = mainCamera.gameObject.GetComponent<CameraLocator>();
            cameraLoc.playerCheck = true;
            cameraLoc.player = gameObject;
            ui = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UImanager>();
            ui.player = this;
            ui.playerCheck = true;
            hpBar.gameObject.SetActive(false);

            //gameObject.tag = "Player";
        }
        


        isDead = false;
        dying = false;
        if (isMine/*gameObject.CompareTag("Player")*/)
        {
            hpBar.gameObject.SetActive(false);

            hp = maxHP;
            mp = maxMP;
        }
    }
    void Move()
    {

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
            myAnim.SetBool("Move", true);
        else
        {
            myAnim.SetBool("Move", false);
            return;
        }
        moveDirection = (Vector3.forward * v) + (Vector3.right * h);

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);

        if (!isAttacking)
            myRig.rotation = Quaternion.Slerp(myRig.rotation, newRotation, rotateSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift)) //달리기
        {
            myAnim.SetBool("Run", true);
            transform.position += moveDirection * (speed * 2.5f) * Time.deltaTime;

        }
        else
        {
            if (myAnim.GetBool("Run"))
                myAnim.SetBool("Run", false);
            transform.position += moveDirection * speed * Time.deltaTime;
        }

    }

    // Update is called once per frame
    void Update()
    {


        if (isMine)
        {

            isGround = GetComponentInChildren<GroundSense>().isGround; //GroundSense클래스의 isGround를 가져와서 자신의 isGround갱신
            if (isDamaging)
            {
                damageTimer += Time.deltaTime;
                if (damageTimer >= 1.5f)
                {
                    isDamaging = false;
                    damageTimer = 0;
                }
            }

            if (!isDead && !isDashing) //사망처리중일 시 이동 불가
            {

                Move();
                Jump();
                Dash();
                //Skill_1();
            }
            else
            {
                //if (!dying)
                //    StartCoroutine(deadProcess()); //isDead가 true일 경우 즉시 사망처리 및 애니메이션 진행 코루틴 호출
            }



            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4)
                /*|| Input.GetKeyDown(KeyCode.Mouse0) */|| Input.GetKeyDown(KeyCode.Mouse1))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Vector3 dir = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z);
                    transform.rotation = Quaternion.LookRotation(dir);
                }
            }





            if (isDashing)
            {
                transform.position += transform.forward * (speed * 3) * Time.deltaTime;
                dashTimer += Time.deltaTime;

                if (dashTimer >= 0.5f)
                {
                    isDashing = false;
                    dashTimer = 0;
                }
            }

            if (mp / maxMP < 1)
            {
                mpTimer += Time.deltaTime;

                if (mpTimer >= 0.2f)
                {
                    mpTimer = 0;
                    mp += 1;
                }
            }


            if (skill_1_Off)
            {
                skill_1_Timer += Time.deltaTime;
                if (skill_1_Timer >= 1.2f)
                    isAttacking = false;
                if (skill_1_Timer >= skill_1_Cooltime)
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
                    isAttacking = false;
                    if (job == Global.Classes.Warrior)
                        wSkill.warriorAnim.SetBool("Skill2_2", false);
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
                if (skill_3_Timer >= 1.0f && job == Global.Classes.Warrior)
                {
                    wSkill.warriorAnim.SetBool("Skill3_2", false);
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

        }
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //바닥에 있으면 점프를 실행
            if (isGround)
            {
                //print("점프 가능 !");
                isGround = false;
                myAnim.SetTrigger("Jump");
                myRig.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }

            else
            {
                return;
            }
        }

    }

    void Dash()
    {

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            isDashing = true;
            myAnim.SetTrigger("Dash");
        }
    }

    [PunRPC]
    public void OnDamage(float damage)
    {
        if (!isDamaging)
        {
            isDamaging = true;
            myAnim.SetTrigger("Damage");
            myRig.AddForce(-transform.forward * jumpPower * 4 + transform.up * jumpPower / 2, ForceMode.Impulse);
            hp -= damage;
            hpBar.SetHealth(hp);
        }
    }

    [PunRPC]
    public void OnSlow(float rate, float time)
    {
        if (photonView.IsMine)
            StartCoroutine(Slow(rate, time));
    }
    IEnumerator Slow(float rate, float time)
    {
        float originSpeed = speed;
        speed *= rate;
        yield return new WaitForSeconds(time);
        speed = originSpeed;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}