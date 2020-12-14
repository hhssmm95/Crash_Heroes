using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ArcherVX : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove Archer;

    Transform Atk2Pos;
    Transform Skill1Pos;
    bool checkReady;
    public bool snipeReady;
    float checkTimer = 0.3f;
    float snipeTimer = 0.3f;
    public bool s3HitReady;
    public bool s4HitReady;
    bool hit;
    List<string> dm;
    private void Awake()
    {

        Archer = GameObject.FindGameObjectWithTag("Archer").GetComponent<CharacterMove>();
        if (gameObject.tag == "ArcherAttack2")
            Atk2Pos = GameObject.FindGameObjectWithTag("ArcherAttack2Pos").GetComponent<Transform>();

    }
    void Start()
    {
        dm = new List<string>();
        //if()
        //Destroy(gameObject, 1.0f);
        if (tag == "ArcherAttack1" || tag == "ArcherAttack2")
        {
            StartCoroutine("destroyEffect");
        }
        else if (tag == "ArcherSkill3")
            StartCoroutine("StayCheck");
        else if (tag == "ArcherSkill4")
            StartCoroutine("SnipeCheck");
        
    }

    // Update is called once per frame
    void Update()
    {
        Locate();
        if(checkReady)
        {
            if (checkTimer >= 0.3f)
                s3HitReady = true;
            else
                checkTimer += Time.deltaTime;
            
        }
        if (snipeReady)
        {
            if (snipeTimer >= 0.3f)
                s4HitReady = true;
            else
                snipeTimer += Time.deltaTime;

        }
    }

    //[PunRPC]
    

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject.name + "파티클충돌 with" + other.gameObject.layer.ToString());
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Archer") && other.gameObject.layer == 9 /*&& Archer.isAttacking*/&& photonView.IsMine )
        {

            var enemy = other.GetComponent<CharacterMove>();
            if(s3HitReady)
            {
                checkTimer = 0.0f;
                s3HitReady = false;
                enemy.GetComponent<PhotonView>().RPC("OnSpecialDamage", RpcTarget.All, Archer.atk * 0.7f, transform.tag);
                Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Archer.atk * 0.6f + "감소 전 피해를 입힘.");
                return;
            }
            else if(s4HitReady)
            {
                snipeTimer = 0.0f;
                s4HitReady = false;
                enemy.GetComponent<PhotonView>().RPC("OnSpecialDamage", RpcTarget.All, Archer.atk * 1.0f, transform.tag);
                Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Archer.atk * 1.2f + "감소 전 피해를 입힘.");
                return;
            }
            if (!dm.Exists(x => x == other.name) && Archer.isAttacking)
            {

                if (tag == "ArcherAttack1" || tag == "ArcherAttack2")
                {
                    dm.Add(other.name);
                    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Archer.atk * 0.9f, Archer.transform.forward);
                    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Archer.atk * 0.9f + "감소 전 피해를 입힘.");

                    SoundManager.Instance.HitSoundPlay(0);
                }
            }
        }

        //if (s3HitReady && !other.CompareTag("Archer") && other.gameObject.layer == 9)
        //{
        //    checkTimer = 0.0f;
        //    s3HitReady = false;
        //    var enemy = other.GetComponent<CharacterMove>();
        //    enemy.GetComponent<PhotonView>().RPC("OnSpecialDamage", RpcTarget.All, Archer.atk * 0.7f, transform.tag);
        //    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Archer.atk * 0.6f + "감소 전 피해를 입힘.");
        //}
        //if (s4HitReady && !other.CompareTag("Archer") && other.gameObject.layer == 9)
        //{
        //    snipeTimer = 0.0f;
        //    s4HitReady = false;
        //    var enemy = other.GetComponent<CharacterMove>();
        //    enemy.GetComponent<PhotonView>().RPC("OnSpecialDamage", RpcTarget.All, Archer.atk * 1.0f, transform.tag);
        //    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Archer.atk * 1.2f + "감소 전 피해를 입힘.");
        //}
    }

    void Locate()
    {
        if (gameObject.CompareTag("ArcherAttack1"))
        {
            transform.position = new Vector3(Archer.transform.position.x, Archer.transform.position.y + 0.5f, Archer.transform.position.z + 0.1f);
        }
        else if (gameObject.CompareTag("ArcherAttack2"))
        {
            transform.position = new Vector3(Atk2Pos.position.x, Atk2Pos.position.y, Atk2Pos.position.z);
        }
        //}
        //else if (gameObject.CompareTag("ArcherVX1"))
        //{
        //    transform.position = new Vector3(Skill1Pos.position.x, Skill1Pos.position.y, Skill1Pos.position.z);
        //}
    }

    IEnumerator StayCheck()
    {
        yield return new WaitForSeconds(0.7f);
        checkReady = true;
    }
    IEnumerator SnipeCheck()
    {
        yield return new WaitForSeconds(2.1f);
        snipeReady = true;
    }

    IEnumerator destroyEffect()
    {
        yield return new WaitForSeconds(1.5f);
        PhotonNetwork.Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
