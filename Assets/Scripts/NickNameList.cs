using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NickNameList : MonoBehaviour
{
    public GameObject nickNameList;
    private NetworkManager networkManager;
    public string[] NameList;
    public string myNickName;

    public bool isStart = false;
    private void Start()
    {
        DontDestroyOnLoad(nickNameList);
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }
    private void Update()
    {
        for (int i = 0; i < networkManager.NickNameList.Length; i++)
        {
            NameList[i] = networkManager.NickNameList[i].text;
            myNickName = networkManager.NickNameInput.text;
        }
    }
}
