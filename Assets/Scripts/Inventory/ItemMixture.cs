using System;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class ItemMixture : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private static ItemMixture _itemMixture;
    private CharacterManager _characterManager;
    private InventoryHandler _inv;
    private ModalPanel _modalPanel;

    private Vector2 _offset;
    private Tooltip _tooltip;
    private Transform _parent;
    public Sprite DefaultSprite;
    public bool ItemLocked;

    private DateTime _time;

    public ItemIns ItemIns;
    private int _stackCnt;
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
                _inv.PrintMessage(ItemIns.Item.Name + " Is ready", Color.green);
            }
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
        if (ItemIns == null)
            return;
        _offset = eventData.position - (Vector2)this.transform.position;
        this.transform.position = eventData.position - _offset;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ItemLocked)
            return;
        if (ItemIns == null)
            return;
        _parent = transform.parent;
        this.transform.SetParent(this.transform.parent.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ItemIns == null)
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
        if (ItemIns == null)
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

        if (ItemIns == null)
        {
            TextMeshProUGUI[] texts = this.transform.parent.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = "";
            texts[1].text = "Empty";
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    internal void LoadItem(CharacterMixture playerMixture)
    {
        LoadItem(playerMixture.ItemId, playerMixture.StackCnt, playerMixture.MixTime);
    }
    internal void LoadItem(int itemId, int stackCnt, int durationMinutes)
    {
        float speed = _inv.GetCrafting();
        _time = DateTime.Now.AddMinutes(durationMinutes * (1-speed));
        _inv.SaveCharacterMixture(itemId, stackCnt, _time);
        LoadItem(itemId,stackCnt, _time);
    }

    internal void LoadItem(int itemId,int stackCnt, DateTime time)
    {
        var itemDatabase = ItemDatabase.Instance();
        var item = itemDatabase.GetItemById(itemId);
        ItemIns = new ItemIns(item,new UserItem(item, stackCnt));
        _stackCnt = stackCnt;
        GetComponent<Image>().sprite = item.GetSprite();
        _time = time;
        ItemLocked = true;

        TextMeshProUGUI[] texts = this.transform.parent.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = _stackCnt > 1 ? _stackCnt.ToString() : "";
        texts[1].text = (_time - DateTime.Now).ToString();
    }




    public void LoadEmpty()
    {
        ItemIns = null;
        GetComponent<Image>().sprite = DefaultSprite;
        _time = DateTime.MinValue;
        ItemLocked = false;
        TextMeshProUGUI[] texts = this.transform.parent.GetComponentsInChildren<TextMeshProUGUI>();
        texts[1].text = "Empty";
        //Save empty item in mixture
        _inv.SaveCharacterMixture(0,0, DateTime.Now);
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

    internal bool IsEmpty()
    {
        if (ItemIns == null)
            return true;
        return false;
    }
}
