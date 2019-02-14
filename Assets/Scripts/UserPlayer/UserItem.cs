using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserItem
{
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public int StackCnt { get; set; }
    public bool Equipped { get; set; }
    public int Order { get; set; }
    public int TimeToUse { get; set; }
    public DateTime ExpirationDate { get; set; }
    public DateTime Created { get; set; }

    public UserItem(OupItem item, int stackCnt=1)
    {
        UserId = 22;
        ItemId = item.Id;
        StackCnt = stackCnt;
        Equipped = false;
        Order = 100;
        TimeToUse = 1;
        ExpirationDate = DateTime.Now.AddDays(item.ExpirationDays);
        Created = DateTime.Now;
    }
}