using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WarriorVX : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove Warrior;
    ParticleSystem particle;
    List<ParticleCollisionEvent> collisionEvents;
    Transform wAtk2Pos;

    bool hit;

    void Start()
    {
        Warrior = GameObject.FindGameObjectWithTag("Warrior").GetComponent<CharacterMove>();
        particle = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        if (tag == "WarriorAttack2")
            wAtk2Pos = GameObject.FindGameObjectWithTag("WarriorAttack2Pos").GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        photonView.RPC("Locate", RpcTarget.All);
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particle, other, collisionEvents);

        //Debug.Log("파티클충돌");
        if (!other.CompareTag("Warrior") && other.layer.ToString() == "Player" && !hit)
        {
            var enemy = other.GetComponent<CharacterMove>();
            //enemy.OnDamage(10);

            //if (tag == "WarriorAttack1" || tag == "WarriorAttack2")
            //{
            //    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Warrior.atk);
            //}
            if (tag == "WarriorAttack3")
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Warrior.atk*1.2f);
            }
            else if (tag == "WarriorSkill2_1" || tag == "WarriorSkill2_2")
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Warrior.atk*0.9f);

            }
            else
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Warrior.atk);
            }

            hit = true;
        }
    }


    [PunRPC]
    void Locate()
    {
        if (gameObject.CompareTag("WarriorAttack1"))
        {
            transform.position = new Vector3(Warrior.transform.position.x, Warrior.transform.position.y + 0.5f, Warrior.transform.position.z + 0.1f);
        }
        else if (gameObject.CompareTag("WarriorAttack2"))
        {
            transform.position = new Vector3(wAtk2Pos.position.x, wAtk2Pos.position.y, wAtk2Pos.position.z);
        }
        else if (gameObject.CompareTag("WarriorAttck3"))
        {
            transform.position = new Vector3(Warrior.transform.position.x + 0.2f, Warrior.transform.position.y + 0.15f, Warrior.transform.position.z - 0.1f);
        }
        else if (gameObject.CompareTag("WarriorSkill2_1"))
            transform.position = new Vector3(Warrior.transform.position.x, Warrior.transform.position.y + 0.406f, Warrior.transform.position.z + 0.1f);

        else if (gameObject.CompareTag("WarriorSkill2_2"))
            transform.position = new Vector3(Warrior.transform.position.x, Warrior.transform.position.y + 0.656f, Warrior.transform.position.z + 0.1f);

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

}
