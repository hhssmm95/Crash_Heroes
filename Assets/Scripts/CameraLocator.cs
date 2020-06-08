using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraLocator :MonoBehaviourPunCallbacks
{
    public GameObject player;
    private Vector3 offset;
    public bool playerCheck;
    bool initComplete;
    // Start is called before the first frame update
    void Start()
    {
        //player =  GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if(playerCheck && !initComplete)
        {
            offset = transform.position - player.transform.position;
            initComplete = true;
        }

        if(playerCheck && initComplete)
            transform.position = player.transform.position + offset;
    }
}
