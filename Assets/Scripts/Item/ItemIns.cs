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
}