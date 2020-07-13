using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NickNameList : MonoBehaviour
{
    private static NickNameList instance;
    public GameObject nickNameList;
    private NetworkManager networkManager;
    public string[] NameList;
    public string myNickName;
    public int playerCount = 0;
    public string roomName;

    public bool isStart = false;
    private void Start()
    {
        DontDestroyOnLoad(nickNameList);
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    public static NickNameList Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<NickNameList>();

                if (obj != null)
                {
                    instance = obj;
                }

                else
                {
                    var newSingleton = new GameObject("Singleton Class").AddComponent<NickNameList>();

                    instance = newSingleton;
                }
            }
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    private void Awake()
    {

        var objs = FindObjectsOfType<NickNameList>();

        if (objs.Length != 1)
        {
            Destroy(gameObject);

            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        playerCount = networkManager.playerCount;
        for (int i = 0; i < networkManager.NickNameList.Length; i++)
        {
            NameList[i] = networkManager.NickNameList[i].text;
            myNickName = networkManager.NickNameInput.text;
            roomName = networkManager.RoomInput.text;
        }
    }
}
