using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DragoonVX : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove Dragoon;
    //ParticleSystem particle;
    //List<ParticleCollisionEvent> collisionEvents;

    Transform Atk2Pos;
    Transform Skill1Pos;
    public bool meteorReady;
    bool s4HitReady;
    float meteorTimer = 0.5f;
    List<string> dm;
    //bool hit;
    void Awake()
    {


        Dragoon = GameObject.FindGameObjectWithTag("Dragoon").GetComponent<CharacterMove>();
        //particle = GetComponent<ParticleSystem>();
        //collisionEvents = new List<ParticleCollisionEvent>();

        if(tag == "DragoonAttack2" || tag == "DragoonSkill2")
            Atk2Pos = GameObject.FindGameObjectWithTag("DragoonAtack2Pos").GetComponent<Transform>();
        
    }

    void Start()
    {
        //if(tag != "DragoonSkill3")
        //    Destroy(gameObject, 1.3f);
        dm = new List<string>();
        if (tag == "DragoonAttack1" || tag == "DragoonAttack2" || tag == "DragoonAttack3")
        {
            StartCoroutine("destroyEffect");
        }
        else if (tag == "DragoonSkill4")
        {
            StartCoroutine("Meteor");
        }
    }
    

    void Update()
    {
        if (meteorReady)
        {
            if (meteorTimer >= 0.5f)
                s4HitReady = true;
            else
                meteorTimer += Time.deltaTime;

        }
        //if(tag == "DragoonSkil3" && !meteorReady)
        //{
        //    if (meteorCount >= 0.7f)
        //        meteorReady = true;
        //    else
        //    {

        //        meteorCount += Time.deltaTime;
        //    }
        //}

        Locate();
    }
    
    private void OnTriggerEnter(Collider other)
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Dragoon") && other.gameObject.layer == 9 /*&& Dragoon.isAttacking*/&& photonView.IsMine)
        {
            var enemy = other.GetComponent<CharacterMove>();
            if (s4HitReady)
            {
                meteorTimer = 0.0f;
                s4HitReady = false;
                enemy.GetComponent<PhotonView>().RPC("OnSpecialDamage", RpcTarget.All, Dragoon.atk * 1.6f, transform.tag, Dragoon.gameObject.tag);
                Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Dragoon.atk * 1.6f + "감소 전 피해를 입힘.");
                return;
            }
            if (!dm.Exists(x => x == other.name) && Dragoon.isAttacking)
            {
                dm.Add(other.name);
                //Debug.Log("VX충돌");


                if (tag == "DragoonSkill1")
                {
                    enemy.GetComponent<PhotonView>().RPC("OnHeavyDamage", RpcTarget.All, Dragoon.atk * 2.5f, Dragoon.transform.forward, Dragoon.gameObject.tag);
                    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Dragoon.atk * 2.5f + "감소 전 피해를 입힘.");
                    var effect = PhotonNetwork.Instantiate("Prefebs/Effect_32_Hit", new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.5f, enemy.transform.position.z), Quaternion.LookRotation(-transform.forward) * enemy.transform.rotation);
                    StartCoroutine(destroyEffect(effect));
                    //enemy.GetComponent<PhotonView>().RPC("OnSlow", RpcTarget.All, 0.5f, 3.0f);
                    //Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + )
                    //enemy.OnSlow(0.5f, 3.0f);
                }
                else if (tag == "DragoonSkill2")
                {
                    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Dragoon.atk * 2.6f, Dragoon.transform.forward, Dragoon.gameObject.tag);
                    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Dragoon.atk * 2.6f + "감소 전 피해를 입힘.");
                    enemy.GetComponent<PhotonView>().RPC("OnSlow", RpcTarget.All, 0.1f, 2.0f);
                    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 10%의 슬로우를 2초동안 적용함");
                    var effect = PhotonNetwork.Instantiate("Prefebs/Effect_43_Hit", new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.5f, enemy.transform.position.z), Quaternion.LookRotation(-transform.forward) * enemy.transform.rotation);
                    StartCoroutine(destroyEffect(effect));
                }
                else if (tag == "DragoonAttack3")
                {
                    enemy.GetComponent<PhotonView>().RPC("OnHeavyDamage", RpcTarget.All, Dragoon.atk * 1.4f, Dragoon.transform.forward, Dragoon.gameObject.tag);
                    var effect = PhotonNetwork.Instantiate("Prefebs/Effect_32_Hit", new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.5f, enemy.transform.position.z), Quaternion.LookRotation(-transform.forward) * enemy.transform.rotation);
                    StartCoroutine(destroyEffect(effect));
                }


                else if (tag == "DragoonAttack1")
                {
                    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Dragoon.atk , Dragoon.transform.forward, Dragoon.gameObject.tag);
                    var effect = PhotonNetwork.Instantiate("Prefebs/Effect_32_Hit", new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.5f, enemy.transform.position.z), Quaternion.LookRotation(-transform.forward) * enemy.transform.rotation);
                    StartCoroutine(destroyEffect(effect));
                }
                else if (tag == "DragoonAttack2")
                {
                    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Dragoon.atk * 1.1f, Dragoon.transform.forward, Dragoon.gameObject.tag);
                    var effect = PhotonNetwork.Instantiate("Prefebs/Effect_32_Hit", new Vector3(enemy.transform.position.x, enemy.transform.position.y + 0.5f, enemy.transform.position.z), Quaternion.LookRotation(-transform.forward) * enemy.transform.rotation);
                    StartCoroutine(destroyEffect(effect));
                }

                SoundManager.Instance.HitSoundPlay(0);
                //enemy.OnDamage(10);
                //hit = true;
            }
            
        }

        //if (other.gameObject.layer == 9 && tag == "DragoonSkill3" && meteorReady)
        //{
        //    var enemy = other.GetComponent<CharacterMove>();
        //    Debug.Log("브레스타격");
        //    //enemy.OnDamage(10);
        //    enemy.GetComponent<PhotonView>().RPC("OnBurn", RpcTarget.All, 10.0f, Dragoon.atk * 0.25f);

        //}
    }
    //private void OnParticleCollision(GameObject other)
    //{
    //    //ParticlePhysicsExtensions.GetCollisionEvents(particle, other, collisionEvents);
    //    //Debug.Log(gameObject.name + "파티클충돌 with" + other.layer.ToString());

    //}

    void Locate()
    {
        
        //if (gameObject.CompareTag("DragoonAttack1"))
        //{
        //    transform.position = new Vector3(Dragoon.transform.position.x, Dragoon.transform.position.y + 0.5f, Dragoon.transform.position.z + 0.1f);
        //}
        //if (gameObject.CompareTag("DragoonAttack2"))
        //{
        //    transform.position = new Vector3(Atk2Pos.position.x, Atk2Pos.position.y, Atk2Pos.position.z);
        //}
        if (gameObject.CompareTag("DragoonAttack3"))
        {
            transform.position = new Vector3(Dragoon.transform.position.x + 0.2f, Dragoon.transform.position.y + 0.15f, Dragoon.transform.position.z - 0.1f);
        }
    
    }

    IEnumerator Meteor()
    {
        yield return new WaitForSeconds(0.7f);
        meteorReady = true;
        //yield return new WaitForSeconds(2.1f);
        //var coll = gameObject.GetComponent<CapsuleCollider>();
        //coll.enabled = false;
    }
    IEnumerator destroyEffect()
    {
        yield return new WaitForSeconds(1.5f);
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
