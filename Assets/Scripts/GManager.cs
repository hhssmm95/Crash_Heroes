using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class GManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string[] prefebsName = { "Knight", "Prefebs/Archer", "Prefebs/Dragoon" };
    public Transform[] spawn_point;
    
    private NickNameList nickNameList;

    public string nickName = null;
    public int playerNum = 0;
    private void Start()
    {
        nickNameList = GameObject.Find("NickNameList").GetComponent<NickNameList>();
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (nickNameList.NameList[i] == nickName)
            {
                playerNum = i;
                break;
            }
        }

        Debug.Log(nickName);
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(nickNameList.NameList[i]);
        }
        //Shuffle(prefebsName);
        //Spawn();
    }
    
    public void Spawn()
    {
        PhotonNetwork.Instantiate(prefebsName[playerNum], spawn_point[playerNum].position, spawn_point[playerNum].rotation);
    }

    private static void Shuffle<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            System.Random ran = new System.Random();
            int randomValue = ran.Next(0, array.Length);
            T temp = array[i];
            array[i] = array[randomValue];
            array[randomValue] = temp;
        }
    }
}