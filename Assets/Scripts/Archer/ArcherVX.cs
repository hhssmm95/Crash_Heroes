using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ArcherVX : MonoBehaviourPunCallbacks, IPunObservable
{
    ParticleSystem particle;
    List<ParticleCollisionEvent> collisionEvents;
    CharacterMove Archer;

    Transform Atk2Pos;
    Transform Skill1Pos;

    bool hit;
    void Start()
    {
        Archer = GameObject.FindGameObjectWithTag("Archer").GetComponent<CharacterMove>();
        if (gameObject.tag == "ArcherVX1")
        {
            Skill1Pos = GameObject.FindGameObjectWithTag("ArcherSkill1Pos").GetComponent<Transform>();
            particle = GetComponent<ParticleSystem>();
            collisionEvents = new List<ParticleCollisionEvent>();
        }
        if (gameObject.tag == "ArcherAttack2")
            Atk2Pos = GameObject.FindGameObjectWithTag("ArcherAttack2Pos").GetComponent<Transform>();

        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Locate();
    }

    //[PunRPC]

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject.name + "파티클충돌 with" + other.gameObject.layer.ToString());
        if (!other.CompareTag("Archer") && other.gameObject.layer == 9 && Archer.isAttacking && !hit)
        {
            var enemy = other.GetComponent<CharacterMove>();
            //enemy.OnDamage(10);

            //if (tag == "ArcherAttack1" || tag == "ArcherAttack2")
            //{
            //    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Archer.atk * 0.9f);
            //}
            if (tag == "ArcherVX1")
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Archer.atk, Archer.transform.forward);
                Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Archer.atk + "감소 전 피해를 입힘.");
                enemy.GetComponent<PhotonView>().RPC("OnStun", RpcTarget.All, 1.0f);
                Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + "1초 기절 상태이상을 적용시킴.");
            }
            else
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Archer.atk * 0.9f, Archer.transform.forward);
                Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Archer.atk * 0.9f + "감소 전 피해를 입힘.");
            }


            hit = true;
        }
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
        else if (gameObject.CompareTag("ArcherVX1"))
        {
            transform.position = new Vector3(Skill1Pos.position.x, Skill1Pos.position.y, Skill1Pos.position.z);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
