using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemContainer {

    private Consumable _consumable = new Consumable();
    private Weapon _weapon = new Weapon();
    private Equipment _equipment = new Equipment();
    private Substance _substance = new Substance();
    private Tool _tool = new Tool();
    //private ItemContainer item;

    public Consumable Consumable
    {
        get { return _consumable; }
        set { _consumable = value; }
    }
    public Weapon Weapon
    {
        get { return _weapon; }
        set { _weapon = value; }
    }

    public Equipment Equipment
    {
        get { return _equipment; }
        set { _equipment = value; }
    }

    public Substance Substance
    {
        get { return _substance; }
        set { _substance = value; }
    }
    public Tool Tool
    {
        get { return _tool; }
        set { _tool = value; }
    }

    public int Id
    {
        get
        {
            if (Consumable != null)
                return Consumable.Id;
            if (Weapon != null)
                return Weapon.Id;
            if (Equipment != null)
                return Equipment.Id;
            if (Substance != null)
                return Substance.Id;
            if (Tool != null)
                return Tool.Id;
            return -1;
        }
    }

    public string Name 
    {
        get
        {
            if (Consumable != null)
                return Consumable.Name;
            if (Weapon != null)
                return Weapon.Name;
            if (Equipment != null)
                return Equipment.Name;
            if (Substance != null)
                return Substance.Name;
            if (Tool != null)
                return Tool.Name;
            return "Empty";
        }
    }
    public string Description
    {
        get
        {
            if (Consumable != null)
                return Consumable.Description;
            if (Weapon != null)
                return Weapon.Description;
            if (Equipment != null)
                return Equipment.Description;
            if (Substance != null)
                return Substance.Description;
            if (Tool != null)
                return Tool.Description;
            return "";
        }
    }

    public string IconPath 
    {
        get
        {
            if (Consumable != null)
                return Consumable.IconPath;
            if (Weapon != null)
                return Weapon.IconPath;
            if (Equipment != null)
                return Equipment.IconPath;
            if (Substance != null)
                return Substance.IconPath;
            if (Tool != null)
                return Tool.IconPath;
            return "";
        }
    }

    public int IconId
    {
        get
        {
            if (Consumable != null)
                return Consumable.IconId;
            if (Weapon != null)
                return Weapon.IconId;
            if (Equipment != null)
                return Equipment.IconId;
            if (Substance != null)
                return Substance.IconId;
            if (Tool != null)
                return Tool.IconId;
            return -1;
        }
    }
    public int Cost
    {
        get
        {
            if (Consumable != null)
                return Consumable.Cost;
            if (Weapon != null)
                return Weapon.Cost;
            if (Equipment != null)
                return Equipment.Cost;
            if (Substance != null)
                return Substance.Cost;
            if (Tool != null)
                return Tool.Cost;
            return 0;
        }
    }
    public int MaxStackCnt
    {
        get
        {
            if (Consumable != null)
                return Consumable.MaxStackCnt;
            if (Weapon != null)
                return Weapon.MaxStackCnt;
            if (Equipment != null)
                return Equipment.MaxStackCnt;
            if (Substance != null)
                return Substance.MaxStackCnt;
            if (Tool != null)
                return Tool.MaxStackCnt;
            return 0;
        }
    }

    public int StackCnt
    {
        get
        {
            if (Consumable != null)
                return Consumable.StackCnt;
            if (Weapon != null)
                return Weapon.StackCnt;
            if (Equipment != null)
                return Equipment.StackCnt;
            if (Substance != null)
                return Substance.StackCnt;
            if (Tool != null)
                return Tool.StackCnt;
            return 0;
        }
    }
    public Item.ItemType Type
    {
        get
        {
            if (Consumable != null)
                return Consumable.Type;
            if (Weapon != null)
                return Weapon.Type;
            if (Equipment != null)
                return Equipment.Type;
            if (Substance != null)
                return Substance.Type;
            if (Tool != null)
                return Tool.Type;
            return Item.ItemType.Empty;
        }
    }
    public Item.ItemRarity Rarity
    {
        get
        {
            if (Consumable != null)
                return Consumable.Rarity;
            if (Weapon != null)
                return Weapon.Rarity;
            if (Equipment != null)
                return Equipment.Rarity;
            if (Substance != null)
                return Substance.Rarity;
            if (Tool != null)
                return Tool.Rarity;
            return 0;
        }
    }

    public bool IsUnique
    {
        get
        {
            if (Consumable != null)
                return Consumable.IsUnique;
            if (Weapon != null)
                return Weapon.IsUnique;
            if (Equipment != null)
                return Equipment.IsUnique;
            if (Substance != null)
                return Substance.IsUnique;
            if (Tool != null)
                return Tool.IsUnique;
            return false;
        }
    }
    //public string Slug { get; set; }

    public int DurationDays
    {
        get
        {
            if (Consumable != null)
                return Consumable.DurationDays;
            if (Weapon != null)
                return Weapon.DurationDays;
            if (Equipment != null)
                return Equipment.DurationDays;
            if (Substance != null)
                return Substance.DurationDays;
            if (Tool != null)
                return Tool.DurationDays;
            return 0;
        }
    }

    public DateTime ExpirationTime
    {
        get
        {
            if (Consumable != null)
                return Consumable.ExpirationTime;
            if (Weapon != null)
                return Weapon.ExpirationTime;
            if (Equipment != null)
                return Equipment.ExpirationTime;
            if (Substance != null)
                return Substance.ExpirationTime;
            if (Tool != null)
                return Tool.ExpirationTime;
            return DateTime.MinValue;
        }
    }
    public int Weight
    {
        get
        {
            if (Consumable != null)
                return Consumable.Weight;
            if (Weapon != null)
                return Weapon.Weight;
            if (Equipment != null)
                return Equipment.Weight;
            if (Substance != null)
                return Substance.Weight;
            if (Tool != null)
                return Tool.Weight;
            return 0;
        }
    }

    public bool IsEnable
    {
        get
        {
            if (Consumable != null)
                return Consumable.IsEnable;
            if (Weapon != null)
                return Weapon.IsEnable;
            if (Equipment != null)
                return Equipment.IsEnable;
            if (Substance != null)
                return Substance.IsEnable;
            if (Tool != null)
                return Tool.IsEnable;
            return false;
        }
    }

    public int[] Values
    {
        get
        {
            if (Consumable != null)
                return new int[6]
                {
                    Consumable.Health, Consumable.Mana, Consumable.Energy,
                    Consumable.Coin, Consumable.Gem, Consumable.Recipe
                };
            if (Weapon != null)
                return new int[9] {
                    (int) Weapon.CarryType, 
                    Weapon.SpeedAttack, Weapon.SpeedDefense,
                    Weapon.AbilityAttack, Weapon.AbilityDefense,
                    Weapon.MagicAttack, Weapon.MagicDefense,
                    Weapon.PoisonAttack, Weapon.PoisonDefense
                };
            if (Equipment != null)
                return new int[12]
                {
                    (int)Equipment.PlaceHolder
                    ,Equipment.Agility 
                    ,Equipment.Bravery 
                    ,Equipment.Carry
                    ,Equipment.CarryCnt
                    ,Equipment.Charming
                    ,Equipment.Intellect 
                    ,Equipment.Crafting 
                    ,Equipment.Researching 
                    ,Equipment.Speed
                    ,Equipment.Stamina
                    ,Equipment.Strength 
                };
            if (Tool != null)
                return new int[2]
                {
                    Tool.TimeToUse,
                    (int)Tool.FavouriteElement
                };
            return null;
        }
    }

    public Equipment.PlaceType PlaceHolder
    {
        get
        {
            return Equipment.PlaceHolder;
        }
    }

    public Weapon.Hands CarryType
    {
        get
        {
            return Weapon.CarryType;
        }
    }

    public ItemContainer(
        int id,string name,string description,
        string iconPath,int iconId,
        int cost, int weight, 
        int maxStackCnt,int stackCnt,
        Item.ItemType type,Item.ItemRarity rarity, bool isUnique,
        int durationDays, DateTime expirationTime,
        int[] values = null )
    {

        switch (type)
        {
            case Item.ItemType.Consumable:
                Consumable = new Consumable(id, name, description, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type, rarity, isUnique, durationDays, expirationTime, values);
                Equipment = null;
                Weapon = null;
                Substance = null;
                Tool = null;
                break;
            case Item.ItemType.Weapon:
                Consumable = null;
                Equipment = null;
                Weapon = new Weapon(id, name, description, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type, rarity, isUnique, durationDays, expirationTime, values);
                Substance = null;
                Tool = null;
                break;
            case Item.ItemType.Equipment:
                Consumable = null;
                Equipment = new Equipment(id, name, description, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type,  rarity, isUnique, durationDays, expirationTime, values);
                Weapon = null;
                Substance = null;
                Tool = null;
                break;
            case Item.ItemType.Substance:
                Consumable = null;
                Equipment = null;
                Weapon = null;
                Substance = new Substance(id, name, description, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type, rarity, isUnique, durationDays, expirationTime, values);
                Tool = null;
                break;
            case Item.ItemType.Tool:
                Consumable = null;
                Equipment = null;
                Weapon = null;
                Substance = null;
                Tool = new Tool(id, name, description, iconPath, iconId, cost, weight, maxStackCnt, stackCnt, type, rarity, isUnique, durationDays, expirationTime, values);
                break;
        }
    }

    public ItemContainer(ItemContainer item)
    {
        switch (item.Type)
        {
            case Item.ItemType.Consumable:
                Consumable = new Consumable(item.Id, item.Name, item.Description, item.IconPath, item.IconId, item.Cost, item.Weight, item.MaxStackCnt, item.StackCnt, item.Type, item.Rarity,item.IsUnique, item.DurationDays, item.ExpirationTime, item.Values);
                Equipment = null;
                Weapon = null;
                Substance = null;
                Tool = null;
                break;
            case Item.ItemType.Weapon:
                Consumable = null;
                Equipment = null;
                Weapon = new Weapon(item.Id, item.Name, item.Description, item.IconPath, item.IconId, item.Cost, item.Weight, item.MaxStackCnt, item.StackCnt, item.Type, item.Rarity, item.IsUnique, item.DurationDays, item.ExpirationTime, item.Values);
                Substance = null;
                Tool = null;
                break;
            case Item.ItemType.Equipment:
                Consumable = null;
                Equipment = new Equipment(item.Id, item.Name, item.Description, item.IconPath, item.IconId, item.Cost, item.Weight, item.MaxStackCnt, item.StackCnt, item.Type, item.Rarity,item.IsUnique, item.DurationDays, item.ExpirationTime, item.Values);
                Weapon = null;
                Substance = null;
                Tool = null;
                break;
            case Item.ItemType.Substance:
                Consumable = null;
                Equipment = null;
                Weapon = null;
                Substance = new Substance(item.Id, item.Name, item.Description, item.IconPath, item.IconId, item.Cost, item.Weight, item.MaxStackCnt, item.StackCnt, item.Type, item.Rarity, item.IsUnique, item.DurationDays, item.ExpirationTime, item.Values);
                Tool = null;
                break;
            case Item.ItemType.Tool:
                Consumable = null;
                Equipment = null;
                Weapon = null;
                Substance = null;
                Tool = new Tool(item.Id, item.Name, item.Description, item.IconPath, item.IconId, item.Cost, item.Weight, item.MaxStackCnt, item.StackCnt, item.Type, item.Rarity, item.IsUnique, item.DurationDays, item.ExpirationTime, item.Values);
                break;
        }
    }

    public ItemContainer()
    {
        Consumable = null;
        Weapon = null;
        Equipment = null;
        Substance = null;
        Tool = null;
    }

    public string GetTooltip()
    {
        switch (Type)
        {
            case Item.ItemType.Consumable:
                return Consumable.GetTooltip();
            case Item.ItemType.Weapon:
                return Weapon.GetTooltip();
            case Item.ItemType.Equipment:
                return Equipment.GetTooltip();
            case Item.ItemType.Substance:
                return Substance.GetTooltip();
            case Item.ItemType.Tool:
                return Tool.GetTooltip();
            default:
                return "";
        }
    }

    public Sprite GetSprite()
    {
        switch (Type)
        {
            case Item.ItemType.Consumable:
                return Consumable.GetSprite();
            case Item.ItemType.Weapon:
                return Weapon.GetSprite();
            case Item.ItemType.Equipment:
                return Equipment.GetSprite();
            case Item.ItemType.Substance:
                return Substance.GetSprite();
            case Item.ItemType.Tool:
                return Tool.GetSprite();
            default:
                return null;
        }
    }

    internal void UseItem(int value)
    {
        switch (Type)
        {
            case Item.ItemType.Tool:
                Tool.TimeToUse -= value;
                if (Tool.TimeToUse ==0)
                {
                    setStackCnt(0);
                }
                break;
        }
    }

    public bool Exist(List<ItemContainer> items)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i].Name == this.Name)
                return true;
        return false;
    }


    public void setStackCnt(int value)
    {
        switch (Type)
        {
            case Item.ItemType.Consumable:
                Consumable.StackCnt = value;
                break;
            case Item.ItemType.Weapon:
                Weapon.StackCnt = value;
                break;
            case Item.ItemType.Equipment:
                Equipment.StackCnt = value;
                break;
            case Item.ItemType.Substance:
                Substance.StackCnt = value;
                break;
            case Item.ItemType.Tool:
                Tool.StackCnt = value;
                break;
        }
    }


    internal void Print()
    {
        if (Id == -1)
        {
            Debug.Log("Id:" + Id);
            return;
        }
        Sprite Sprite = this.GetSprite();
        Debug.Log("Id:" + Id + " Name:" + Name + " Sprite:" + Sprite.name + " Type:" + Type + "StackCnt:" + StackCnt + " Rarity:" + Rarity );
    }
}
