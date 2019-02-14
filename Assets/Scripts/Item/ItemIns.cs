using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIns
{
    public OupItem Item;
    public UserItem UserItem;

    public ItemIns(OupItem item,UserItem userItem)
    {
        Item = item;
        UserItem = userItem;
    }

    internal void Print()
    {
        Debug.Log("User= " + UserItem.UserId + "Item = " + Item.Name + "(" + UserItem.StackCnt +")");
    }
}
