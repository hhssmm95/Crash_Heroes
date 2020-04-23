using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterMove : MonoBehaviour //캐릭터의 전반적인 입력들과 애니메이션, 상태 처리 클래스
{
    private Animator myAnim;
    private Rigidbody myRig;
    //private float delay = 1.0f; //점프 딜레이를 위한 카운터
    private bool jumpCooltime; //점프 후 아직 쿨타임 중일경우 true
    public static bool dying; //캐릭터 사망 애니메이션 중복 재생 방지용 변수
    public bool isGround; //캐릭터의 발이 땅에 붙어있을때 true
    public float hp = 100;
    public float mp = 100;
    public int potion;

    //public GameObject Fireball;
    //public GameObject SkillSpot;

    public float speed = 2.0f; // 캐릭터의 좌우 이동속도
    public float jumpPower = 5.0f;
    public float rotateSpeed = 10.0f;
    public static bool isDead; // ※전역변수, true일때 즉시 사망 애니메이션 진행
    //public GameObject GameOverPanel;
    public Vector3 moveDirection;

    ////public float skill_1_cooltime = 1.0f;
    //private bool inSkill_1_Cooltime;
    //private float skill_1_delay = 1.0f;
    //public float sk2CoolTime = 5.0f;
    //public float sk3CoolTime = 10.0f;
    //public float sk4CoolTime = 60.0f;

    private float h;
    private float v;
    //private int score;
    void Start()
    {
        myAnim = gameObject.GetComponent<Animator>();
        myRig = gameObject.GetComponent<Rigidbody>();
        //SkillSpot = GameObject.FindGameObjectWithTag("SkillSpawnSpot");
        isDead = false;
        dying = false;

    }
    void Move()
    {
        //Vector3 moveDirection = Vector3.zero;

        //if (Input.GetAxis("Horizontal") > 0) //우 이동
        //{

        //    myAnim.SetBool("run", true);
        //    gameObject.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
        //    moveDirection = Vector3.right;
        //}
        //else if (Input.GetAxis("Horizontal") < 0) //좌 이동
        //{

        //    myAnim.SetBool("run", true);
        //    gameObject.transform.eulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
        //    moveDirection = Vector3.left;
        //}
        //else if (Input.GetAxis("Vertical") > 0) // 앞 이동
        //{

        //    myAnim.SetBool("run", true);
        //    gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        //    moveDirection = Vector3.forward;
        //    //inputForward = false;
        //    //if (delay >= 0.5f && isGround) // 땅에 서있고 점프 쿨타임이 돌아왔을시
        //    //{
        //    //    jumpCooltime = false;
        //    //    delay = 0.0f;
        //    //    //Fire(new Vector3(transform.position.x, transform.position.y, transform.position.z + 3.0f)); // 다음 타일(현재z좌표+3)까지 포물선(점프)이동 함수 
        //    //    jumpCooltime = true;
        //    //}
        //}
        //else if (Input.GetAxis("Vertical") < 0) // 뒤 이동
        //{
        //    myAnim.SetBool("run", true);
        //    gameObject.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        //    moveDirection = Vector3.back;

        //    //inputBackward = false;
        //    //if (delay >= 0.5f && isGround)
        //    //{
        //    //    jumpCooltime = false;
        //    //    delay = 0.0f;
        //    //    //Fire(new Vector3(transform.position.x, transform.position.y, transform.position.z - 3.0f));
        //    //    jumpCooltime = true;
        //    //}
        //}
        //if (Input.GetAxis("Horizontal") > 0) //우 이동
        //{

        //    myAnim.SetBool("run", true);
        //    gameObject.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
        //    moveDirection = Vector3.right;
        //}
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if (h != 0 || v != 0)
            myAnim.SetInteger("animation", 17);
        //myAnim.SetBool("run", true);
        else if(h == 0 || v == 0 || !Input.GetKeyDown(KeyCode.Z))
        {
            myAnim.SetInteger("animation", 13);
            //myAnim.SetBool("run", false);
            return;
        }
        moveDirection = (Vector3.forward * v) + (Vector3.right * h);

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        myRig.rotation = Quaternion.Slerp(myRig.rotation, newRotation, rotateSpeed *Time.deltaTime);
        transform.position += moveDirection *speed * Time.deltaTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead) //사망처리중일 시 이동 불가
        {
            Move();
            Jump();
            //Skill_1();
        }
        else
        {
            //if (!dying)
            //    StartCoroutine(deadProcess()); //isDead가 true일 경우 즉시 사망처리 및 애니메이션 진행 코루틴 호출
        }
        isGround = GetComponentInChildren<GroundSense>().isGround; //GroundSense클래스의 isGround를 가져와서 자신의 isGround갱신
        //if (inSkill_1_Cooltime)
        //    skill_1_delay += Time.deltaTime;

        //FindObjectOfType()
        


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
                myRig.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }

            //공중에 떠있는 상태이면 점프하지 못하도록 리턴
            else
            {
                //print("점프 불가능 !");
                return;
            }
        }

    }
    //}

    //void Skill_1()
    //{
    //    if (skill_1_delay >= 1.0f && Input.GetKeyDown(KeyCode.Z))
    //    {
    //        inSkill_1_Cooltime = false;
    //        skill_1_delay = 0.0f;
    //        Instantiate(Fireball, SkillSpot.transform.position, Fireball.transform.rotation);
    //        inSkill_1_Cooltime = true;
    //    }

    //}

    //IEnumerator Skill_1()
    //{
    //}


    //void SetVelocity(Vector3 velocity)
    //{
    //    myRig.velocity = velocity;
    //}


    //Vector3 GetVelocity(Vector3 currentPos, Vector3 targetPos, float initialAngle) //포물선 이동 관련 함수
    //{
    //    float gravity = Physics.gravity.magnitude;
    //    float angle = initialAngle * Mathf.Deg2Rad;

    //    Vector3 planarTarget = new Vector3(targetPos.x, 0, targetPos.z);
    //    Vector3 planarPosition = new Vector3(currentPos.x, 0, currentPos.z);

    //    float distance = Vector3.Distance(planarTarget, planarPosition);
    //    float yOffset = currentPos.y - targetPos.y;

    //    float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

    //    Vector3 velocity = new Vector3(0f, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

    //    float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition) * (targetPos.x > currentPos.x ? 1 : -1);
    //    Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

    //    return finalVelocity;
    //}

    //void Fire(Vector3 target) //포물선 이동(점프) 함수
    //{
    //    Vector3 velocity = GetVelocity(transform.position, target, 60f);
    //    SetVelocity(velocity);

    //}



    //IEnumerator deadProcess() //사망처리
    //{
    //    dying = true;
    //    myRig.velocity = Vector3.zero;
    //    myRig.AddForce(Vector3.up * 3.0f, ForceMode.Impulse);
    //    gameObject.transform.eulerAngles = new Vector3(0.0f, -90.0f, 0.0f);
    //    yield return new WaitForSeconds(0.1f);
    //    gameObject.transform.eulerAngles = new Vector3(0.0f, -180.0f, 0.0f);
    //    yield return new WaitForSeconds(0.1f);
    //    gameObject.transform.eulerAngles = new Vector3(0.0f, -270.0f, 0.0f);
    //    yield return new WaitForSeconds(0.1f);
    //    gameObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    //    yield return new WaitForSeconds(0.1f);
    //    gameObject.transform.eulerAngles = new Vector3(90.0f, 0.0f, 0.0f);

    //    myAnim.StopPlayback();
    //    yield return new WaitForSeconds(2.0f);
    //    GameOverPanel.SetActive(true);
    //    Time.timeScale = 0.0f;
    //    //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //    //isDead = false;
    //    //GameOverPanel.SetActive(false);
    //    //isDead = false;
    //}
}