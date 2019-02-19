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
        if (_characterManager.UserPlayer.FBLoggedIn)
            _facebook = FacebookHandler.Instance();
    }

    void Start()
    {
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
        if (draggedItem != null)
            ConsumeItem(draggedItem);

        if (draggedResearch != null)
        {
            ResearchSlot researchingSlot = ResearchSlot.Instance();
            if (researchingSlot.ReadyToUse())
            {
                ConsumeResearch(draggedResearch);
                researchingSlot.LoadEmpty();
            }
        }
    }

    private void ConsumeItem(ItemData draggedItem)
    {
        if (draggedItem.ItemIns == null)
            return;
        ItemEquipment existingEquipment;
        ItemIns itemEquipment;
        switch (draggedItem.ItemIns.Item.Type)
        {
            case OupItem.ItemType.Consumable:
                //Use all the stack
                if (_inv.UseItem(draggedItem.ItemIns))
                    draggedItem.ItemIns.UserItem.StackCnt = 0;
                break;
            case OupItem.ItemType.Equipment:
                if (draggedItem.ItemIns.UserItem.StackCnt == draggedItem.ItemIns.Item.MaxStackCnt)
                {
                    existingEquipment = _inv.EquiSlots[(int)draggedItem.ItemIns.Item.PlaceHolder]
                        .GetComponentInChildren<ItemEquipment>();
                    itemEquipment = existingEquipment.ItemIns;
                    if (itemEquipment.Item.Id == draggedItem.ItemIns.Item.Id)
                    {
                        _inv.PrintMessage("You are equipped with the same Equipment" ,Color.yellow);
                        break;
                    }
                    if (_inv.UseItem(draggedItem.ItemIns))
                    {
                        //load new Item to equipments
                        existingEquipment.LoadItem(draggedItem.ItemIns);
                        //unload new Item to equipments
                        draggedItem.LoadItem(itemEquipment);
                        _inv.UnUseItem(itemEquipment);
                        _inv.UpdateInventory(true);
                        _inv.UpdateEquipments(true);
                    }
                }
                else
                    _inv.PrintMessage(
                        "You need " + (draggedItem.ItemIns.Item.MaxStackCnt - draggedItem.ItemIns.UserItem.StackCnt) + " of this item to equip",
                        Color.yellow);
                break;
            case OupItem.ItemType.Weapon:
                //Todo: Add the logic of two hand item 
                existingEquipment = _inv.EquiSlots[(int)OupItem.PlaceType.Left].GetComponentInChildren<ItemEquipment>();
                itemEquipment = existingEquipment.ItemIns;
                if (itemEquipment!= null)
                {
                    existingEquipment = _inv.EquiSlots[(int)OupItem.PlaceType.Right]
                        .GetComponentInChildren<ItemEquipment>();
                    itemEquipment = existingEquipment.ItemIns;
                }
                if (draggedItem.ItemIns.Item.CarryType == OupItem.Hands.OneHand)
                {
                    if (_inv.UseItem(draggedItem.ItemIns))
                    {
                        //load new Item to equipments
                        existingEquipment.LoadItem(draggedItem.ItemIns);
                        //unload new Item to equipments
                        draggedItem.LoadItem(itemEquipment);
                        _inv.UnUseItem(itemEquipment);
                        _inv.UpdateInventory(true);
                        _inv.UpdateEquipments(true);
                    }
                }
                else
                    _inv.PrintMessage("It is not possible to carry this weapon yet", Color.yellow);
                break;
            case OupItem.ItemType.Tool:
                existingEquipment = _inv.EquiSlots[(int)OupItem.PlaceType.Left].GetComponentInChildren<ItemEquipment>();
                itemEquipment = existingEquipment.ItemIns;
                if (itemEquipment != null)
                {
                    existingEquipment = _inv.EquiSlots[(int)OupItem.PlaceType.Right]
                        .GetComponentInChildren<ItemEquipment>();
                    itemEquipment = existingEquipment.ItemIns;
                }
                existingEquipment.LoadItem(draggedItem.ItemIns);
                draggedItem.LoadItem(itemEquipment);
                _inv.UpdateInventory(true);
                _inv.UpdateEquipments(true);
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