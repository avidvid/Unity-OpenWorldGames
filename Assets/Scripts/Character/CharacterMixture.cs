using System;
using UnityEngine;

[Serializable]
public class CharacterMixture
{
    public int Id;
    public int UserId;
    public int ItemId;
    public int StackCnt;
    public DateTime MixTime;

    public CharacterMixture(int userId, int itemId, int stackCnt, DateTime time)
    {
        Id = 0;
        UserId = userId;
        ItemId = itemId;
        StackCnt = stackCnt;
        MixTime = time;
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