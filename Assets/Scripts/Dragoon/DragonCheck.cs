using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DragonCheck : MonoBehaviourPunCallbacks, IPunObservable
{
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            FireDragon dragon = other.GetComponent<FireDragon>();
            dragon.isCheck = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
