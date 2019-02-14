
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResearchSlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private static ResearchSlot _researchingSlot;
    public Sprite DefaultSprite;

    private CharacterManager _characterManager;
    private ModalPanel _modalPanel;
    private GUIManager _GUIManager;
    private Vector2 _offset;
    private Tooltip _tooltip;
    private Transform _parent;
    public bool ItemLocked;
    private bool _setEmpty;

    public Research _targetResearch;
    public int _level;
    private DateTime _time;


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
            texts[0].text = _level.ToString();
            //texts[1].color = Color.magenta;
            texts[1].text = TimeHandler.PrintTime(_time - DateTime.Now);
            if (DateTime.Now > _time)
            {
                //texts[1].color = Color.green;
                texts[1].text = "Ready";
                ItemLocked = false;
                _GUIManager.PrintMessage(_targetResearch.Name + " Is ready", Color.green);
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
        if (_targetResearch == null)
            return;
        _tooltip.Activate(_targetResearch);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_targetResearch == null)
            return;
        _tooltip.Deactivate();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (ItemLocked)
            return;
        if (_targetResearch == null)
            return;
        _offset = eventData.position - (Vector2)this.transform.position;
        this.transform.position = eventData.position - _offset;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ItemLocked)
            return;
        if (_targetResearch == null)
            return;
        _parent = transform.parent;
        this.transform.SetParent(this.transform.parent.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_targetResearch == null)
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
        if (_characterManager.UserPlayer.Gem > gem)
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
        if (_targetResearch == null)
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

        if (_targetResearch == null)
        {
            TextMeshProUGUI[] texts = this.transform.parent.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = "Empty";
        }
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    internal bool IsEmpty()
    {
        if (_targetResearch == null)
            return true;
        return false;
    }

    internal void LoadResearch(CharacterResearching researching)
    {
        LoadResearch(researching.ResearchId, researching.Level, researching.ResearchTime);
    }
    internal void LoadResearch(int researchId,int level, DateTime time)
    {
        _targetResearch = _characterManager.GetResearchById(researchId);
        _level = level;
        GetComponent<Image>().sprite = _targetResearch.GetSprite();
        _time = time;
        ItemLocked = true;
    }

    public void LoadEmpty()
    {
        ItemLocked = false;
        _targetResearch = null;
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

