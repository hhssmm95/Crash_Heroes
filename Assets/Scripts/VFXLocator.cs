using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXLocator : MonoBehaviour
{
    GameObject player;
    Transform atk2Pos;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        atk2Pos = GameObject.FindGameObjectWithTag("WarriorAttack2Pos").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.CompareTag("WarriorAttack1"))
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, player.transform.position.z + 0.1f);
        }
        else if(gameObject.CompareTag("WarriorAttack2"))
        {
            transform.position = new Vector3(atk2Pos.position.x, atk2Pos.position.y, atk2Pos.position.z);
        }
        else if(gameObject.CompareTag("WarriorAttck3"))
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.1f, player.transform.position.z);
        }
        
    }
}
