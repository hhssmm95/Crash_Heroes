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
    float meteorCount;
    //bool hit;
    void Awake()
    {
        Dragoon = GameObject.FindGameObjectWithTag("Dragoon").GetComponent<CharacterMove>();
        //particle = GetComponent<ParticleSystem>();
        //collisionEvents = new List<ParticleCollisionEvent>();

        if(tag == "DragoonAttack2" || tag == "DragoonSkill2")
            Atk2Pos = GameObject.FindGameObjectWithTag("DragoonAtack2Pos").GetComponent<Transform>();
        if(tag == "DragoonSkill1")
            Skill1Pos = GameObject.FindGameObjectWithTag("DragoonSkill1Pos").GetComponent<Transform>();
    }

    void Start()
    {
        if(tag != "DragoonSkill3")
            Destroy(gameObject, 1.3f);
        else
        {
            StartCoroutine("Meteor");
        }
    }
    

    void Update()
    {
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
        if (!other.CompareTag("Dragoon") && other.gameObject.layer == 9 && Dragoon.isAttacking/* && !hit*/)
        {
            //Debug.Log("VX충돌");
            var enemy = other.GetComponent<CharacterMove>();


            if (tag == "DragoonSkill1")
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Dragoon.atk * 1.2f, Dragoon.transform.forward);
                Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Dragoon.atk * 1.2f + "감소 전 피해를 입힘.");
                enemy.GetComponent<PhotonView>().RPC("OnSlow", RpcTarget.All, 0.5f, 3.0f);
                //Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + )
                //enemy.OnSlow(0.5f, 3.0f);
                return;
            }
            else if (tag == "DragoonSkill2")
            {
                enemy.GetComponent<PhotonView>().RPC("OnHeavyDamage", RpcTarget.All, Dragoon.atk * 1.5f, Dragoon.transform.forward);
            }
            else if (tag == "DragoonAttack3")
            {
                enemy.GetComponent<PhotonView>().RPC("OnHeavyDamage", RpcTarget.All, Dragoon.atk * 1.3f, Dragoon.transform.forward);
            }
            
            else
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Dragoon.atk * 1.1f, Dragoon.transform.forward);
            }
            SoundManager.Instance.HitSoundPlay(0);
            //enemy.OnDamage(10);
            //hit = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 9 && tag == "DragoonSkill3" && meteorReady)
        {
            var enemy = other.GetComponent<CharacterMove>();
            Debug.Log("브레스타격");
            //enemy.OnDamage(10);
            enemy.GetComponent<PhotonView>().RPC("OnBurn", RpcTarget.All, 10.0f, Dragoon.atk * 0.25f);

        }
    }
    //private void OnParticleCollision(GameObject other)
    //{
    //    //ParticlePhysicsExtensions.GetCollisionEvents(particle, other, collisionEvents);
    //    //Debug.Log(gameObject.name + "파티클충돌 with" + other.layer.ToString());

    //}

    void Locate()
    {
        if (gameObject.CompareTag("DragoonAttack1"))
        {
            transform.position = new Vector3(Dragoon.transform.position.x, Dragoon.transform.position.y + 0.5f, Dragoon.transform.position.z + 0.1f);
        }
        else if (gameObject.CompareTag("DragoonAttack2"))
        {
            transform.position = new Vector3(Atk2Pos.position.x, Atk2Pos.position.y, Atk2Pos.position.z);
        }
        else if (gameObject.CompareTag("DragoonAttack3"))
        {
            transform.position = new Vector3(Dragoon.transform.position.x + 0.2f, Dragoon.transform.position.y + 0.15f, Dragoon.transform.position.z - 0.1f);
        }
        else if (gameObject.CompareTag("DragoonSkill1"))
        {
            transform.position = new Vector3(Skill1Pos.position.x, Skill1Pos.position.y, Skill1Pos.position.z);
        }
        else if (gameObject.CompareTag("DragoonSkill2"))
        {
            transform.position = new Vector3(Atk2Pos.position.x, Atk2Pos.position.y, Atk2Pos.position.z);
        }
    
    }

    IEnumerator Meteor()
    {
        yield return new WaitForSeconds(0.7f);
        meteorReady = true;
        yield return new WaitForSeconds(4.0f);
        var coll = gameObject.GetComponent<CapsuleCollider>();
        coll.enabled = false;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
