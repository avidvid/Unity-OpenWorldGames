using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : Item
{
    public int TimeToUse { get; set; }
    public ElementIns.ElementType FavouriteElement { get; set; }

    public Tool() : base()
    {
    }

    public Tool(int id, string name, string desc, string iconPath, int iconId, int cost, int weight, int maxStackCnt, int stackCnt, ItemType type, ItemRarity rarity, bool isUnique, int durationDays, DateTime expirationTime, int[] values)
        : base(id, name, desc, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type, rarity, isUnique, durationDays, expirationTime)
    {
        TimeToUse = values[0];
        FavouriteElement = (ElementIns.ElementType)values[1];
    }

    public override void Usage()
    {
    }

    public override string GetTooltip()
    {
        string tooltip = base.GetTooltip();
        tooltip += "\nThis is Equipment";
        return tooltip;
    }
}
