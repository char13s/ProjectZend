using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Inventory :MonoBehaviour{

    [SerializeField] private static List<RelicData> items=new List<RelicData>();
    private List<GameObject> slots = new List<GameObject>();
    [SerializeField] private GameObject emptyItem;
    private static Inventory instance;
    public static List<RelicData> Items { get => items; set => items = value; }

    public static event UnityAction saveGame;
    public static event UnityAction<GameObject> sendObj;
    public static event UnityAction updateThisInvent;
    public static Inventory GetInventory() => instance;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        }
        else {
            instance = this;
        }
        
    }
    private void Start() {
        //RewardSlot.sendRelic += AddItems;
        RelicSlot.tellInventSendFirst += CheckFirstSlot;
    }
    public void AddItems(RelicData item) {
        if (HasItem(item.ID)) {
            RelicData i = GetItem(item);
            i.Quantity++;
            print(i.RelicName+" "+i.Quantity);
        }
        else {
            Items.Add(item);
            item.Quantity = 0;
            item.Quantity++;
            print(item.RelicName + " " + item.Quantity);
            if (saveGame != null) {
                saveGame();
            }
        }
        if (updateThisInvent != null)
            updateThisInvent();
    }
    public void RemoveItem(RelicData item) {
        item.Quantity--;
        if (item.Quantity == 0) {
            Items.Remove(item);
        }
        if (updateThisInvent != null)
            updateThisInvent();
    }
    public bool HasItem(int ID) {
        foreach (RelicData i in Items) {
            if (i.ID == ID) {
                return true;
            }
        }
        return false;
    }
    public RelicData GetItem(RelicData item) {
        foreach (RelicData i in Items) {
            if (i.ID == item.ID) {
                return i;
            }
        }
        return null;
    }
    public void GenerateInvent(GameObject invent) {
        ClearInventSpace(invent);
        print(Items.Count);
        foreach (RelicData relic in Items) {
            GameObject item=Instantiate(emptyItem, invent.transform);
            item.GetComponent<RelicHolder>().Relic = relic;
            ItemInvent itemVent =item.GetComponent<ItemInvent>();
            itemVent.Relic = relic;
            itemVent.ItemSprite.sprite = SpriteAssign.GetSprite().SetImage(relic);
            slots.Add(item);
        }
    }
    private void CheckFirstSlot() {
        if (items[0] != null) {
            print(slots[0]); 
            if (sendObj != null) {
                sendObj(slots[0]);
            }
        }
        else {
            print("invent empty");
        }
    }
    private void ClearInventSpace(GameObject invent) {
        foreach (Transform transform in invent.transform) {
           Destroy(transform.gameObject);
        }
        slots = new List<GameObject>();
    }






























    /*[SerializeField] private List<ItemData> items = new List<ItemData>();
   
    private List<SpellTag> relics = new List<SpellTag>();
    private int itemIndex = 0;
    private ItemData mainItem;
    private int selectedList;
    private ItemData leftItem;
    private ItemData rightItem;
    private bool control;
    [SerializeField] private Button useButton;
    //private List<string> itemNames = new List<string>();
    private List<GameObject> buttons = new List<GameObject>();
    public static event UnityAction<Sprite,string,string> mainItemSet;
    public static event UnityAction<int> menuSet;
    public static event UnityAction set;
    public List<GameObject> Buttons { get => buttons; set => buttons = value; }
    public List<ItemData> Items { get => items; set => items = value; }
    public ItemData MainItem { get => mainItem; set { mainItem = value; MainItemManagement(); } }

    public ItemData LeftItem { get => leftItem; set { leftItem = value; } }
    public ItemData RightItem { get => rightItem; set { rightItem = value; } }
    public int ItemIndex { get => itemIndex; set { itemIndex = Mathf.Clamp(value, 0, Items.Count) ; } }

    public int SelectedList { get => selectedList; set { selectedList = value; } }


    // Start is called before the first frame update
    public void Start() {
        ItemData.ItemDataUpdate += UpdateInvent;
        GameController.onGameWasStarted += UpdateInvent;
    }

    #region Quick Item Menu
    private void SwitchDpadControl(bool val) {
        control = val;
    }
    private void RightSlide() {
        if (control) {
            ItemIndex++;
            if (itemIndex == Items.Count) {
                ItemIndex = 0;
            }
            MainItemSwitching();
        }
    }
    private void LeftSlide() {
        if (control) {
            if (itemIndex == 0) {
                ItemIndex = Items.Count;
            }

            ItemIndex--;

            MainItemSwitching();
        }
    }
    private void MainItemSwitching() {
        if (items.Count != 0) {
            MainItem = items[ItemIndex];
        }
    }
    private void SwitchSelected() {
        switch (SelectedList) {
            case 0:
                SelectedList = 1;
                if (menuSet != null) {
                    menuSet(SelectedList);
                }
                if (set != null) {
                    set();
                }
                break;
            case 1:
                SelectedList = 0;
                if (menuSet != null) {
                    menuSet(SelectedList);
                }
                if (set != null) {
                    set();
                }
                break;
        }

    }
    private void MenuSwitch() {

    }

    #endregion

    public void AddItem(ItemData item) {

        if (HasItem(item.ID)) {
            ItemData i = GetItem(item);

            i.Quantity++;
            //Debug.Log(i.Quantity);
            
                MainItemManagement();
            
            //inventory.Add(item);
        }
        else {
            //UiManager.itemAdded(item.ItemDescription, SpriteAssign.SetImage(item));
            Items.Add(item);
            item.Quantity = 0;
            item.Quantity++;
            //ButtonCreation(item);

        }
        //if (items.Count == 1) {
        //    MainItem = item;
        //}
        Debug.Log(Items.Count);
        MainItem=items[ItemIndex];
        MainItemManagement();
    }
    public void AddRelic(SpellTag relic) {
        relics.Add(relic);
    }
    #region Item management
    private void MainItemManagement() {
        if (mainItem != null) {

            if (mainItemSet != null) {
                mainItemSet(SetImage(mainItem), mainItem.ItemName, mainItem.Quantity.ToString());
            }
        }
        else {

        }
    }
    public ItemData GetItem(ItemData item) {
        foreach (ItemData i in Items) {
            if (i.ID == item.ID) {

                return i;
            }
        }

        return null;
    }
    public void UseItem() {
        if (mainItem != null) {
            mainItem.UseItem();
            mainItem.Quantity--;//change this mess
            MainItemManagement();
            if (mainItem.Quantity == 0) {
                Items.Remove(mainItem);
                if (leftItem != null) {
                    mainItem = leftItem;
                }
                else {
                    mainItem = null;
                }
            }
        }
    }
    #endregion

    //private void UseRelic() {
    //    relics.
    //}
    public void SellItem(Items item, int n) {
        item.data.Quantity -= n;
        if (item.data.Quantity == 0) {
            Items.Remove(item.data);
        }
    }
    public bool HasItem(int ID) {
        foreach (ItemData i in Items) {
            if (i.ID == ID) {
                return true;
            }
        }
        return false;
    }
    private Sprite SetImage(ItemData i) {
        return SpriteAssign.SetImage(i);
    }
    public void ButtonCreation(ItemData i) {
        GameObject c = new GameObject();
        c.name = "Item";
        c.AddComponent<Text>();
        c.AddComponent<Items>();
        c.GetComponent<Items>().data = i;
        c.GetComponent<Text>().text = i.ItemName;
        c.AddComponent<Button>();
        c.GetComponent<Text>().color = Color.black;
        c.GetComponent<Text>().font = UiManager.GetUiManager().LuckiestGuy;
        c.GetComponent<Text>().resizeTextForBestFit = true;
        c.transform.SetParent(SetToInvent(i));
        string d = i.ItemDescription;
        c.GetComponent<Button>().onClick.AddListener(c.GetComponent<Items>().IconClick);
        GameObject t = new GameObject();
        t.AddComponent<CanvasRenderer>();
        t.AddComponent<Text>();
        t.transform.SetParent(c.transform);
        GetQuantity(t.GetComponent<Text>(), i);
        t.transform.localPosition = new Vector3(-62, 0, 0);
        t.GetComponent<Text>().color = Color.black;
        t.GetComponent<Text>().font = UiManager.GetUiManager().LuckiestGuy;
        t.GetComponent<Text>().resizeTextForBestFit = true;
        Buttons.Add(c);
    }

    private Transform SetToInvent(ItemData i) {
        switch (i.Type) {
            case ItemData.ItemType.Normal:
                return UiManager.GetUiManager().ItemInvent.transform;
            default:
                break;
        }
        return null;
    }
    private void GetQuantity(Text t, ItemData i) {
        t.text = i.Quantity.ToString();
        if (i.Quantity == 0) {
            items.Remove(i);
        }
    }
    private void UpdateInvent() {
        //if (buttons.Count != 0) {
        //    foreach (GameObject b in Buttons) {
        //        GetQuantity(b.transform.GetChild(0).GetComponent<Text>(), b.GetComponent<Items>().data);
        //
        //
        //        if (b.GetComponent<Items>().data.Quantity == 0) {
        //            Debug.Log("Item was removed");
        //            b.SetActive(false);
        //        }
        //    }
        //}
    }
    */
}

