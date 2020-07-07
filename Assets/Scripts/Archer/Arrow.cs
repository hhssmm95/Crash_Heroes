using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    float speed = 10;

    void Start()
    {
        if (gameObject.tag == "BigArrow")
            speed *= 2;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Warrior") && other.gameObject.layer.ToString() == "Player")
        {
            var enemy = other.GetComponent<CharacterMove>();
            if (gameObject.tag == "BigArrow")
                enemy.OnDamage(30);
            else
                enemy.OnDamage(10);
            Destroy(gameObject);
        }
        else if(!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
