using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VFXLocator : MonoBehaviourPunCallbacks, IPunObservable
{
    GameObject Warrior;
    GameObject Archer;
    GameObject Dragoon;
    //GameObject Mage;
    //GameObject[] Player; //0: warrior , 1: archer, 2: dragoon, 3: mage
    Transform wAtk2Pos;
    Transform aAtk2Pos;
    Transform aSkill1Pos;
    Transform dAtk2Pos;
    Transform dSkill1Pos;
    void Start()
    {
        Warrior = GameObject.FindGameObjectWithTag("Warrior");
        Archer = GameObject.FindGameObjectWithTag("Archer");
        Dragoon = GameObject.FindGameObjectWithTag("Dragoon");
        // Warrior = GameObject.FindGameObjectWithTag("Warrior");

        //player = GameObject.FindGameObjectWithTag("Player");
        //var status = player.GetComponent<CharacterMove>();


        wAtk2Pos = GameObject.FindGameObjectWithTag("WarriorAttack2Pos").GetComponent<Transform>();
        aAtk2Pos = GameObject.FindGameObjectWithTag("ArcherAttack2Pos").GetComponent<Transform>();
        aSkill1Pos = GameObject.FindGameObjectWithTag("ArcherSkill1Pos").GetComponent<Transform>();
        dAtk2Pos = GameObject.FindGameObjectWithTag("DragoonAttack2Pos").GetComponent<Transform>();
        dSkill1Pos = GameObject.FindGameObjectWithTag("DragoonSkill1Pos").GetComponent<Transform>();

        Destroy(gameObject, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        photonView.RPC("SkillEffect", RpcTarget.All);
    }

    [PunRPC]
    void SkillEffect()
    {
        if (gameObject.CompareTag("WarriorAttack1"))
        {
            transform.position = new Vector3(Warrior.transform.position.x, Warrior.transform.position.y + 0.5f, Warrior.transform.position.z + 0.1f);
        }
        else if (gameObject.CompareTag("WarriorAttack2"))
        {
            transform.position = new Vector3(wAtk2Pos.position.x, wAtk2Pos.position.y, wAtk2Pos.position.z);
        }
        else if (gameObject.CompareTag("WarriorAttck3"))
        {
            transform.position = new Vector3(Warrior.transform.position.x + 0.2f, Warrior.transform.position.y + 0.15f, Warrior.transform.position.z - 0.1f);
        }
        else if (gameObject.CompareTag("WarriorSkill2_1"))
            transform.position = new Vector3(Warrior.transform.position.x, Warrior.transform.position.y + 0.406f, Warrior.transform.position.z + 0.1f);

        else if (gameObject.CompareTag("WarriorSkill2_2"))
            transform.position = new Vector3(Warrior.transform.position.x, Warrior.transform.position.y + 0.656f, Warrior.transform.position.z + 0.1f);

        else if (gameObject.CompareTag("ArcherVX1"))
            transform.position = new Vector3(aSkill1Pos.position.x, aSkill1Pos.position.y, aSkill1Pos.position.z);

        else if (gameObject.CompareTag("DragoonAttack1"))
        {
            transform.position = new Vector3(Dragoon.transform.position.x, Dragoon.transform.position.y + 0.5f, Dragoon.transform.position.z + 0.1f);
        }
        else if (gameObject.CompareTag("DragoonAttack2"))
        {
            transform.position = new Vector3(dAtk2Pos.position.x, dAtk2Pos.position.y, dAtk2Pos.position.z);
        }
        else if (gameObject.CompareTag("DragoonAttck3"))
        {
            transform.position = new Vector3(Dragoon.transform.position.x + 0.2f, Dragoon.transform.position.y + 0.15f, Dragoon.transform.position.z - 0.1f);
        }
        else if (gameObject.CompareTag("DragoonSkill1"))
        {
            transform.position = new Vector3(dSkill1Pos.position.x, dSkill1Pos.position.y, dSkill1Pos.position.z);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

}
