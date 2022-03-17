using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportBehavior : StateMachineBehaviour
{
    [SerializeField] private Kid kid;
    //[SerializeField] private GroundCheck groundCheck;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        kid = GameObject.FindGameObjectWithTag("Kid").GetComponent<Kid>();
        kid.enabled = false;
        //groundCheck = GameObject.FindGameObjectWithTag("Kid").transform.Find("GroundCheck").GetComponent<GroundCheck>();
        InputManager.playerInput.actions["Jump"].Disable();
        InputManager.playerInput.actions["Teleport"].Disable();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        kid.enabled = true;
        InputManager.playerInput.actions["Jump"].Enable();
        InputManager.playerInput.actions["Teleport"].Enable();
    }

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