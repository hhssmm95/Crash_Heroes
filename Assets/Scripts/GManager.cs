using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public Transform[] spawn_point;
    public PhotonView PV;
    public CharacterMove characterMove;
    public AudioClip[] musicList;
    AudioSource audioSource;

    public bool[] deadList;

    public GameObject player;

    public GameObject characterPanel;
    public GameObject knightImage;
    public GameObject archerImage;
    public GameObject dragoonImage;
    public GameObject mageImage;
    public GameObject lenaImage;
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
    public bool lenaPick = false;
    public bool isSpawn = false;

    public int playerNum = 0;
    public bool isPick;
    public bool isTimeOver = false;
    private void Start()
    {
        text_Time = GameObject.FindWithTag("Timer").GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
        
        StartCoroutine("PlayMusicList", 0);

        //픽 선택, 죽음판정 초기화
        SetLocalTag("isPick", false);
        SetLocalTag("isDie", false);

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.LocalPlayer.NickName)
            {
                playerNum = i;
                break;
            }
        }
        
        //방장이 캐릭터 선택창 RPC로 실행
        if (!PhotonNetwork.IsMasterClient) return;
        PV.RPC("ShowCharacterPanelRPC", RpcTarget.All);
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //플레이어들의 캐릭터 선택 확인
            if (!AllPickCheak()) return;

            //캐릭터 선택창 숨기기
            PV.RPC("HideCharacterPanelRPC", RpcTarget.All);

            if (!isSpawn)
            {
                PV.RPC("SpawnRPC", RpcTarget.All);
            }

            //타이머가 끝나면 멈춤
            if (isTimeOver == false)
            {
                Timer();
            }

            int count = 0;

            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if ((bool)PhotonNetwork.PlayerList[i].CustomProperties["isDie"])
                {
                    count++;
                }
            }

            //한명이 살아남았거나 남은 시간이 없을때
            //if ((count == PhotonNetwork.PlayerList.Length - 1) || isTimeOver)
            //{
            //    PV.RPC("WinOrLose", RpcTarget.All);
            //    StartCoroutine("GameOut");
            //    //exitButton.SetActive(true);
            //}
        }

        //각자 실행하는 것이며 죽었을때 실행
        if (player.GetComponent<CharacterMove>().isDead == true)
        {
            SetLocalTag("isDie", true);
        }
    }

    #region 캐릭터 선택
    [PunRPC]
    void ShowCharacterPanelRPC()
    {
        characterPanel.SetActive(true);
    }

    [PunRPC]
    void HideCharacterPanelRPC()
    {
        characterPanel.SetActive(false);
    }

    public bool AllPickCheak()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (!(bool)PhotonNetwork.PlayerList[i].CustomProperties["isPick"])
            {
                return false;
            }
        }
        return true;
    }

    public void Pick(int num)
    {
        if (!(bool)PhotonNetwork.LocalPlayer.CustomProperties["isPick"])
        {
            PV.RPC("CharacterPick", RpcTarget.All, num);
            SetLocalTag("isPick", true);
        }
    }

    public void PickName(string name)
    {
        pickName = name;
    }

    [PunRPC]
    public void CharacterPick(int pickNum)
    {
        if (pickNum == 1)
        {
            //pickName = "Prefebs/Knight";
            //knightPick = true;
            knightImage.transform.GetChild(0).gameObject.SetActive(false);
            knightImage.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (pickNum == 2)
        {
            //pickName = "Prefebs/Archer";
            //archerPick = true;
            archerImage.transform.GetChild(0).gameObject.SetActive(false);
            archerImage.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (pickNum == 3)
        {
            //pickName = "Prefebs/Dragoon";
            //dragoonPick = true;
            dragoonImage.transform.GetChild(0).gameObject.SetActive(false);
            dragoonImage.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (pickNum == 4)
        {
            //pickName = "Prefebs/Mage";
            //magePick = true;
            mageImage.transform.GetChild(0).gameObject.SetActive(false);
            mageImage.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (pickNum == 5)
        {
            //pickName = "Prefebs/Lena";
            //lenaPick = true;
            lenaImage.transform.GetChild(0).gameObject.SetActive(false);
            lenaImage.transform.GetChild(2).gameObject.SetActive(true);
        }
    }
    #endregion

    #region 캐릭터 생성
    [PunRPC]
    public void SpawnRPC()
    {
        player = PhotonNetwork.Instantiate(pickName, spawn_point[playerNum].position, spawn_point[playerNum].rotation) as GameObject;
        isSpawn = true;
    }

    
    #endregion

    #region 타이머

    //준비시간
    [PunRPC]
    void CountDownRPC()
    {

    }

    public void Timer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            LimitTime -= Time.deltaTime;
            text_Time.text = Mathf.Round(LimitTime).ToString();
            if (LimitTime < 0)
            {
                isTimeOver = true;
                LimitTime = 0;
                text_Time.text = "0";
            }
        }
    }
    #endregion

    [PunRPC]
    private void WinOrLose()
    {
        //살아있으면 승리
        if (player.GetComponent<CharacterMove>().isDead == false)
        {
            victoryPanel.SetActive(true);
        }
        //죽어있으면 패배
        if(player.GetComponent<CharacterMove>().isDead == true)
        { 
            DefeatPanel.SetActive(true);
        }
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


    #region 음악
    IEnumerator PlayMusicList(int num)
    {
        audioSource.clip = musicList[num];
        audioSource.Play();
        audioSource.loop = true;
        yield return 0;
    }
    #endregion

    void SetLocalTag(string key, bool cheak)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { key, cheak } });
    }

    bool GetLocalTag(string key)
    {
        return (bool)PhotonNetwork.LocalPlayer.CustomProperties[key];
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        //{
        //    if (stream.IsWriting)
        //    {
        //        stream.SendNext(pickList[i]);
        //        stream.SendNext(deadList[i]);
        //        stream.SendNext(player.GetComponent<CharacterMove>().isDead);
        //    }
        //    else
        //    {
        //        pickList[i] = (bool)stream.ReceiveNext();
        //        deadList[i] = (bool)stream.ReceiveNext();
        //        player.GetComponent<CharacterMove>().isDead = (bool)stream.ReceiveNext();
        //    }
        //}

        if (stream.IsWriting) stream.SendNext(text_Time.text);
        else text_Time.text = (string)stream.ReceiveNext();
    }
}