using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DragoonVX : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove Dragoon;
    ParticleSystem particle;
    List<ParticleCollisionEvent> collisionEvents;

    Transform Atk2Pos;
    Transform Skill1Pos;

    bool hit;
    void Start()
    {
        Dragoon = GameObject.FindGameObjectWithTag("Dragoon").GetComponent<CharacterMove>();
        particle = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();

        Atk2Pos = GameObject.FindGameObjectWithTag("DragoonAttack2Pos").GetComponent<Transform>();
        Skill1Pos = GameObject.FindGameObjectWithTag("DragoonSkill1Pos").GetComponent<Transform>();

        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        photonView.RPC("Locate", RpcTarget.All);
    }

    [PunRPC]
    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particle, other, collisionEvents);
        //Debug.Log("파티클충돌");
        if (!other.CompareTag("Dragoon") && other.layer.ToString() == "Player" && !hit)
        {
            //Debug.Log("VX충돌");
            var enemy = other.GetComponent<CharacterMove>();
            if (gameObject.CompareTag("DragoonSkill1"))
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Dragoon.atk*1.2f);
                enemy.GetComponent<PhotonView>().RPC("OnSlow", RpcTarget.All, 0.5f, 3.0f);
                //enemy.OnSlow(0.5f, 3.0f);
                return;
            }
            else if(gameObject.CompareTag("DragoonSkill2"))
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Dragoon.atk * 1.5f);
                return;
            }
            else if(gameObject.CompareTag("DragoonAttack3"))
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

    [PunRPC]
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
        else if (gameObject.CompareTag("DragoonAttck3"))
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
