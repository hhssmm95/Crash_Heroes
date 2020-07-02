using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GManager : MonoBehaviourPunCallbacks
{
    public string[] player_prefab;
    public Transform[] spawn_point;
    private void Start()
    {
        Spawn();
    }
    
    public void Spawn()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            int ran = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.Instantiate(player_prefab[ran], spawn_point[ran].position, spawn_point[ran].rotation);
        }
    }
}