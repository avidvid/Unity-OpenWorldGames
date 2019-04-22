using System;
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
    //todo: make all set private to not screw db ?? 
    public int Id;
    public string Name;
    public string Description;
    public string IconPath;
    public int IconId;
    public bool IsAnimated;
    public bool HasFightMode;
    public CharacterType Move;
    public DataTypes.Range AttackR;
    public DataTypes.Range DefenseR;
    public AttackType AttackT;
    public AttackType DefenseT;
    public float BasicAttack;
    public float BasicDefense;
    public SpeedType Speed;
    public BodyType Body;
    public CarryType Carry;
    public ViewType View;
    public DataTypes.Rarity Rarity;
    public TerrainIns.TerrainType FavoriteTerrainTypes;
    public string DropItems;
    public float DropChance;
    public bool IsEnable;

    public Character(int id, string name, string desc, 
        CharacterType moveT, DataTypes.Range attackR, DataTypes.Range defenseR, AttackType attackT, AttackType defenseT,
        int basicAttack, int basicDefense, 
        SpeedType speedT, BodyType bodyT, CarryType carryT, ViewType viewT, DataTypes.Rarity rarity,
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
    }
    public Sprite GetSprite()
    {
        Sprite[] characterSprites = Resources.LoadAll<Sprite>("Characters/" + Name.Replace(" ", ""));
        // Get specific sprite
        return characterSprites.Single(s => s.name == "down_3");
    }
    public List<Sprite> GetSprites()
    {
        List<Sprite> moveSprites = new List<Sprite>();
        // Load all sprites in atlas
        Sprite[] abilityIconsAtlas = Resources.LoadAll<Sprite>("Characters/" + Name.Replace(" ", ""));
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
        return (RuntimeAnimatorController)Resources.Load(animationPath + Name.Replace(" ", "") + "Controller");
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