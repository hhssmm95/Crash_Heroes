using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FlamesToggle : MonoBehaviourPunCallbacks, IPunObservable
{

	public GameObject flameObject;
	public bool activateFlame;


    void Start()
	{
		flameObject.SetActive (false);
		activateFlame = false;
	}

	// Update is called once per frame
	void Update ()
    {
  //      if (activateFlame == true)
  //      {
  //          photonView.RPC("flameOn", RpcTarget.All);

  //      }

		//if (activateFlame == false)
		//{
  //          photonView.RPC("flameOff", RpcTarget.All);
  //      }
			
	}

    [PunRPC]
    void flameOn()
    {
        flameObject.SetActive(true);
        activateFlame = true;
    }

    [PunRPC]
    void flameOff()
    {
        flameObject.SetActive(false);
        //activateFlame = false;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(activateFlame);
        }
        else
            activateFlame = (bool)stream.ReceiveNext();
    }
}
