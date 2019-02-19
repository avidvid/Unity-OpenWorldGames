using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Equipment : Item {

    public enum PlaceType
    {
        Head,   // Hat, Crown
        Mask,   // Mask, Helmet
        Neck,
        Shoulders,
        Chest,
        Gloves,
        Ring,
        Belt,
        Tail,
        Legs,
        Feet,
        Right,
        Left,
        Charm,
        None
    }

    public PlaceType PlaceHolder { get; set; }
    public int Agility { get; set; }
    public int Bravery { get; set; }
    public int Carry { get; set; }
    public int CarryCnt { get; set; }
    public int Charming { get; set; }
    public int Intellect { get; set; }
    public int Crafting { get; set; }
    public int Researching { get; set; }
    public int Speed { get; set; }
    public int Stamina { get; set; }
    public int Strength { get; set; }
    
    

    public Equipment() : base()
    {
    }

    public Equipment(int id, string name, string desc, string iconPath, int iconId, int cost, int weight, int maxStackCnt, int stackCnt, ItemType type, ItemRarity rarity, bool isUnique, int durationDays, DateTime expirationTime, int[] values)
        : base(id, name, desc, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type, rarity, isUnique, durationDays, expirationTime)
    {
        PlaceHolder = (PlaceType) values[0];
        Agility = values[1];
        Bravery = values[2];
        Carry = values[3];
        CarryCnt = values[4];
        Charming = values[5];
        Intellect = values[6];
        Crafting = values[7];
        Researching = values[8];
        Speed = values[9];
        Stamina = values[10];
        Strength = values[11];
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
