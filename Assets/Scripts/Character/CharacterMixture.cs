using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterMixture
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public int StackCnt { get; set; }
    public DateTime MixTime { get; set; }
    public int HealthCheck { get; set; }

    public CharacterMixture(int userId, int itemId, int stackCnt, DateTime time)
    {
        Id = 0;
        UserId = userId;
        ItemId = itemId;
        StackCnt = stackCnt;
        MixTime = time;
        HealthCheck = 0;
    }
    public CharacterMixture()
    {
    }
    internal string MyInfo()
    {
        var itemDatabase = ItemDatabase.Instance();
        return Id + "-" + itemDatabase.GetItemById(ItemId).Name + " (" + StackCnt + ") in " + MixTime;
    }
    internal void Print()
    {
        Debug.Log("CharacterMixture = " + MyInfo());
    }
}