using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class RelicHolder : MonoBehaviour
{
    public static event UnityAction updateInvent;
    [SerializeField] private RelicData relic;
    private Image panel;
    Button button;

    public RelicData Relic { get => relic; set => relic = value; }

    private void Start() {
        button = GetComponent<Button>();
        panel = GetComponent<Image>();
        button.onClick.AddListener(SendToGameManager);
        //CheckIfEmpty();
    }
    private void SendToGameManager() {
        //sendSkill.Invoke(skill, SkillSlotSetter.LastSkillSlotSetterSelected.ID);
        RelicSlot.LastRelSlotSelected.SetRelic(Relic);
        if (updateInvent != null) {
            updateInvent();
        }
        Inventory.GetInventory().RemoveItem(relic);
    }
    private void CheckIfEmpty() {
        if (Relic == null) {
            Color temp = new Color();
            temp.a = 0;
            panel.color= temp;
        }
    }
}
