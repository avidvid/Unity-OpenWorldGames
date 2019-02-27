using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _inventoryManager;
    private CharacterManager _characterManager;
    private UserDatabase _userDatabase;

    internal List<ItemIns> InvCarry = new List<ItemIns>();
    internal List<ItemIns> InvEquipment = new List<ItemIns>();
    internal List<ItemIns> InvBank = new List<ItemIns>();


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
    void Awake()
    {
        _inventoryManager = InventoryManager.Instance();
        _characterManager = CharacterManager.Instance();
        _userDatabase = UserDatabase.Instance();
    }

    void Start()
    {
        RefreshInventory();

        ValidateInventory();
        Debug.Log("IM-InvCarry.Count = " + InvCarry.Count);
        Debug.Log("IM-InvEquipment.Count = " + InvEquipment.Count);
        Debug.Log("IM-InvBank.Count = " + InvBank.Count);
    }

    private void RefreshInventory()
    {
        var userInvItems = _characterManager.CharacterInventory;
        InvEquipment.Clear();
        InvBank.Clear();
        InvCarry.Clear();
        foreach (var itemIns in userInvItems)
        {
            if (itemIns.UserItem.Equipped)
                InvEquipment.Add(itemIns);
            else if (itemIns.UserItem.Stored)
                InvBank.Add(itemIns);
            else
                InvCarry.Add(itemIns);
        }
    }

    private void ValidateInventory()
    {
        for (int i = 0; i < InvCarry.Count; i++)
            for (int j = 0; j < InvCarry.Count; j++)
                if (i != j && InvCarry[i].UserItem.Order == InvCarry[j].UserItem.Order)
                    throw new Exception("IM-InvCarry Invalid!!!");
        for (int i = 0; i < InvEquipment.Count; i++)
        {
            for (int j = 0; j < InvEquipment.Count; j++)
                if (i != j && InvEquipment[i].UserItem.Order == InvEquipment[j].UserItem.Order)
                    throw new Exception("IM-InvEquipment Invalid!!!");
            if (InvEquipment[i].Item.Type == OupItem.ItemType.Equipment)
                if ((int) InvEquipment[i].Item.PlaceHolder != InvEquipment[i].UserItem.Order)
                    throw new Exception("IM-InvEquipment Invalid Equipment Order!!!");
            if (InvEquipment[i].Item.Type == OupItem.ItemType.Tool || InvEquipment[i].Item.Type == OupItem.ItemType.Weapon)
                if (InvEquipment[i].UserItem.Order != (int)OupItem.PlaceType.Left 
                        && InvEquipment[i].UserItem.Order != (int)OupItem.PlaceType.Right)
                    throw new Exception("IM-InvEquipment Invalid Weapon/Tool Order!!!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (UpdateInventory)
        {
            RefreshInventory();
            _userDatabase.UpdateUserInventory(InvCarry, InvEquipment);
            UpdateInventory = false;
            PrintInventory();
        }
    }
    internal bool HaveAvailableSlot()
    {
        return _characterManager.CharacterSetting.CarryCnt > InvCarry.Count;
    }

    internal void PrintInventory()
    {
        var carry = "";
        var equipment = "";
        foreach (var itemIns in InvCarry)
            carry += itemIns.UserItem.Order + "-" + itemIns.Item.Name + "(" + itemIns.UserItem.StackCnt + ")#";
        foreach (var itemIns in InvEquipment)
            equipment += itemIns.UserItem.Order + "-" + itemIns.Item.Name + "(" + itemIns.UserItem.StackCnt + ")#";
        Debug.Log("IM-PrintInventory InvCarry=" + InvCarry.Count +":"+ carry+ " InvEquipment= " + InvEquipment.Count + ":" + equipment);
    }
}