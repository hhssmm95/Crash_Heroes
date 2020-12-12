using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DragoonSkill3Slash : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove Dragoon;
    // Start is called before the first frame update
    void Start()
    {
        Dragoon = GameObject.FindGameObjectWithTag("Dragoon").GetComponent<CharacterMove>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Dragoon") && other.gameObject.layer == 9/* && !hit*/)
        {
            var enemy = other.GetComponent<CharacterMove>();
            enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Dragoon.atk * 2.8f, Dragoon.transform.forward);

        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}