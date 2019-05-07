using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;


//#### Any update here need to be align with MonsterSetting
[Serializable]
public class CharacterSetting {
    public int Id;
    public int UserId;
    public int CharacterId;
    public string Name;
    public int Level;
    public bool Updated;
    public string LastUpdated;
    public bool IsEnable;
    public bool FightMode;
    public bool Alive;
    public int Life;
    public int Coin;
    public int Experience;
    public int MaxExperience;
    public int HandsCnt;
    public int MaxHealth;
    public int Health;
    public int MaxMana;
    public int Mana;
    public int MaxEnergy;
    public int Energy;
    public float SpeedAttack;
    public float SpeedDefense;
    public float AbilityAttack;
    public float AbilityDefense;
    public float MagicAttack;
    public float MagicDefense;
    public float PoisonAttack;
    public float PoisonDefense;
    public Character.Elements Element;
    public float Carry;
    public int CarryCnt;
    public float Speed;
    public float Intellect;
    public float Agility;
    public float Strength;
    public float Stamina;
    public float Crafting;
    public float Researching;
    public float Bravery;
    public float Charming;

    public CharacterSetting(int id = -1, int userId = -1, int characterId = -1, string name = null,
        int level = 0,  bool updated = true,  bool isEnable = true, 
        bool fightMode = false, bool alive = true,
        int life = 0, int coin = 100,
        int experience = 0, int maxExperience = 0, int handsCnt = 2, 
        int maxHealth = 0,  int health = 0, int maxMana = 0, int mana = 0, int maxEnergy = 0,int energy = 0, 
        float attackSpeed = 0, float defenseSpeed = 0, float abilityAttack = 0, float abilityDefense = 0,
        float magicAttack = 0, float magicDefense = 0, float poisonAttack = 0, float poisonDefense = 0, 
        Character.Elements element = 0,float carry = 0, int carryCnt = 0,
        float speed = 0, float intellect = 0, float agility = 0, float strength = 0, float stamina = 0,
        float crafting = 0, float researching = 0, float bravery = 0, float charming = 0)
    {
        Id = id;
        UserId = userId;
        CharacterId = characterId;
        Name = name;
        Level = level;
        Life = life;
        Updated = updated;
        LastUpdated = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        IsEnable = isEnable;
        FightMode = fightMode;
        Alive = alive;
        Coin = coin;
        Experience = experience;
        MaxExperience = maxExperience;
        HandsCnt = handsCnt;
        MaxHealth = maxHealth;
        Health = health;
        MaxMana = maxMana;
        Mana = mana;
        MaxEnergy = maxEnergy;
        Energy = energy;
        SpeedAttack = attackSpeed;
        SpeedDefense = defenseSpeed;
        AbilityAttack = abilityAttack;
        AbilityDefense = abilityDefense;
        MagicAttack = magicAttack;
        MagicDefense = magicDefense;
        PoisonAttack = poisonAttack;
        PoisonDefense = poisonDefense;
        Element = element;
        Carry = carry;
        CarryCnt = carryCnt;
        Agility = agility;
        Bravery = bravery;
        Charming = charming;
        Intellect = intellect;
        Crafting = crafting;
        Researching = researching;
        Speed = speed;
        Stamina = stamina;
        Strength = strength;
    }
    public CharacterSetting()
    {
        Id = -1;
    }
    public CharacterSetting(CharacterSetting characterSetting)
    {
        Id = characterSetting.Id;
        UserId = characterSetting.UserId;
        CharacterId = characterSetting.CharacterId;
        Name = characterSetting.Name;
        Level = characterSetting.Level;
        Life = characterSetting.Life;
        Updated = characterSetting.Updated;
        LastUpdated = characterSetting.LastUpdated;
        IsEnable = characterSetting.IsEnable;
        FightMode = characterSetting.FightMode;
        Alive = characterSetting.Alive;
        Coin = characterSetting.Coin;
        Experience = characterSetting.Experience;
        MaxExperience = characterSetting.MaxExperience;
        HandsCnt = characterSetting.HandsCnt;
        MaxHealth = characterSetting.MaxHealth;
        Health = characterSetting.Health;
        MaxMana = characterSetting.MaxMana;
        Mana = characterSetting.Mana;
        MaxEnergy = characterSetting.MaxEnergy;
        Energy = characterSetting.Energy;
        SpeedAttack = characterSetting.SpeedAttack;
        SpeedDefense = characterSetting.SpeedDefense;
        AbilityAttack = characterSetting.AbilityAttack;
        AbilityDefense = characterSetting.AbilityDefense;
        MagicAttack = characterSetting.MagicAttack;
        MagicDefense = characterSetting.MagicDefense;
        PoisonAttack = characterSetting.PoisonAttack;
        PoisonDefense = characterSetting.PoisonDefense;
        Agility = characterSetting.Agility;
        Bravery = characterSetting.Bravery;
        Carry = characterSetting.Carry;
        CarryCnt = characterSetting.CarryCnt;
        Charming = characterSetting.Charming;
        Intellect = characterSetting.Intellect;
        Crafting = characterSetting.Crafting;
        Researching = characterSetting.Researching;
        Speed = characterSetting.Speed;
        Stamina = characterSetting.Stamina;
        Strength = characterSetting.Strength;
    }
    internal void Print()
    {
        Debug.Log("CharacterSetting = " + MyInfo());
    }
    internal string MyInfo()
    {
        return Id + "-" + Name + " (" + Level + ")"
               + " Energy:" + Energy
               + " Health:" + Health
               + " Coin:" + Coin
               + " Mana:" + Mana
               + " Intellect:" + Intellect
            ;
    }
    internal Sprite GetSpellSprite()
    {
        var spells = Resources.LoadAll<Sprite>("Spells").ToList();
        if (PoisonAttack > AbilityAttack   && PoisonAttack > MagicAttack)
            return spells[1];
        if (MagicAttack > PoisonAttack &&  MagicAttack  > AbilityAttack )
            return spells[2];
        if (AbilityAttack > PoisonAttack && AbilityAttack > MagicAttack)
            return spells[0];
        return spells[3];
    }
    internal string FieldValue(string field)
    {
        switch (field)
        {
            case "MaxHealth":
                return MaxHealth.ToString();
            case "Health":
                return Health.ToString();
            case "Energy":
                return Energy.ToString();
            case "Experience":
                return Experience.ToString();
            case "Coin":
                return Coin.ToString();
            case "CarryCnt":
                return CarryCnt.ToString();
            case "Life":
                return Life.ToString();
            case "Alive":
                return Alive.ToString();
            case "Agility":
                return Agility.ToString();
            case "Crafting":
                return Crafting.ToString();
            default :
                return field +" Is Undefined in characterSetting";
        }
    }
    internal string GetInfo(string field)
    {
        switch (field)
        {
            case "Defense":
                return "(" + AbilityDefense + "/"+ MagicDefense + "/"+ PoisonDefense + ")";
            case "Attack":
                return "(" + AbilityAttack + "/" + MagicAttack + "/" + PoisonAttack + ")";
            default:
                return "Undefined field in characterSetting GetInfo";
        }
    }
    #region HealthCheck
    internal int CalculateHealthCheck()
    {
        return 
                (this.Coin * (int) FieldCode.Coin +
                0)
                * RandomHelper.StringToRandomNumber(Name)
            ;
    }
    internal int CalculateHealthCheckByField(int value, string field)
    {
        var fieldCode = (int) GetFieldCode(field);
        return value * fieldCode * RandomHelper.StringToRandomNumber(Name);
    }
    private static FieldCode GetFieldCode(string field)
    {
        switch (field)
        {
            case "Health":
                return FieldCode.Health;
            case "Mana":
                return FieldCode.Mana;
            case "Energy":
                return FieldCode.Energy;
            case "Experience":
                return FieldCode.Experience;
            case "Coin":
                return FieldCode.Coin;
            case "CarryCnt":
                return FieldCode.CarryCnt;
            case "Life":
                return FieldCode.Life;
            case "Alive":
                return FieldCode.Alive;
            case "Agility":
                return FieldCode.Agility;
            case "Crafting":
                return FieldCode.Crafting;
            default:
                return FieldCode.None;
        }
    }
    public enum FieldCode
    {
        None = 0,
        Health =1,
        Mana=2,
        Energy=3,
        Experience=4,
        Coin=5,
        CarryCnt=6,
        Life=7,
        Alive=8,
        Agility=9,
        Crafting=10,
    }
    #endregion
}
