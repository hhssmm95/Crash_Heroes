﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{

    public Vector3 itemPosition;
    public float x, y, z;
    public float timer;
    public float length;

    public int count;
    public int map = 1;
    // Start is called before the first frame update
    void Start()
    {
        itemPosition = new Vector3(0,0,0);
        timer = 0.0f;
        count = 0;
        x = y = z = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (count < (int)(timer / 60.0f)&& PhotonNetwork.IsMasterClient)
        {
            GameObject[] potion = GameObject.FindGameObjectsWithTag("potion");
            if(!((int)potion.Length>=3))
                Randomcall();
        }
    }

    [PunRPC]
    void Randomcall()
    {
        count++;
        int i = Random.Range(0, 2);
        int j = Random.Range(0, 2);
        int k = Random.Range(0, 2);
        if (map == 1)
        {

            z = 8.0f;
            if ((int)i != 0)
            {
                x = Random.Range(0, length);
                y = Random.Range(0, (length *length) - (x * x));
                if ((int)j != 0)
                    x *= -1;
                if ((int)k != 0)
                    y *= -1;
            }
            else
            {
                x = Random.Range(-8, -12);
                y = ((x + 8) * -2.25f) + 19;
            }
        }
        else if(map==3)
        {
            x = Random.Range(0, length);
            y = Random.Range(0, Mathf.Sqrt((length * length) - (x * x)));
            if ((int)j != 0)
                x *= -1;
            if ((int)k != 0 && y<5.0f)
                y *= -1;
        }
        else
        {
            x = Random.Range(0, length);
            y = Random.Range(0, Mathf.Sqrt((length * length) - (x * x)));
            if ((int)j != 0)
                x *= -1;
            if ((int)k != 0)
                y *= -1;
        }
        itemPosition = new Vector3(x, z, y);
        switch((int)Random.Range(0, 3))
        {
            case 0:
                PhotonNetwork.Instantiate("Prefebs/Bottle_Mana", itemPosition, Quaternion.Euler(0, 0, 0));
                break;
            case 1:
                PhotonNetwork.Instantiate("Prefebs/Bottle_Health", itemPosition, Quaternion.Euler(0, 0, 0));
                break;
            case 2:
                PhotonNetwork.Instantiate("Prefebs/Bottle_Endurance", itemPosition, Quaternion.Euler(0, 0, 0));
                break;

        }
        
    }
}
