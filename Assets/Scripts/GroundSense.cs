using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSense : MonoBehaviour
{
    private GameObject player; //플레이어 오브젝트 
    public bool isGround; //발이 땅에 닿아있다면 true 
    // Start is called before the first frame update
    void Start()
    {
        player = transform.parent.gameObject; //부모 오브젝트 받아옴
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != player)
        {
            isGround = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject != player)
        {
            isGround = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject != player && isGround == false)
            isGround = true;
    }
}
