using System;
using UnityEngine;

[Serializable]
public class UserItem
{
    public int Id;
    public int UserId;
    public int ItemId;
    public int StackCnt;
    public bool Equipped;
    public bool Stored;
    public int Order;
    public int TimeToUse;

    public UserItem(ItemContainer item, int userId, int stackCnt=1, int order = -1)
    {
        Id = UnityEngine.Random.Range(0, 1999999999);
        UserId = userId;
        ItemId = item.Id;
        StackCnt = stackCnt;
        Equipped = false;
        Stored = false;
        Order = order;
        TimeToUse = item.MaxTimeToUse;
    }
    public UserItem(UserItem item)
    {
        Id = item.Id;
        UserId = item.UserId;
        ItemId = item.ItemId;
        StackCnt = item.StackCnt;
        Equipped = item.Equipped;
        Stored = item.Stored;
        Order = item.Order;
        TimeToUse = item.TimeToUse;
    }

    public UserItem()
    {
    }


    internal void Print()
    {
        Debug.Log("UserItem = " + MyInfo());
    }
    internal string MyInfo()
    {
        if (Id == -1)
            return "Empty Item";
        ItemDatabase itemDatabase = ItemDatabase.Instance();
        try
        {
            var item = itemDatabase.GetItemById(ItemId);
            return "UserItem (" + Id + ")" +
                   " UserId:" + UserId +
                   " Item:" + item.Name +
                   " StackCnt:" + StackCnt +
                   (Stored ? " S " : "") +
                   (Equipped ? " E " : "") +
                   " Order" + Order +
                   " TimeToUse:" + TimeToUse
                ;
        }
        catch (Exception e)
        {
            return "Empty Item";
        }
    }
    public override bool Equals(object obj)
    {
        //Check for null and compare run-time types.
        if (obj == null || this.GetType() != obj.GetType())
            return false;
        UserItem item = (UserItem)obj;
        return (Id == item.Id) 
               && (UserId == item.UserId)
               && (ItemId == item.ItemId)
               && (StackCnt == item.StackCnt)
               && (Stored == item.Stored)
               && (Equipped == item.Equipped)
               && (Order == item.Order)
               && (TimeToUse == item.TimeToUse)
            ;
    }
}