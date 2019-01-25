using System;
using UnityEngine;

[Serializable]
public class Research
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconPath { get; set; }
    public int IconId { get; set; }
    public int Value { get; set; }
    public string Target { get; set; }
    public int MaxLevel { get; set; }
    public int DurationMinutes { get; set; }
    public int RequiredResearchId1 { get; set; }
    public int RequiredResearchLevel1 { get; set; }
    public int RequiredResearchId2 { get; set; }
    public int RequiredResearchLevel2 { get; set; }
    public int RequiredResearchId3 { get; set; }
    public int RequiredResearchLevel3 { get; set; }
    public int RequiredItem { get; set; }
    public bool IsEnable { get; set; }

    public string GetTooltip()
    {
        var tooltip = "<color=Green>  " + this.Id + "  -" + this.Name + "</color>\n\n" + this.Description ;
        return tooltip;
    }



    public Sprite GetSprite()
    {
        Sprite[] spriteList = Resources.LoadAll<Sprite>(IconPath);
        return spriteList[IconId];
    }
    internal int CalculatePrice(int level)
    {
        if (level <= 1)
            return Value;
        //todo:have a better logic here 
        return level * Value;
    }

    internal int CalculateTime(int level)
    {
        if (level <= 1)
            return DurationMinutes;
        //todo:have a better logic here 
        return level * DurationMinutes;
    }

    internal float CalculateValue(int level)
    {
        if (level <= 1)
            return Value;
        //todo:have a better logic here 
        return level * Value;
    }

    internal void Print()
    {
        Debug.Log("Research = " + Id + "-" + Name + " (" + Description + ")");
    }

    internal bool NeedLine()
    {
        if (RequiredResearchId1 != -1 || RequiredResearchId2 != -1 || RequiredResearchId3 != -1)
            return true;
        return false;
    }
}
