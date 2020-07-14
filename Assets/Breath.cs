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
        //collisionEvents = new List<ParticleCollisionEvent>();
        Destroy(gameObject, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(VX, other, collisionEvents);
        for (int i = 0; i < collisionEvents.Count; i++)
        {
            if (other.gameObject.layer == 9 && !hit)
            {
                Debug.Log("브레스타격");
                var enemy = other.GetComponent<CharacterMove>();
                //enemy.OnDamage(10);
                enemy.GetComponent<PhotonView>().RPC("OnBurn", RpcTarget.All, 10.0f, Dragoon.atk * 0.25f);
                hit = true;
            }
        }
    }
    
}