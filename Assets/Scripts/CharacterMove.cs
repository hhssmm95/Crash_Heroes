using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class CharacterMove : MonoBehaviourPunCallbacks, IPunObservable //캐릭터의 전반적인 입력들과 애니메이션, 상태 처리 클래스
{
    public HealthBar hpBar; 
    Vector3 cameraOffset;
    public Animator myAnim;
    private Rigidbody myRig;

    private Camera mainCamera;
    public Global.Classes job;

    //private float delay = 1.0f; //점프 딜레이를 위한 카운터
    private bool jumpCooltime; //점프 후 아직 쿨타임 중일경우 true
    public bool dying; //캐릭터 사망 애니메이션 중복 재생 방지용 변수
    public bool isGround; //캐릭터의 발이 땅에 붙어있을때 true
    public float maxHP;
    public float hp;
    public float maxMP;
    public float mp;
    public float atk;
    public float def;
    public float maxST = 100.0f; //최대 스테미나통
    public float st; //현재 스테미나
    public float br; //Mage를 위한 배리어 수치

    public float hpRegen; //체력,마나,스테 자연회복량
    public float mpRegen;
    public float stRegen;

    float mpTimer; //초당 자연회복 간격 타이머
    float hpTimer;
    float stTimer;

    bool isRunning; //달리기(쉬프트) 체크 트리거

    private bool isDamaging; // 피격 처리중 체크 트리거
    public bool isDashing; // 대쉬중 체크 트리거
    public bool isStun; //기절 체크 트리거 
    public bool isBurn; //화상 체크 트리거

    float burnDuration; //화상 지속시간
    float burnDurationTimer; // 화상 지속시간 타이머
    float burnDamage; //화상 틱데미지량
    float burnTimer; //화상 틱데미지 간격 타이머

    float damageTimer; //피격처리(무적시간) 타이머

    float moveTimer; //걷는 사운드 세부조절 타이머



    public float speed = 2.0f; // 캐릭터 이동속도

    //private Quaternion movement;

    private float jumpPower = 5.0f;
    private float rotateSpeed = 10.0f;

    public bool isDead; // ※전역변수, true일때 즉시 사망 애니메이션 진행

    private Vector3 moveDirection; //이동 방향
    public bool isAttacking; //공격중 체크 트리거 (캐릭터별 Skill 스크립트에서 제어)

    float dashTimer; //대쉬 거리(대쉬 시간) 체크 타이머
    float runTimer;
    private float h;
    private float v;

    private bool isMine;

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
    public MageSkill mSkill;
    public LenaSkill lSkill;

    GameObject BurnEffect;

    bool isWS4Hit;
    bool isDS4Hit;
    bool isAS3Hit;

    public bool dashAttacking_warrior;
    public bool stopWhileAttack;
    float stopTimer;

    bool dashOff;

    public string user;
    public int Photnet_AnimationSync;
    //MageSkill mSkill;
    void Start()
    {

        myAnim = gameObject.GetComponent<Animator>();
        myRig = gameObject.GetComponent<Rigidbody>();

        wSkill = gameObject.GetComponent<WarriorSkill>();
        aSkill = gameObject.GetComponent<ArcherSkill>();
        dSkill = gameObject.GetComponent<DragoonSkill>();
        mSkill = gameObject.GetComponent<MageSkill>();
        lSkill = gameObject.GetComponent<LenaSkill>();

        //if (wSkill != null)
        //    job = Global.Classes.Warrior;
        //else if (aSkill != null)
        //    job = Global.Classes.Archer;
        //else if (dSkill != null)
        //    job = Global.Classes.Dragoon;
        //else if (mSkill != null)
        //    job = Global.Classes.Mage;

        if (gameObject.tag == "Warrior")
            job = Global.Classes.Warrior;
        else if (gameObject.tag == "Archer")
            job = Global.Classes.Archer;
        else if (gameObject.tag == "Dragoon")
            job = Global.Classes.Dragoon;
        else if (gameObject.tag == "Mage")
            job = Global.Classes.Mage;
        else if (gameObject.tag == "Lena")
            job = Global.Classes.Lena;
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
                mSkill.photonView.RPC("InitStatus", RpcTarget.All);

                break;

            case Global.Classes.Lena:
                lSkill.photonView.RPC("InitStatus", RpcTarget.All);
                break;
        }

        hpBar.GetComponent<PhotonView>().RPC("SetMaxHealth", RpcTarget.All, maxHP);

        if (photonView.IsMine)
        {
            isMine = true;

            //hpBar.SetMaxHealth(maxHP);

            //SkillControl skill = gameObject.GetComponent<SkillControl>();
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y + 3.806f, transform.position.z - 3f); //원래6
            //mainCamera.transform.position = cameraGuide.transform.position;
            //.transform.rotation = cameraGuide.transform.rotation;
            cameraOffset = new Vector3(0, 6.806f, -6f); //mainCamera.transform.position - transform.position;
            //cameraLoc = mainCamera.gameObject.GetComponent<CameraLocator>();
            //cameraLoc.playerCheck = true;
            //cameraLoc.player = gameObject;
            ui = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UImanager>();
            if (ui != null)
                Debug.Log("UI연결댬");
            ui.player = this;
            ui.playerCheck = true;
            Debug.Log("UI플레이어체크함");
            hpBar.gameObject.SetActive(false);

            //gameObject.tag = "Player";
        }
        


        isDead = false;
        dying = false;
        if (photonView.IsMine/*gameObject.CompareTag("Player")*/)
        {
            //hpBar.gameObject.SetActive(false);

            hp = maxHP;
            mp = maxMP;
            st = maxST;
        }
    }
    void Move()
    {

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if ((h != 0 || v != 0) && !isRunning)
            myAnim.SetBool("Move", true);
        else if (h == 0 && v == 0)
        {
            myAnim.SetBool("Move", false);
            myAnim.SetBool("Run", false);
            isRunning = false;
            runTimer = 0;
            return;
        }
        
        moveDirection = (Vector3.forward * v) + (Vector3.right * h);

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        
        if (!isAttacking)
            myRig.rotation = Quaternion.Slerp(myRig.rotation, newRotation, rotateSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift)) //달리기
        {
            if (!isRunning && st>=20.0f)
            {
                stTimer = 0;
                st -= 20.0f;
                isRunning = true;

                myAnim.SetBool("Run", true);
            }

        }
        else
        {
            if (isRunning)
            {

                myAnim.SetBool("Run", false);
                isRunning = false;
                runTimer = 0;
            }

        }


        if (isRunning)
        {
            if (st >= 0.0f)
            {
                /*runTimer += Time.deltaTime;
                if (runTimer >= 1.0f)
                {
                    runTimer = 0;
                    st -= 10.0f;
                }*/
                st -= 10.0f * Time.deltaTime;
                transform.position += moveDirection * (speed * 2.5f) * Time.deltaTime;
            }
            else
            {
                isRunning = false;
                myAnim.SetBool("Run", false);
                myAnim.SetBool("Move", true);
            }
        }
        else
        {
            transform.position += moveDirection * speed * Time.deltaTime; //이동 최종 연산
        }

    }

    void CooltimeReset()
    {
        if(Input.GetKey(KeyCode.P))
        {
            skill_1_Off = false;
            skill_1_Timer = 0;

            skill_2_Off = false;
            skill_2_Timer = 0;

            skill_3_Off = false;
            skill_3_Timer = 0;

            skill_4_Off = false;
            skill_4_Timer = 0;

            skill_5_Off = false;
            skill_5_Timer = 0;

            mp = maxMP;
            st = maxST;
            hp = maxHP;
            isDead = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(myAnim.GetBool("Move"))
        {
            moveTimer += Time.deltaTime;
            if(moveTimer >= 0.5f)
            {
                moveTimer = 0;
                SoundManager.Instance.FootstepsSoundPlay(0);
            }
        }
        else if(myAnim.GetBool("Run"))
        {
            moveTimer += Time.deltaTime;
            if (moveTimer >= 0.2f)
            {
                moveTimer = 0;
                SoundManager.Instance.FootstepsSoundPlay(0);
            }
        }
        else
        {
            moveTimer = 0;
        }

        //if(!stopWhileAttack && isAttacking)
        //{
        //    stopWhileAttack = true;
        //}
        //if(stopWhileAttack)
        //{
        //    stopTimer += Time.deltaTime;
        //    if(stopTimer >= 1.0f)
        //    {
        //        stopWhileAttack = false;
        //        stopTimer = 0;
        //    }
        //}
        user = photonView.Owner.NickName;
        if (isDead)
        {
            OnDead();
        }

        if (photonView.IsMine)
        {
            mainCamera.transform.position = transform.position + cameraOffset;
            //mainCamera.transform.position = cameraGuide.transform.position;
            //mainCamera.transform.rotation = cameraGuide.transform.rotation;

            isGround = GetComponentInChildren<GroundSense>().isGround; //GroundSense클래스의 isGround를 가져와서 자신의 isGround갱신
            if (isDamaging)
            {
                damageTimer += Time.deltaTime;
                if (damageTimer >= 0.4f)
                {
                    isDamaging = false;
                    damageTimer = 0;
                }
            }

            if (!isDead && !isDashing && !isStun && !stopWhileAttack) //사망처리중일 시 이동 불가
            {

                Move();

                //
                //    photonView.RPC("Jump", RpcTarget.All);
                //if (Input.GetKeyDown(KeyCode.Space))
                //    Jump();
                if (Input.GetKeyDown(KeyCode.Mouse1) && !dashOff)
                {
                    SetLookAtMousePos();
                    //photonView.RPC("Dash", RpcTarget.All);
                    StartCoroutine("Dash");
                }
                //Dash();
                //Skill_1();
            }
            else
            {
                //if (!dying)
                //    StartCoroutine(deadProcess()); //isDead가 true일 경우 즉시 사망처리 및 애니메이션 진행 코루틴 호출
            }

           

            // 마우스 위치 바라보는 코드 빠짐//
            //if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4)
            //    /*|| Input.GetKeyDown(KeyCode.Mouse0) */|| Input.GetKeyDown(KeyCode.Mouse1) && isDead && isStun)
            //{
            //    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            //    RaycastHit hit;

            //    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            //    {
            //        Vector3 dir = new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z);
            //        transform.rotation = Quaternion.LookRotation(dir);
            //    }
            //}

            CooltimeReset();



            if (isDashing)
            {
                //isAttacking = true;
                transform.position += transform.forward * (speed * 4) * Time.deltaTime;
                //dashTimer += Time.deltaTime;

                //if (dashTimer >= 0.5f)
                //{
                //    isDashing = false;
                //    //isAttacking = false;
                //    dashTimer = 0;
                //}
            }

            if (mp / maxMP < 1 && !isDead)
            {
                mpTimer += Time.deltaTime;

                if (mpTimer >= 1.0f)
                {
                    mpTimer = 0;
                    mp += mpRegen;// (mpRegen*0.1f);
                    if (mp > maxMP)
                        mp = maxMP;
                }
            }
            if (hp / maxHP < 1 && !isDead)
            {
                hpTimer += Time.deltaTime;

                if (hpTimer >= 5.0f)
                {
                    hpTimer = 0;
                    hp += hpRegen;
                    if (hp > maxHP)
                        hp = maxHP;
                    hpBar.GetComponent<PhotonView>().RPC("SetHealth", RpcTarget.All, hp);
                }
            }
            if (st / maxST < 1 && !isDead && !isRunning) //달리고있지 않을때 스태미나 자동 회복
            {
                stTimer += Time.deltaTime;

                if (stTimer >= 1.0f)
                {
                    stTimer = 0;
                    st += stRegen;//(stRegen*0.1f);
                    if (st > maxST)
                        st = maxST;
                }
            }

            if (skill_1_Off)
            {
                skill_1_Timer += Time.deltaTime;
                //if (skill_1_Timer >= 1.2f)
                //    isAttacking = false;
                if (skill_1_Timer >= skill_1_Cooltime)
                {
                    skill_1_Off = false;
                    skill_1_Timer = 0;
                }
            }
            if (skill_2_Off)
            {
                skill_2_Timer += Time.deltaTime;
                

                if (skill_2_Timer >= skill_2_Cooltime)
                {
                    skill_2_Off = false;
                    skill_2_Timer = 0;
                }
            }
            if (skill_3_Off)
            {
                skill_3_Timer += Time.deltaTime;
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
            if (skill_5_Off)
            {
                skill_5_Timer += Time.deltaTime;
                if (skill_5_Timer >= skill_5_Cooltime)
                {
                    skill_5_Off = false;
                    skill_5_Timer = 0;
                }
            }

            //if (hp <= 0)
            //    isDead = true;

            if (isBurn)
            {
                if (burnDurationTimer >= burnDuration)
                {
                    isBurn = false;
                    burnTimer = 0;
                    burnDurationTimer = 0;
                    burnDuration = 0;
                    burnDamage = 0;
                    return;
                }
                burnTimer += Time.deltaTime;
                burnDurationTimer += Time.deltaTime;
            }

            if (isBurn && burnTimer >= 1.0f && !isDead)
            {
                burnTimer = 0;
                photonView.RPC("OnTrueDamage", RpcTarget.All, burnDamage , -transform.forward);
                //OnBurn 함수만들어서 제어, if(isBurn) {Timer++;} 추가
            }

            //hpBar.SetHealth(hp);

        }
    }
        //[PunRPC]
        //void Jump()
        //{
        //    //바닥에 있으면 점프를 실행
        //    if (isGround)
        //    {
        //        //print("점프 가능 !");
        //        isGround = false;
        //        myAnim.SetTrigger("Jump");
        //        Photnet_AnimationSync = 1;
        //        myRig.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        //    }
        //    else
        //    {
        //        return;
        //    }
        //}
    
        
    

    [PunRPC]
    public void OnDamage(float damage , Vector3 normal)
    {
        if (photonView.IsMine && !isDamaging && !isDead)
        {
            isDamaging = true;
            myAnim.SetTrigger("Damage");
            Photnet_AnimationSync = 2;
            myRig.AddForce(normal * (jumpPower/3), ForceMode.Impulse);
            if (def <= damage)
            {
                if (br > damage)
                {
                    br -= damage;
                    Debug.Log(gameObject.name + "(이)가 " + damage + "데미지를 전량 배리어로 방어함");
                    return;
                }

                if (br>0)
                    Debug.Log(gameObject.name + "(이)가 " + damage + "데미지를 배리어"+ br +" 과 방어력 " + def + " 만큼 경감하여 " + (damage - br - def) + " 피해를 입음");
                else
                    Debug.Log(gameObject.name + "(이)가" + damage + "데미지를 방어력 " + def + " 만큼 경감하여 " + (damage - def) + " 피해를 입음");
                
                hp -= ((damage - br) - def);
                br -= damage;
            }
            if (br < 0)
            {
                br = 0;
            }
            //hpBar.SetHealth(hp);
            hpBar.GetComponent<PhotonView>().RPC("SetHealth", RpcTarget.All, hp);
            if(hp<=0)
            {
                isDead = true;
                Debug.Log(gameObject.name + "(이)가 사망");
            }
        }
    }

    [PunRPC]
    public void OnSpecialDamage(float damage, string tag)
    {
        if (photonView.IsMine && !isDamaging && !isDead)
        {
            switch(tag)
            {
                case "WarriorSkill4" :
                    myAnim.SetTrigger("Damage");
                    Photnet_AnimationSync = 2;
                    if (def <= damage)
                    {
                        if (br > damage)
                        {
                            br -= damage;
                            Debug.Log(gameObject.name + "(이)가 " + damage + "데미지를 전량 배리어로 방어함");
                            return;
                        }

                        if (br > 0)
                            Debug.Log(gameObject.name + "(이)가 " + damage + "데미지를 배리어" + br + " 과 방어력 " + def + " 만큼 경감하여 " + (damage - br - def) + " 피해를 입음");
                        else
                            Debug.Log(gameObject.name + "(이)가" + damage + "데미지를 방어력 " + def + " 만큼 경감하여 " + (damage - def) + " 피해를 입음");

                        hp -= ((damage - br) - def);
                        br -= damage;
                    }
                    if (br < 0)
                    {
                        br = 0;
                    }
                    //hpBar.SetHealth(hp);

                    break;

                case "ArcherSkill3":
                    myAnim.SetTrigger("Damage");
                    Photnet_AnimationSync = 2;
                    if (def <= damage)
                    {
                        if (br > damage)
                        {
                            br -= damage;
                            Debug.Log(gameObject.name + "(이)가 " + damage + "데미지를 전량 배리어로 방어함");
                            return;
                        }

                        if (br > 0)
                            Debug.Log(gameObject.name + "(이)가 " + damage + "데미지를 배리어" + br + " 과 방어력 " + def + " 만큼 경감하여 " + (damage - br - def) + " 피해를 입음");
                        else
                            Debug.Log(gameObject.name + "(이)가" + damage + "데미지를 방어력 " + def + " 만큼 경감하여 " + (damage - def) + " 피해를 입음");

                        hp -= ((damage - br) - def);
                        br -= damage;
                    }
                    if (br < 0)
                    {
                        br = 0;
                    }
                    break;

                case "DragoonSkill4":

                    break;

                case "ArcherSkill4":
                    myAnim.SetTrigger("Damage");
                    Photnet_AnimationSync = 2;
                    if (def <= damage)
                    {
                        if (br > damage)
                        {
                            br -= damage;
                            return;
                        }

                        if (br > 0)
                            Debug.Log(gameObject.name + "(이)가 " + damage + "데미지를 배리어" + br + " 과 방어력 " + def + " 만큼 경감하여 " + (damage - br - def) + " 피해를 입음");
                        else
                            Debug.Log(gameObject.name + "(이)가" + damage + "데미지를 방어력 " + def + " 만큼 경감하여 " + (damage - def) + " 피해를 입음");

                        hp -= ((damage - br) - def);
                        br -= damage;
                    }
                    if (br < 0)
                    {
                        br = 0;
                    }
                    break;
                default:
                    hpBar.GetComponent<PhotonView>().RPC("SetHealth", RpcTarget.All, hp);
                    if (hp <= 0)
                    {
                        isDead = true;
                        Debug.Log(gameObject.name + "(이)가 사망");
                    }
                    break;
            }
        }
    }

    [PunRPC]
    public void OnHeavyDamage(float damage, Vector3 normal)
    {
        if (photonView.IsMine && !isDamaging && !isDead)
        {
            isDamaging = true;
            myAnim.SetTrigger("HeavyDamage");
            Photnet_AnimationSync = 3;
            myRig.AddForce(normal * jumpPower/2, ForceMode.Impulse);

            if (def <= damage)
            {
                if (br > damage)
                {
                    br -= damage;
                    Debug.Log(gameObject.name + "(이)가 " + damage + "데미지를 전량 배리어로 방어함");
                    return;
                }
                else
                    Destroy(GameObject.FindWithTag("Shield"));

                if (br > 0)
                    Debug.Log(gameObject.name + "(이)가 " + damage + "데미지를 배리어" + br + " 과 방어력 " + def + " 만큼 경감하여 " + (damage - br - def) + " 피해를 입음");
                else
                    Debug.Log(gameObject.name + "(이)가" + damage + "데미지를 방어력 " + def + " 만큼 경감하여 " + (damage - def) + " 피해를 입음");

                hp -= ((damage - br) - def);
                br -= damage;

                if (br < 0)
                {
                    br = 0;
                }
            }
            //hpBar.SetHealth(hp);
            hpBar.GetComponent<PhotonView>().RPC("SetHealth", RpcTarget.All, hp);
            if (hp <= 0)
            {
                isDead = true;
                Debug.Log(gameObject.name + "(이)가 사망");
            }
        }
    }


    [PunRPC]
    public void OnTrueDamage(float damage, Vector3 normal)
    {
        if (photonView.IsMine && !isDamaging && !isDead)
        {
            isDamaging = true;
            myAnim.SetTrigger("Damage");
            Photnet_AnimationSync = 2;
            myRig.AddForce(normal * (jumpPower / 3), ForceMode.Impulse);

            hp -= damage;
            
            //hpBar.SetHealth(hp);
            hpBar.GetComponent<PhotonView>().RPC("SetHealth", RpcTarget.All, hp);
            if (hp <= 0)
            {
                isDead = true;
                Debug.Log(gameObject.name + "(이)가 사망");
            }
        }
    }

    [PunRPC]
    public void OnSlow(float rate, float time)
    {
        if (photonView.IsMine && !isDead)
            StartCoroutine(Slow(rate, time));
    }

    [PunRPC]
    public void OnStun(float time)
    {
        if (photonView.IsMine && !isDead)
            StartCoroutine(Stun(time));
    }

    [PunRPC]
    public void OnBurn(float duration, float damage)
    {
        if (photonView.IsMine && !isDead)
        {
            if(isBurn)
            {  
                if(BurnEffect != null)
                {
                    Destroy(BurnEffect);
                }

                burnDurationTimer = 0;
                burnDuration = duration;
                burnDamage = damage;

                BurnEffect = PhotonNetwork.Instantiate("Prefebs/VFX/OnBurnVX", new Vector3(transform.position.x, transform.position.y + 0.55f, transform.position.z), transform.rotation);
                BurnEffect.transform.parent = gameObject.transform;
                Destroy(BurnEffect, duration);
                return;
            }
            isBurn = true;
            burnDuration = duration;
            burnDamage = damage;
            if (isMine)
            {
                BurnEffect = PhotonNetwork.Instantiate("Prefebs/VFX/OnBurnVX", new Vector3(transform.position.x, transform.position.y + 0.55f, transform.position.z), transform.rotation);
                BurnEffect.transform.parent = gameObject.transform;
                Destroy(BurnEffect, duration);
            }
        }
    }

    [PunRPC]
    public void OnHeal(float amount)
    {
        if (photonView.IsMine && !isDead)
        {
            hp += amount;
            if (hp > maxHP)
                hp = maxHP;
        }
    }

    [PunRPC]
    public void BarriorFill(float amount)
    {
        if (photonView.IsMine && !isDead)
        {
            br = amount*maxHP;
        }
    }

    [PunRPC]
    public void BarriorBroken()
    {
        if (GameObject.FindWithTag("Shield").activeInHierarchy)
            Destroy(GameObject.FindWithTag("Shield"));
    }
    //[PunRPC]
    public void OnDead()
    {
        if (!dying)
        {
            dying = true;
            //myAnim.Play("Die2");
            //myAnim.SetBool("isDead", true);
            myAnim.SetTrigger("isDead");
            Photnet_AnimationSync = 4;
        }

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

    public void ControlOff()
    {
        stopWhileAttack = true;
    }

    public void ControlOn()
    {
        stopWhileAttack = false;
    }

    public void DashOn()
    {
        isDashing = true;
    }
    public void DashOff()
    {
        isDashing = false;
    }

    IEnumerator Dash()
    {
        dashOff = true;
        //st -= 20;
        //isDashing = true;
        myAnim.SetTrigger("Dash");
        Photnet_AnimationSync = 1;
        yield return new WaitForSeconds(10.0f);
        dashOff = false;

    }

    IEnumerator Slow(float rate, float time)
    {
        float originSpeed = speed;
        speed *= (1.0f - rate);
        yield return new WaitForSeconds(time);
        speed = originSpeed;
    }

    IEnumerator Stun(float time)
    {
        isStun = true;
        myAnim.SetBool("isStun", true);
        yield return new WaitForSeconds(time);
        myAnim.SetBool("isStun", false);
        isStun = false;
    }
    


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(Photnet_AnimationSync);
            Photnet_AnimationSync = 0;


        }
        else
        {
            Photnet_AnimationSync = (int)stream.ReceiveNext();
            if (!photonView.IsMine)
            {
                switch (Photnet_AnimationSync)
                {
                    //case 1:
                    //    myAnim.SetTrigger("Jump");
                    //    Photnet_AnimationSync = 0;
                    //    break;

                    case 1:
                        myAnim.SetTrigger("Dash");
                        break;

                    case 2:
                        myAnim.SetTrigger("Damage");
                        break;

                    case 3:
                        myAnim.SetTrigger("HeavyDamage");
                        break;

                    case 4:
                        myAnim.SetTrigger("isDead");
                        break;
                }
            }
            Photnet_AnimationSync = 0;

        }
    }
}