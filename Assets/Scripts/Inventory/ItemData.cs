using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemData : MonoBehaviour,IPointerDownHandler,IBeginDragHandler,IDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler
{

	// Use this for initialization
    public ItemIns ItemIns;
    public int SlotIndex;
    public Transform Parent;


    private Vector2 _offset;
    private InventoryHandler _inv;
    private Tooltip _tooltip;

    public Sprite EmptySprite;

    void Awake()
    {
        _inv = InventoryHandler.Instance();
        _tooltip = Tooltip.Instance();
    }

    void Update()
    {
        if (ItemIns == null)
            return;
        if (ItemIns.UserItem.StackCnt == 0)
        {
            //Logic of adding empty Item to Slot 
            ItemIns = null;
            this.transform.name = "Empty";
            this.GetComponent<Image>().sprite = EmptySprite;
            //Update Text
            this.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            _inv.InvSlots[SlotIndex].transform.name = this.transform.name;
            _inv.UpdateInventory(true);
        }
        else
        {
            var stackCntText = this.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (ItemIns.UserItem.StackCnt.ToString() != stackCntText.text)
                stackCntText.text = ItemIns.UserItem.StackCnt > 1 ? ItemIns.UserItem.StackCnt.ToString() : "";
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ItemIns == null)
            return;
        _tooltip.Activate(ItemIns);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemIns == null)
            return;
        _tooltip.Deactivate();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Todo: you can move the sprite by clicking couple time fix it 
        if (ItemIns == null)
            return;
        _offset = eventData.position - (Vector2) this.transform.position;
        this.transform.position = eventData.position - _offset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ItemIns == null)
            return;
        Parent = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ItemIns == null)
            return;
        this.transform.position = eventData.position - _offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Parent != null)
        {
            this.transform.SetParent(Parent);
            this.transform.position = Parent.position;
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    internal void LoadItem()
    {
        print("Item data loading " + (ItemIns==null?"Empty": ItemIns.Item.Name) );
        if (ItemIns == null)
        {
            this.ItemIns = null;
            GetComponent<Image>().sprite = EmptySprite;
            var stackCntText = this.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            stackCntText.text = "";
            transform.parent.name = this.transform.name = "Empty";
        }
        else
        {
            transform.parent.name = this.transform.name = ItemIns.Item.Name;
            this.ItemIns.UserItem.Order = SlotIndex;
            this.ItemIns.UserItem.Equipped = false;
            GetComponent<Image>().sprite = ItemIns.Item.GetSprite();
            this.transform.GetComponentInChildren<TextMeshProUGUI>().text = ItemIns.UserItem.StackCnt > 1 ? ItemIns.UserItem.StackCnt.ToString() : "";
        }
        _inv.UpdateInventory(true);
    }

    internal void LoadItem(ItemIns itemIns)
    {
        this.ItemIns = itemIns;
        LoadItem();
    }
}
