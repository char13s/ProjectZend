using UnityEngine;

public class SendToClone : StateMachineBehaviour
{
    [SerializeField] Animator bitch;
    private void Awake() {
        
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //stateInfo.
        //animator.gameObject.GetComponent<MainAnimator>().ClothesAnim.Play(stateInfo.fullPathHash,0,0);
    }
}
