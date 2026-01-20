using System.Collections.Generic;
using UnityEngine;

public class WearClothes : MonoBehaviour
{
    [SerializeField] GameObject mainBody;
    [SerializeField] SkinnedMeshRenderer hair;
    //[SerializeField] private SkinnedMeshRenderer playerBody;
    private GameObject clothing1;
    //[SerializeField] private GameObject[] tops;
    private List<GameObject> tops = new List<GameObject>();
    private GameObject currentTop;
    private int currentTopIndex=0;
    [SerializeField] private ClothingItem test;
    private Material skinMaterial;
    public Texture2D masterSkinMask;

    public GameObject CurrentTop { get => currentTop; set { currentTop = value; } }

    private void Awake() {
        Transform bodyMesh = mainBody.transform.Find("Body");
        if (bodyMesh != null) {
            skinMaterial = bodyMesh.GetComponent<SkinnedMeshRenderer>().material;
            // APPLY THE MASK AUTOMATICALLY ON START
            if (masterSkinMask != null) {
                skinMaterial.SetTexture("_MaskMap", masterSkinMask);
            }
        }
    }
    private void Start() {
        GetSets();
        AttachClothing(test);
        //PutOn(test.clothingPrefab);
    }
    private void GetSets() {
        foreach (GameObject go in Resources.LoadAll<GameObject>("Tops Assets")) {
            tops.Add(go);
        }
    }
    public void PutOn(GameObject clothingPrefab) {
        // 1. Instantiate the hoodie
        GameObject instance = Instantiate(clothingPrefab, mainBody.transform);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;

        // 2. DISABLE UniVRM scripts on the CLONE to prevent the error
        // We don't want the hoodie trying to "look at" things or calculate its own physics
        MonoBehaviour[] vrmScripts = instance.GetComponentsInChildren<MonoBehaviour>();
        foreach (var script in vrmScripts) {
            // We disable all scripts that aren't the mesh renderers themselves
            if (!(script is SkinnedMeshRenderer)) {
                script.enabled = false;
            }
        }

        // 3. Map Main Body Bones
        Dictionary<string, Transform> masterBoneMap = new Dictionary<string, Transform>();
        foreach (Transform child in mainBody.GetComponentsInChildren<Transform>()) {
            if (!masterBoneMap.ContainsKey(child.name))
                masterBoneMap.Add(child.name, child);
        }

        // 4. Process the Renderers
        SkinnedMeshRenderer[] targetRenderers = instance.GetComponentsInChildren<SkinnedMeshRenderer>();
        SkinnedMeshRenderer masterRenderer = mainBody.GetComponentInChildren<SkinnedMeshRenderer>();

        foreach (var sRenderer in targetRenderers) {
            Transform[] boneArray = new Transform[sRenderer.bones.Length];
            for (int i = 0; i < sRenderer.bones.Length; i++) {
                string targetBoneName = sRenderer.bones[i].name;
                if (masterBoneMap.TryGetValue(targetBoneName, out Transform foundBone)) {
                    boneArray[i] = foundBone;
                }
                else {
                    boneArray[i] = masterRenderer.rootBone;
                }
            }

            sRenderer.bones = boneArray;
            sRenderer.rootBone = masterRenderer.rootBone;
            sRenderer.updateWhenOffscreen = true;
        }

        // 5. Instead of destroying the Root, we just hide it.
        // This keeps the 'Transform' alive so the UniVRM scripts don't throw errors,
        // but it removes the "ghost" skeleton from your scene view.
        Transform hoodieRoot = instance.transform.Find("Root");
        if (hoodieRoot != null) {
            hoodieRoot.gameObject.SetActive(false);
        }
        //SetSkinMask(clothingPrefab);
    }
    public void AttachClothing(ClothingItem item) {
        if (currentTop != null) Destroy(currentTop);

        currentTop = Instantiate(item.clothingPrefab, mainBody.transform);
        currentTop.transform.localPosition = Vector3.zero;
        currentTop.transform.localRotation = Quaternion.identity;

        // 1. Disable VRM scripts to prevent errors
        foreach (var script in currentTop.GetComponentsInChildren<MonoBehaviour>()) {
            if (!(script is SkinnedMeshRenderer)) script.enabled = false;
        }

        // 2. RUN THE FIXED BONE MAPPING
        ApplyBones(currentTop);

        // 3. Update Masking
        SetSkinMask(item);
    }

    private void ApplyBones(GameObject instance) {
        SkinnedMeshRenderer bodyRenderer = mainBody.GetComponentInChildren<SkinnedMeshRenderer>();

        // Map the Body's bones
        Dictionary<string, Transform> masterBones = new Dictionary<string, Transform>();
        foreach (Transform b in bodyRenderer.bones) masterBones[b.name] = b;

        SkinnedMeshRenderer[] clothingRenderers = instance.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (var cRenderer in clothingRenderers) {
            Transform[] newBones = new Transform[cRenderer.bones.Length];

            for (int i = 0; i < cRenderer.bones.Length; i++) {
                string boneName = cRenderer.bones[i].name;

                if (masterBones.ContainsKey(boneName)) {
                    // If the body has this bone, follow the body
                    newBones[i] = masterBones[boneName];
                }
                else {
                    // FIX: If the body DOESN'T have this bone (like Drawstrings),
                    // keep its original bone so it doesn't stretch to the floor!
                    newBones[i] = cRenderer.bones[i];
                }
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
    void OnSwap(int val) {
        if (CurrentTop != null) {
            Destroy(CurrentTop);
        }
        currentTopIndex += val;
        if (currentTopIndex == tops.Count) {
            currentTopIndex = 0;
        }
        CurrentTop = tops[currentTopIndex];
    }
    public void SetSkinMask(ClothingItem item) {
        if (skinMaterial == null) return;

        // We send 1.0 to HIDE and 0.0 to SHOW
        skinMaterial.SetFloat("_Hide_Arms", item.hideArms ? 1f : 0f);
        skinMaterial.SetFloat("_Hide_Torso", item.hideTorso ? 1f : 0f);
        skinMaterial.SetFloat("_Hide_UpperLegs", item.hideUpperLegs ? 1f : 0f);
        skinMaterial.SetFloat("_Hide_LowerLegs", item.hideLowerLegs ? 1f : 0f);
    }
}
