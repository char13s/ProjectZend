using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]

public abstract class Skill : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData) {
        OnHighlighted();
    }
    
    [SerializeField] private GameObject skillTimeline;
    [SerializeField] private string skillName;
    [SerializeField] private int skillId;
    [SerializeField] private int level;
    [SerializeField] private int mpCost;
    [SerializeField] private int skillCost;
    [TextArea(3, 10)]
    [SerializeField] private string description;
    
    //[SerializeField] private Text skillNameText;

    private bool unlocked;
    private static Skill lastSkillSelected;
    public static event UnityAction closeList;
    public static event UnityAction<string,string,int> sendDescription;   
    public static Skill LastSkillSelected { get => lastSkillSelected; set => lastSkillSelected = value; }
    public int MpCost { get => mpCost; set => mpCost = value; }
    public int SkillId { get => skillId; set => skillId = value; }
    public string SkillName { get => skillName; set => skillName = value; }
    public int Level { get => level; set => level = value; }
    public string Description { get => description; set => description = value; }
    public bool Unlocked { get => unlocked; set => unlocked = value; }
    public int SkillCost { get => skillCost; set => skillCost = value; }
    public GameObject SkillTimeline { get => skillTimeline; set => skillTimeline = value; }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetSkill);      
    }
    public Skill GetSkill(int skill) {
        if (skillId == skill) {
            return this;
        }
        return null;
    }
    public void SetSkill()
    {
        Debug.Log("SetLastSelectedSkill works");
        LastSkillSelected = this;
        //SkillSlotSetter.LastSkillSlotSetterSelected.SetSkill(this);
        //lastSkillSelected = this;
        Debug.Log(lastSkillSelected);
    }
    public void TryToUse() {
        if (NewZend.GetPlayer().stats.MPLeft > mpCost) {
            UseSkill();
        }
        else { 
        //Probably grey out the skill to show it cant be used
        }
    }
    public void OnHighlighted() {
        if (sendDescription != null) {
            sendDescription(Description,Level.ToString(),mpCost);
        }
    }
    public virtual void UseSkill() {
        if (SkillTimeline!=null&& NewZend.GetPlayer().stats.MPLeft > mpCost) {
            NewZend.GetPlayer().stats.MPLeft -= mpCost;
            NewZend.GetPlayer().PlayingSkill = true;
            NewZend zend = NewZend.GetPlayer();
        Instantiate(SkillTimeline, zend.transform.position, Quaternion.identity);
        }
    }
}
