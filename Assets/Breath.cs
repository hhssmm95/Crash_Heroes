using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Breath : MonoBehaviourPunCallbacks
{
    ParticleSystem VX;
    List<ParticleCollisionEvent> collisionEvents;
    CharacterMove Dragoon;
    bool hit;
    void Start()
    {
        Dragoon = GameObject.FindGameObjectWithTag("Dragoon").GetComponent<CharacterMove>();
        VX = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(VX, other, collisionEvents);
        //Debug.Log("파티클충돌");
        if (other.gameObject.layer == 9 && Dragoon.isAttacking && !hit)
        {
            Debug.Log("브레스타격");
            var enemy = other.GetComponent<CharacterMove>();
            //enemy.OnDamage(10);
            enemy.GetComponent<PhotonView>().RPC("OnBurn", RpcTarget.All, 10.0f, Dragoon.atk * 0.25f);
            hit = true;
        }
    }
    
}