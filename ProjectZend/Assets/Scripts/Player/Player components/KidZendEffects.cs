using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidZendEffects : MonoBehaviour
{


    [SerializeField] private GameObject darkSlash;
    [SerializeField] private GameObject azaLight;
    [SerializeField] private GameObject lightSwordShot;
    [SerializeField] private GameObject fader;
    [Header("Hair States")]
    [SerializeField] private GameObject superHair;
    [SerializeField] private GameObject normalHair;
    [SerializeField] private GameObject UIHair;
    [SerializeField] private GameObject AEHair;
    [SerializeField] private GameObject DeMHair;
    [SerializeField] private GameObject UIBox;
    [Header("Auras")]
    [SerializeField] private GameObject superAura;
    [SerializeField] private GameObject dbzAura;
    [SerializeField] private GameObject chargingAura;
    [SerializeField] private GameObject auraBurst;
    [SerializeField] private GameObject auraImpactBurst;
    [SerializeField] private GameObject afterImage;
    [SerializeField] private GameObject swordAura;
    [SerializeField] private GameObject auraFistR;
    [SerializeField] private GameObject auraFistL;
    

    [Header("Body Points")]
    [SerializeField] private GameObject point;
    [SerializeField] private GameObject centerPoint;
    [SerializeField] private GameObject originPoint;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private SkinnedMeshRenderer skin;
    [Header("Sword Points")]
    [SerializeField] private GameObject swordTip;

    [Header("Materials to swap")]
    [SerializeField] private Material baseIris;
    [SerializeField] private Material baseEyeBalls;
    [SerializeField] private Material demonIris;
    [SerializeField] private Material demonEyeBalls;
    [SerializeField] private Material baseFace;
    [SerializeField] private Material demonFace;
    private GameObject currentHair;
    private NewZend player;

    private void Start() {
        //ExperimentalInputs.shootSword += SummonLightSword;
        player = GetComponent<NewZend>();
        //TransformationTimeline.switchEyes += SwitchEyes;
        //CurrentHair = normalHair;
    }
    public GameObject Point { get => point; set => point = value; }
    public GameObject CurrentHair { get => currentHair; set { currentHair = value;ClearExtraEffects(); } }

    public GameObject Fader { get => fader; set => fader = value; }
    public GameObject RightHand { get => rightHand; set => rightHand = value; }
    public GameObject ChargingAura { get => chargingAura; set => chargingAura = value; }
    public GameObject AuraBurst { get => auraBurst; set => auraBurst = value; }
    public GameObject CenterPoint { get => centerPoint; set => centerPoint = value; }
    public GameObject AfterImage { get => afterImage; set => afterImage = value; }
    public GameObject SuperAura { get => superAura; set => superAura = value; }
    public GameObject SwordAura { get => swordAura; set { swordAura = value; print(value); } }
    public GameObject AuraFistR { get => auraFistR; set => auraFistR = value; }
    public GameObject AuraFistL { get => auraFistL; set => auraFistL = value; }
    public GameObject DbzAura { get => dbzAura; set => dbzAura = value; }
    public GameObject NormalHair { get => normalHair; set => normalHair = value; }
    public GameObject SuperHair { get => superHair; set => superHair = value; }
    public GameObject OriginPoint { get => originPoint; set => originPoint = value; }
    public GameObject AuraImpactBurst { get => auraImpactBurst; set => auraImpactBurst = value; }

    public void CreateSlash() {
        Instantiate(darkSlash,Point.transform.position,Point.transform.rotation);
    }
    private void SummonLightSword() {
        Instantiate(lightSwordShot, azaLight.transform.position, azaLight.transform.rotation);
    }
    public void SwordAuraControl(bool val) {
        SwordAura.SetActive(val);
    }
    private void ChangeHair() {
        //ClearAllHair();
        player.stats.TransformationMod = 2;
        CurrentHair.SetActive(false);
        SuperHair.SetActive(true);
        CurrentHair = SuperHair;
    }
    private void ChangeHairBack() {
        player.stats.TransformationMod = 1;
        CurrentHair.SetActive(false);
        NormalHair.SetActive(true);
        CurrentHair = NormalHair;
    }
    private void UIActivate() {
        currentHair.SetActive(false);
        UIHair.SetActive(true);
        CurrentHair = UIHair;
    }
    private void AEActivate() {
        CurrentHair.SetActive(false);
        AEHair.SetActive(true);
        CurrentHair = AEHair;
    }
    private void DeMActivate() {
        CurrentHair.SetActive(false);
        DeMHair.SetActive(true);
        CurrentHair = DeMHair;
    }
    private void ClearExtraEffects() {
        UIBox.SetActive(false);
    }
    private void SwitchEyes(bool val) {
        Material[] copy=skin.materials;

        if (val) {
            copy[2] = demonIris;
            copy[4] = demonFace;
            copy[5] = demonEyeBalls;
            skin.materials = copy;
            Debug.Log("Form up");
        }
        else {
            copy[2] = baseIris;
            copy[4] = baseFace;
            copy[5] = baseEyeBalls;
            skin.materials = copy;
            Debug.Log("Form down");
        }
    }
    public void SpawnSwordFlameTrail() { 
    
    }
}
