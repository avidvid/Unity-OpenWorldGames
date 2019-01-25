using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Substance : Item
{

    public Substance() : base()
    {
    }


    public Substance(int id, string name, string desc, string iconPath, int iconId, int cost, int weight, int maxStackCnt, int stackCnt, ItemType type, ItemRarity rarity, bool isUnique, int durationDays, DateTime expirationTime, int[] values)
        : base(id, name, desc, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type, rarity, isUnique, durationDays, expirationTime)
    {

    }

    public override void Usage()
    {
    }

    public override string GetTooltip()
    {
        string tooltip = base.GetTooltip();
        tooltip += "\nThis is Substance";
        return tooltip;
    }

}
