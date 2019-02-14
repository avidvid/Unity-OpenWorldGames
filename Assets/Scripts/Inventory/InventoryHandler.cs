using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    private static InventoryHandler _inv;
    private int _playerSlots;
    private GUIManager _GUIManager;

    private ItemDatabase _itemDatabase;
    private UserDatabase _userDatabase;
    private CharacterManager _characterManager;
    //private ModalPanel _modalPanel; 

    private bool _updateInventory;
    private bool _updateEquipments;
    private bool _inTerrain = true;
    private ItemMixture _itemMixture;
    private ResearchSlot _researchingSlot;
    private GameObject _inventoryPanel;
    private GameObject _popupAction; 
    private GameObject _slotPanel;

    [SerializeField]
    private GameObject InventorySlot;
    [SerializeField]
    private GameObject InventorySlotBroken;
    [SerializeField]
    private GameObject InventoryItem;
    [SerializeField]
    private Sprite _lockSprite;

    private List<ItemIns> _invItems = new List<ItemIns>();
    private List<ItemIns> _equipments = new List<ItemIns>();
    public List<GameObject> InvSlots = new List<GameObject>();
    public SlotEquipment[] EquiSlots = new SlotEquipment[14];

    private int _slotAmount =30;
    public bool ShowInventory;
    
    // Use this for initialization
    void Awake()
    {
        _inv = Instance();
        _itemDatabase = ItemDatabase.Instance();
        _userDatabase = UserDatabase.Instance();
        _characterManager = CharacterManager.Instance();
        _itemMixture = ItemMixture.Instance();
        _researchingSlot = ResearchSlot.Instance();
        _GUIManager = GUIManager.Instance();

        _popupAction = GameObject.Find("Popup Action");
        _inventoryPanel = GameObject.Find("Inventory Panel");
        _slotPanel = _inventoryPanel.transform.Find("Slot Panel").gameObject;

        //Disable All buttons inside building 
        var insideBuilding = GameObject.Find("Building Interior");
        if (insideBuilding!=null)
        {
            _inTerrain= false;
            GameObject.Find("ButtonShop").GetComponent<Button>().interactable = false;
            GameObject.Find("ButtonSetting").GetComponent<Button>().interactable = false;
            GameObject.Find("ButtonAbout").GetComponent<Button>().interactable = false;
            GameObject.Find("Item Mixture").GetComponent<Button>().interactable = false;
            GameObject.Find("Research Slot").GetComponent<Button>().interactable = false;
            GameObject.Find("PlayerPic").GetComponent<Button>().interactable = false;
            GameObject.Find("CharacterPic").GetComponent<Button>().interactable = false;
        }
    }

    void Start()
    {
        _playerSlots = _characterManager.CharacterSetting.CarryCnt;
        var userInvItems = _characterManager.CharacterInventory;
        foreach (var itemIns in userInvItems)
        {
            if (itemIns.UserItem.Equipped)
                _equipments.Add(itemIns);
            else
                _invItems.Add(itemIns);
        }
        //Equipment
        EquiSlots = _inventoryPanel.GetComponentsInChildren<SlotEquipment>();
        for (int i = 0; i < EquiSlots.Length; i++)
        {
            EquiSlots[i].name = "Slot " + EquiSlots[i].EquType;
            EquiSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = EquiSlots[i].EquType.ToString();
            ItemEquipment equipmentItem = EquiSlots[i].GetComponentInChildren<ItemEquipment>();
            equipmentItem.ItemIns = null;
            equipmentItem.name = "Empty";
            foreach (var equipmentIns in _equipments)
            {
                if (equipmentIns.Item.PlaceHolder == EquiSlots[i].EquType)
                {

                    equipmentItem.ItemIns = equipmentIns;
                    equipmentItem.name = equipmentIns.Item.Name;
                    equipmentIns.UserItem.Order = (int) EquiSlots[i].EquType;
                    equipmentItem.GetComponent<Image>().sprite = _equipments[i].Item.GetSprite();
                    break;
                }
            }
        }
        //Item Mixture
        InitMixture(_characterManager.CharacterMixture);
        //Researching
        InitResearching(_characterManager.CharacterResearching);
        //Inventory
        for (int i = 0; i < _slotAmount; i++)
        {
            if (i < _playerSlots)
            {
                InvSlots.Add(Instantiate(InventorySlot));
                InvSlots[i].GetComponent<SlotData>().SlotIndex = i;
                //print(i + "-" +InvSlots[i].GetComponent<SlotData>().SlotIndex );
            }
            else
                InvSlots.Add(Instantiate(InventorySlotBroken));

            InvSlots[i].transform.SetParent(_slotPanel.transform);
            if (i < _playerSlots)
            {
                GameObject itemObject = Instantiate(InventoryItem);
                ItemData data = itemObject.GetComponent<ItemData>();
                data.ItemIns = _invItems[i];
                data.SlotIndex = i;
                //print(_invItems[i].Id + "-" + i);
                itemObject.transform.SetParent(InvSlots[i].transform);
                itemObject.transform.position = Vector2.zero;
                InvSlots[i].name = itemObject.name = _invItems[i].Item.Name;
                if (_invItems[i].Item.Id != -1)
                {
                    itemObject.GetComponent<Image>().sprite = _invItems[i].Item.GetSprite();
                    itemObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _invItems[i].UserItem.StackCnt > 1 ? _invItems[i].UserItem.ToString() :"";
                }
            }
            //todo: lets user buy a slot 
            else
            {
                if (i == _playerSlots)
                {
                    Button button = InvSlots[i].GetComponentInChildren<Button>();
                    button.GetComponent<Image>().sprite = _lockSprite;
                    InvSlots[i].name = button.name = "Lock";
                    if (_inTerrain)
                        button.interactable = true;
                }
            }
            InvSlots[i].transform.localScale = Vector3.one;
        }
    }

    void Update()
    {
        if (ShowInventory)
        {
            _inventoryPanel.SetActive(true);
            ShowInventory = false;
        }

        if (_updateInventory || _updateEquipments)
        {
            //Save new inventory 
            if (_updateInventory)
            {
                _invItems.Clear();
                for (int i = 0; i < _playerSlots; i++)
                {
                    var tmpItem = InvSlots[i].transform.GetChild(0).GetComponent<ItemData>().ItemIns;
                    _invItems.Add(tmpItem);
                }
                _updateInventory = false;
            }
            //Save new Equipments 
            if (_updateEquipments)
            {
                _equipments.Clear();
                foreach (var equipmentSlot in EquiSlots)
                {
                    var tmpItem = equipmentSlot.transform.GetComponentInChildren<ItemEquipment>().ItemIns;
                }
                _updateEquipments = false;
            }
            _characterManager.SaveCharacterInventory();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_inventoryPanel.activeSelf)
                _inventoryPanel.SetActive(false);
            else
                _inventoryPanel.SetActive(true);
        }
    }

    public bool UseItem(ItemIns item)
    {
        if (item == null)
            return false;
        //Use 1/10 of Max energy to use an item
        if (_characterManager.UseEnergy(_characterManager.CharacterSetting.MaxEnergy / 10))
        {
            string itemStatus = _characterManager.CharacterSettingUseItem(item, true);
            if (itemStatus != "")
                _inv.PrintMessage(itemStatus, Color.yellow);
            return true;
        }
        _inv.PrintMessage("Not enough energy to use this item", Color.yellow);
        return false;
    }

    public void UnUseItem(ItemIns item)
    {
        if (item == null)
            return;
        _characterManager.CharacterSettingUnuseItem(item, true);
    }


    internal bool HaveAvailableSlot()
    {
        if (_invItems.Count< _playerSlots)
            return true;
        return false;
    }

    internal bool UseEnergy(int amount)
    {
        return _characterManager.UseEnergy(amount);
    }

    public void UpdateEquipments(bool value)
    {
        _updateEquipments = value;
    }

    public void UpdateInventory(bool value)
    {
        _updateInventory = value;
    }

    //Should match with the AddItemToInventory in CharacterManager
    public bool AddItemToInventory(int itemId,int stackCnt =1)
    {
        var item = BuildItemFromDatabase(itemId);
        if (_characterManager.ItemIsInInventory(item.Id))
        {
            if (item.MaxStackCnt == 1)
            {
                PrintMessage("You Can Only Carry one of this item!", Color.yellow);
                return false;
            }
            for (int i = 0; i < _playerSlots; i++)
            {
                var tmpItem = InvSlots[i].transform.GetChild(0).GetComponent<ItemData>().ItemIns;
                if (tmpItem != null)
                {
                    if (tmpItem.Item.Id == item.Id)
                    {
                        //It can be stacked in the existing slot that has the same item
                        if (tmpItem.UserItem.StackCnt + stackCnt <= tmpItem.Item.MaxStackCnt)
                        {
                            tmpItem.UserItem.StackCnt += stackCnt;
                            UpdateInventory(true);
                            return true;
                        }
                        //It can NOT be stacked in the existing slot that has the same item so we go to the next Slot
                    }
                }
            }
        }
        else{
            for (int i = 0; i < _playerSlots; i++)
            {
                var tmpItem = InvSlots[i].transform.GetChild(0).GetComponent<ItemData>();
                if (tmpItem.ItemIns == null)
                {
                    tmpItem.LoadItem(new ItemIns(item,new UserItem(item,stackCnt)));
                }
                UpdateInventory(true);
                return true;
            }
        }
        PrintMessage("Not Enough room in inventory",Color.red);
        return false;
    }
    public void OpenInventoryPanel()
    {
        _popupAction.SetActive(false);
        if (_inventoryPanel.activeSelf)
            _inventoryPanel.SetActive(false);
        else
            _inventoryPanel.SetActive(true);
    }
    public bool InventoryPanelStat()
    {
        return _inventoryPanel.activeSelf;
    }
    public void GoToRecipeScene()
    {
        _itemMixture = ItemMixture.Instance();
        if (!_itemMixture.ItemLocked)
        {
            if (_itemMixture.IsEmpty())
            {
                BuildTrainStarter();
                SceneManager.LoadScene(SceneSettings.SceneIdForRecipes);
            }
        }
    }
    public void GoToResearchScene()
    {
        _researchingSlot = ResearchSlot.Instance();
        if (!_researchingSlot.ItemLocked)
        {
            if (_researchingSlot.IsEmpty())
            {
                BuildTrainStarter();
                SceneManager.LoadScene(SceneSettings.SceneIdForResearch);
            }
        }
    }
    public void GoToProfileScene()
    {
        BuildTrainStarter();
        //switch the scene
        SceneManager.LoadScene(SceneSettings.SceneIdForProfile);
    }
    public void GoToMenuSceneOption()
    {
        BuildTrainStarter("InventoryHandler","Option");
        //switch the scene
        SceneManager.LoadScene(SceneSettings.SceneIdForMenu);
    }

    public void GoToMenuSceneSocial()
    {
        BuildTrainStarter("InventoryHandler", "Social");
        //switch the scene
        SceneManager.LoadScene(SceneSettings.SceneIdForMenu);
    }
    public void GoToCreditScene()
    {
        BuildTrainStarter();
        //switch the scene
        SceneManager.LoadScene(SceneSettings.SceneIdForCredits);
    }
    public void GoToStoreScene()
    {
        //OfferListHandler
        BuildTrainStarter("InventoryHandler", "Terrain");        
        //switch the scene
        SceneManager.LoadScene(SceneSettings.SceneIdForStore);
    }
    public void GoToSlotShop()
    {
        //OfferListHandler
        BuildTrainStarter("InventoryHandler", "Inventory");
        //switch the scene
        SceneManager.LoadScene(SceneSettings.SceneIdForStore);
    }
    private void BuildTrainStarter(string domain = null,string content = null)
    {
        //Preparing to return to terrain
        GameObject go = new GameObject();
        //Make go unDestroyable
        GameObject.DontDestroyOnLoad(go);
        var starter = go.AddComponent<SceneStarter>();
        Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        starter.PreviousPosition = player.position;
        starter.ShowInventory = true;
        go.name = domain ?? "Unknown in Inv";
        starter.Content = content ?? "Unknown in Inv";
    }
    private void InitResearching(CharacterResearching researching)
    {
        if (researching == null)
            _researchingSlot.LoadEmpty();
        else 
            _researchingSlot.LoadResearch(researching);
    }
    private void InitMixture(CharacterMixture playerMixture)
    {
        if (playerMixture == null)
            _itemMixture.LoadEmpty();
        else
            _itemMixture.LoadItem(playerMixture);
    }

    public void SaveCharacterMixture(int itemId, int stackCnt, DateTime time)
    {
        _characterManager.SaveCharacterMixture(itemId,  stackCnt, time);
    }

    public Recipe CheckRecipes(int first, int second)
    {
        return _userDatabase.FindUserRecipes( first,  second);
    }

    internal void PrintMessage(string message,Color color)
    {
        _GUIManager.PrintMessage(message, color);
    }

    public OupItem BuildItemFromDatabase(int id)
    {
        return _itemDatabase.GetItemById(id);
    }
    public bool ElementToolUse(ElementIns element=null)
    {
        ElementIns.ElementType targetType = element != null ? element.Type : ElementIns.ElementType.Hole;
        //Check Left hand for tool
        ItemIns toolIns = null;
        var toolEquipment = _inv.EquiSlots[(int)OupItem.PlaceType.Left].GetComponentInChildren<ItemEquipment>();
        if (toolEquipment!=null)
        {
            toolIns = toolEquipment.ItemIns;
            if (toolIns.Item.Type == OupItem.ItemType.Tool ||
                toolIns.UserItem.TimeToUse > 0 ||
                targetType == toolIns.Item.FavoriteElement)
            {
                toolEquipment.UseItem(1);
                return true;
            }
        }
        //Check Right hand for tool
        toolEquipment = _inv.EquiSlots[(int)OupItem.PlaceType.Right].GetComponentInChildren<ItemEquipment>();
        if (toolEquipment != null)
        {
            toolIns = toolEquipment.ItemIns;
            if (toolIns.Item.Type == OupItem.ItemType.Tool ||
                toolIns.UserItem.TimeToUse > 0 ||
                targetType == toolIns.Item.FavoriteElement)
            {
                toolEquipment.UseItem(1);
                return true;
            }
        }
        PrintMessage("You don't have a right tool to use", Color.yellow);
        return false;
    }

    //Middle Man
    public void AddCharacterSetting(string field, float value)
    {
        _characterManager.AddCharacterSetting("Experience", value);
    }

    internal float GetCrafting()
    {
        return _characterManager.GetCharacterAttribute("Crafting");
    }

    public static InventoryHandler Instance()
    {
        if (!_inv)
        {
            _inv = FindObjectOfType(typeof(InventoryHandler)) as InventoryHandler;
            if (!_inv)
                Debug.LogError("There needs to be one active InventoryHandler script on a GameObject in your scene.");
        }
        return _inv;
    }

}
