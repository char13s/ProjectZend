using UnityEngine;

using UnityEngine.Events;
public class Dash : StateMachineBehaviour {
    [SerializeField] private GameObject burst;
    [SerializeField] private GameObject reminant;
    [SerializeField] private bool freefall;
    [SerializeField] private float move;


    private AudioClip sound;
    public static event UnityAction<AudioClip> dash;
    public static event UnityAction dashu;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (dashu != null) {
            dashu();
        }

       
        if (dash != null) {
            dash(sound);
        }
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        
    }

}
