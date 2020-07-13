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

        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //photonView.RPC("Locate", RpcTarget.All);
        Locate();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name + "파티클충돌 ");
        if (!other.CompareTag("Warrior") && other.gameObject.layer == 9 && Warrior.isAttacking && !hit)
        {
            
            var enemy = other.GetComponent<CharacterMove>();
            //enemy.OnDamage(10);

            //if (tag == "WarriorAttack1" || tag == "WarriorAttack2")
            //{
            //    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Warrior.atk);
            //}
            if (tag == "WarriorAttack3")
            {
                enemy.GetComponent<PhotonView>().RPC("OnHeavyDamage", RpcTarget.All, Warrior.atk * 1.2f, Warrior.transform.forward);
            }
            else if (tag == "WarriorSkill2_1" || tag == "WarriorSkill2_2")
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Warrior.atk * 0.9f, Warrior.transform.forward);

            }
            else
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Warrior.atk, Warrior.transform.forward);
            }

            hit = true;
        }
    }

    
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
