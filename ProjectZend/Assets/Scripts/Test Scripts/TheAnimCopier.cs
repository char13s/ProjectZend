using UnityEngine;

public class TheAnimCopier : MonoBehaviour
{
    [SerializeField] private MainAnimator mainObject;
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate() {
        //AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        //AnimatorClipInfo[] clipInfoMain = mainObject.Anim.GetCurrentAnimatorClipInfo(0);
        AnimatorStateInfo stateInfoMain = mainObject.Anim.GetCurrentAnimatorStateInfo(0);
        //if (clipInfo[0].clip.name != clipInfoMain[0].clip.name) {
        //    
        //   // mainObject.Anim.Play(stateInfoMain.fullPathHash, 0, 0);//stateInfoMain.normalizedTime
        //   // anim.Play(stateInfoMain.fullPathHash, 0, 0);
        //}
        //anim.Update(Time.deltaTime);
        //mainObject.Anim.Update(Time.deltaTime);
        //anim.Play(stateInfoMain.fullPathHash, 0, stateInfoMain.normalizedTime);
    }
}
