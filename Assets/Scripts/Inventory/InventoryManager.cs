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
        var userInvItems = _characterManager.CharacterInventory;
        foreach (var itemIns in userInvItems)
        {
            if (itemIns.UserItem.Equipped)
                InvEquipment.Add(itemIns);
            else if (itemIns.UserItem.Stored)
                InvBank.Add(itemIns);
            else
                InvCarry.Add(itemIns);
        }
        Debug.Log("IM-InvCarry.Count = " + InvCarry.Count);
        Debug.Log("IM-InvEquipment.Count = " + InvEquipment.Count);
        Debug.Log("IM-InvBank.Count = " + InvBank.Count);
    }
    // Update is called once per frame
    void Update()
    {
        if (UpdateInventory)
        {
            _userDatabase.UpdateUserInventory(InvCarry, InvEquipment);
            UpdateInventory = false;
        }
    }
    internal bool HaveAvailableSlot()
    {
        return _characterManager.CharacterSetting.CarryCnt > InvCarry.Count;
    }
}