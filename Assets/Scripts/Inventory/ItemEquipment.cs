using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemEquipment : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemIns ItemIns;
    public Sprite EmptySprite;


    private InventoryHandler _inv;
    private Tooltip _tooltip;
    private Vector2 _offset;
    private Transform _parent;

    // Use this for initialization
    void Start ()
    {
        _inv = InventoryHandler.Instance();
        _tooltip = Tooltip.Instance();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (ItemIns == null)
            return;
        if (ItemIns.UserItem.StackCnt == 0)
        {
            LoadItem(null);
            _inv.UpdateInventory();
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
        if (ItemIns == null)
            return;
        _offset = eventData.position - (Vector2)this.transform.position;
        this.transform.position = eventData.position - _offset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ItemIns == null)
            return;
        _parent = transform.parent;
        this.transform.SetParent(this.transform.parent.parent.parent.parent);
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
        if (_parent == null)
            return;
        this.transform.SetParent(_parent);
        this.transform.position = _parent.position;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    internal void LoadItem()
    {
        if (ItemIns == null)
        {
            this.ItemIns = null;
            GetComponent<Image>().sprite = EmptySprite;
            this.transform.name = "Empty";
        }
        else
        {
            Sprite itemSprite = ItemIns.Item.GetSprite();
            GetComponent<Image>().sprite = itemSprite != null ? itemSprite : EmptySprite;
            this.transform.name = ItemIns.Item.Name;
        }
        _inv.UpdateInventory();

    }
    internal void LoadItem(ItemIns itemIns)
    {
        if (ItemIns!=null)
            _inv.UnUseItem(ItemIns);
        this.ItemIns = itemIns;
        LoadItem();
    }

    internal void UseItem(int count)
    {
        ItemIns.UserItem.TimeToUse-= count;
        if (ItemIns.UserItem.TimeToUse <=0)
            ItemIns.UserItem.StackCnt = 0;
    }
}
