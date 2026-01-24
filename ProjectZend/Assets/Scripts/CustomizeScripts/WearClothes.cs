using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WearClothes : MonoBehaviour
{
    [SerializeField] GameObject mainBody;
    [SerializeField] SkinnedMeshRenderer hair;
    //[SerializeField] private SkinnedMeshRenderer playerBody;
    //[SerializeField] private GameObject[] tops;
    private List<ClothingItem> tops = new List<ClothingItem>();
    private List<ClothingItem> bottoms = new List<ClothingItem>();
    private List<ClothingItem> shoes = new List<ClothingItem>();

    private GameObject currentTop;
    private GameObject currentBottom;
    private GameObject currentShoes;
    // Track position for each category
    private int topIndex = 0;
    private int bottomIndex = 0;
    private int shoeIndex = 0;
    //[SerializeField] private ClothingItem top;
    //[SerializeField] private ClothingItem bottom;
    //[SerializeField] private ClothingItem shoe;
    private Material skinMaterial;
    public Texture2D masterSkinMask;
    //Ui slots
    [SerializeField]private TextMeshProUGUI textTop;
    [SerializeField]private TextMeshProUGUI textBottoms;
    [SerializeField]private TextMeshProUGUI textShoes;
    public GameObject CurrentTop { get => currentTop; set { currentTop = value; } }

    private void Awake() {
        Transform bodyMesh = mainBody.transform.Find("Body");
        if (bodyMesh != null) {
            // Use .material to create a session-only instance
            skinMaterial = bodyMesh.GetComponent<SkinnedMeshRenderer>().material;

            // Ensure the character is visible when the game starts
            ResetSkinMask();

            if (masterSkinMask != null) {
                skinMaterial.SetTexture("_MaskMap", masterSkinMask);
            }
        }
    }

    private void Start() {

        GetSets();
        //AttachClothing(top);
        //AttachClothing(bottom);
        //AttachClothing(shoe);
        //PutOn(test.clothingPrefab);
        if (tops.Count > 0) AttachClothing(tops[0]);
        if (bottoms.Count > 0) AttachClothing(bottoms[0]);
        if (shoes.Count > 0) AttachClothing(shoes[0]);
    }
    private void GetSets() {
        tops.AddRange(Resources.LoadAll<ClothingItem>("Tops Assets"));
        bottoms.AddRange(Resources.LoadAll<ClothingItem>("Bottoms Assets"));
        shoes.AddRange(Resources.LoadAll<ClothingItem>("Shoes Assets"));
    }

    public void AttachClothing(ClothingItem item) {
        // 1. Identify slot
        if (item.itemType == ClothingType.Top) {
            if (currentTop != null) DestroyImmediate(currentTop); // Use DestroyImmediate for wardrobe logic
        }
        else if (item.itemType == ClothingType.Bottom) {
            if (currentBottom != null) DestroyImmediate(currentBottom);
        }
        else if (item.itemType == ClothingType.Shoes) {
            if (currentShoes != null) DestroyImmediate(currentShoes);
        }

        // 2. Spawn the new one
        GameObject newClothing = Instantiate(item.clothingPrefab, mainBody.transform);

        // 3. Link the Data BEFORE doing anything else
        var holder = newClothing.AddComponent<ClothingDataHolder>();
        holder.itemData = item;

        // 4. Assign the slot
        if (item.itemType == ClothingType.Top) currentTop = newClothing;
        else if (item.itemType == ClothingType.Bottom) currentBottom = newClothing;
        else if (item.itemType == ClothingType.Shoes) currentShoes = newClothing;

        // 5. Physics and Bone Mapping
        ApplyBonesFixed(newClothing);

        // 6. NOW update the mask
        UpdateBodyMask();
    }

    private void ApplyBonesFixed(GameObject instance) {
        SkinnedMeshRenderer bodyRenderer = mainBody.GetComponentInChildren<SkinnedMeshRenderer>();
        Dictionary<string, Transform> masterBones = new Dictionary<string, Transform>();
        foreach (Transform b in bodyRenderer.bones) masterBones[b.name] = b;

        SkinnedMeshRenderer[] clothingRenderers = instance.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (var cRenderer in clothingRenderers) {
            Transform[] newBones = new Transform[cRenderer.bones.Length];
            for (int i = 0; i < cRenderer.bones.Length; i++) {
                string boneName = cRenderer.bones[i].name;
                // If body has it, use body bone. If not (Drawstrings), keep original.
                newBones[i] = masterBones.ContainsKey(boneName) ? masterBones[boneName] : cRenderer.bones[i];
            }
            cRenderer.bones = newBones;
            cRenderer.rootBone = bodyRenderer.rootBone;
            cRenderer.updateWhenOffscreen = true;
        }
    }

    private void ChangeColor(Color temp) {
        print("HairSwapButton selected");

        hair.material.color = temp;
       // hairMaterial = hair.material;
       // hair.color = temp;
       // hair.material = hairMaterial;
    }

    public void SwapClothing(ClothingType type, int direction) {
        if (type == ClothingType.Top && tops.Count > 0) {
            topIndex = (topIndex + direction + tops.Count) % tops.Count;
            AttachClothing(tops[topIndex]);
            textTop.text = tops[topIndex].itemName;
        }
        else if (type == ClothingType.Bottom && bottoms.Count > 0) {
            bottomIndex = (bottomIndex + direction + bottoms.Count) % bottoms.Count;
            AttachClothing(bottoms[bottomIndex]);
            textBottoms.text = bottoms[bottomIndex].itemName;
        }
        else if (type == ClothingType.Shoes && shoes.Count > 0) {
            shoeIndex = (shoeIndex + direction + shoes.Count) % shoes.Count;
            AttachClothing(shoes[shoeIndex]);
            textShoes.text = shoes[shoeIndex].itemName;
        }
    }

    public void SetSkinMask(ClothingItem item) {
        if (skinMaterial == null) return;
        Debug.Log($"Applying Mask Texture: {masterSkinMask.name}");
        skinMaterial.SetTexture("_MaskMap", masterSkinMask);
        // We send 1.0 to HIDE and 0.0 to SHOW
        skinMaterial.SetFloat("_Hide_Arms", item.hideArms ? 1f : 0f);
        skinMaterial.SetFloat("_Hide_Torso", item.hideTorso ? 1f : 0f);
        skinMaterial.SetFloat("_Hide_UpperLegs", item.hideUpperLegs ? 1f : 0f);
        skinMaterial.SetFloat("_Hide_LowerLegs", item.hideLowerLegs ? 1f : 0f);
    }

    public void UpdateBodyMask() {
        // Default: Everything is visible (0)
        float hArms = 0, hTorso = 0, hULegs = 0, hLLegs = 0;

        // We check every slot. If an item exists, it adds its "Hide" requests to the total.
        ProcessSlot(currentTop, ref hArms, ref hTorso, ref hULegs, ref hLLegs);
        ProcessSlot(currentBottom, ref hArms, ref hTorso, ref hULegs, ref hLLegs);
        ProcessSlot(currentShoes, ref hArms, ref hTorso, ref hULegs, ref hLLegs);

        // Send the combined totals to the shader
        // Use the variable we already grabbed in Awake!
        if (skinMaterial != null) {
            skinMaterial.SetFloat("_Hide_Arms", hArms);
            skinMaterial.SetFloat("_Hide_Torso", hTorso);
            skinMaterial.SetFloat("_Hide_UpperLegs", hULegs);
            skinMaterial.SetFloat("_Hide_LowerLegs", hLLegs);
        }
        print(hArms+" "+hTorso + " " +hULegs + " " +hLLegs);
    }

    void ProcessSlot(GameObject obj, ref float a, ref float t, ref float u, ref float l) {
        if (obj == null) return;

        ClothingDataHolder holder = obj.GetComponent<ClothingDataHolder>();

        if (holder != null && holder.itemData != null) {
            // Use '||=' logic (if it was already 1, keep it 1)
            if (holder.itemData.hideArms) a = 1;
            if (holder.itemData.hideTorso) t = 1;
            if (holder.itemData.hideUpperLegs) u = 1;
            if (holder.itemData.hideLowerLegs) l = 1;
        }
    }

    // These are "Wrapper" functions for your UI buttons
    public void NextTop() => SwapClothing(ClothingType.Top, 1);
    public void PrevTop() => SwapClothing(ClothingType.Top, -1);

    public void NextBottom() => SwapClothing(ClothingType.Bottom, 1);
    public void PrevBottom() => SwapClothing(ClothingType.Bottom, -1);

    public void NextShoes() => SwapClothing(ClothingType.Shoes, 1);
    public void PrevShoes() => SwapClothing(ClothingType.Shoes, -1);

    public void SaveOutfit() {
        PlayerPrefs.SetInt("SavedTop", topIndex);
        PlayerPrefs.SetInt("SavedBottom", bottomIndex);
        PlayerPrefs.SetInt("SavedShoe", shoeIndex);
        PlayerPrefs.Save();
        Debug.Log("Outfit Saved!");
    }

    public void LoadOutfit() {
        topIndex = PlayerPrefs.GetInt("SavedTop", 0); // Default to 0 if none saved
        bottomIndex = PlayerPrefs.GetInt("SavedBottom", 0);
        shoeIndex = PlayerPrefs.GetInt("SavedShoe", 0);

        // Apply the loaded indices
        if (tops.Count > topIndex) AttachClothing(tops[topIndex]);
        if (bottoms.Count > bottomIndex) AttachClothing(bottoms[bottomIndex]);
        if (shoes.Count > shoeIndex) AttachClothing(shoes[shoeIndex]);
    }

    // Create this helper method to reuse whenever you need a clean slate
    public void ResetSkinMask() {
        if (skinMaterial != null) {
            skinMaterial.SetFloat("_Hide_Arms", 0f);
            skinMaterial.SetFloat("_Hide_Torso", 0f);
            skinMaterial.SetFloat("_Hide_UpperLegs", 0f);
            skinMaterial.SetFloat("_Hide_LowerLegs", 0f);
        }
    }

    private void OnApplicationQuit() {
        // This ensures that when you return to the Editor, 
        // your character isn't still missing their torso.
        ResetSkinMask();
    }
}
