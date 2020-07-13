using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArcherSkill1VX : MonoBehaviourPunCallbacks, IPunObservable
{
    public CharacterMove Archer;
    public Transform Skill1Pos;
    bool hit;

    private void Awake()
    {
        Archer = GameObject.FindGameObjectWithTag("Archer").GetComponent<CharacterMove>();
        if(Archer == null)
            Debug.Log("아쳐스킬1 아쳐 미싱");

        Skill1Pos = GameObject.FindGameObjectWithTag("ArcherSkill1Pos").GetComponent<Transform>();
        if (Skill1Pos == null)
            Debug.Log("ArcherSkill1Pos is missing");
    }

    void Start()
    {
        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Skill1Pos.position.x, Skill1Pos.position.y, Skill1Pos.position.z);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Archer") && other.gameObject.layer == 9 && Archer.isAttacking && !hit)
        {
            CharacterMove enemy = other.GetComponent<CharacterMove>();

            enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Archer.atk, Archer.transform.forward);
            Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + Archer.atk + "감소 전 피해를 입힘.");
            enemy.GetComponent<PhotonView>().RPC("OnStun", RpcTarget.All, 1.0f);
            Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + "1초 기절 상태이상을 적용시킴.");

            hit = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

}
