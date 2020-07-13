using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Arrow : MonoBehaviourPunCallbacks, IPunObservable
{
    //CharacterMove Archer;
    float atk;
    Vector3 normal;
    float speed = 10;
    bool hit;
    void Start()
    {
        atk = GameObject.FindGameObjectWithTag("Archer").GetComponent<CharacterMove>().atk;
        normal = GameObject.FindGameObjectWithTag("Archer").transform.forward;
        if (gameObject.tag == "BigArrow")
            speed *= 2;
        Destroy(gameObject, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine && !other.CompareTag("Archer") && other.gameObject.layer == 9 && other.GetComponent<PhotonView>().IsMine && !hit)
        {
            var enemy = other.GetComponent<CharacterMove>();

            if (gameObject.tag == "BigArrow")
            {
                enemy.GetComponent<PhotonView>().RPC("OnHeavyDamage", RpcTarget.All, atk * 1.5f, normal);
            }
            else if (gameObject.tag == "MultiArrow")
            {
                enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, atk * 1.2f, normal);
            }
            else
            {
                enemy.GetComponent<PhotonView>().RPC("OnHeavyDamage", RpcTarget.All, atk * 1.3f, normal);
            }
            hit = true;
        }

        Destroy(gameObject);

    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
