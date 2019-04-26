using System;
using System.Runtime.Remoting;
using UnityEngine;

[Serializable]
public class ItemContainer
{
    public enum ItemType
    {
        Consumable,
        Weapon,
        Equipment,
        Substance,
        Tool,
        Element,
        Quest,
        Empty
    }

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
    public enum Hands
    {
        OneHand,
        TwoHands,
        ThreeHands,
        FourHands
    }

    public int Id;
    public string Name ;
    public string Description ;
    public string IconPath ;
    public int IconId ;
    public int Cost ;
    public int Weight ;
    public int MaxStackCnt ;
    public bool Unique ;
    public int ExpirationDays ;
    public ItemType Type ;
    public DataTypes.Rarity Rarity ;
    public bool IsEnable ;
    //Consumable
    public int Health ;
    public int Mana ;
    public int Energy ;
    public int Coin ;
    public int Gem ;
    public int Recipe ;
    public int Egg ;
    //Equipment
    public PlaceType PlaceHolder ;
    public int Agility ;
    public int Bravery ;
    public int Carry ;
    public int CarryCnt ;
    public int Charming ;
    public int Intellect ;
    public int Crafting ;
    public int Researching ;
    public int Speed ;
    public int Stamina ;
    public int Strength ;
    //Weapon
    public Hands CarryType ;
    public int SpeedAttack ;
    public int SpeedDefense ;
    public int AbilityAttack ;
    public int AbilityDefense ;
    public int MagicAttack ;
    public int MagicDefense ;
    public int PoisonAttack ;
    public int PoisonDefense ;
    //Tool
    public int MaxTimeToUse ;
    public ElementIns.ElementType FavoriteElement ;

    protected ItemContainer(
        int id, string name, string desc,
        string iconPath, int iconId, int cost,
        int weight, int maxStackCnt,bool unique, int expirationDays,
        ItemType type, DataTypes.Rarity rarity)
    {
        Id = id;
        Name = name;
        Description = desc;
        IconPath = iconPath;
        IconId = iconId;
        Cost = cost;
        Weight = weight;
        Type = type;
        MaxStackCnt = maxStackCnt;
        Unique = unique;
        ExpirationDays = expirationDays;
        Rarity = rarity;
        IsEnable = true;
        switch (Type)
        {
            case ItemType.Consumable:
                Health = Mana = Energy = Coin = Gem = Recipe = Egg = 0;
                break;
            case ItemType.Equipment:
                PlaceHolder = PlaceType.None;
                Carry = CarryCnt = Speed =
                Agility = Bravery = Charming = Intellect = Crafting = Researching = Stamina = Strength = 0;
                break;
            case ItemType.Weapon:
                CarryType = Hands.OneHand;
                SpeedAttack = SpeedDefense =
                AbilityAttack = AbilityDefense =
                MagicAttack = MagicDefense =
                PoisonAttack = PoisonDefense = 0;
                break;
            case ItemType.Tool:
                CarryType = Hands.OneHand;
                MaxTimeToUse = 0;
                FavoriteElement = ElementIns.ElementType.Hole;
                break;
        }
    }
    protected ItemContainer()
    {
    }
    public Sprite GetSprite()
    {
        Sprite[] spriteList = Resources.LoadAll<Sprite>(IconPath);
        return spriteList[IconId];
    }
    internal void Print()
    {
        Debug.Log("Item = " + MyInfo());
    }
    internal string MyInfo()
    {
        if (Id == -1)
            return "Empty Item";
        return Id + " Name:" + Name +
               //" Des:" + Description +
               " Rarity:" + Rarity +
               " Type:" + Type +
               " =>" + TypeInfo() +
               " Sprite:" + GetSprite().name +
                (Unique?" Unique":"")
            ;
    }

    private string TypeInfo()
    {
        switch (Type)
        {
            case ItemType.Consumable:
                return Health + "/" + Mana + "/" + Energy + "/" + Coin + "/" + Gem + "/" + Recipe + "/" + Egg;
            case ItemType.Equipment:
                return PlaceHolder + "/" + Agility + "/" + Bravery + "/" + Carry + "/" + CarryCnt + "/" + Charming +
                       "/" + Intellect
                       + Crafting + "/" + Researching + "/" + Speed + "/" + Stamina + "/" + Strength;
            case ItemType.Weapon:
                return CarryType + "/" + SpeedAttack + "/" + SpeedDefense + "/" + AbilityAttack + "/" + AbilityDefense +
                       "/" + MagicAttack
                       + MagicDefense + "/" + PoisonAttack + "/" + PoisonDefense;
            case ItemType.Tool:
                return CarryType + "/" + MaxTimeToUse + "/" + FavoriteElement + "/" + Egg;
            default:
                return "";
        }
    }
}