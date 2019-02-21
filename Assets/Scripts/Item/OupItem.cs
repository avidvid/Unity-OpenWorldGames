using System;
using UnityEngine;

[Serializable]
public class OupItem
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
    public enum ItemRarity
    {
        Sacred = 0,
        Artifact = 1,
        Legendary = 2,
        Saga = 3,
        Rare = 10,
        Uncommon = 40,
        Common = 100
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
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconPath { get; set; }
    public int IconId { get; set; }
    public int Cost { get; set; }
    public int Weight { get; set; }
    public int MaxStackCnt { get; set; }
    public int ExpirationDays { get; set; }
    public ItemType Type { get; set; }
    public ItemRarity Rarity { get; set; }
    public bool IsEnable { get; set; }
    //Consumable
    public int Health { get; set; }
    public int Mana { get; set; }
    public int Energy { get; set; }
    public int Coin { get; set; }
    public int Gem { get; set; }
    public int Recipe { get; set; }
    public int Egg { get; set; }
    //Equipment
    public PlaceType PlaceHolder { get; set; }
    public int Agility { get; set; }
    public int Bravery { get; set; }
    //todo: remove Carry 
    public int Carry { get; set; }
    public int CarryCnt { get; set; }
    public int Charming { get; set; }
    public int Intellect { get; set; }
    public int Crafting { get; set; }
    public int Researching { get; set; }
    //todo: remove Speed 
    public int Speed { get; set; }
    public int Stamina { get; set; }
    public int Strength { get; set; }
    //Weapon
    public Hands CarryType { get; set; }
    public int SpeedAttack { get; set; }
    public int SpeedDefense { get; set; }
    public int AbilityAttack { get; set; }
    public int AbilityDefense { get; set; }
    public int MagicAttack { get; set; }
    public int MagicDefense { get; set; }
    public int PoisonAttack { get; set; }
    public int PoisonDefense { get; set; }
    //Tool
    public int MaxTimeToUse { get; set; }
    public ElementIns.ElementType FavoriteElement { get; set; }

    protected OupItem(
        int id, string name, string desc,
        string iconPath, int iconId, int cost,
        int weight, int maxStackCnt,int expirationDays,
        ItemType type, ItemRarity rarity)
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
                MaxTimeToUse = 0;
                FavoriteElement = ElementIns.ElementType.Hole;
                break;

        }
    }
    protected OupItem()
    {
    }
    public virtual string GetTooltip()
    {
        string color;
        switch (Type)
        {
            case ItemType.Consumable:
                color = "green";
                break;
            case ItemType.Weapon:
            case ItemType.Equipment:
                color = "blue";
                break;
            default:
                color = "white";
                break;
        }
        var tooltip = "<color=" + color + ">  " + this.Id + "  -" + this.Name + "</color>\n\n" + this.Description + "\n<color=yellow>Cost:" + this.Cost + "</color>";
        return tooltip;
    }
    public Sprite GetSprite()
    {
        Sprite[] spriteList = Resources.LoadAll<Sprite>(IconPath);
        return spriteList[IconId];
    }
    internal void Print()
    {
        if (Id == -1)
        {
            Debug.Log("Id:" + Id);
            return;
        }
        Sprite Sprite = this.GetSprite();
        Debug.Log("Id:" + Id + " Name:" + Name + " Sprite:" + Sprite.name + " Type:" + Type + " Rarity:" + Rarity);
    }
}