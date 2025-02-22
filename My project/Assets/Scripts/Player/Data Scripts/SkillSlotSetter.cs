using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class SkillSlotSetter : MonoBehaviour ,ISelectHandler
{
    public static event UnityAction<GameObject> sendObj;
    public static event UnityAction<string, string, int> sendDetail;
    public static event UnityAction<Skill,int> sendSkill;
    public static event UnityAction askForSkill;
    public static event UnityAction<int, SkillSlotSetter> setSlot;
    [SerializeField] private int iD;
    private Skill skillSlot;
    [SerializeField] private TextMeshProUGUI Text;
    [SerializeField] private GameObject skillList;
    [SerializeField] private GameObject relicList;
    private static SkillSlotSetter lastSkillSlotSetterSelected;
    public static SkillSlotSetter LastSkillSlotSetterSelected { get => lastSkillSlotSetterSelected; set => lastSkillSlotSetterSelected = value; }
    public int ID { get => iD; set => iD = value; }

    private void OnEnable() {
        GameManager.setSetters += SetSkillFromGM;
        if (setSlot != null) {
            setSlot(ID,this);
        }
        if (askForSkill != null) {
            askForSkill();
        }
    }
    private void OnDisable() {
        GameManager.setSetters -= SetSkillFromGM;
    }
    private void Start() {
        //GetComponent<Button>().onClick.AddListener(GoToSkillList);
    }
    public void GoToSkillList() {
        if (sendObj != null) { 
            sendObj(SkillList.GetSkillList().FirstSelected);
        }
        LastSkillSlotSetterSelected = this;
    }
    public void SetSkill(Skill skill) {
        //sendObj.Invoke(gameObject);
        //skillSlot= skill;
        sendSkill.Invoke(skill,ID);
        Text.text = skill.SkillName;
    }
    private void SetSkillFromGM(Skill skill, int ID) {
        print("Processed set skill");
        if (ID == iD&&skill!=null) {
            SetSkill(skill);
            print("Ran set skill");
        }
    }
    public void OnSelect(BaseEventData eventData) {
        OnHighlighted();
    }
    public void OnHighlighted() {
        if (skillSlot != null) {
            if(sendDetail!=null)
                sendDetail(skillSlot.Description, skillSlot.Level.ToString(), skillSlot.MpCost);
        }
        else {
            if(sendDetail != null)
               sendDetail(" "," ",0);
        }
        if(relicList)
            relicList.SetActive(false);
        if(skillList)
            skillList.SetActive(true);
    }
}
