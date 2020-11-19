using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Arrow : MonoBehaviourPunCallbacks, IPunObservable
{
    //CharacterMove Archer;
    float atk;
    Vector3 normal;
    //float speed = 10;
    bool hit;
    void Start()
    {
        atk = GameObject.FindGameObjectWithTag("Archer").GetComponent<CharacterMove>().atk;
        normal = GameObject.FindGameObjectWithTag("Archer").transform.forward;
        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine && !other.CompareTag("Archer") && other.gameObject.layer == 9 && other.GetComponent<PhotonView>().IsMine && !hit)
        {
            var enemy = other.GetComponent<CharacterMove>();

            enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, atk * 1.3f, normal);
            Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + atk * 1.3f + "감소 전 피해를 입힘.");
            SoundManager.Instance.HitSoundPlay(1);
            hit = true;
        }

        Destroy(gameObject);

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
