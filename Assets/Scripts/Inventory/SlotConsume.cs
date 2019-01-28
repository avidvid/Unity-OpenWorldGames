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
        _characterPic.image.sprite =  _characterManager.Character.GetSprite();
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
        if (draggedItem.Item.Id == -1)
            return;
        ItemEquipment existingEquipment;
        ItemContainer itemEquipment;
        switch (draggedItem.Item.Type)
        {
            case Item.ItemType.Consumable:
                //Use all the stack
                if (_inv.UseItem(draggedItem.Item))
                    draggedItem.Item.setStackCnt(0);
                break;
            case Item.ItemType.Equipment:
                if (draggedItem.Item.StackCnt == draggedItem.Item.MaxStackCnt)
                {
                    existingEquipment = _inv.EquiSlots[(int)draggedItem.Item.Equipment.PlaceHolder]
                        .GetComponentInChildren<ItemEquipment>();
                    itemEquipment = existingEquipment.Item;
                    if (itemEquipment.Id == draggedItem.Item.Id)
                    {
                        _inv.PrintMessage("You are equipped with the same Equipment" ,Color.yellow);
                        break;
                    }
                    if (_inv.UseItem(draggedItem.Item))
                    {
                        //load new Item to equipments
                        existingEquipment.LoadItem(draggedItem.Item);
                        //unload new Item to equipments
                        draggedItem.LoadItem(itemEquipment);
                        _inv.UnUseItem(itemEquipment);
                        _inv.UpdateInventory(true);
                        _inv.UpdateEquipments(true);
                    }
                }
                else
                    _inv.PrintMessage(
                        "You need " + (draggedItem.Item.MaxStackCnt - draggedItem.Item.StackCnt) + " of this item to equip",
                        Color.yellow);
                break;
            case Item.ItemType.Weapon:
                //Todo: Add the logic of two hand item 
                existingEquipment = _inv.EquiSlots[(int) Equipment.PlaceType.Left].GetComponentInChildren<ItemEquipment>();
                itemEquipment = existingEquipment.Item;
                if (itemEquipment.Id != -1)
                {
                    existingEquipment = _inv.EquiSlots[(int) Equipment.PlaceType.Right]
                        .GetComponentInChildren<ItemEquipment>();
                    itemEquipment = existingEquipment.Item;
                }
                if (draggedItem.Item.CarryType == Weapon.Hands.OneHand)
                {
                    if (_inv.UseItem(draggedItem.Item))
                    {
                        //load new Item to equipments
                        existingEquipment.LoadItem(draggedItem.Item);
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
            case Item.ItemType.Tool:
                existingEquipment = _inv.EquiSlots[(int) Equipment.PlaceType.Left].GetComponentInChildren<ItemEquipment>();
                itemEquipment = existingEquipment.Item;
                if (itemEquipment.Id != -1)
                {
                    existingEquipment = _inv.EquiSlots[(int) Equipment.PlaceType.Right]
                        .GetComponentInChildren<ItemEquipment>();
                    itemEquipment = existingEquipment.Item;
                }
                existingEquipment.LoadItem(draggedItem.Item);
                draggedItem.LoadItem(itemEquipment);
                _inv.UpdateInventory(true);
                _inv.UpdateEquipments(true);
                break;
            default:
                _GUIManager.PrintMessage(draggedItem.Item.Name + " can not be used", Color.yellow);
                break;
        }
    }
    private void ConsumeResearch(ResearchSlot draggedResearch)
    {
        if (draggedResearch.CharResearch.Id == -1)
            return;
        _characterManager.CharacterSettingApplyResearch(draggedResearch.CharResearch, draggedResearch.TargetResearch);
    }
}