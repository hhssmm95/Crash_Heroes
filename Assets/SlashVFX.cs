using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashVFX : MonoBehaviour
{
    private Transform player;
    public float speed = 4;
    private Vector3 dir;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        dir = player.transform.forward;
        Destroy(gameObject, 3);
        transform.rotation = Quaternion.LookRotation(dir);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<CharacterMove>().OnDamage(5);
        }
    }
}
