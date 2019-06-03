using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotDelete : MonoBehaviour, IDropHandler
{
    private MessagePanelHandler _messagePanelHandler;
    private CharacterManager _characterManager;
    private InventoryHandler _inv;

    void Start()
    {
        _messagePanelHandler = MessagePanelHandler.Instance();
        _characterManager =CharacterManager.Instance();
        _inv = InventoryHandler.Instance();
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        ItemData draggedItem = eventData.pointerDrag.GetComponent<ItemData>();

        if ( draggedItem == null)
            return;
        if (draggedItem.ItemIns == null)
            return;
        _messagePanelHandler.ShowMessage("Are you sure you want to Recycle this Item for %50 of Cost ?", MessagePanel.PanelType.YesCancel,() => { DeleteItem(draggedItem.ItemIns); });
    }

    private void DeleteItem(ItemIns itemIns)
    {
        _characterManager.AddCharacterSetting("Coin", (int) (itemIns.UserItem.StackCnt * itemIns.Item.Cost /2));
        itemIns.UserItem.StackCnt = 0;
        _inv.UpdateInventory();
    }

}
