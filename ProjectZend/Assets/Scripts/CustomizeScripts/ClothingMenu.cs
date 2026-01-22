using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class ClothingMenu : MonoBehaviour
{
    public List<ClothingItem> allTops;
    public GameObject buttonPrefab;
    public Transform gridParent; // The "Options Grid" from your screenshot
    public WearClothes characterFixer;

    void Start() {
        foreach (var item in allTops) {
            GameObject btn = Instantiate(buttonPrefab, gridParent);
            //btn.GetComponent<Image>().sprite = item.icon;
            btn.GetComponent<Button>().onClick.AddListener(() => {
               // characterFixer.AttachClothing(item.clothingPrefab);

                // Handle the skin masking automatically!
                characterFixer.SetSkinMask(item);
            });
        }
    }
}
