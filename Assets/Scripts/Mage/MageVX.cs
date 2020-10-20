using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MageVX : MonoBehaviour
{
    CharacterMove Mage;
    //ParticleSystem particle;
    //List<ParticleCollisionEvent> collisionEvents;
    Transform mAtkPos;
    // Start is called before the first frame update
    private void Awake()
    {
        Mage = GameObject.FindGameObjectWithTag("Mage").GetComponent<CharacterMove>();
    }
    void Start()
    {
        
        //particle = GetComponent<ParticleSystem>();
        //collisionEvents = new List<ParticleCollisionEvent>();
        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Locate();
    }

    void Locate()
    {

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
