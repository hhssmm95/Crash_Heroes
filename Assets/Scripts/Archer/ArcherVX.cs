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
    float checkTimer = 0.5f;
    float snipeTimer = 0.5f;
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
            if (checkTimer >= 0.5f)
                s3HitReady = true;
            else
                checkTimer += Time.deltaTime;
            
        }
        if (snipeReady)
        {
            if (snipeTimer >= 0.5f)
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
                enemy.GetComponent<PhotonView>().RPC("OnSpecialDamage", RpcTarget.All, Archer.atk * 0.964f, transform.tag, Archer.gameObject.tag);
                Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Archer.atk * 0.964f + "감소 전 피해를 입힘.");
                var effect = PhotonNetwork.Instantiate("Prefebs/Effect_17_ArrowHit", new Vector3(enemy.transform.position.x, enemy.transform.position.y + 1.1f, enemy.transform.position.z), Quaternion.Euler(90, 0, 0));
                StartCoroutine(destroyEffect(effect));
                SoundManager.Instance.HitSoundPlay(4);
                //SoundManager.Instance.ArcherSoundPlay(5);

            }
            else if(s4HitReady)
            {
                snipeTimer = 0.0f;
                s4HitReady = false;
                enemy.GetComponent<PhotonView>().RPC("OnSpecialDamage", RpcTarget.All, Archer.atk * 1.9f, transform.tag, Archer.gameObject.tag);
                Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Archer.atk * 1.9f + "감소 전 피해를 입힘.");
                SoundManager.Instance.HitSoundPlay(4);
                return;
            }

            if (!dm.Exists(x => x == other.name) && Archer.isAttacking)
            {
                

                if (tag == "ArcherAttack1")
                {
                    dm.Add(other.name);
                    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Archer.atk * 0.9f, Archer.transform.forward, Archer.gameObject.tag);
                    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Archer.atk * 0.9f + "감소 전 피해를 입힘.");
                    var effect = PhotonNetwork.Instantiate("Prefebs/Effect_29_Hit", new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.6f, enemy.transform.position.z), Quaternion.LookRotation(-transform.forward) * Quaternion.Euler(-120,90,-90));
                    StartCoroutine(destroyEffect(effect));

                    SoundManager.Instance.HitSoundPlay(3);
                }
                else if (tag == "ArcherAttack2")
                {
                    dm.Add(other.name);
                    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Archer.atk * 1.0f, Archer.transform.forward, Archer.gameObject.tag);
                    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Archer.atk * 1.0f + "감소 전 피해를 입힘.");
                    var effect = PhotonNetwork.Instantiate("Prefebs/Effect_29_Hit", new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.6f, enemy.transform.position.z), Quaternion.LookRotation(-transform.forward) * Quaternion.Euler(-120, 90, -90));
                    StartCoroutine(destroyEffect(effect));

                    SoundManager.Instance.HitSoundPlay(3);
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
        float second = 0;
        yield return new WaitForSeconds(0.7f);
        checkReady = true;
        while (second >= 2.5f)
        {
            SoundManager.Instance.ArcherSoundPlay(5);
            yield return new WaitForSeconds(0.2f);
            second += 0.2f;
        }
    }
    IEnumerator SnipeCheck()
    {
        float second = 0;
        yield return new WaitForSeconds(2.1f);
        snipeReady = true;
        while (second >= 1.5f)
        {
            SoundManager.Instance.ArcherSoundPlay(7);
            yield return new WaitForSeconds(0.2f);
            second += 0.2f;
        }
    }

    IEnumerator destroyEffect()
    {
        yield return new WaitForSeconds(1.5f);
        if (this.gameObject != null)
            PhotonNetwork.Destroy(gameObject);
    }
    IEnumerator destroyEffect(GameObject effect)
    {
        yield return new WaitForSeconds(1.5f);
        if (effect != null)
            PhotonNetwork.Destroy(effect);
    }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
