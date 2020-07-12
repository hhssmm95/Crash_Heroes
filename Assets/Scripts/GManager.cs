using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class GManager : MonoBehaviourPunCallbacks//IPunObservable
{
    public Transform[] spawn_point;
    private NickNameList nickNameList;
    public PhotonView PV;
    private bool[] ready = { true, true, true, true};

    public GameObject characterPanel;
    public GameObject knightImage;
    public GameObject archerImage;
    public GameObject dragoonImage;
    public GameObject mageImage;

    public string pickName = null;
    public bool knightPick = false;
    public bool archerPick = false;
    public bool dragoonPick = false;
    public bool magePick = false;

    private bool IsSpawn = false;

    public int playerNum = 0;
    private void Start()
    {
        nickNameList = GameObject.Find("NickNameList").GetComponent<NickNameList>();
        
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            if (nickNameList.NameList[i] == nickNameList.myNickName)
            {
                playerNum = i;
                break;
            }
        }

        if (PhotonNetwork.PlayerList.Length == nickNameList.playerCount)
        {
            characterPanel.SetActive(true);
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                ready[i] = false;
            }
        }    
    }

    public void Spawn()
    {
        PhotonNetwork.Instantiate(pickName, spawn_point[playerNum].position, spawn_point[playerNum].rotation);
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

    public void Pick(int num)
    {
        PV.RPC("PickCheck", RpcTarget.All, num);
        characterPanel.SetActive(false);
        Spawn();
    }
    [PunRPC]
    public void PickCheck(int pickNum)
    {
        if (pickNum == 1 && knightPick == false)
        {
            pickName = "Prefebs/Knight";
            knightPick = true;
            knightImage.transform.GetChild(0).gameObject.SetActive(false);
            knightImage.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (pickNum == 2 && archerPick == false)
        {
            pickName = "Prefebs/Archer";
            archerPick = true;
            archerImage.transform.GetChild(0).gameObject.SetActive(false);
            archerImage.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (pickNum == 3 && dragoonPick == false)
        {
            pickName = "Prefebs/Dragoon";
            dragoonPick = true;
            dragoonImage.transform.GetChild(0).gameObject.SetActive(false);
            dragoonImage.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (pickNum == 4 && magePick == false)
        {
            pickName = "Prefebs/Mage";
            magePick = true;
            mageImage.transform.GetChild(0).gameObject.SetActive(false);
            mageImage.transform.GetChild(2).gameObject.SetActive(true);
        }
    }
}