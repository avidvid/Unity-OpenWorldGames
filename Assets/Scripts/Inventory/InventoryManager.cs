using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _inventoryManager;
    private CharacterManager _characterManager;
    private UserDatabase _userDatabase;

    internal List<ItemIns> UserInvItems;

    internal bool UpdateInventory;

    #region InventoryManager Instance
    public static InventoryManager Instance()
    {
        if (!_inventoryManager)
        {
            _inventoryManager = FindObjectOfType(typeof(InventoryManager)) as InventoryManager;
            if (!_inventoryManager)
                Debug.LogError("There needs to be one active InventoryManager script on a GameObject in your scene.");
        }
        return _inventoryManager;
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("***IM*** Start!");
        _inventoryManager = InventoryManager.Instance();
        _characterManager = CharacterManager.Instance();
        _userDatabase = UserDatabase.Instance();

        RefreshInventory();

        ValidateInventory();
        Debug.Log("IM-UserInvItems.Count = " + UserInvItems.Count);
    }
    private void RefreshInventory()
    {
        UserInvItems = _characterManager.CharacterInventory;
    }
    private void ValidateInventory()
    {
        for (int i = 0; i < UserInvItems.Count; i++)
        {
            for (int j = 0; j < UserInvItems.Count; j++)
                if (i != j 
                    && UserInvItems[i].UserItem.Order == UserInvItems[j].UserItem.Order 
                    && UserInvItems[i].UserItem.Equipped == UserInvItems[j].UserItem.Equipped
                    && UserInvItems[i].UserItem.Stored == UserInvItems[j].UserItem.Stored)
                    throw new Exception("IM-UserInvItems Invalid Order!!!" + UserInvItems[i].UserItem.MyInfo() +" && "+ UserInvItems[j].UserItem.MyInfo());
            if (UserInvItems[i].Item.Type == ItemContainer.ItemType.Equipment && UserInvItems[i].UserItem.Equipped)
                if ((int)UserInvItems[i].Item.PlaceHolder != UserInvItems[i].UserItem.Order)
                    throw new Exception("IM-UserInvItems Invalid Equipment PlaceHolder!!! " + UserInvItems[i].UserItem.MyInfo());
            if ((UserInvItems[i].Item.Type == ItemContainer.ItemType.Tool 
                || UserInvItems[i].Item.Type == ItemContainer.ItemType.Weapon) 
                && UserInvItems[i].UserItem.Equipped
                && UserInvItems[i].UserItem.Order != (int)ItemContainer.PlaceType.Left 
                && UserInvItems[i].UserItem.Order != (int)ItemContainer.PlaceType.Right)
                    throw new Exception("IM-UserInvItems Invalid Weapon/Tool Hand!!! "+ UserInvItems[i].UserItem.MyInfo());
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (UpdateInventory)
        {
            ValidateInventory();
            _userDatabase.UpdateUserInventory();
            UpdateInventory = false;
        }
    }
    internal bool HaveAvailableSlot()
    {
        var carryItems = UserInvItems.FindAll(l => !l.UserItem.Equipped && !l.UserItem.Stored).Count;
        return _characterManager.CharacterSetting.CarryCnt > carryItems;
    }

    internal void PrintInventory()
    {
        var userInv = "";
        foreach (var itemIns in UserInvItems)
            userInv += itemIns.UserItem.Order +
                    (itemIns.UserItem.Stored ? " (S)-" : "-") +
                    (itemIns.UserItem.Equipped ? " (E)-" : "-") +
                     itemIns.Item.Name + "(" + itemIns.UserItem.StackCnt + ")#";
        Debug.Log("IM-PrintInventory =" + userInv);
    }
    internal void AddItemToInventory(ItemIns itemIns)
    {
        Debug.Log("IM-Item " + itemIns.MyInfo() + " Will be added to order " + itemIns.UserItem.Order);
        _characterManager.CharacterInventory.Add(itemIns);
        _userDatabase.UpdateUserInventory(itemIns.UserItem);
        RefreshInventory();
    }
    internal void DeleteItemFromInventory(ItemIns itemIns)
    {
        Debug.Log("IM-Item " + itemIns.MyInfo() + " Will be Deleted from Order " + itemIns.UserItem.Order);
        _characterManager.CharacterInventory.Remove(itemIns);
        _userDatabase.DeleteItemFromInventory(itemIns.UserItem);
        RefreshInventory();
    }
}