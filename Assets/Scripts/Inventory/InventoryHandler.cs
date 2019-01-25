using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    private static InventoryHandler _inv;
    private int _playerSlots;
    private GUIManager _GUIManager;

    private ItemDatabase _itemDatabase;
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

    private List<ItemContainer> _invItems = new List<ItemContainer>();
    public List<GameObject> InvSlots = new List<GameObject>();
    public SlotEquipment[] EquiSlots = new SlotEquipment[14];
    private List<ItemContainer> _equipments = new List<ItemContainer>();

    private int _slotAmount;
    public bool ShowInventory;
    
    // Use this for initialization
    void Awake()
    {
        _inv = InventoryHandler.Instance();
        _itemDatabase = ItemDatabase.Instance();
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
        _invItems = _characterManager.CharacterInventory;
        _slotAmount = _invItems.Count;
        //Equipment
        _equipments = _characterManager.CharacterSetting.Equipments;

        SlotEquipment[] equiSlots = _inventoryPanel.GetComponentsInChildren<SlotEquipment>();
        for (int i = 0; i < equiSlots.Length; i++)
            EquiSlots[(int)equiSlots[i].EquType] = equiSlots[i];
        for (int i = 0; i < EquiSlots.Length; i++)
        {
            //print("index : "+i+"-"+_equiSlots[i].EquType + (int)_equiSlots[i].EquType + " id from in=  "+ _equipments[i]);
            EquiSlots[i].name = "Slot " + EquiSlots[i].EquType;
            EquiSlots[i].GetComponentInChildren<Text>().text = EquiSlots[i].EquType.ToString();
            ItemEquipment equipmentItem = EquiSlots[i].GetComponentInChildren<ItemEquipment>();
            if (_equipments[i].Id == -1)
            {
                equipmentItem.Item = new ItemContainer();
                equipmentItem.name = "Empty";
            }
            else
            {
                ItemContainer tempItem = _equipments[i];
                equipmentItem.Item =
                    new ItemContainer(
                        tempItem.Id, tempItem.Name, tempItem.Description,
                        tempItem.IconPath, tempItem.IconId,
                        tempItem.Cost, tempItem.Weight,
                        tempItem.MaxStackCnt, tempItem.MaxStackCnt, //*** Equipment only accept maxStacks
                        tempItem.Type, tempItem.Rarity, tempItem.IsUnique,
                        tempItem.DurationDays, tempItem.ExpirationTime,
                        tempItem.Values);
                equipmentItem.name = tempItem.Name;
                equipmentItem.GetComponent<Image>().sprite = tempItem.GetSprite();
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
                data.Item = _invItems[i];
                data.SlotIndex = i;
                //print(_invItems[i].Id + "-" + i);
                itemObject.transform.SetParent(InvSlots[i].transform);
                itemObject.transform.position = Vector2.zero;
                InvSlots[i].name = itemObject.name = _invItems[i].Name;
                if (_invItems[i].Id != -1)
                {
                    itemObject.GetComponent<Image>().sprite = _invItems[i].GetSprite();
                    itemObject.transform.GetChild(0).GetComponent<Text>().text = _invItems[i].StackCnt > 1 ? _invItems[i].StackCnt.ToString() :"";
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

        if (_updateInventory)
        {
            //print("#####_updateInventory " +_invItems.Count);
            //Todo: security vulnerability: might be able to change inv 
            //Refresh _invItems based on the interface 
            for (int i = 0; i < _playerSlots; i++)
            {
                ItemContainer tmpItem = InvSlots[i].transform.GetChild(0).GetComponent<ItemData>().Item;
                //tmpItem.Print();
                _invItems[i] = tmpItem;
            }
            //Save new inventory 
            _characterManager.SaveCharacterInventory();
            _updateInventory = false;
        }

        if (_updateEquipments)
        {
            for (int i = 0; i < _equipments.Count; i++)
            {
                ItemContainer tmpItem = EquiSlots[i].transform.GetChild(0).GetComponent<ItemEquipment>().Item;
                if (_equipments[i].Id == tmpItem.Id)
                    if (_equipments[i].Type != Item.ItemType.Tool)
                        continue;
                    else if (_equipments[i].Tool.TimeToUse == tmpItem.Tool.TimeToUse)
                            continue;
                _equipments[i] = tmpItem;
            }
            //Save new Equipments 
            _characterManager.SaveCharacterEquipments(_equipments);
            _updateEquipments = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_inventoryPanel.activeSelf)
                _inventoryPanel.SetActive(false);
            else
                _inventoryPanel.SetActive(true);
        }
    }
    internal bool HaveAvailableSlot()
    {
        for (int i = 0; i < _playerSlots; i++)
        {
            ItemData tmpItem = InvSlots[i].transform.GetChild(0).GetComponent<ItemData>();
            if (tmpItem.Item.Id != -1)
                continue;
            return true;
        }
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

    public bool UseItem(ItemContainer item)
    {
        if (item.Id == -1)
            return false;
        //Use 1/10 of Max energy to use an item
        if (_characterManager.UseEnergy(_characterManager.CharacterSetting.MaxEnergy / 10))
        {
            string itemStatus =  _characterManager.CharacterSettingUseItem(item, true);
            if (itemStatus!="")
                _inv.PrintMessage(itemStatus, Color.yellow);
            return true;
        }
        _inv.PrintMessage("Not enough energy to use this item",Color.yellow);
        return false;
    }

    public void UnUseItem(ItemContainer item)
    {
        if (item.Id ==-1)
            return;
        _characterManager.CharacterSettingUnuseItem(item, true);
    }

    //Should match with the AddItemToInventory in CharacterManager
    public bool AddItemToInventory(int itemId)
    {
        ItemContainer item = BuildItemFromDatabase(itemId);
        //CheckUniqueness
        if (item.IsUnique)
            if (_characterManager.InventoryExists(item.Id))
            {
                PrintMessage("You Can Only Carry one of this item!", Color.yellow);
                return false;
            }
        for (int i = 0; i < _playerSlots; i++)
        {
            ItemData tmpItem = InvSlots[i].transform.GetChild(0).GetComponent<ItemData>();
            if (tmpItem.Item.Id == itemId)
            {
                //It can be stacked in the existing slot that has the same item
                if (tmpItem.Item.StackCnt < tmpItem.Item.MaxStackCnt)
                {
                    tmpItem.Item.setStackCnt(tmpItem.Item.StackCnt + 1);
                    UpdateInventory(true);
                    return true;
                }
                //It can NOT be stacked in the existing slot that has the same item so we go to the next Slot
                continue;
            }
            //Slot is occupied and it is not the same item 
            if (tmpItem.Item.Id != -1)
                continue;
            tmpItem.LoadItem(item);
            UpdateInventory(true);
            return true;
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
            if (_itemMixture.Item.Id == -1)
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
            if (_researchingSlot.CharResearch.Id == -1)
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
            return;
        if (researching.Id == -1)
            _researchingSlot.LoadEmpty();
        else 
            _researchingSlot.LoadResearch(researching.CharResearch, researching.Time);
    }
    private void InitMixture(CharacterMixture playerMixture)
    {
        if (playerMixture == null)
            return;
        if (playerMixture.Item == null)
            _itemMixture.LoadEmpty();
        else if (playerMixture.Item.Id == -1)
            _itemMixture.LoadEmpty();
        else
            _itemMixture.LoadItem(playerMixture.Item, playerMixture.Time);
    }

    public void SaveCharacterMixture(ItemContainer item, DateTime time)
    {
        _characterManager.SaveCharacterMixture(item, time);
    }

    public Recipe CheckRecipes(int first, int second)
    {
        return _itemDatabase.FindRecipes( first,  second);
    }

    internal void PrintMessage(string message,Color color)
    {
        _GUIManager.PrintMessage(message, color);
    }

    public ItemContainer BuildItemFromDatabase(int id)
    {
        if (id == -1)
            return new ItemContainer();
        return new ItemContainer(_itemDatabase.FindItem(id));
    }
    public bool ElementToolUse(ElementIns element=null)
    {
        ItemEquipment existingEquipment = _inv.EquiSlots[(int)Equipment.PlaceType.Left].GetComponentInChildren<ItemEquipment>();
        ItemContainer itemEquipment = existingEquipment.Item;
        ElementIns.ElementType targetType = element != null ? element.Type: ElementIns.ElementType.Hole;
        if (itemEquipment.Id != -1 && 
            itemEquipment.Type == Item.ItemType.Tool && 
            itemEquipment.StackCnt > 0 &&
            targetType == itemEquipment.Tool.FavouriteElement)
        {
            existingEquipment.Item.UseItem(1);
            UpdateEquipments(true);
            return true;
        }
        else
        { 
            existingEquipment = _inv.EquiSlots[(int)Equipment.PlaceType.Right].GetComponentInChildren<ItemEquipment>();
            itemEquipment = existingEquipment.Item;
            if (itemEquipment.Id != -1 &&
                itemEquipment.Type == Item.ItemType.Tool &&
                itemEquipment.StackCnt > 0 &&
                targetType == itemEquipment.Tool.FavouriteElement)
            {
                existingEquipment.Item.UseItem(1);
                UpdateEquipments(true);
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
