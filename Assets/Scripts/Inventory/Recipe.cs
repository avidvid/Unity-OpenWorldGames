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



    public virtual string GetDescription()
    {
        ItemDatabase itemDatabase = ItemDatabase.Instance();
        try
        {
            return "Ready to mix " + Rarity + " Recipe" +
                   FirstItemCnt + " of "+
                   itemDatabase.FindItem(FirstItemId).Name+
                   " with " + SecondItemCnt + " of " +
                   itemDatabase.FindItem(SecondItemId).Name +
                   " to make " + FinalItemCnt + " of " +
                   itemDatabase.FindItem(FinalItemId).Name +
                   " for " + Energy + " energy ? ";
            //todo: current font doesn't support ( ) 
            //return "Ready to mix " +
            //       _itemDatabase.FindItem(FirstItemId).Name
            //       + "(" + FirstItemCnt + ") +" +
            //       _itemDatabase.FindItem(SecondItemId).Name
            //       + "(" + SecondItemCnt + ") and make " +
            //       _itemDatabase.FindItem(FinalItemId).Name
            //       + "(" + FinalItemCnt + ") for " +
            //       Energy + " energy ? ";
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return "Ready to mix items?";
        }
    }

    internal string Print()
    {
        ItemDatabase itemDatabase = ItemDatabase.Instance();
        try
        {
            return "Recipe: " + itemDatabase.FindItem(FirstItemId).Name +
                   " with " + itemDatabase.FindItem(SecondItemId).Name;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return "Print Recipe";
        }
    }
}
