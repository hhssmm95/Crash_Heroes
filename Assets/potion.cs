using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class potion : MonoBehaviour
{
    public int type, amount;
    public GameObject eff, bottle;
    private float timer;
    // type 1=HP, 2=MP
    // amount : 회복량
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Transform>().Rotate(0, 90.0f * Time.deltaTime, 0);
        timer += Time.deltaTime;
        if(timer>5.0f && eff.activeInHierarchy)
        {
            eff.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==9)
        {
            other.GetComponent<PhotonView>().RPC("GetItem", RpcTarget.All,type,amount);
            this.GetComponent<PhotonView>().RPC("RemovePotion", RpcTarget.All);
        }
    }

    [PunRPC]
    void RemovePotion()
    {
        Destroy(gameObject);
    }
}
