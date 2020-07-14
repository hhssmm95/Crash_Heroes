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
        //dir = warrior.transform.forward;
        Destroy(gameObject, 3.0f);
        //transform.rotation = Quaternion.LookRotation(dir);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (-transform.right) * speed * Time.deltaTime;
    }
    
    //[PunRPC]
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Warrior") && other.gameObject.layer == 9 && !hit)
        {
            var enemy = other.GetComponent<CharacterMove>();
            enemy.GetComponent<PhotonView>().RPC("OnHeavyDamage", RpcTarget.All, warrior.atk*1.5f, -transform.right);
            Debug.Log(tag + "스킬이 " + enemy.gameObject.name + "에게 " + warrior.atk * 1.5f + "감소 전 피해를 입힘.");
            SoundManager.Instance.HitSoundPlay(0);
            hit = true;
            
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
