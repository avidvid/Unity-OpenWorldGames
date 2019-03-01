using System;
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
        Debug.Log("ItemIns = " + MyInfo());
    }
    internal string MyInfo()
    {
        return "User= " + UserItem.UserId + "Item = " + Item.Name + "(" + UserItem.StackCnt + ")";
    }
    internal string GetTooltip()
    {
        string color;
        switch (Item.Type)
        {
            case OupItem.ItemType.Consumable:
                color = "green";
                break;
            case OupItem.ItemType.Weapon:
            case OupItem.ItemType.Equipment:
                color = "blue";
                break;
            default:
                color = "white";
                break;
        }
        var tooltip = "<color=" + color + ">" + // Item.Id + "-" + 
                        Item.Name + " ( # " + UserItem.StackCnt + " )</color>" +
                      "\n" +
                      "\n" +
                      Item.Description +
                      "\n" +
                      "<color=yellow>Cost: " + Item.Cost + "</color>";
        return tooltip;
    }
}