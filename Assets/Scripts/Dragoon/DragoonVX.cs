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

    bool hit;
    void Start()
    {
        Dragoon = GameObject.FindGameObjectWithTag("Dragoon").GetComponent<CharacterMove>();
        //particle = GetComponent<ParticleSystem>();
        //collisionEvents = new List<ParticleCollisionEvent>();

        if(tag == "DragoonAttack2" || tag == "DragoonSkill2")
            Atk2Pos = GameObject.FindGameObjectWithTag("DragoonAtack2Pos").GetComponent<Transform>();
        if(tag == "DragoonSkill1")
            Skill1Pos = GameObject.FindGameObjectWithTag("DragoonSkill1Pos").GetComponent<Transform>();

        Destroy(gameObject, 1.0f);
    }
    

    void Update()
    {
        Locate();
    }
    
    private void OnParticleCollision(GameObject other)
    {
        //ParticlePhysicsExtensions.GetCollisionEvents(particle, other, collisionEvents);
        //Debug.Log(gameObject.name + "파티클충돌 with" + other.layer.ToString());
        if (!other.CompareTag("Dragoon") && other.gameObject.layer == 9 && Dragoon.isAttacking && !hit)
        {
            //Debug.Log("VX충돌");
            var enemy = other.GetComponent<CharacterMove>();


            if (tag == "DragoonSkill1")
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Dragoon.atk*1.2f, -transform.forward);
                Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Dragoon.atk * 1.2f + "감소 전 피해를 입힘.");
                enemy.GetComponent<PhotonView>().RPC("OnSlow", RpcTarget.All, 0.5f, 3.0f, -transform.forward);
                //Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + )
                //enemy.OnSlow(0.5f, 3.0f);
                return;
            }
            else if(tag == "DragoonSkill2")
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Dragoon.atk * 1.5f);
                return;
            }
            else if(tag == "DragoonAttack3")
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Dragoon.atk * 1.3f);
            }
            else
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Dragoon.atk * 1.1f);
            }
            //enemy.OnDamage(10);
            hit = true;
        }
    }
    
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


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
