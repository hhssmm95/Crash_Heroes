using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SlashVFX : MonoBehaviourPunCallbacks, IPunObservable
{
    private CharacterMove warrior;
    public float speed = 4;
    private Vector3 dir;
    bool hit;
    void Start()
    {
        warrior = GameObject.FindGameObjectWithTag("Warrior").GetComponent<CharacterMove>();
        dir = warrior.transform.forward;
        //Destroy(gameObject, 3);
        //transform.rotation = Quaternion.LookRotation(dir);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }
    
    //[PunRPC]
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Warrior") && other.gameObject.layer.ToString() == "Player" && !hit)
        {
            var enemy = other.GetComponent<CharacterMove>();
            enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, warrior.atk*1.5f);
            hit = true;
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
