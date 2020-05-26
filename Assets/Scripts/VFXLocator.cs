using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXLocator : MonoBehaviour
{
    GameObject player;
    Transform atk2Pos;
    Transform skill1Pos;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        var status = player.GetComponent<CharacterMove>();
        if (status.job == Global.Classes.Warrior)
            atk2Pos = GameObject.FindGameObjectWithTag("WarriorAttack2Pos").GetComponent<Transform>();
        else if (status.job == Global.Classes.Archer)
        {
            atk2Pos = GameObject.FindGameObjectWithTag("ArcherAttack2Pos").GetComponent<Transform>();
            skill1Pos = GameObject.FindGameObjectWithTag("ArcherSkill1Pos").GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.CompareTag("WarriorAttack1"))
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, player.transform.position.z + 0.1f);
        }
        else if (gameObject.CompareTag("WarriorAttack2"))
        {
            transform.position = new Vector3(atk2Pos.position.x, atk2Pos.position.y, atk2Pos.position.z);
        }
        else if (gameObject.CompareTag("WarriorAttck3"))
        {
            transform.position = new Vector3(player.transform.position.x+0.2f, player.transform.position.y + 0.15f, player.transform.position.z-0.1f);
        }
        else if(gameObject.CompareTag("WarriorSkill2_1"))
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.406f, player.transform.position.z+0.1f);

        else if (gameObject.CompareTag("WarriorSkill2_2"))
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.656f, player.transform.position.z+0.1f);
        else if (gameObject.CompareTag("ArcherVX1"))
            transform.position = new Vector3(skill1Pos.position.x, skill1Pos.position.y, skill1Pos.position.z);
    }

    
}
