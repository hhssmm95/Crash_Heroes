using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragoonVX : MonoBehaviour
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

            var enemy = other.GetComponent<CharacterMove>();
            if (gameObject.CompareTag("DragoonSkill1"))
            {
                enemy.OnDamage(10);
                //enemy.OnSlow(0.5f, 3.0f);
                return;
            }
            else if(gameObject.CompareTag("DragoonSkill2"))
            {

                enemy.OnDamage(10);
            }
        }
    }
}
