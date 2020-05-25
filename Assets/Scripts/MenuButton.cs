using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MenuButton : MonoBehaviour
{
    public void OnClickExit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
