using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthBar : MonoBehaviourPunCallbacks, IPunObservable
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    [PunRPC]
    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    [PunRPC]
    public void SetHealth(float health)
    {
        slider.value = health;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(slider.maxValue);
            stream.SendNext(slider.value);
            stream.SendNext(fill.color);
        }
        else
        {
            slider.maxValue = (float)stream.ReceiveNext();
            slider.value = (float)stream.ReceiveNext();
            fill.color = (Color)stream.ReceiveNext();
        }
    }
}
