﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresidentWaveSmb : StateMachineBehaviour {
    private PresidentController _presidentControllerScript;
    
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	    if (_presidentControllerScript == null)
	    {
            _presidentControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PresidentController>();
        }
        _presidentControllerScript.DecreaseStamina();
        _presidentControllerScript.ActivateConeForHandwave();
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _presidentControllerScript.DeactivateConeForHandwave();
    }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
