using UnityEngine;

public class HairMaterialSwapper : MonoBehaviour
{
    [SerializeField] private Renderer hairRenderer;
    [SerializeField] private Material[] hairMaterials;
    [SerializeField] private int currentMaterialIndex = 0;
    private void Start()
    {
        if (hairRenderer == null || hairMaterials.Length == 0)
        {
            Debug.LogError("HairRenderer or HairMaterials not set up correctly.");
            return;
        }
        UpdateHairMaterial();
    }
    public void NextMaterial()
    {
        currentMaterialIndex = (currentMaterialIndex + 1) % hairMaterials.Length;
        UpdateHairMaterial();
    }
    public void PreviousMaterial()
    {
        currentMaterialIndex = (currentMaterialIndex - 1 + hairMaterials.Length) % hairMaterials.Length;
        UpdateHairMaterial();
    }
    private void UpdateHairMaterial()
    {
        if (hairRenderer != null && hairMaterials.Length > 0)
        {
            hairRenderer.material = hairMaterials[currentMaterialIndex];
        }
    }
    public Material GetCurrentMaterial()
    {
        if (hairMaterials.Length > 0)
        {
            return hairMaterials[currentMaterialIndex];
        }
        return null;
    }
    public int GetCurrentMaterialIndex()
    {
        return currentMaterialIndex;

    }
    public void SetCurrentMaterialIndex(int index)
    {
        if (index >= 0 && index < hairMaterials.Length)
        {
            currentMaterialIndex = index;
            UpdateHairMaterial();
        }
        else
        {
            Debug.LogWarning("Index out of bounds for hair materials.");
        }
    }
    public void SetHairRenderer(Renderer renderer)
    {
        hairRenderer = renderer;
        UpdateHairMaterial();
    }
    public Renderer GetHairRenderer()
    {
        return hairRenderer;

    }
}
