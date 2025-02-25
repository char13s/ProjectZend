using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SkillList : MonoBehaviour
{
    public static event UnityAction<GameObject> sendObj;
    public static event UnityAction sendCheck;
    private static SkillList instance;
    [SerializeField] private GameObject firstSelected;
    [Header("Fire Skills")]
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject fireTornado;
    [Header("Sight Skills")]
    [SerializeField] private GameObject stunStare;
    [Header("Dark Skills")]
    [SerializeField] private GameObject darkSlash;
    [SerializeField] private GameObject shadowBall;
    [Header("Lightning Skills")]
    [SerializeField] private GameObject input;
    [Header("Ice Skills")]
    [SerializeField] private GameObject iceSnipe;

    public GameObject FirstSelected { get => firstSelected; set => firstSelected = value; }

    public static SkillList GetSkillList() => instance;
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
    }
    private void OnEnable() {
        if (sendCheck != null) {
            sendCheck();
        }
        //if (sendObj != null) {
        //    sendObj(firstSelected);
        //}
        //#region Dark nodes
        //DarkSlashSkillNode.unlockSkill += SetDarkSlash;
        //ShadowSkillNode.unlockSkill += SetShadowBall;
        //#endregion
        //#region Eye nodes
        //StunStareNode.unlockSkill += SetStunStare;
        //#endregion
        //#region Ice nodes
        //IceSnipeNode.unlockSkill += SetIceSnipe;
        //#endregion
        //#region Electric nodes
        //InputNode.unlockSkill += SetInput;
        //#endregion
        
        
    }
    private void OnDisable() {

    }

    #region eye skills
    private void SetStunStare() {
        stunStare.SetActive(true);
    } 
    #endregion
    #region Dark Skills
    private void SetDarkSlash() {
        darkSlash.SetActive(true);
    }
    private void SetShadowBall() {
        shadowBall.SetActive(true);
    } 
    #endregion
    #region Ice skills
    private void SetIceSnipe() {
        iceSnipe.SetActive(true);
    }
    #endregion

    #region Electric skills
    private void SetInput() {
        input.SetActive(true);
    }  
    #endregion
}
