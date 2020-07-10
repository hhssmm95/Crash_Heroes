using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Breath : MonoBehaviourPunCallbacks, IPunObservable
{
    ParticleSystem VX;
    List<ParticleCollisionEvent> collisionEvents;
    CharacterMove Dragoon;
    void Start()
    {
        VX = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnParticleCollision(GameObject other)
    {
        //ParticlePhysicsExtensions.GetCollisionEvents(ArcherVX1, other, collisionEvents);
        //Debug.Log("파티클충돌");
        if (!other.CompareTag("Dragoon") && other.gameObject.layer.ToString() == "Player")
        {
            Debug.Log("브레스타격");
            var enemy = other.GetComponent<CharacterMove>();
            //enemy.OnDamage(10);
            enemy.GetComponent<PhotonView>().RPC("OnBurn", RpcTarget.All, 10.0f, Dragoon.atk * 0.25f);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}