using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Arrow : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove Archer;
    float atk;
    Vector3 normal;
    //float speed = 10;
    bool hit;
    void Start()
    {
        Archer = GameObject.FindGameObjectWithTag("Archer").GetComponent<CharacterMove>();
        atk = Archer.GetComponent<CharacterMove>().atk;
        normal = Archer.transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Archer") && other.gameObject.layer == 9)
        {
            var enemy = other.GetComponent<CharacterMove>();

            if (tag == "ArcherSkill1" && !hit)
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, atk * 1.3f, normal);
                Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + atk * 1.3f + "감소 전 피해를 입힘.");
                SoundManager.Instance.HitSoundPlay(1);
                if (enemy.isDead == true)
                    Archer.CountKill();
                hit = true;
            }
            //else if(tag == "ArcherSkill2")
            //{
            //    enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, atk * 2.6f, normal);
            //    Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + atk * 2.6f + "감소 전 피해를 입힘.");
            //}
            
        }

        Destroy(gameObject);

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
