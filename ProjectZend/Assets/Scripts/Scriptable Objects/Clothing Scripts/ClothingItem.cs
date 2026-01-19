using UnityEngine;
[CreateAssetMenu(fileName = "NewClothing", menuName = "Customization/ClothingItem")]
public class ClothingItem : ScriptableObject
{
    public string itemName;
    public GameObject clothingPrefab;
    //public Sprite icon;

    [Header("Skin Masking Toggles")]
    public bool hideArms;
    public bool hideTorso;
    public bool hideUpperLegs;
    public bool hideLowerLegs;
}