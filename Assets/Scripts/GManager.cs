using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class GManager : MonoBehaviourPunCallbacks
{
    public string[] player_prefab;
    public Transform[] spawn_point;
    
    NetworkManager networkManager;

    public string nickName = null;
    public int playerNum = 0;
    private void Start()
    {
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
        Spawn();
    }
    
    public void Spawn()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (networkManager.NickNameList[i].text == nickName)
            {
                playerNum = i;
                break;
            }
        }
        PhotonNetwork.Instantiate(player_prefab[playerNum], spawn_point[playerNum].position, spawn_point[playerNum].rotation);
    }

    
}