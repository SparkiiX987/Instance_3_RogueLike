using UnityEngine;

public class OnSleepState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Entr� dans Sleep !");
    }
}
