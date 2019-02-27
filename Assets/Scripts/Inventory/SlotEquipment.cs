using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotEquipment : MonoBehaviour, IDropHandler
{
    public OupItem.PlaceType EquType;
    // Use this for initialization
    private InventoryHandler _inv;

    void Start()
    {
        _inv = InventoryHandler.Instance();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnDrop(PointerEventData eventData)
    {
        ItemData draggedItem = eventData.pointerDrag.GetComponent<ItemData>();
        if (draggedItem == null)
            return;
        if (draggedItem.ItemIns == null)
            return;
        var item = draggedItem.ItemIns.Item;
        var userItem = draggedItem.ItemIns.UserItem;
        //Wearing Equipment
        if (EquType != OupItem.PlaceType.None)
        {
            switch (item.Type)
            {
                case OupItem.ItemType.Equipment:
                    if (item.PlaceHolder == EquType)
                        if (userItem.StackCnt == item.MaxStackCnt)
                        {
                            ItemEquipment existingEquipment = this.transform.GetComponentInChildren<ItemEquipment>();
                            if (_inv.UseItem(draggedItem.ItemIns))
                            {
                                //Swap ItemIns
                                var itemEquipment = existingEquipment.ItemIns;
                                draggedItem.ItemIns.UserItem.Order = (int)EquType;
                                draggedItem.ItemIns.UserItem.Equipped = true;
                                existingEquipment.LoadItem(draggedItem.ItemIns);
                                draggedItem.LoadItem(itemEquipment);
                            }
                        }
                        else
                            _inv.PrintMessage("You need " + (item.MaxStackCnt - userItem.StackCnt) + " of this item to equip", Color.yellow);
                    else
                        _inv.PrintMessage("You cannot equip this item here", Color.yellow);
                    break;
                case OupItem.ItemType.Weapon:
                case OupItem.ItemType.Tool:
                    //Todo: Add the logic of two hand item 
                    if (EquType == OupItem.PlaceType.Left || EquType == OupItem.PlaceType.Right)
                        if (item.CarryType == OupItem.Hands.OneHand)
                        {
                            //Todo: Add logic of hands carry 
                            ItemEquipment existingEquipment = this.transform.GetComponentInChildren<ItemEquipment>();
                            if (_inv.UseItem(draggedItem.ItemIns))
                            {
                                //Swap ItemIns
                                var itemEquipment = existingEquipment.ItemIns;
                                draggedItem.ItemIns.UserItem.Order = (int) EquType;
                                draggedItem.ItemIns.UserItem.Equipped = true;
                                existingEquipment.LoadItem(draggedItem.ItemIns);
                                draggedItem.LoadItem(itemEquipment);
                            }
                        }
                        else
                            _inv.PrintMessage("It is not possible to carry this weapon yet", Color.yellow);
                    else
                        _inv.PrintMessage("You cannot equip this item here", Color.yellow);
                    break;
            }
        }
    }
}