using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class mageShield : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }
    [PunRPC]
    public void BarriorDest()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
