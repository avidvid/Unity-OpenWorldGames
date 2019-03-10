﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Character {
    public enum CharacterType
    {
        Walk,
        Fly,
        Swim
    }
    public enum AttackRange
    {
        Short = 1,
        Medium,
        Long
    }
    public enum DefenseRange
    {
        Short = 1,
        Medium,
        Long
    }
    public enum AttackType
    {
        Strength, 
        Magic,
        Poison,
        StrengthMagic,
        StrengthPoison,
        MagicPoison,
        StrengthMagicPoison,
    }
    public enum SpeedType
    {
        None = 0,
        Slug = 2,
        Slow = 3,
        Regular = 5,
        Fast = 7,
        Rapid = 10
    }
    public enum BodyType
    {
        Tiny = 1,
        Slim,
        Regular,
        Muscle,
        Tank
    }
    public enum CarryType
    {
        Slight = 3,
        Light,
        Normal,
        Heavy,
        Hefty
    }
    public enum Elements
    {
        None,
        Ice,
        Fire,
        Water,
        Earth,
        Metal
    }
    public enum ViewType
    {
        None,
        Small,
        Medium,
        Large,
        ELarge = 6
    }
    public enum CharRarity
    {
        None = -1,
        Saint =0,
        Legendary=2,
        Saga=3,
        Rare=10,
        Uncommon=40,
        Common=100
    }

    //todo: make all set private to not screw db ?? 
    public int Id { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string IconPath { get; set; }
    public int IconId { get; set; }
    public bool IsAnimated { get; set; }
    public bool HasFightMode { get; set; }
    public CharacterType Move { get; set; }
    public AttackRange AttackR { get; set; }
    public DefenseRange DefenseR { get; set; }
    public AttackType AttackT { get; set; }
    public AttackType DefenseT { get; set; }
    public float BasicAttack { get; set; }
    public float BasicDefense { get; set; }
    public SpeedType Speed { get; set; }
    public BodyType Body { get; set; }
    public CarryType Carry { get; set; }
    public ViewType View { get; set; }
    public CharRarity Rarity { get; set; }
    public TerrainIns.TerrainType FavoriteTerrainTypes { get; set; }
    public string DropItems { get; set; }
    public float DropChance { get; set; }
    public bool IsEnable { get; set; }
    public string Slug { get; set; }

    public Character(int id, string name, string desc, 
        CharacterType moveT, AttackRange attackR, DefenseRange defenseR, AttackType attackT, AttackType defenseT,
        int basicAttack, int basicDefense, 
        SpeedType speedT, BodyType bodyT, CarryType carryT, ViewType viewT, CharRarity rarity,
        TerrainIns.TerrainType favoriteTerrainTypes, string dropItems,float dropChance)
    {
        Id = id;
        Name = name;
        Description = desc;
        IconPath = "Somewhere";
        IconId = id;
        IsAnimated = true;
        HasFightMode = false;
        Move = moveT;
        AttackR = attackR;
        DefenseR = defenseR;
        AttackT = attackT;
        DefenseT = defenseT;
        BasicAttack = basicAttack;
        BasicDefense = basicDefense;
        Speed = speedT;
        Body = bodyT;
        Carry = carryT;
        View = viewT;
        Rarity = rarity;
        DropItems = dropItems;
        DropChance = dropChance;
        FavoriteTerrainTypes = favoriteTerrainTypes;
        IsEnable = true;
        Slug = name.Replace(" ", "");
    }
    public Character()
    {
        Id = -1;
    }
    public Character(Character character,bool enable = true)
    {
        Id = character.Id;
        Name = character.Name;
        Description = character.Description;
        IconPath = character.IconPath;
        IconId = character.IconId;
        IsAnimated = character.IsAnimated;
        HasFightMode = character.HasFightMode;
        Move = character.Move;
        AttackR = character.AttackR;
        DefenseR = character.DefenseR;
        AttackT = character.AttackT;
        DefenseT = character.DefenseT;
        BasicAttack = character.BasicAttack;
        BasicDefense = character.BasicDefense;
        Speed = character.Speed;
        Body = character.Body;
        Carry = character.Carry;
        View = character.View;
        Rarity = character.Rarity;
        DropItems = character.DropItems;
        DropChance = character.DropChance;
        FavoriteTerrainTypes = character.FavoriteTerrainTypes;
        IsEnable = enable;
        Slug = character.Slug;
    }
    public Sprite GetSprite()
    {
        Sprite[] characterSprites = Resources.LoadAll<Sprite>("Characters/" + Slug);
        // Get specific sprite
        return characterSprites.Single(s => s.name == "down_3");
    }
    public List<Sprite> GetSprites()
    {
        List<Sprite> moveSprites = new List<Sprite>();
        // Load all sprites in atlas
        Sprite[] abilityIconsAtlas = Resources.LoadAll<Sprite>("Characters/" + Slug);
        // Get specific sprite
        moveSprites.Add(abilityIconsAtlas.Single(s => s.name == "right_3"));
        moveSprites.Add(abilityIconsAtlas.Single(s => s.name == "left_3"));
        moveSprites.Add(abilityIconsAtlas.Single(s => s.name == "up_3"));
        moveSprites.Add(abilityIconsAtlas.Single(s => s.name == "down_3"));
        return moveSprites;
    }
    public RuntimeAnimatorController GetAnimator()
    {
        // Load Animation Controllers
        string animationPath = "Characters/Animations/";
        return (RuntimeAnimatorController)Resources.Load(animationPath + Slug + "Controller");
    }
    internal bool CheckAttackType(AttackType attackT, string checkSum)
    {
        switch (checkSum)
        {
            case "Strength":
                if (attackT == AttackType.Strength ||
                    attackT == AttackType.StrengthMagic ||
                    attackT == AttackType.StrengthPoison ||
                    attackT == AttackType.StrengthMagicPoison)
                    return true;
                break;
            case "Magic":
                if (attackT == AttackType.Magic ||
                    attackT == AttackType.StrengthMagic ||
                    attackT == AttackType.MagicPoison ||
                    attackT == AttackType.StrengthMagicPoison)
                    return true;
                break;
            case "Poison":
                if (attackT == AttackType.Poison ||
                    attackT == AttackType.StrengthPoison ||
                    attackT == AttackType.StrengthPoison ||
                    attackT == AttackType.StrengthMagicPoison)
                    return true;
                break;
        }
        return false;
    }
    internal string GetTooltip()
    {
        var tooltip = "<color=green>  " + Id + "  -" + Name + "</color>\n\n" + Description;
        return tooltip;
    }
    internal void Print()
    {
        Debug.Log("Character = " +  MyInfo());
    }
    internal string MyInfo()
    {
        return Id + "-" + Name + " (" + Description + ")";
    }
}
