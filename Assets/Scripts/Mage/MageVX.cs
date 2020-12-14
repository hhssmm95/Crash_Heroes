using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class MageVX : MonoBehaviour
{
    CharacterMove Mage;
    
    public float damageStart;
    public float damageTime;
    private float timer;
    List<string> dm;
    // Start is called before the first frame update
    private void Awake()
    {
        Mage = GameObject.FindGameObjectWithTag("Mage").GetComponent<CharacterMove>();
    }
    void Start()
    {
        dm = new List<string>();
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }
    
    private void OnTriggerStay(Collider enemy)
    {
        if (damageStart < timer && timer < (damageStart + damageTime) && enemy.gameObject.layer == 9 && enemy.gameObject.tag !="Mage")
        {
            if(!dm.Exists(x=>x==enemy.name))
            {
                dm.Add(enemy.name);
                switch (gameObject.name)
                {
                    case "MageSkill1VX(Clone)":
                        enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Mage.atk * 1.96f, Mage.transform.forward);
                        enemy.GetComponent<PhotonView>().RPC("PlaySE", RpcTarget.All, 4, 0);
                        Debug.Log(gameObject.name + "스킬이 " + enemy.gameObject.name + "에게 " + Mage.atk * 1.96f + "감소 전 피해를 입힘.");
                        break;
                    case "MageSkill2VX(Clone)":
                        enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Mage.atk * 2.45f, Mage.transform.forward);
                        Debug.Log(gameObject.name + "스킬이 " + enemy.gameObject.name + "에게 " + Mage.atk * 2.45f + "감소 전 피해를 입힘.");
                        break;
                    case "MageSkill3VX(Clone)":
                        enemy.GetComponent<PhotonView>().RPC("OnHeavyDamage", RpcTarget.All, Mage.atk * 2.2f, Mage.transform.forward);
                        Debug.Log(gameObject.name + "스킬이 " + enemy.gameObject.name + "에게 " + Mage.atk * 2.2f + "감소 전 피해를 입힘.");
                        break;

                    case "MageAttack1VX(Clone)":
                        enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Mage.atk * 0.8f, Mage.transform.forward);
                        Debug.Log(gameObject.name + "스킬이 " + enemy.gameObject.name + "에게 " + Mage.atk * 0.8f + "감소 전 피해를 입힘.");
                        break;
                    case "MageAttack2VX(Clone)":
                        enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Mage.atk * 0.9f, Mage.transform.forward);
                        Debug.Log(gameObject.name + "스킬이 " + enemy.gameObject.name + "에게 " + Mage.atk * 0.9f + "감소 전 피해를 입힘.");
                        break;
                    case "MageAttack3VX(Clone)":
                        enemy.GetComponent<PhotonView>().RPC("OnDamage", RpcTarget.All, Mage.atk * 1.1f, Mage.transform.forward);
                        Debug.Log(gameObject.name + "스킬이 " + enemy.gameObject.name + "에게 " + Mage.atk * 1.1f + "감소 전 피해를 입힘.");
                        break;
                }
            }
            
        }
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
