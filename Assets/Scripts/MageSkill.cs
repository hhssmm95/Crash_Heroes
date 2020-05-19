using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill : ClassParent
{
    
    void Start()
    {
        player = gameObject.GetComponent<CharacterMove>();
        playerAnim = gameObject.GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
