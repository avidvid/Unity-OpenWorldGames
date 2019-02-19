using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotData : MonoBehaviour,IDropHandler{

    public int SlotIndex;
    public Sprite EmptySprite;
    // Use this for initialization
    private InventoryHandler _inv;
    private ItemMixture _itemMixture;
    private ModalPanel _modalPanel;

    void Start()
    {
        _inv = InventoryHandler.Instance();
        _itemMixture = ItemMixture.Instance();
        _modalPanel = ModalPanel.Instance();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnDrop(PointerEventData eventData)
    {
        ItemData draggedItem = eventData.pointerDrag.GetComponent<ItemData>();
        ItemMixture mixedItem = eventData.pointerDrag.GetComponent<ItemMixture>();
        ItemEquipment equipedItem = eventData.pointerDrag.GetComponent<ItemEquipment>();


        if (mixedItem == null && draggedItem == null && equipedItem == null)
            return;
        if (mixedItem != null)
        {
            if (mixedItem.ItemIns == null)
                return;
            if (!mixedItem.ReadyToUse())
                return;
        }
        if (draggedItem != null)
            if (draggedItem.ItemIns == null)
                return;
        if (equipedItem != null)
            if (equipedItem.ItemIns == null)
                return;
        ItemData existingItem = this.transform.GetComponentInChildren<ItemData>();
        if (existingItem == null)
            return;

        //Item from Mixture Area
        if (mixedItem != null)
            if (mixedItem.ItemIns != null)
            {
                //#################################### not empty slot
                if (existingItem.ItemIns != null)
                    _inv.PrintMessage("New item should be placed in an empty inventory slot", Color.yellow);
                //#################################### empty slot
                else
                {
                    //Set the slot Item
                    existingItem.transform.name = mixedItem.ItemIns.Item.Name;
                    existingItem.GetComponent<Image>().sprite = mixedItem.ItemIns.Item.GetSprite();
                    TextMeshProUGUI stackCntText = existingItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    stackCntText.text = mixedItem.ItemIns.UserItem.StackCnt > 1 ? mixedItem.ItemIns.UserItem.StackCnt.ToString() : "";
                    existingItem.ItemIns = new ItemIns( mixedItem.ItemIns.Item, mixedItem.ItemIns.UserItem);
                    _inv.InvSlots[SlotIndex].name = mixedItem.ItemIns.Item.Name;
                    //Delete mixedItem Item
                    mixedItem.LoadEmpty();
                    _inv.UpdateInventory(true);
                }
                return;
            }
        //Item from equipments Area
        if (equipedItem != null)
            if (equipedItem.ItemIns != null)
            {
                //#################################### not empty slot
                if (existingItem.ItemIns != null)
                    _inv.PrintMessage("New item should be placed in an empty inventory slot", Color.yellow);
                //#################################### empty slot
                else
                {
                    //Set the slot Item
                    existingItem.transform.name = equipedItem.ItemIns.Item.Name;
                    existingItem.GetComponent<Image>().sprite = equipedItem.ItemIns.Item.GetSprite();
                    TextMeshProUGUI stackCntText = existingItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                    stackCntText.text = equipedItem.ItemIns.UserItem.StackCnt > 1 ? equipedItem.ItemIns.UserItem.StackCnt.ToString() : "";
                    existingItem.ItemIns = new ItemIns(equipedItem.ItemIns.Item, equipedItem.ItemIns.UserItem);
                    _inv.InvSlots[SlotIndex].name = equipedItem.ItemIns.Item.Name;
                    //unload new Item to equipments
                    equipedItem.LoadItem();

                    _inv.UpdateInventory(true);
                    _inv.UpdateEquipments(true);
                }
                return;
            }
        if (draggedItem != null)
            if (SlotIndex != draggedItem.SlotIndex )
            {
                //#################################### Stacking: Same items Stack them together
                if (existingItem.ItemIns.Item.Id == draggedItem.ItemIns.Item.Id )
                {
                    if (existingItem.ItemIns.UserItem.StackCnt + draggedItem.ItemIns.UserItem.StackCnt > existingItem.ItemIns.Item.MaxStackCnt)
                    {
                        draggedItem.ItemIns.UserItem.StackCnt = draggedItem.ItemIns.UserItem.StackCnt - (existingItem.ItemIns.Item.MaxStackCnt - existingItem.ItemIns.UserItem.StackCnt);
                        existingItem.ItemIns.UserItem.StackCnt = existingItem.ItemIns.Item.MaxStackCnt;
                    }
                    else
                    {

                        draggedItem.ItemIns.UserItem.StackCnt = 0;
                        //todo: load empty
                        existingItem.ItemIns.UserItem.StackCnt = draggedItem.ItemIns.UserItem.StackCnt + existingItem.ItemIns.UserItem.StackCnt;  
                    }
                    Text stackCntText = existingItem.transform.GetChild(0).GetComponent<Text>();
                    if (existingItem.ItemIns.UserItem.StackCnt > 1)
                        stackCntText.text = existingItem.ItemIns.UserItem.StackCnt.ToString();
                    _inv.UpdateInventory(true);
                }
                else
                {
                    Recipe newRecipe = _inv.CheckRecipes(existingItem.ItemIns.Item.Id, draggedItem.ItemIns.Item.Id);
                    //#################################### Mixing
                    if (newRecipe != null)
                    {
                        if (newRecipe.FirstItemCnt <= existingItem.ItemIns.UserItem.StackCnt)
                            if (newRecipe.SecondItemCnt <= draggedItem.ItemIns.UserItem.StackCnt)
                            {
                                if (_itemMixture.ItemIns != null)
                                    _inv.PrintMessage("You are already making an Item", Color.yellow);
                                else
                                    //Mixing items Logic (Lambda) can also be 2 func in it :  () => { InstantiateObject(thingToSpawn); InstantiateObject(thingToSpawn, thingToSpawn); }
                                    _modalPanel.Choice(newRecipe.GetDescription(), ModalPanel.ModalPanelType.YesCancel, () => { MixItemData(ref existingItem, ref draggedItem, newRecipe);});
                            }
                            else //Not enough materials 
                                _inv.PrintMessage("Not enough " + draggedItem.ItemIns.Item.Name +
                                                  " in the inventory, You need " +
                                                  (newRecipe.SecondItemCnt - draggedItem.ItemIns.UserItem.StackCnt) + " more", Color.yellow);
                        else //Not enough materials 
                            _inv.PrintMessage("Not enough " + existingItem.ItemIns.Item.Name + " in the inventory, You need " +
                                              (newRecipe.FirstItemCnt - existingItem.ItemIns.UserItem.StackCnt) + " more", Color.yellow);
                    }
                    //#################################### Swaping: Unmixable items Stack them together
                    else
                    {
                        existingItem.transform.SetParent(_inv.InvSlots[draggedItem.SlotIndex].transform);
                        _inv.InvSlots[draggedItem.SlotIndex].name = existingItem.name;
                        existingItem.transform.position = _inv.InvSlots[draggedItem.SlotIndex].transform.position;
                        existingItem.SlotIndex = draggedItem.SlotIndex;
                        draggedItem.SlotIndex = SlotIndex;
                        _inv.UpdateInventory(true);
                    }
                }
            }
    }

    private void MixItemData(ref ItemData existingItem, ref ItemData draggedItem,Recipe newRecipe)
    {
        if (!_inv.UseEnergy(newRecipe.Energy))
        {
            _inv.PrintMessage("Not enough energy to mix these items", Color.yellow);
            return;
        }
        existingItem.ItemIns.UserItem.StackCnt = existingItem.ItemIns.UserItem.StackCnt - newRecipe.FirstItemCnt;
        draggedItem.ItemIns.UserItem.StackCnt = draggedItem.ItemIns.UserItem.StackCnt - newRecipe.SecondItemCnt;

        var item = _inv.BuildItemFromDatabase(newRecipe.FinalItemId);
        int stackCnt = Math.Min(newRecipe.FinalItemCnt, item.MaxStackCnt);
        _itemMixture.LoadItem(item.Id, stackCnt, newRecipe.DurationMinutes);
        _inv.PrintMessage("Making " + item.Name + " starts",Color.green);
        _inv.AddCharacterSetting("Experience", newRecipe.Energy);

        if (existingItem.ItemIns.UserItem.StackCnt == 0)
        {
            //Logic of adding empty Item to Slot 
            existingItem.ItemIns = null;
            existingItem.transform.name = "Empty";
            existingItem.GetComponent<Image>().sprite = EmptySprite;
            //Update Text
            TextMeshProUGUI stackCntText = existingItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            stackCntText.text = "";
            _inv.InvSlots[SlotIndex].name = existingItem.name;
        }
        else
        {
            //Update Text
            TextMeshProUGUI stackCntText = existingItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            stackCntText.text = existingItem.ItemIns.UserItem.StackCnt > 1 ? existingItem.ItemIns.UserItem.StackCnt.ToString() : "";
        }
        if (draggedItem.ItemIns.UserItem.StackCnt == 0)
        {
            //Logic of adding empty Item to Slot 
            draggedItem.ItemIns = null;
            draggedItem.transform.name = "Empty";
            draggedItem.GetComponent<Image>().sprite = EmptySprite;
            _inv.InvSlots[SlotIndex].name = draggedItem.name;
        }
        _inv.UpdateInventory(true);
    }
}


//logic of adding an item to a slot
//existingItem.Item = item;
//existingItem.GetComponent<Image>().sprite = item.GetSprite();
//if (item.StackCnt > 1)
//existingItem.transform.GetChild(0).GetComponent<Text>().text = item.StackCnt.ToString();
//existingItem.name = item.Name;
//_inv.InvSlots[SlotIndex].name = existingItem.name;