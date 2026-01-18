using System.Collections.Generic;
using UnityEngine;

public class WearClothes : MonoBehaviour
{
    [SerializeField] GameObject mainBody;
    [SerializeField] SkinnedMeshRenderer hair;
    //[SerializeField] private SkinnedMeshRenderer playerBody;
    [SerializeField] private GameObject clothing1;
    [SerializeField] private GameObject[] tops;
    private void Start() {
        PutOn(clothing1);
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
    }
    private void ChangeColor(Color temp) {
        print("HairSwapButton selected");

        hair.material.color = temp;
       // hairMaterial = hair.material;
       // hair.color = temp;
       // hair.material = hairMaterial;
    }
}
