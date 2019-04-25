using System;

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
    public DateTime Created;

    public UserItem(ItemContainer item, int stackCnt=1, int order = -1)
    {
        Id = 0;
        UserId = 22;
        ItemId = item.Id;
        StackCnt = stackCnt;
        Equipped = false;
        Stored = false;
        Order = order;
        TimeToUse = 1;
        Created = DateTime.Now;
    }
    public UserItem()
    {
    }
}