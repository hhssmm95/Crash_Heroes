using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WarriorVX : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove Warrior;
    //ParticleSystem particle;
    //List<ParticleCollisionEvent> collisionEvents;
    Transform wAtk2Pos;

    public float skill4Timer;
    public bool s4HitReady;
    bool hit;
    bool checkReady;
    float checkTimer = 0.5f;
    public int count;
    public List<string> dm;
    void Start()
    {
        Warrior = GameObject.FindGameObjectWithTag("Warrior").GetComponent<CharacterMove>();
        dm = new List<string>();
        //particle = GetComponent<ParticleSystem>();
        //collisionEvents = new List<ParticleCollisionEvent>();
        if (tag == "WarriorAttack2")
            wAtk2Pos = GameObject.FindGameObjectWithTag("WarriorAttack2Pos").GetComponent<Transform>();
        else if(tag == "WarriorSkill4")
        {
            StartCoroutine("StayCheck");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //photonView.RPC("Locate", RpcTarget.All);
        Locate();
        //if(checkReady)
        //{
        //    skill4Timer += Time.deltaTime;
        //    if(skill4Timer >= 0.1f && !s4HitReady)
        //    {
        //        s4HitReady = true;
        //    }
        //}
        if (checkReady)
        {
            if (checkTimer >= 0.5f)
                s4HitReady = true;
            else
                checkTimer += Time.deltaTime;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        

    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Warrior") && other.gameObject.layer == 9 && photonView.IsMine)
        {
            var enemy = other.GetComponent<CharacterMove>();
            if (s4HitReady)
            {
                checkTimer = 0.0f;
                s4HitReady = false;

                //skill4Timer = 0.0f;
                count++;
                enemy.GetComponent<PhotonView>().RPC("OnSpecialDamage", RpcTarget.All, Warrior.atk * 1.375f, transform.tag);
                Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Warrior.atk * 1.375f + "감소 전 피해를 입힘.");
                //enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Warrior.atk * 5.5f, Warrior.transform.forward);
                //Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Warrior.atk * 5.5f + "감소 전 피해를 입힘.");
                if (enemy.isDead == true)
                    Warrior.CountKill();
                return;
            }
            if (!dm.Exists(x => x == other.name) && Warrior.isAttacking)
            {
                dm.Add(other.name);
                if (tag == "WarriorAttck3")
                {
                    enemy.GetComponent<PhotonView>().RPC("OnHeavyDamage", RpcTarget.All, Warrior.atk * 1.2f, Warrior.transform.forward);
                    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Warrior.atk * 1.2f + "감소 전 피해를 입힘.");
                }
                else if (tag == "WarriorSkill2_1" || tag == "WarriorSkill2_2")
                {
                    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Warrior.atk * 0.9f, Warrior.transform.forward);
                    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Warrior.atk * 0.9f + "감소 전 피해를 입힘.");
                }
                else if (tag == "WarriorSkill1")
                {
                    enemy.GetComponent<PhotonView>().RPC("OnHeavyDamage", RpcTarget.All, Warrior.atk * 2.2f, Warrior.transform.forward);
                    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Warrior.atk * 2.2f + "감소 전 피해를 입힘.");
                }
                else if (tag == "WarriorSkill4")
                {
                    enemy.GetComponent<PhotonView>().RPC("OnStun", RpcTarget.All, 2.0f);
                    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + "2초 기절 상태이상을 적용시킴.");
                }
                else if(tag == "WarriorAttack1")
                {
                    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Warrior.atk, Warrior.transform.forward);
                    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Warrior.atk + "감소 전 피해를 입힘.");
                }
                else if (tag == "WarriorAttack2")
                {
                    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Warrior.atk*1.1f, Warrior.transform.forward);
                    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Warrior.atk*1.1f + "감소 전 피해를 입힘.");
                }
                if (enemy.isDead == true)
                    Warrior.CountKill();
                SoundManager.Instance.HitSoundPlay(0);
            }
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
    IEnumerator StayCheck()
    {
        yield return new WaitForSeconds(0.5f);
        checkReady = true;
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
