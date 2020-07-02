using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShotSlash : StateMachineBehaviour
{
    public ParticleSystem VFX;
    public Transform shotPos;
    public Transform player;
    bool trigger;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        trigger = false;
    }

    //[PunRPC]
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (stateInfo.normalizedTime >= 0.2f && !trigger)
        //{
        //    trigger = true;
        //    shotPos = GameObject.FindGameObjectWithTag("WarriorSkill3Pos").transform;
        //    player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //    Vector3 dir = player.transform.forward;
        //    //PhotonNetwork.Instantiate("Prefebs/VFX/WarriorSkill3VX", new Vector3(shotPos.transform.position.x, shotPos.transform.position.y, shotPos.transform.position.z), Quaternion.LookRotation(dir) * VFX.transform.rotation)
        //    //    .GetComponent<PhotonView>().RPC("SkillEffect", RpcTarget.All);

        //    Instantiate(VFX, new Vector3(shotPos.transform.position.x, shotPos.transform.position.y, shotPos.transform.position.z), Quaternion.LookRotation(dir) * VFX.transform.rotation);
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
