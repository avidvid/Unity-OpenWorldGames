using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotDelete : MonoBehaviour, IDropHandler
{
    
    private ModalPanel _modalPanel;
    private CharacterManager _characterManager;

    void Start()
    {
        _modalPanel = ModalPanel.Instance();
        _characterManager =CharacterManager.Instance();
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        ItemData draggedItem = eventData.pointerDrag.GetComponent<ItemData>();

        if ( draggedItem == null)
            return;
        if (draggedItem.Item.Id ==-1)
            return;
        _modalPanel.Choice("Are you sure you want to Recycle this Item for %50 of Cost ?", ModalPanel.ModalPanelType.YesCancel,() => { DeleteItem(draggedItem.Item); });
    }

    private void DeleteItem(ItemContainer item)
    {
        _characterManager.AddCharacterSetting("Coin", (int) (item.StackCnt*item.Cost)/2);
        item.setStackCnt(0);
    }

}
