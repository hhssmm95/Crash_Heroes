using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    
    public bool attacking;
    CharacterMove player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Warrior").GetComponent<CharacterMove>();
    }

    // Update is called once per frame
    void Update()
    {

        if (player.isAttacking == true && !attacking)
            attacking = true;
        else if (player.isAttacking == false && attacking)
            attacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Warrior") && attacking && other.gameObject.layer.ToString() == "Player")
        {
            
            var enemy = other.GetComponent<CharacterMove>();
            enemy.OnDamage(10);
        }
    }
}
