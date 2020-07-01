using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class SlashVFX : MonoBehaviourPunCallbacks, IPunObservable
{
    private Transform player;
    public float speed = 4;
    private Vector3 dir;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        dir = player.transform.forward;
        //Destroy(gameObject, 3);
        //transform.rotation = Quaternion.LookRotation(dir);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }
    
    [PunRPC]
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<CharacterMove>();
            enemy.OnDamage(10);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
