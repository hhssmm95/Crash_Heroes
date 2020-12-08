using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ArcherSkill2VX : MonoBehaviourPunCallbacks, IPunObservable
{
    float atk;
    void Start()
    {
        atk = GameObject.FindGameObjectWithTag("Archer").GetComponent<CharacterMove>().atk;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Archer") && other.gameObject.layer == 9)
        {
            CharacterMove enemy = other.GetComponent<CharacterMove>();

            enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, atk * 2.6f, transform.forward);
            Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + atk * 2.6f + "감소 전 피해를 입힘.");

            //SoundManager.Instance.HitSoundPlay(0);
            //hit = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

}
