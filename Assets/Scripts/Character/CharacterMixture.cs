using System;
using System.Globalization;
using UnityEngine;

[Serializable]
public class CharacterMixture
{
    public int Id;
    public int UserId;
    public int ItemId;
    public int StackCnt;
    public string MixTime;


    public CharacterMixture()
    {
        Id = 0;
    }
    internal string MyInfo()
    {
        if (ItemId!= 0)
        {
            var itemDatabase = ItemDatabase.Instance();
            return Id + "-" + itemDatabase.GetItemById(ItemId).Name + " (" + StackCnt + ") in " + MixTime;
        }
        return Id + "-Empty";
    }
    internal void Print()
    {
        Debug.Log("CharacterMixture = " + MyInfo());
    }

    internal void SetEmpty()
    {
        Id = 0;
        UserId = 0;
        ItemId = 0;
        StackCnt = 0;
        MixTime = "";
    }
    internal void SetValues(int userId, int itemId, int stackCnt, DateTime time)
    {
        Id = UnityEngine.Random.Range(0, 1999999999);
        UserId = userId;
        ItemId = itemId;
        StackCnt = stackCnt;
        MixTime = time.ToString(CultureInfo.InvariantCulture);
    }
}