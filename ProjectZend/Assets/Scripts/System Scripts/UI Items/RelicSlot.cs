using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class RelicSlot : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData) {
        if (!secondHighlight)
            OnHighlighted();
        else {
            SecondHighlighted();
        }
    }
    public static event UnityAction<GameObject> sendObj;
    public static event UnityAction<string, string, int> sendDetail;
    public static event UnityAction<RelicData, int> sendRelicData;
    public static event UnityAction requestData;
    public static event UnityAction lockInteract;
    public static event UnityAction<int> switchControls;
    public static event UnityAction tellInventSendFirst;
    private static RelicSlot lastRelSlotSelected;
    RelicData relic;
    [SerializeField] private int slotId;
    //[SerializeField] private RelicUIDisplay relicUI;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject list;
    [SerializeField] private GameObject skillList;
    private Button button;
    [SerializeField] private bool secondHighlight;
    [SerializeField] private bool lockable;

    public static RelicSlot LastRelSlotSelected { get => lastRelSlotSelected; set => lastRelSlotSelected = value; }
    private void OnEnable() {
        GameManager.setRelicSlots += SoftSetRelic;
        lockInteract += LockInteraction;
    }
    private void OnDisable() {
        GameManager.setRelicSlots -= SoftSetRelic;
        lockInteract -= LockInteraction;
    }
    private void Start() {
        if (requestData != null) {
            requestData();        
        }
    }
    public void OnHighlighted() {
        //open the relic list
        if (relic != null) {
            if (sendDetail != null)
                sendDetail(relic.Description, relic.RelicName, relic.Level);
        }
        else {
            if (sendDetail != null)
                sendDetail(" ", " ", 0);
        }
        if (!list.activeSelf) { 
            skillList.SetActive(false);
            list.SetActive(true);
        }
    }
    private void SecondHighlighted() {
        if (relic != null) {
            if (sendDetail != null)
                sendDetail(relic.Description, relic.RelicName, relic.Level);
        }
        else {
            if (sendDetail != null)
                sendDetail(" ", " ", 0);
        }
    }
    private void SoftPress() {
        LastRelSlotSelected = this;
    }
    public void OnPressed() {
        if (tellInventSendFirst!=null) {
            tellInventSendFirst();
        }     
        //sendObj.Invoke(RelicList.GetRelicList().FirstSelected);
        lastRelSlotSelected = this;
        if (lockable) {
            lockInteract.Invoke();
        }
    }
    public void OnSecondPressed() {
        SetRelic(GameManager.GetManager().HoldRelic);
        if (switchControls != null) {
            switchControls(0);
        }
    }
    public void SetRelic(RelicData relic) {
        if (this.relic != null)
            Unequip();
        this.relic = relic;
        sendObj.Invoke(gameObject);
        //relicUI.Relic = relic;
        text.text = relic.RelicName;
        sendRelicData(relic, slotId);
    }
    private void SoftSetRelic(RelicData relic, int id) {
        if (slotId == id&&relic!=null) {
            this.relic = relic;
            text.text = relic.RelicName;
        }
    }
    private void LockInteraction() {
        button.interactable = false;
    }
    private void Unequip() {
        Inventory.GetInventory().AddItems(relic);
        relic.Unequipped();
    }

}
