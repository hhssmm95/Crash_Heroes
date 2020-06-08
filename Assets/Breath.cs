using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breath : MonoBehaviour
{
    ParticleSystem VX;
    List<ParticleCollisionEvent> collisionEvents;
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
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("VX충돌");
            var enemy = other.GetComponent<CharacterMove>();
            enemy.OnDamage(10);
        }
    }
}