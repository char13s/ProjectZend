using UnityEngine;

public class MainAnimator : MonoBehaviour
{
    Animator anim;
    [SerializeField] Animator[] clothesAnim;
    public Animator Anim { get => anim; set => anim = value; }
    //public Animator ClothesAnim { get => clothesAnim; set => clothesAnim = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Anim = GetComponent<Animator>();
    }
    private void AllAnimatorsPlay() { 
    
    }
    private void SetToAllAnimators() { 
    
    }
}
