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
                    if (_inv.AddItemToInventory(mixedItem.ItemIns.Item, mixedItem.ItemIns.UserItem.StackCnt,existingItem.SlotIndex))
                    {
                        //    //Set the slot Item
                        //    existingItem.transform.parent.name = existingItem.transform.name = mixedItem.ItemIns.Item.Name;
                        //    existingItem.GetComponent<Image>().sprite = mixedItem.ItemIns.Item.GetSprite();
                        //    var stackCntText = existingItem.transform.GetComponentInChildren<TextMeshProUGUI>();
                        //    stackCntText.text = mixedItem.ItemIns.UserItem.StackCnt > 1 ? mixedItem.ItemIns.UserItem.StackCnt.ToString() : "";
                        //    var newItemIns = new ItemIns(mixedItem.ItemIns.Item, mixedItem.ItemIns.UserItem);
                        //    ;
                        //    existingItem.ItemIns = newItemIns;

                        //Delete mixedItem Item
                        mixedItem.LoadEmpty();
                        _inv.UpdateInventory(true);
                    }
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
                    existingItem.ItemIns = equipedItem.ItemIns;
                    existingItem.transform.parent.name = existingItem.transform.name = existingItem.ItemIns.Item.Name;
                    existingItem.GetComponent<Image>().sprite = existingItem.ItemIns.Item.GetSprite();
                    var stackCntText = existingItem.transform.GetComponentInChildren<TextMeshProUGUI>();
                    stackCntText.text = existingItem.ItemIns.UserItem.StackCnt > 1 ? existingItem.ItemIns.UserItem.StackCnt.ToString() : "";
                    existingItem.ItemIns.UserItem.Order = existingItem.SlotIndex;
                    existingItem.ItemIns.UserItem.Equipped = false;
                    //unload new Item to equipments
                    equipedItem.LoadItem(null);
                    _inv.UpdateInventory(true);
                    _inv.UpdateEquipments(true);
                }
                return;
            }
        if (draggedItem != null)
            if (SlotIndex != draggedItem.SlotIndex )
            {
                //#################################### Empty slot logic swap
                if (existingItem.ItemIns == null)
                {
                    //Swap Parent
                    draggedItem.transform.SetParent(existingItem.transform.parent);
                    existingItem.transform.SetParent(draggedItem.Parent);
                    draggedItem.Parent = null;
                    //Swap positions
                    existingItem.transform.position = existingItem.transform.parent.position;
                    draggedItem.transform.position = draggedItem.transform.parent.position;
                    //Swap SlotIndex
                    var temp2 = existingItem.SlotIndex;
                    existingItem.SlotIndex = draggedItem.SlotIndex;
                    draggedItem.ItemIns.UserItem.Order = draggedItem.SlotIndex = temp2;
                    //Swapping names
                    existingItem.transform.parent.name = "Empty";
                    draggedItem.transform.parent.name = draggedItem.ItemIns.Item.Name;
                    _inv.UpdateInventory(true);
                }
                //#################################### Stacking: Same items Stack them together
                else if (existingItem.ItemIns.Item.Id == draggedItem.ItemIns.Item.Id )
                {
                    if (existingItem.ItemIns.UserItem.StackCnt + draggedItem.ItemIns.UserItem.StackCnt > existingItem.ItemIns.Item.MaxStackCnt)
                    {
                        draggedItem.ItemIns.UserItem.StackCnt = draggedItem.ItemIns.UserItem.StackCnt - (existingItem.ItemIns.Item.MaxStackCnt - existingItem.ItemIns.UserItem.StackCnt);
                        existingItem.ItemIns.UserItem.StackCnt = existingItem.ItemIns.Item.MaxStackCnt;
                    }
                    else
                    {
                        existingItem.ItemIns.UserItem.StackCnt = draggedItem.ItemIns.UserItem.StackCnt + existingItem.ItemIns.UserItem.StackCnt;
                        draggedItem.ItemIns.UserItem.StackCnt = 0;
                    }
                    var stackCntText = existingItem.transform.GetComponentInChildren<TextMeshProUGUI>();
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
                        //Swap Parent
                        draggedItem.transform.SetParent(existingItem.transform.parent);
                        existingItem.transform.SetParent(draggedItem.Parent);
                        draggedItem.Parent = null;
                        //Swap positions
                        existingItem.transform.position = existingItem.transform.parent.position;
                        draggedItem.transform.position = draggedItem.transform.parent.position;
                        //Swap SlotIndex
                        var temp2 = existingItem.SlotIndex;
                        existingItem.ItemIns.UserItem.Order = existingItem.SlotIndex = draggedItem.SlotIndex;
                        draggedItem.ItemIns.UserItem.Order= draggedItem.SlotIndex = temp2;
                        //Swapping names
                        existingItem.transform.parent.name = existingItem.ItemIns.Item.Name;
                        draggedItem.transform.parent.name = draggedItem.ItemIns.Item.Name;

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
        
        //Update stackCnt Text
        var stackCntText = existingItem.transform.GetComponentInChildren<TextMeshProUGUI>();
        stackCntText.text = existingItem.ItemIns.UserItem.StackCnt > 1 ? existingItem.ItemIns.UserItem.StackCnt.ToString() : "";

        stackCntText = draggedItem.transform.GetComponentInChildren<TextMeshProUGUI>();
        stackCntText.text = draggedItem.ItemIns.UserItem.StackCnt > 1 ? draggedItem.ItemIns.UserItem.StackCnt.ToString() : "";

        _inv.UpdateInventory(true);
    }
}