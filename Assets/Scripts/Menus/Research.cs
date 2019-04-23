using System;
using UnityEngine;

[Serializable]
public class Research
{
    public int Id;
    public string Name;
    public string Description;
    public string IconPath;
    public int IconId;
    public int Value;
    public string Target;
    public int MaxLevel;
    public int DurationMinutes;
    public int RequiredResearchId1;
    public int RequiredResearchLevel1;
    public int RequiredResearchId2;
    public int RequiredResearchLevel2;
    public int RequiredResearchId3;
    public int RequiredResearchLevel3;
    public int RequiredItem;
    public bool IsEnable;

    public string GetTooltip()
    {
        var tooltip = "<color=green>  " + this.Id + "  -" + this.Name + "</color>\n\n" + this.Description ;
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
