using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemEquipment : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public ItemContainer Item;
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
        if (Item.Id == -1)
            return;
        if (Item.StackCnt == 0)
        {
            LoadItem();
            _inv.UpdateEquipments(true);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        _tooltip.Activate(Item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        _tooltip.Dectivate();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        _offset = eventData.position - (Vector2)this.transform.position;
        this.transform.position = eventData.position - _offset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        _parent = transform.parent;
        this.transform.SetParent(this.transform.parent.parent.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        this.transform.position = eventData.position - _offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_parent == null)
            return;
        this.transform.position = _parent.position;
        this.transform.SetParent(_parent);
        this.transform.SetSiblingIndex(0);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void LoadItem(ItemContainer item =null)
    {
        if (item == null)
        {
            Item = new ItemContainer();
            GetComponent<Image>().sprite = EmptySprite;
            this.transform.name = "Empty";
        }
        else
        {
            Item = item;
            Sprite itemSprite = Item.GetSprite();
            GetComponent<Image>().sprite = itemSprite!=null? itemSprite:EmptySprite;
            this.transform.name = Item.Name;
        }
    }

}
