using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemInvent : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI quantity;
    [SerializeField] private TextMeshProUGUI itemName;
    private RelicData relic;
    public RelicData Relic { get => relic; set => relic = value; }
    public Image ItemSprite { get => itemSprite; set => itemSprite = value; }

    // Start is called before the first frame update
    private void Start() {
        itemName.text = relic.RelicName;
        quantity.text = relic.Quantity.ToString();
    }
}
