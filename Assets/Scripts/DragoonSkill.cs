using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragoonSkill : ClassParent
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
