using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ItemMixture : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private static ItemMixture _itemMixture;
    private CharacterManager _characterManager;
    private ModalPanel _modalPanel;
    private InventoryHandler _inv;
    private Vector2 _offset;
    private Tooltip _tooltip;
    private DateTime _time;
    private Transform _parent;
    public Sprite DefaultSprite;
    public ItemContainer Item;
    public bool ItemLocked;
    void Awake()
    {
        _itemMixture = ItemMixture.Instance();
        _inv = InventoryHandler.Instance();
        _tooltip = Tooltip.Instance();
        _characterManager = CharacterManager.Instance();
        _modalPanel = ModalPanel.Instance();
    }
    // Update is called once per frame
    void Update()
    {
        if (ItemLocked)
        {
            TextMeshProUGUI[] texts = this.transform.parent.GetComponentsInChildren<TextMeshProUGUI>();
            //texts[1].color = Color.magenta;
            texts[1].text = TimeHandler.PrintTime(_time - DateTime.Now);
            if (DateTime.Now > _time)
            {
                //texts[1].color = Color.green;
                texts[1].text = "Ready";
                ItemLocked = false;
                _inv.PrintMessage(Item.Name + " Is ready", Color.green);
            }
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
        _tooltip.Deactivate();
    }

    internal bool ReadyToUse()
    {
        if (DateTime.Now > _time)
            return true;
        return false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (ItemLocked)
            return;
        if (Item.Id == -1)
            return;
        _offset = eventData.position - (Vector2)this.transform.position;
        this.transform.position = eventData.position - _offset;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ItemLocked)
            return;
        if (Item.Id == -1)
            return;
        _parent = transform.parent;
        this.transform.SetParent(this.transform.parent.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Item.Id == -1)
            return;
        if (_time < DateTime.Now)
            SceneSettings.GoToRecipeScene();
        else
            _modalPanel.Warning("Are you sure you want to buy out your wait time for " + TimeHandler.GemTimeValue(_time - DateTime.Now) + " gem(s)?",
                ModalPanel.ModalPanelType.YesNo,
                SpendGem,
                SceneSettings.GoToRecipeScene);
    }

    private void SpendGem()
    {
        var timer = _time - DateTime.Now;
        var gem = TimeHandler.GemTimeValue(timer);
        if (_characterManager.UserPlayer.Gem > gem)
            _characterManager.AddCharacterSetting("Gem", -gem);
        else
        {
            _modalPanel.Choice("You don't have enough Gem ! ", ModalPanel.ModalPanelType.Ok);
            SceneSettings.GoToShopScene("Gem");
            return;
        }
        _characterManager.SetCharacterMixtureTime(DateTime.Now);
        _time = DateTime.Now;
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (ItemLocked)
            return;
        if (Item.Id == -1)
            return;
        this.transform.position = eventData.position - _offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (ItemLocked)
            return;
        this.transform.position = _parent.position;
        this.transform.SetParent(_parent);
        this.transform.SetSiblingIndex(0);

        if (Item.Id == -1)
        {
            TextMeshProUGUI[] texts = this.transform.parent.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = "";
            texts[1].text = "Empty";
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    internal void LoadItem(ItemContainer item, int durationMinutes)
    {
        float speed = _inv.GetCrafting();
        _time = DateTime.Now.AddMinutes(durationMinutes * (1-speed));
        _inv.SaveCharacterMixture(item, _time);
        LoadItem(item, _time);
    }

    internal void LoadItem(ItemContainer item, DateTime time)
    {
        Item = item;
        GetComponent<Image>().sprite = Item.GetSprite();
        _time = time;
        ItemLocked = true;

        TextMeshProUGUI[] texts = this.transform.parent.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = item.StackCnt > 1 ? item.StackCnt.ToString() : "";
        texts[1].text = (_time - DateTime.Now).ToString();
    }

    public void LoadEmpty()
    {
        Item = new ItemContainer();
        GetComponent<Image>().sprite = DefaultSprite;
        _time = DateTime.MinValue;
        ItemLocked = false;
        TextMeshProUGUI[] texts = this.transform.parent.GetComponentsInChildren<TextMeshProUGUI>();
        texts[1].text = "Empty";
    }


    public static ItemMixture Instance()
    {
        if (!_itemMixture)
        {
            _itemMixture = FindObjectOfType(typeof(ItemMixture)) as ItemMixture;
            if (!_itemMixture)
                Debug.LogError("There needs to be one active ItemMixture script on a GameObject in your scene.");
        }
        return _itemMixture;
    }


}
