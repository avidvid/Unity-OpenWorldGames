using System.Collections;
using Facebook.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotConsume: MonoBehaviour, IDropHandler
{
    private FacebookHandler _facebook;
    private InventoryHandler _inv;
    private static ModalPanel _modalPanel;
    private static GUIManager _GUIManager;
    private CharacterManager _characterManager;

    private  Button _playerPic;
    private Button _characterPic;
    private TextMeshProUGUI _characterName;

    // Use this for initialization
    void Awake()
    {
        _inv = InventoryHandler.Instance();
        _characterManager = CharacterManager.Instance();
        _GUIManager = GUIManager.Instance();
    }
    void Start()
    {
        if (_characterManager.UserPlayer.FBLoggedIn)
            _facebook = FacebookHandler.Instance();
        _playerPic = GameObject.Find("PlayerPic").GetComponent<Button>();
        _characterPic = GameObject.Find("CharacterPic").GetComponent<Button>();
        _characterName = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>(); ;
        _characterPic.image.sprite =  _characterManager.MyCharacter.GetSprite();
        _characterName.text = _characterManager.CharacterSetting.Name;
        StartCoroutine("LoadProfilePicture");
    }
    private IEnumerator LoadProfilePicture()
    {
        if (_facebook != null)
            if (FB.IsLoggedIn)
                _playerPic.image.sprite = _facebook.GetProfilePic();
        yield return "Done";
    }
    public void OnDrop(PointerEventData eventData)
    {
        ItemData draggedItem = eventData.pointerDrag.GetComponent<ItemData>();
        ResearchSlot draggedResearch = eventData.pointerDrag.GetComponent<ResearchSlot>();
        if (draggedItem == null && draggedResearch == null)
            return;
        if (draggedResearch != null)
        {
            if (draggedResearch.ReadyToUse())
            {
                ResearchSlot researchingSlot = ResearchSlot.Instance();
                ConsumeResearch(draggedResearch);
                researchingSlot.LoadEmpty();
            }
            return;
        }
        ConsumeItem(draggedItem);
    }
    private void ConsumeItem(ItemData draggedItem)
    {
        if (draggedItem.ItemIns == null)
            return;
        switch (draggedItem.ItemIns.Item.Type)
        {
            case ItemContainer.ItemType.Consumable:
                //Use all the stack
                if (_inv.UseItem(draggedItem.ItemIns))
                {
                    _inv.UpdateInventory();
                    draggedItem.ItemIns.UserItem.StackCnt = 0;
                }
                break;
            case ItemContainer.ItemType.Equipment:
            case ItemContainer.ItemType.Weapon:
            case ItemContainer.ItemType.Tool:
            case ItemContainer.ItemType.Substance:
                _inv.PrintMessage("You can not consume this item", Color.yellow);
                break;
            default:
                _GUIManager.PrintMessage(draggedItem.ItemIns.Item.Name + " can not be used", Color.yellow);
                break;
        }
    }
    private void ConsumeResearch(ResearchSlot draggedResearch)
    {
        if (draggedResearch.TargetResearch == null)
            return;
        _characterManager.CharacterSettingApplyResearch(draggedResearch.TargetResearch, draggedResearch.Level);
    }
}