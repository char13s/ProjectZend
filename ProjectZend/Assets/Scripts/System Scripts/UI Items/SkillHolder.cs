using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class SkillHolder : MonoBehaviour
{
    [SerializeField] private Skill skill;
    [SerializeField] private GameObject list;
    public static event UnityAction<Skill, int> sendSkill;
    public static event UnityAction closeList;
    public static event UnityAction<GameObject> sendBackToSlot;
    private TextMeshProUGUI nameComp;
    Button button;
    private void Start() {
        button = GetComponent<Button>();
        nameComp = GetComponentInChildren<TextMeshProUGUI>();
        nameComp.text = skill.SkillName;
        if(skill.Unlocked)
            
        CheckSkill();
    }
    private void OnEnable() {
        SkillList.sendCheck += CheckSkill;
    }
    private void OnDisable() {
        SkillList.sendCheck -= CheckSkill;
    }
    private void SendToGameManager() {
        sendSkill.Invoke(skill,SkillSlotSetter.LastSkillSlotSetterSelected.ID);
        SkillSlotSetter.LastSkillSlotSetterSelected.SetSkill(skill);
        if(sendBackToSlot!=null)
            sendBackToSlot(SkillSlotSetter.LastSkillSlotSetterSelected.gameObject);
        CloseList();
    }
    private void CheckSkill() {
        if (!skill.Unlocked) {
            Unselectable();
        }
        else { 
        button.onClick.AddListener(SendToGameManager);
            ColorBlock colors = button.colors;
            colors.normalColor = Color.red;
            button.colors = colors;
        }
    }
    private void Unselectable() {
        ColorBlock colors = button.colors;
        colors.normalColor = Color.grey;
        button.colors = colors;
        button.onClick.RemoveListener(SendToGameManager);
    }
    private void CloseList() {
        print("closelist");
            if(list!=null)
                list.SetActive(false);
           /// SkillList.ClosePage();
        
    }
}
