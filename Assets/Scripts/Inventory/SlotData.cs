using System;
using System.Collections;
using System.Collections.Generic;
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
            if (mixedItem.Item.Id == -1)
                return;
            if (!mixedItem.ReadyToUse())
                return;
        }
        if (draggedItem != null)
            if (draggedItem.Item.Id == -1)
                return;
        if (equipedItem != null)
            if (equipedItem.Item.Id == -1)
                return;
        ItemData[] existingItems = this.transform.GetComponentsInChildren<ItemData>();
        if (existingItems.Length <= 0)
            return;
        ItemData existingItem = existingItems[0];

        //Item from Mixture Area
        if (mixedItem != null)
            if (mixedItem.Item.Id != -1)
            {
                //#################################### not empty slot
                if (existingItem.Item.Id != -1)
                    _inv.PrintMessage("New item should be placed in an empty inventory slot", Color.yellow);
                //#################################### empty slot
                else
                {
                    //Set the slot Item
                    existingItem.transform.name = mixedItem.Item.Name;
                    existingItem.GetComponent<Image>().sprite = mixedItem.Item.GetSprite();
                    Text stackCntText = existingItem.transform.GetChild(0).GetComponent<Text>();
                    stackCntText.text = mixedItem.Item.StackCnt > 1 ? mixedItem.Item.StackCnt.ToString() : "";
                    existingItem.Item = mixedItem.Item;
                    _inv.InvSlots[SlotIndex].name = mixedItem.Item.Name;
                    //Delete mixure Item
                    mixedItem.LoadEmpty();
                    //Save empty item in mixture
                    _inv.SaveCharacterMixture(mixedItem.Item, DateTime.Now);
                    _inv.UpdateInventory(true);
                }
                return;
            }

        //Item from equipments Area
        if (equipedItem != null)
            if (equipedItem.Item.Id != -1)
            {
                //#################################### not empty slot
                if (existingItem.Item.Id != -1)
                    _inv.PrintMessage("New item should be placed in an empty inventory slot", Color.yellow);
                //#################################### empty slot
                else
                {
                    //Set the slot Item
                    existingItem.transform.name = equipedItem.Item.Name;
                    existingItem.GetComponent<Image>().sprite = equipedItem.Item.GetSprite();
                    Text stackCntText = existingItem.transform.GetChild(0).GetComponent<Text>();
                    stackCntText.text = equipedItem.Item.StackCnt > 1 ? equipedItem.Item.StackCnt.ToString() : "";
                    existingItem.Item = equipedItem.Item;
                    _inv.InvSlots[SlotIndex].name = equipedItem.Item.Name;
                    //unload new Item to equipments
                    equipedItem.LoadItem();
                    _inv.UnUseItem(existingItem.Item);

                    _inv.UpdateInventory(true);
                    _inv.UpdateEquipments(true);
                }
                return;
            }
        Debug.Log(draggedItem.SlotIndex+" Dragged to "+ SlotIndex + "-" + existingItem.Item.Name);
        if (SlotIndex != draggedItem.SlotIndex )
        {
            //#################################### Stacking: Same items Stack them together
            if (existingItem.Item.Id == draggedItem.Item.Id )
            {
                if (existingItem.Item.StackCnt + draggedItem.Item.StackCnt > existingItem.Item.MaxStackCnt)
                {
                    draggedItem.Item.setStackCnt(draggedItem.Item.StackCnt - (existingItem.Item.MaxStackCnt - existingItem.Item.StackCnt));
                    existingItem.Item.setStackCnt(existingItem.Item.MaxStackCnt);
                }
                else
                {
                    existingItem.Item.setStackCnt(existingItem.Item.StackCnt + draggedItem.Item.StackCnt);
                    draggedItem.Item.setStackCnt(0);
                }
                Text stackCntText = existingItem.transform.GetChild(0).GetComponent<Text>();
                if (existingItem.Item.StackCnt > 1)
                    stackCntText.text = existingItem.Item.StackCnt.ToString();
                _inv.UpdateInventory(true);
            }
            else
            {
                Recipe newRecipe = _inv.CheckRecipes(existingItem.Item.Id, draggedItem.Item.Id);
                //#################################### Mixing
                if (newRecipe != null)
                {
                    if (newRecipe.FirstItemCnt <= existingItem.Item.StackCnt)
                        if (newRecipe.SecondItemCnt <= draggedItem.Item.StackCnt)
                        {
                            //todo: check for Energy in recepie to make it
                            if (_itemMixture.Item.Id != -1)
                                _inv.PrintMessage("You are already making an Item", Color.yellow);
                            else
                                //Mixing items Logic (Lambda) can also be 2 func in it :  () => { InstantiateObject(thingToSpawn); InstantiateObject(thingToSpawn, thingToSpawn); }
                                _modalPanel.Choice(newRecipe.GetDescription(), ModalPanel.ModalPanelType.YesCancel, () => { MixItemData(ref existingItem, ref draggedItem, newRecipe);});
                        }
                        else //Not enough materials 
                            _inv.PrintMessage("Not enough " + draggedItem.Item.Name +
                                              " in the inventory, You need " +
                                              (newRecipe.SecondItemCnt - draggedItem.Item.StackCnt) + " more", Color.yellow);
                    else //Not enough materials 
                        _inv.PrintMessage("Not enough " + existingItem.Item.Name + " in the inventory, You need " +
                                          (newRecipe.FirstItemCnt - existingItem.Item.StackCnt) + " more", Color.yellow);
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
        existingItem.Item.setStackCnt(existingItem.Item.StackCnt - newRecipe.FirstItemCnt);
        draggedItem.Item.setStackCnt(draggedItem.Item.StackCnt - newRecipe.SecondItemCnt);

        ItemContainer item = _inv.BuildItemFromDatabase(newRecipe.FinalItemId);
        item.setStackCnt(Math.Min(newRecipe.FinalItemCnt, item.MaxStackCnt));

        _itemMixture.LoadItem(item, newRecipe.DurationMinutes);
        _inv.PrintMessage("Making " + item.Name + " starts",Color.green);
        _inv.AddCharacterSetting("Experience", newRecipe.Energy);

        if (existingItem.Item.StackCnt == 0)
        {
            //Logic of adding empty Item to Slot 
            existingItem.Item = new ItemContainer();
            existingItem.transform.name = "Empty";
            existingItem.GetComponent<Image>().sprite = EmptySprite;
            //Update Text
            Text stackCntText = existingItem.transform.GetChild(0).GetComponent<Text>();
            stackCntText.text = "";
            _inv.InvSlots[SlotIndex].name = existingItem.name;
        }
        else
        {
            //Update Text
            Text stackCntText = existingItem.transform.GetChild(0).GetComponent<Text>();
            stackCntText.text = existingItem.Item.StackCnt > 1 ? existingItem.Item.StackCnt.ToString() : "";
        }
        if (existingItem.Item.StackCnt == 0)
        {
            //Logic of adding empty Item to Slot 
            existingItem.Item = new ItemContainer();
            existingItem.transform.name = "Empty";
            existingItem.GetComponent<Image>().sprite = EmptySprite;
            _inv.InvSlots[SlotIndex].name = existingItem.name;
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