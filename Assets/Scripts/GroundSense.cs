using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GroundSense : MonoBehaviourPunCallbacks, IPunObservable
{
    //private GameObject player; //플레이어 오브젝트 
    CharacterMove player;
    public bool isGround; //발이 땅에 닿아있다면 true 
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject.GetComponent<CharacterMove>(); //부모 오브젝트 받아옴
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            isGround = true;
            //player.myAnim.SetBool("Jump", false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            isGround = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player") && isGround == false)
            isGround = true;
    }

    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) stream.SendNext(isGround);
        else isGround = (bool)stream.ReceiveNext();
    }
}
