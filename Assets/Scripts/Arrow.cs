using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    float speed = 10;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<CharacterMove>();
            enemy.OnDamage(10);
            Destroy(gameObject);
        }
        else if(!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
