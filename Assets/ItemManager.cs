using System.Collections;
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
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(count < (int)(timer/60.0f))
        {
            count++;
            randomcall();
            itemPosition = new Vector3(x, z, y);
            PhotonNetwork.Instantiate("Prefebs/Bottle_Mana", itemPosition, Quaternion.Euler(0, 0, 0));
        }
    }

    void randomcall()
    {
        if (map == 1)
        {
            int i = Random.Range(0, 2);
            int j = Random.Range(0, 2);
            int k = Random.Range(0, 2);
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
        else
        {
            x = Random.Range(0, length);
            y = Random.Range(0, (length * length) - (x * x));
            if ((int)j != 0)
                x *= -1;
            if ((int)k != 0 && y<5.0f)
                y *= -1;
        }
    }
}
