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
    private Material skinMaterial;
    private void Start() {
        GetSets();
        //PutOn(clothing1);
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
        // 1. Remove previous clothing
        if (currentTop != null) Destroy(currentTop);

        // 2. Spawn the new clothing
        currentTop = Instantiate(item.clothingPrefab, mainBody.transform);
        currentTop.transform.localPosition = Vector3.zero;
        currentTop.transform.localRotation = Quaternion.identity;

        // 3. Prevent UniVRM scripts from crashing by disabling them on the clone
        MonoBehaviour[] vrmScripts = currentTop.GetComponentsInChildren<MonoBehaviour>();
        foreach (var script in vrmScripts) {
            if (!(script is SkinnedMeshRenderer)) script.enabled = false;
        }

        // 4. Run the bone re-mapping logic
        ApplyBonesToClothing(currentTop);

        // 5. CALL THE MASKING LOGIC
        SetSkinMask(item);
    }
    private void ApplyBonesToClothing(GameObject clothingInstance) {
        SkinnedMeshRenderer bodyRenderer = mainBody.GetComponentInChildren<SkinnedMeshRenderer>();

        // Map master bones into a dictionary for speed
        Dictionary<string, Transform> masterBones = new Dictionary<string, Transform>();
        foreach (Transform b in bodyRenderer.bones) masterBones[b.name] = b;

        // Apply bones to all parts of the clothing (e.g., Hoodie and Strings)
        SkinnedMeshRenderer[] cRenderers = clothingInstance.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var renderer in cRenderers) {
            Transform[] newBones = new Transform[renderer.bones.Length];
            for (int i = 0; i < renderer.bones.Length; i++) {
                string boneName = renderer.bones[i].name;
                newBones[i] = masterBones.ContainsKey(boneName) ? masterBones[boneName] : bodyRenderer.rootBone;
            }
            renderer.bones = newBones;
            renderer.rootBone = bodyRenderer.rootBone;
            renderer.updateWhenOffscreen = true;
        }

        // Hide the hoodie's internal skeleton root to stop it from showing up in the scene
        Transform internalRoot = clothingInstance.transform.Find("Root");
        if (internalRoot) internalRoot.gameObject.SetActive(false);
    }
    private void ChangeColor(Color temp) {
        print("HairSwapButton selected");

        hair.material.color = temp;
       // hairMaterial = hair.material;
       // hair.color = temp;
       // hair.material = hairMaterial;
    }
    void OnSwap(int val) {
        if (currentTop != null) {
            Destroy(currentTop);
        }
        currentTopIndex += val;
        if (currentTopIndex == tops.Count) {
            currentTopIndex = 0;
        }
        currentTop = tops[currentTopIndex];
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
