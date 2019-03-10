using System;
using UnityEngine;

public class ItemIns
{
    public ItemContainer Item;
    public UserItem UserItem;
    
    public ItemIns(ItemContainer item,UserItem userItem)
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
            case ItemContainer.ItemType.Consumable:
                color = "green";
                break;
            case ItemContainer.ItemType.Weapon:
            case ItemContainer.ItemType.Equipment:
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