using UnityEngine;

public class MainAnimator : MonoBehaviour
{
    Animator anim;

    public Animator Anim { get => anim; set => anim = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Anim = GetComponent<Animator>();
    }
}
