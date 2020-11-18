using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private NickNameList nickNameList;
    public int playerCount = 0;
    public AudioClip[] musicList;
    AudioSource audioSource;

    [Header("MainPanel")]
    public GameObject MainPanel;
    public InputField NickNameInput;

    [Header("LobbyPanel")]
    public GameObject LobbyPanel;
    public InputField RoomInput;
    public Text WelcomeText;
    public Text LobbyInfoText;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;

    [Header("RoomPanel")]
    public GameObject RoomPanel;
    public Text ListText;
    public Text[] NickNameList;
    public Text RoomInfoText;
    public Text[] ChatText;
    public InputField ChatInput;

    [Header("ETC")]
    public Text StatusText;
    public PhotonView PV;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        nickNameList = GameObject.Find("NickNameList").GetComponent<NickNameList>();
        if(PhotonNetwork.InRoom)
        {
            //방으로 돌아가기
            ShowPanel(RoomPanel);
            StartCoroutine("PlayMusicList", 0);
            RoomUpdate();
        }

        StartCoroutine("PlayMusicList", 0);
    }
    #region 방리스트 갱신
    // ◀버튼 -2 , ▶버튼 -1 , 셀 숫자
    public void MyListClick(int num)
    {
        if (num == -2) --currentPage;
        else if (num == -1) ++currentPage;
        else PhotonNetwork.JoinRoom(myList[multiple + num].Name);
        MyListRenewal();
    }

    private void MyListRenewal()
    {
        // 최대페이지
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // 이전, 다음버튼
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // 페이지에 맞는 리스트 대입
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
                else myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
        }
        MyListRenewal();
    }
    #endregion

    #region 서버연결
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Screen.SetResolution(1920, 1080, false);
    }

    private void Update()
    {
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        LobbyInfoText.text = (PhotonNetwork.CountOfPlayers - PhotonNetwork.CountOfPlayersInRooms) + "로비 / " + PhotonNetwork.CountOfPlayers + "접속";

        if (ChatInput.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            Send();
            //Debug.Log("send via return key");
        }
        //엔터키를 통한 채팅 입력
    }

    public void Connect()
    {
        if (NickNameInput.text == "" || NickNameInput.text.Length > 6) return;
        PhotonNetwork.GameVersion = "0.0.0";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        
    }

    public override void OnJoinedLobby()
    {
        ShowPanel(LobbyPanel);
        StartCoroutine("PlayMusicList", 1);
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        myList.Clear();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ShowPanel(MainPanel);
    }
    #endregion

    #region 방
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Room" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 4}) ;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        StopCoroutine("RoomUpdate");
    }

    public override void OnJoinedRoom()
    {
        ShowPanel(RoomPanel);
        StartCoroutine("PlayMusicList", 2);
        ChatInput.text = "";
        for (int i = 0; i < ChatText.Length; i++)
            ChatText[i].text = "";
        
        if(PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //방을 만든 사람은 0에 자기번호, 참여가능 슬롯 0, 참여불가능 슬롯 -1
            int max = PhotonNetwork.CurrentRoom.MaxPlayers - 1;
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { "0", PhotonNetwork.LocalPlayer.ActorNumber},
                { "1", 0}, {"2", 2 <= max ? 0 : -1}, {"3", 3<=max ? 0 : -1 } });
        }
        else
        {
            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                if(GetRoomTag(i) == 0)
                {
                    SetRoomTag(i, PhotonNetwork.LocalPlayer.ActorNumber);
                    break;
                }
            }
        }
        StartCoroutine("RoomUpdate");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        RoomInput.text = ""; CreateRoom();
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        RoomInput.text = ""; CreateRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //RoomRenewal();
        ChatRPC("<color=yellow>" + newPlayer.NickName + "님이 참가하셨습니다</color>");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //RoomRenewal();
        ChatRPC("<color=yellow>" + otherPlayer.NickName + "님이 퇴장하셨습니다</color>");
    }

    //private void RoomRenewal()
    //{
    //    if(PhotonNetwork.LocalPlayer.IsMasterClient)
    //    {
    //        for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
    //        {
    //            NickNameList[i].text = "";
    //            Debug.Log(i + NickNameList[i].text);
    //        }
    //    }
        
    //    for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
    //    {
    //        if (NickNameList[i].text == "")
    //        {
    //            NickNameList[i].text = PhotonNetwork.PlayerList[i].NickName;
    //            break;
    //        }
    //    }
    //    RoomInfoText.text = PhotonNetwork.CurrentRoom.Name + " / " + PhotonNetwork.CurrentRoom.PlayerCount + "명 / " + "최대 : " + PhotonNetwork.CurrentRoom.MaxPlayers;
    //    playerCount = PhotonNetwork.PlayerList.Length;
    //}

    IEnumerator RoomUpdate()
    {

        while (PhotonNetwork.InRoom)
        {
            yield return new WaitForSeconds(0.2f);
            if (!PhotonNetwork.InRoom) yield break;

            for (int i = 0; i < PhotonNetwork.CurrentRoom.MaxPlayers; i++)
            {
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    if (GetPlayer(i) == null && GetRoomTag(i) > 0)
                    {
                        SetRoomTag(i, 0);
                    }
                }

                if (GetRoomTag(i) > 0)
                {
                    NickNameList[i].text = GetPlayer(i).NickName;
                }
            }
        }
    }
    #endregion

    #region 채팅
    public void Send()
    {
        if(ChatInput.text!="" && ChatInput.text !="\n")
            PV.RPC("ChatRPC", RpcTarget.All, PhotonNetwork.NickName + " : " + ChatInput.text);
        ChatInput.text = "";
    }

    [PunRPC] // RPC는 플레이어가 속해있는 방 모든 인원에게 전달한다
    void ChatRPC(string msg)
    {
        bool isInput = false;
        for (int i = 0; i < ChatText.Length; i++)
            if (ChatText[i].text == "")
            {
                isInput = true;
                ChatText[i].text = msg;
                break;
            }
        if (!isInput) // 꽉차면 한칸씩 위로 올림
        {
            for (int i = 1; i < ChatText.Length; i++) ChatText[i - 1].text = ChatText[i].text;
            ChatText[ChatText.Length - 1].text = msg;
        }
    }
    #endregion

    #region 게임시작

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        { 
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
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

    #region 기타
    void ShowPanel(GameObject showPanel)
    {
        MainPanel.SetActive(false);
        LobbyPanel.SetActive(false);
        RoomPanel.SetActive(false);

        showPanel.SetActive(true);
    }

    void SetRoomTag(int slotIndex, int value)
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { slotIndex.ToString(), value } });
    }

    int GetRoomTag(int slotIndex)
    {
        return (int)PhotonNetwork.CurrentRoom.CustomProperties[slotIndex.ToString()];
    }

    Player GetPlayer(int slotIndex)
    {
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].ActorNumber == GetRoomTag(slotIndex))
                return PhotonNetwork.PlayerList[i];
        }
        return null;
    }
    #endregion
}

