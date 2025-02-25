using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class SkillButton : MonoBehaviour
{
    private int mpRequired;
    [SerializeField] private Skill skillAssigned;
    [SerializeField] private int skill;
    [SerializeField] private int slotNum;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField]private Button button;
    bool usable;
    public static event UnityAction<SkillButton> sendSkillSlot;
    public static event UnityAction<SkillButton, int> sendRef;
    public static event UnityAction<int> sendMpCost;
    public static event UnityAction checkMP;
    public int MpRequired { get => mpRequired; set => mpRequired = value; }
    public Skill SkillAssigned { get => skillAssigned; set { skillAssigned = value;if(value!=null) SetSkill(); } }

    public TextMeshProUGUI SkillName { get => skillName; set => skillName = value; }

    private void OnEnable() {
        //button = GetComponent<Button>();
        //button.onClick.AddListener(GetSkill);
        
        if (skillAssigned != null) {
           CheckSkillCost(); 
        }
        checkMP += CheckSkillCost;
       
    }
    private void OnDisable() {
        checkMP -= CheckSkillCost;
        NewZend.playerStart -= SendRef;
    }
    private void Awake() {
         NewZend.playerStart += SendRef;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        if (skillAssigned != null) {
            SetSkill();
        }
    }
    private void SendRef() { 
        if(sendRef!=null)
            sendRef(this, slotNum);
    }
    private void CheckSkillCost() {
        print("checked skill cost");
        if (skillAssigned != null) {
          if (NewZend.GetPlayer().stats.MPLeft < skillAssigned.MpCost) {
            button.interactable = false;
            usable = false;
            
        }
        else {
            button.interactable = true;
            usable = true;
        }  
        }
        
    }
    public void SetSkill() {

        if (SkillName != null) { 
            SkillName.text = skillAssigned.SkillName;
        }
            MpRequired = skillAssigned.MpCost;
            skill = skillAssigned.SkillId;
    }
    private void OnGameLoaded() {
        skillAssigned.GetSkill(skill);
    }
    private void NullSkill() {
        skillAssigned = null;
    }
    private void GetSkill() {
        if (sendSkillSlot != null) {
            sendSkillSlot(this);
        }
    }
    public void UseSkill() {
        print("Skill used");
        if (skillAssigned != null && usable) {
            
            skillAssigned.UseSkill();
            //sendMpCost.Invoke(-SkillAssigned.MpCost);
        }
        else {
            print("Skill null fam");
        }
        checkMP.Invoke();
    }
}
