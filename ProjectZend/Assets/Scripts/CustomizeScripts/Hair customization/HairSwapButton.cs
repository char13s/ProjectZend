using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class HairSwapButton : MonoBehaviour, ISelectHandler
{
[SerializeField] private Color hairColor;
    [SerializeField] private SkinnedMeshRenderer hairRenderer;
    [SerializeField] private Material hairMaterial;
    private void ChangeColor(Color temp) {
        print("HairSwapButton selected"); 

        hairRenderer.material.color = temp;
        hairMaterial = hairRenderer.material;
        hairMaterial.color = temp;
        hairRenderer.material = hairMaterial;
    }

    public void OnSelect(BaseEventData eventData)
    {
        ChangeColor(hairColor);
    }
}
