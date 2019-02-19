using System;

[Serializable]
public class UserItem
{
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public int StackCnt { get; set; }
    public bool Equipped { get; set; }
    public bool Stored { get; set; }
    public int Order { get; set; }
    public int TimeToUse { get; set; }
    public bool Deleted { get; set; }
    public DateTime Created { get; set; }

    public UserItem(OupItem item, int stackCnt=1)
    {
        UserId = 22;
        ItemId = item.Id;
        StackCnt = stackCnt;
        Equipped = false;
        Stored = false;
        Order = 100;
        TimeToUse = 1;
        Deleted = false;
        Created = DateTime.Now;
    }
    public UserItem()
    {
    }
}