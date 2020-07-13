using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public Transform[] spawn_point;
    private NickNameList nickNameList;
    public PhotonView PV;
    public CharacterMove characterMove;
    public AudioClip[] musicList;
    AudioSource audioSource;

    public GameObject characterPanel;
    public GameObject knightImage;
    public GameObject archerImage;
    public GameObject dragoonImage;
    public GameObject mageImage;
    public GameObject victoryPanel;
    public GameObject DefeatPanel;
    public GameObject exitButton;
    public GameObject uiManager;

    public Text text_Time;
    public float LimitTime = 200;

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
        audioSource = GetComponent<AudioSource>();

        StartCoroutine("PlayMusicList", 0);

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
        if (pickList[0] == true && pickList[1] == true && pickList[2] == true && pickList[3] == true)
        {
            if (LimitTime > 0)
            {
                TimerAndGameOver();
            }
        }
    }

    #region 캐릭터 생성
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
    #endregion

    #region 타이머
    public void TimerAndGameOver()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LimitTime -= Time.deltaTime;
            text_Time.text = Mathf.Round(LimitTime).ToString();
            if (LimitTime <= 0)
            {
                LimitTime = 0;
                text_Time.text = "0";
                PV.RPC("WinOrLose", RpcTarget.All);
                StartCoroutine("GameOut");
                //exitButton.SetActive(true);
            }
        }
    }

    [PunRPC]
    public void WinOrLose()
    {
        //살아있으면 승리
        //victoryPanel.SetActive(true);
        //죽어있으면 패배
        //DefeatPanel.SetActive(true);
    }

    IEnumerator GameOut()
    {
        //시간이 다 되면
        uiManager.SetActive(false);
        yield return new WaitForSeconds(4);
        PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.CurrentRoom.IsVisible = true;
        PhotonNetwork.LoadLevel(0);
    }

    #endregion

    #region 음악
    IEnumerator PlayMusicList(int num)
    {
        audioSource.clip = musicList[num];
        audioSource.Play();
        audioSource.loop = true;
        yield return 0;
    }
    #endregion

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