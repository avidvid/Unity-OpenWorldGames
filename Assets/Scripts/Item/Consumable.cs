using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Consumable : Item {
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Energy { get; set; }
    public int Coin { get; set; }
    public int Gem { get; set; }
    public int Recipe { get; set; }


    public Consumable() : base()
    {
    }

    public Consumable(int id, string name, string desc, string iconPath, int iconId, int cost, int weight, int maxStackCnt, int stackCnt, ItemType type, ItemRarity rarity, bool isUnique, int durationDays, DateTime expirationTime, int[] values =null)
        : base(id, name, desc,  iconPath,  iconId, cost, weight, maxStackCnt, stackCnt, type, rarity, isUnique, durationDays, expirationTime)
    {
        if (values == null)
        {
            Health = Mana  = 0;
            return;
        }
        Health = values[0];
        Mana = values[1];
        Energy = values[2];
        Coin = values[3];
        Gem = values[4];
        Recipe = values[5];
    }
    public override void Usage()
    {
    }

    public override string GetTooltip()
    {
        string tooltip = base.GetTooltip();
        tooltip += "\nThis is Consumable";
        return tooltip;
    }
}
