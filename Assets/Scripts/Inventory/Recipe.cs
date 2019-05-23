using System;
using UnityEngine;

[Serializable]
public class Recipe
{
    public int Id;
    public int FirstItemId;
    public int FirstItemCnt;
    public int SecondItemId;
    public int SecondItemCnt;
    public int FinalItemId;
    public int FinalItemCnt;
    public int DurationMinutes;
    public DataTypes.Rarity Rarity;
    public int Energy;
    public bool IsPublic;
    public bool IsEnable;
    public virtual string GetDescription()
    {
        ItemDatabase itemDatabase = ItemDatabase.Instance();
        try
        {
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

    internal void Print()
    {
        Debug.Log("Recipe: " + MyInfo());
    }
    internal string MyInfo()
    {
        ItemDatabase itemDatabase = ItemDatabase.Instance();
        try
        {
            return Id + "-" + itemDatabase.GetItemById(FirstItemId).Name +
                   " with " + itemDatabase.GetItemById(SecondItemId).Name +
                   " to " + itemDatabase.GetItemById(FinalItemId).Name +
                   " Enable " + IsEnable +
                   " Public " + IsPublic;
        }
        catch (Exception e)
        {
            return "Empty" ;
        }
    }
}
