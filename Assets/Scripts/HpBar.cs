using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HpBarSlider : MonoBehaviourPunCallbacks, IPunObservable
{
    public Slider slider;

    [PunRPC]
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    [PunRPC]
    public void SetHealth(int health)
    {
        slider.value = health;
    }

    void Update()
    {
        if (slider.value <= 0)
            slider.GetComponent<PhotonView>().RPC("FillOff", RpcTarget.All);
        //transform.Find("Fill Area").gameObject.SetActive(false);
        else
            slider.GetComponent<PhotonView>().RPC("FillOn", RpcTarget.All);
        //transform.Find("Fill Area").gameObject.SetActive(true);
    }

    [PunRPC]
    void FillOff()
    {
        transform.Find("Fill Area").gameObject.SetActive(false);
    }

    [PunRPC]
    void FillOn()
    {
        transform.Find("Fill Area").gameObject.SetActive(true);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(slider.maxValue);
            stream.SendNext(slider.value);
        }
        else
        {
            slider.maxValue = (float)stream.ReceiveNext();
            slider.value = (float)stream.ReceiveNext();
        }
    }
}
