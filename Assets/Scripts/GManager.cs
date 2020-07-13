using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.UI;

public class GManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public Transform[] spawn_point;
    private NickNameList nickNameList;
    public PhotonView PV;
    

    public GameObject characterPanel;
    public GameObject knightImage;
    public GameObject archerImage;
    public GameObject dragoonImage;
    public GameObject mageImage;

    public Text text_Time;
    public float LimitTime = 300;

    public string pickName = null;
    public bool knightPick = false;
    public bool archerPick = false;
    public bool dragoonPick = false;
    public bool magePick = false;

    public int playerNum = 0;
    public bool[] pickList = { true, true, true, true };
    public bool isPickCheck = false;

    public bool isSpawn = false;
    private void Start()
    {
        text_Time = GameObject.FindWithTag("Timer").GetComponent<Text>();
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
                pickList[i] = false;
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            Debug.Log(pickList[i] + "지속확인" + i);
        }
        if (pickList[0] == true && pickList[1] == true && pickList[2] == true && pickList[3] == true)
        {
            Timer();
        }
    }

    public void Spawn()
    {
        PhotonNetwork.Instantiate(pickName, spawn_point[playerNum].position, spawn_point[playerNum].rotation);
    }

    public void Pick(int num)
    {
        if (isPickCheck == false)
        {
            isPickCheck = true;
            PV.RPC("PickCheck", RpcTarget.All, num, playerNum);
            characterPanel.SetActive(false);
            Spawn();
        }
    }

    [PunRPC]
    public void PickCheck(int pickNum, int playerNum)
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
        pickList[playerNum] = true;
    }
    public void Timer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LimitTime -= Time.deltaTime;
            text_Time.text = Mathf.Round(LimitTime).ToString();

            if (LimitTime <= 0)
            {
                text_Time.text = "0";
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (stream.IsWriting) stream.SendNext(pickList[i]);
            else pickList[i] = (bool)stream.ReceiveNext();
        }

        if (stream.IsWriting) stream.SendNext(text_Time.text);
        else text_Time.text = (string)stream.ReceiveNext();
    }
}