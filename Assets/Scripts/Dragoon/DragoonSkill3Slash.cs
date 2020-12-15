using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DragoonSkill3Slash : MonoBehaviourPunCallbacks, IPunObservable
{
    CharacterMove Dragoon;
    List<string> dm;
    // Start is called before the first frame update
    void Start()
    {
        Dragoon = GameObject.FindGameObjectWithTag("Dragoon").GetComponent<CharacterMove>();
        dm = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Dragoon") && other.gameObject.layer == 9/* && !hit*/)
        {
            if (!dm.Exists(x => x == other.name))
            {
                var enemy = other.GetComponent<CharacterMove>();
                dm.Add(enemy.name);

                enemy.GetComponent<PhotonView>().RPC("OnHeavyDamage", RpcTarget.All, Dragoon.atk * 2.8f, Dragoon.transform.forward, Dragoon.gameObject.tag);
                SoundManager.Instance.HitSoundPlay(7);
            }
            

        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}