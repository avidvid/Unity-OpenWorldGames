using System;
using UnityEngine;

[Serializable]
public class Recipe{

    public enum RecipeRarity
    {
        Sacred = 0,
        Artifact = 1,
        Legendary = 2,
        Saga = 3,
        Rare = 10,
        Uncommon = 40,
        Common = 100
    }

    public int Id { get; set; }
    public int FirstItemId { get; set; }
    public int FirstItemCnt { get; set; }
    public int SecondItemId { get; set; }
    public int SecondItemCnt { get; set; }
    public int FinalItemId { get; set; }
    public int FinalItemCnt { get; set; }
    public int DurationMinutes { get; set; }
    public RecipeRarity Rarity { get; set; }
    public int Energy { get; set; }
    public bool IsPublic { get; set; }
    public bool IsEnable { get; set; }
    public int HealthCheck { get; set; }

    public virtual string GetDescription()
    {
        ItemDatabase itemDatabase = ItemDatabase.Instance();
        try
        {
            //todo: current font doesn't support ( ) now it support so changed 2019-03-15 delete this part  
            //return "Ready to mix " + Rarity + " Recipe" +
            //       FirstItemCnt + " of "+
            //       itemDatabase.GetItemById(FirstItemId).Name+
            //       " with " + SecondItemCnt + " of " +
            //       itemDatabase.GetItemById(SecondItemId).Name +
            //       " to make " + FinalItemCnt + " of " +
            //       itemDatabase.GetItemById(FinalItemId).Name +
            //       " for " + Energy + " energy ? ";
            return "Ready to mix " +
                   itemDatabase.GetItemById(FirstItemId).Name
                   + "(" + FirstItemCnt + ") +" +
                   itemDatabase.GetItemById(SecondItemId).Name
                   + "(" + SecondItemCnt + ") and make " +
                   itemDatabase.GetItemById(FinalItemId).Name
                   + "(" + FinalItemCnt + ") for " +
                   Energy + " energy ? ";
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return "Ready to mix items?";
        }
    }

    internal void Print()
    {
        ItemDatabase itemDatabase = ItemDatabase.Instance();
        try
        {
            Debug.Log("Recipe(" + Id + "): " + itemDatabase.GetItemById(FirstItemId).Name +
                   " with " + itemDatabase.GetItemById(SecondItemId).Name +
                   " to " + itemDatabase.GetItemById(FinalItemId).Name +
                   " Enable " + IsEnable +
                   " Public " + IsPublic);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    internal Recipe Reverse()
    {
        int temp = FirstItemId;
        FirstItemId = SecondItemId;
        SecondItemId = temp;
        temp = FirstItemCnt;
        FirstItemCnt = SecondItemCnt;
        SecondItemCnt = temp;
        return this;
    }
}
