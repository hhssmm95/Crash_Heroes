using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ArcherSkill1 : MonoBehaviourPunCallbacks, IPunObservable
{
    ParticleSystem ArcherVX1;
    List<ParticleCollisionEvent> collisionEvents;
    void Start()
    {
        ArcherVX1 = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    [PunRPC]
    private void OnParticleCollision(GameObject other)
    {
        //ParticlePhysicsExtensions.GetCollisionEvents(ArcherVX1, other, collisionEvents);
        //Debug.Log("파티클충돌");
        if(other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<CharacterMove>();
            enemy.OnDamage(10);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
