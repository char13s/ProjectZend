using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class AuraControlSlider : MonoBehaviour
{
    public static event UnityAction completed;
    public static event UnityAction defeated;
    public static event UnityAction animate;
    [SerializeField] private Slider bar;
    private float amount;
    [SerializeField] private float decreaseRate;
    private bool done;
    public float Amount { get => amount; set { amount = value; bar.value = value;CheckCompletion();  } }
    public bool Done { get => done; set => done = value; }

    // Start is called before the first frame update
    void Start()
    {
        Done = false;
        bar.maxValue = 100;
        Amount = 45;
        StartCoroutine(DecreaseBar());
    }
    public void StartGame() {
        
    }
    IEnumerator DecreaseBar() {
        YieldInstruction wait = new WaitForSeconds(decreaseRate);
        while (isActiveAndEnabled&&!Done) { 
        yield return wait;
        Amount--;
        }
    }
    private void CheckCompletion() {
        if (Amount == 100) {
            Done = true;
            if(completed!=null)
                completed();
            
        }
        else if (Amount == 0) {
            Done = true;
            if(defeated!=null)
                defeated();
            
        }
        else {
            if (animate != null) {
                animate();
            }
        }
    }
}
