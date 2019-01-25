using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : Item
{
    public enum Hands
    {
        OneHand,  
        TwoHands,
        ThreeHands,
        FourHands
    }

    public Hands CarryType { get; set; }
    public int SpeedAttack { get; set; }
    public int SpeedDefense { get; set; }
    public int AbilityAttack { get; set; }
    public int AbilityDefense { get; set; }
    public int MagicAttack { get; set; }
    public int MagicDefense { get; set; }
    public int PoisonAttack { get; set; }
    public int PoisonDefense { get; set; }

    public Weapon() : base()
    {
    }

    public Weapon(int id, string name, string desc, string iconPath, int iconId, int cost, int weight, int maxStackCnt, int stackCnt, ItemType type, ItemRarity rarity, bool isUnique, int durationDays, DateTime expirationTime, int[] values)
        : base(id, name, desc, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type, rarity, isUnique, durationDays, expirationTime)
    {
        CarryType = (Hands)values[0];
        SpeedAttack = values[1];
        SpeedDefense = values[2];
        AbilityAttack  = values[3];
        AbilityDefense = values[4];
        MagicAttack = values[5];
        MagicDefense = values[6];
        PoisonAttack = values[7];
        PoisonDefense = values[8];
    }

    public override void Usage()
    {
    }

    public override string GetTooltip()
    {
        string tooltip = base.GetTooltip();
        tooltip += "\nThis is Weapon";
        return tooltip;
    }

}
