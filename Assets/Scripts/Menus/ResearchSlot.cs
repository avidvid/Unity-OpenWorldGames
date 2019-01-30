
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResearchSlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private static ResearchSlot _researchingSlot;
    public CharacterResearch CharResearch;
    public Research TargetResearch;
    public Sprite DefaultSprite;

    private CharacterManager _characterManager;
    private ModalPanel _modalPanel;
    private GUIManager _GUIManager;
    private Vector2 _offset;
    private Tooltip _tooltip;
    private DateTime _time;
    private Transform _parent;
    public bool ItemLocked;
    private bool _setEmpty;

    void Awake()
    {
        _researchingSlot = ResearchSlot.Instance();
        _tooltip = Tooltip.Instance();
        _GUIManager = GUIManager.Instance();
        _characterManager = CharacterManager.Instance();
        _modalPanel = ModalPanel.Instance();
    }
    // Update is called once per frame
    void Update()
    {
        if (ItemLocked)
        {
            TextMeshProUGUI[] texts = this.transform.parent.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = CharResearch.Level.ToString();
            //texts[1].color = Color.magenta;
            texts[1].text = TimeHandler.PrintTime(_time - DateTime.Now);
            if (DateTime.Now > _time)
            {
                //texts[1].color = Color.green;
                texts[1].text = "Ready";
                ItemLocked = false;
                _GUIManager.PrintMessage(TargetResearch.Name + " Is ready", Color.green);
            }
        }
        if (_setEmpty)
        {
            TextMeshProUGUI[] texts = this.transform.parent.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = "";
            texts[1].text = "Empty";
        }
    }
    internal bool ReadyToUse()
    {
        if (DateTime.Now > _time)
            return true;
        return false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CharResearch.Id == -1)
            return;
        _tooltip.Activate(TargetResearch);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (CharResearch.Id == -1)
            return;
        _tooltip.Dectivate();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (ItemLocked)
            return;
        if (CharResearch.Id == -1)
            return;
        _offset = eventData.position - (Vector2)this.transform.position;
        this.transform.position = eventData.position - _offset;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ItemLocked)
            return;
        if (CharResearch.Id == -1)
            return;
        _parent = transform.parent;
        this.transform.SetParent(this.transform.parent.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (CharResearch.Id == -1)
            return;
        if (_time < DateTime.Now)
            SceneSettings.GoToResearchScene();
        else
            _modalPanel.Warning("Are you sure you want to buy out your wait time for " + TimeHandler.GemTimeValue(_time - DateTime.Now) + " gem(s)?",
                ModalPanel.ModalPanelType.YesNo,
                SpendGem,
                SceneSettings.GoToResearchScene);
    }
    private void SpendGem()
    {
        var gem = TimeHandler.GemTimeValue(_time - DateTime.Now);
        if (_characterManager.CharacterSetting.Gem > gem)
            _characterManager.AddCharacterSetting("Gem", -gem);
        else
        {
            _modalPanel.Choice("You don't have enough Gem ! ", ModalPanel.ModalPanelType.Ok);
            SceneSettings.GoToShopScene("Gem");
            return;
        }
        _characterManager.SetCharacterResearchingTime(DateTime.Now);
        _time = DateTime.Now;
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (ItemLocked)
            return;
        if (CharResearch.Id == -1)
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

        if (CharResearch.Id == -1)
        {
            TextMeshProUGUI[] texts = this.transform.parent.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = "Empty";
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    internal void LoadResearch(CharacterResearch charResearch, DateTime time)
    {
        CharResearch = new CharacterResearch(charResearch.ResearchId, charResearch.UserPlayerId, charResearch.Level);
        TargetResearch = _characterManager.FindResearch(CharResearch.ResearchId);
        GetComponent<Image>().sprite = TargetResearch.GetSprite();
        _time = time;
        ItemLocked = true;
    }

    public void LoadEmpty()
    {
        ItemLocked = false;
        CharResearch = new CharacterResearch();
        GetComponent<Image>().sprite = DefaultSprite;
        _time = DateTime.MinValue;
        _setEmpty = true;
    }
    public static ResearchSlot Instance()
    {
        if (!_researchingSlot)
        {
            _researchingSlot = FindObjectOfType(typeof(ResearchSlot)) as ResearchSlot;
            if (!_researchingSlot)
                Debug.LogError("There needs to be one active ItemMixture script on a GameObject in your scene.");
        }
        return _researchingSlot;
    }
}

