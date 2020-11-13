using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MageVX : MonoBehaviour
{
    CharacterMove Mage;
    //ParticleSystem particle;
    //List<ParticleCollisionEvent> collisionEvents;
    Transform mAtkPos;

    bool hit;
    // Start is called before the first frame update
    private void Awake()
    {
        Mage = GameObject.FindGameObjectWithTag("Mage").GetComponent<CharacterMove>();
    }
    void Start()
    {
        
        //particle = GetComponent<ParticleSystem>();
        //collisionEvents = new List<ParticleCollisionEvent>();
        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Locate();
    }

    void Locate()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject.name + "파티클충돌 with" + other.gameObject.layer.ToString());
        if (!other.CompareTag("Mage") && other.gameObject.layer == 9 && Mage.isAttacking && !hit)
        {
            var enemy = other.GetComponent<CharacterMove>();
            //enemy.OnDamage(10);

            //if (tag == "ArcherAttack1" || tag == "ArcherAttack2")
            //{
            //    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Archer.atk * 0.9f);
            //}
            //if (tag == "ArcherVX1")
            //{
            //    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Archer.atk, Archer.transform.forward);
            //    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Archer.atk + "감소 전 피해를 입힘.");
            //    //enemy.GetComponent<PhotonView>().RPC("OnStun", RpcTarget.All, 1.0f);
            //    //Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + "1초 기절 상태이상을 적용시킴.");
            ////}
            //if(gameObject.tag == "ArcherSkill1")
            //{
            //    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Archer.atk, Archer.transform.forward);
            //    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Archer.atk + "감소 전 피해를 입힘.");
            //    enemy.GetComponent<PhotonView>().RPC("OnStun", RpcTarget.All, 1.0f);
            //    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + "1초 기절 상태이상을 적용시킴.");
            //}
            
            if (gameObject.name == "MageSkill1VX")
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Mage.atk * 1.1f, Mage.transform.forward);
                Debug.Log(gameObject.name + "스킬이 " + enemy.gameObject.name + "에게 " + Mage.atk * 1.1f + "감소 전 피해를 입힘.");
            }
            if (gameObject.name == "MageSkill2VX")
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Mage.atk * 1.2f, Mage.transform.forward);
                Debug.Log(gameObject.name + "스킬이 " + enemy.gameObject.name + "에게 " + Mage.atk * 1.2f + "감소 전 피해를 입힘.");
            }
            if (gameObject.name == "MageSkill3VX")
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Mage.atk * 1.5f, Mage.transform.forward);
                Debug.Log(gameObject.name + "스킬이 " + enemy.gameObject.name + "에게 " + Mage.atk * 1.5f + "감소 전 피해를 입힘.");
            }
            //SoundManager.Instance.HitSoundPlay(0);

            hit = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
