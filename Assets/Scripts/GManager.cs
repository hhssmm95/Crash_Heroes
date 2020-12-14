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
    public bool isAllPick = false;
    public int playerNum = 0;
    public bool isTimeOver = false;
    public bool isGameOver;
    public int GameMode;
    public int killCount = 0;
    private void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        //GameMode가 1이면 배틀로얄, 2라면 데스 매치
        GameMode = (int)PhotonNetwork.LocalPlayer.CustomProperties["mode"];

        text_Time = GameObject.FindWithTag("Timer").GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();

        StartCoroutine("PlayMusicList", 0);

        //픽 선택, 죽음 판정 초기화
        //SetLocalTag("isPick", false);
        SetLocalTag("isDie", false);

        //자신의 번호찾기(스폰 위치 정할때 사용)
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName == PhotonNetwork.LocalPlayer.NickName)
            {
                playerNum = i;
                break;
            }
        }

        if (GameMode == 2)
        {
            if (PhotonNetwork.PlayerList.Length == 2)
            {
                killCount = 5;
            }
            if (PhotonNetwork.PlayerList.Length > 2)
            {
                killCount = 10;
            }
        }

        //방장이 캐릭터 선택창 RPC로 실행
        if (!PhotonNetwork.IsMasterClient) return;
        PV.RPC("ShowCharacterPanelRPC", RpcTarget.All);
        StartCoroutine("LeaveGame");
    }

    private void Update()
    {
        #region 배틀로얄
        if (GameMode == 1)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //플레이어들의 캐릭터 선택 확인
                if (characterPanel.activeSelf)
                {
                    if (!AllPickCheak())
                        return;
                    else if (AllPickCheak())
                    {
                        //캐릭터 선택창 숨기기
                        PV.RPC("HideCharacterPanelRPC", RpcTarget.All);
                    }
                }

                //캐릭터 스폰
                if (!isSpawn)
                {
                    PV.RPC("SpawnRPC", RpcTarget.All);
                }

                LimitTime -= Time.deltaTime;
                text_Time.text = Mathf.Round(LimitTime).ToString();
                if (LimitTime < 0)
                {
                    LimitTime = 0;
                    text_Time.text = "0";
                    isTimeOver = true;
                }

                //죽어있는 플레이어의 수를 센다.
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
                //    isGameOver = true;
                //}
            }

            //각자 실행하는 것이며 죽었을때 실행
            if (player == null) return;

            if (player.GetComponent<CharacterMove>().isDead == true)
            {
                SetLocalTag("isDie", true);
            }
        }
        #endregion
        #region 데스매치
        else if (GameMode == 2)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //플레이어들의 캐릭터 선택 확인
                if (characterPanel.activeSelf)
                {
                    if (!AllPickCheak())
                        return;
                    else if (AllPickCheak())
                    {
                        //캐릭터 선택창 숨기기
                        PV.RPC("HideCharacterPanelRPC", RpcTarget.All);
                    }
                }

                //캐릭터 스폰
                if (!isSpawn)
                {
                    PV.RPC("SpawnRPC", RpcTarget.All);
                }

                LimitTime -= Time.deltaTime;
                text_Time.text = Mathf.Round(LimitTime).ToString();
                if (LimitTime < 0)
                {
                    LimitTime = 0;
                    text_Time.text = "0";
                    isTimeOver = true;
                }

                for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    if((int)PhotonNetwork.PlayerList[i].CustomProperties["KillCount"] == killCount)
                    {
                        DeathMatchEnd();
                    }
                    else
                    {
                        print(i + "번째 " + PhotonNetwork.PlayerList[i].NickName + "은/는 " +(int)PhotonNetwork.PlayerList[i].CustomProperties["KillCount"] + "명 잡았습니다.");
                    }
                }
            }

            if(player.GetComponent<CharacterMove>().isDead)
            {
                StartCoroutine("ReSpawn");
            }
        }
        #endregion
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

    //방장만 모든 플레이어의 캐릭터 선택 여부를 확인
    public bool AllPickCheak()
    {
        if (characterPanel.activeSelf)
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
        return false;
    }

    //캐릭터 픽
    public void Pick(int num)
    {
        if (!(bool)PhotonNetwork.LocalPlayer.CustomProperties["isPick"])
        {
            PV.RPC("CharacterPick", RpcTarget.All, num);
            SetLocalTag("isPick", true);
        }
    }

    // 캐릭터 이름확인
    public void PickName(string name)
    {
        pickName = name;
    }

    //캐릭터를 픽하면 선택창의 캐릭터 비활성
    [PunRPC]
    public void CharacterPick(int pickNum)
    {
        if (pickNum == 1)
        {
            knightImage.transform.GetChild(0).gameObject.SetActive(false);
            knightImage.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (pickNum == 2)
        {
            archerImage.transform.GetChild(0).gameObject.SetActive(false);
            archerImage.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (pickNum == 3)
        {
            dragoonImage.transform.GetChild(0).gameObject.SetActive(false);
            dragoonImage.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (pickNum == 4)
        {
            mageImage.transform.GetChild(0).gameObject.SetActive(false);
            mageImage.transform.GetChild(2).gameObject.SetActive(true);
        }

        if (pickNum == 5)
        {
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
        characterMove = player.GetComponent<CharacterMove>();
    }

    IEnumerator ReSpawn()
    {
        yield return new WaitForSeconds(2.0f);
        PhotonNetwork.Destroy(player);
        yield return new WaitForSeconds(1.0f);
        player = PhotonNetwork.Instantiate(pickName, spawn_point[playerNum].position, spawn_point[playerNum].rotation) as GameObject;
        characterMove = player.GetComponent<CharacterMove>();
    }
    #endregion

    #region 타이머
    public void Timer()
    {

    }
    #endregion

    #region 게임끝
    [PunRPC]
    private void WinOrLose()
    {
        if (player != null)
        {
            //살아있으면 승리
            if (player.GetComponent<CharacterMove>().isDead == false)
            {
                victoryPanel.SetActive(true);
            }
            //죽어있으면 패배
            else if (player.GetComponent<CharacterMove>().isDead == true)
            {
                DefeatPanel.SetActive(true);
            }

            SetLocalTag("isPick", false);
            SetLocalTag("isDie", false);
            Destroy(player);
        }
    }

    [PunRPC]
    private void DeathMatchEnd()
    {
        if((int)PhotonNetwork.LocalPlayer.CustomProperties["KillCount"] == killCount)
        {
            victoryPanel.SetActive(true);
        }
        else
        {
            DefeatPanel.SetActive(true);
        }
    }

    IEnumerator LeaveGame()
    {
        yield return new WaitUntil(() => isGameOver);
        print("게임 끝....");
        //시간이 다 되면
        yield return new WaitForSeconds(4);
        if (PhotonNetwork.IsMasterClient)
        {
            print("게임 종료 중... 방으로 돌아간다");
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(0);
        }
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

    void SetLocalTag(string key, bool cheak)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { key, cheak } });
    }
    void SetLocalTag(string key, int Count)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { key, Count } });
    }

    bool GetLocalTag(string key)
    {
        return (bool)PhotonNetwork.LocalPlayer.CustomProperties[key];
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //방장이 타이머를 재고 시간을 동기화
        if (stream.IsWriting) stream.SendNext(text_Time.text);
        else text_Time.text = (string)stream.ReceiveNext();
    }
}