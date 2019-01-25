using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;


//#### Any update here need to be align with MonsterSetting
[Serializable]
public class CharacterSetting {
    public int Id { get; set; }
    public int UserPlayerId { get; set; }
    public int CharacterId { get; set; }
    public string Name { get; set; }
    public int Level { get; set; }
    public bool Updated { get; set; }
    public DateTime LastUpdated { get; set; }
    public bool IsEnable { get; set; }
    public bool FightMode { get; set; }
    public bool Alive { get; set; }
    public int Life { get; set; }
    public int Coin { get; set; }
    public int Gem { get; set; }
    public int Experience { get; set; }
    public int MaxExperience { get; set; }
    public int HandsCnt { get; set; }
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    public int MaxMana { get; set; }
    public int Mana { get; set; }
    public int MaxEnergy { get; set; }
    public int Energy { get; set; }
    public float SpeedAttack { get; set; }
    public float SpeedDefense { get; set; }
    public float AbilityAttack { get; set; }
    public float AbilityDefense { get; set; }
    public float MagicAttack { get; set; }
    public float MagicDefense { get; set; }
    public float PoisonAttack { get; set; }
    public float PoisonDefense { get; set; }
    public Character.Elements Element { get; set; }
    public float Carry { get; set; }
    public int CarryCnt { get; set; }
    public float Speed { get; set; }
    public float Intellect { get; set; }
    public float Agility { get; set; }
    public float Strength { get; set; }
    public float Stamina { get; set; }
    public float Crafting { get; set; }
    public float Researching { get; set; }
    public float Bravery { get; set; }
    public float Charming { get; set; }
    public List<ItemContainer> Equipments { get; set; }
    //Todo: add the hashcode

    public CharacterSetting(int id = -1, int userPlayerId = -1, int characterId = -1, string name = null,
        int level = 0,  bool updated = true, bool isEnable = true, 
        bool fightMode = false, bool alive = true,
        int life = 0, int coin = 100, int gem = 0,
        int experience = 0, int maxExperience = 0, int handsCnt = 2, 
        int maxHealth = 0,  int health = 0, int maxMana = 0, int mana = 0, int maxEnergy = 0,int energy = 0, 
        float attackSpeed = 0, float defenseSpeed = 0, float abilityAttack = 0, float abilityDefense = 0,
        float magicAttack = 0, float magicDefense = 0, float poisonAttack = 0, float poisonDefense = 0, 
        Character.Elements element = 0,float carry = 0, int carryCnt = 0,
        float speed = 0, float intellect = 0, float agility = 0, float strength = 0, float stamina = 0,
        float crafting = 0, float researching = 0, float bravery = 0, float charming = 0, List<ItemContainer>  equipments = null)
    {
        Id = id;
        UserPlayerId = userPlayerId;
        CharacterId = characterId;
        Name = name;
        Level = level;
        Life = life;
        Updated = updated;
        LastUpdated = DateTime.Now;
        IsEnable = isEnable;
        FightMode = fightMode;
        Alive = alive;
        Coin = coin;
        Gem = gem;
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
        if (equipments ==null)
        {
            Equipments = new List<ItemContainer>();
            for (int i = 0; i < (int) Equipment.PlaceType.None; i++)
                Equipments.Add(new ItemContainer());
        }
        else
            Equipments = equipments;
    }
    
    public CharacterSetting()
    {
        Id = -1;
    }

    public CharacterSetting(CharacterSetting characterSetting)
    {
        Id = characterSetting.Id;
        UserPlayerId = characterSetting.UserPlayerId;
        CharacterId = characterSetting.CharacterId;
        Name = characterSetting.Name;
        Level = characterSetting.Level;
        Life = characterSetting.Life;
        Updated = characterSetting.Updated;
        LastUpdated = DateTime.Now;
        IsEnable = characterSetting.IsEnable;
        FightMode = characterSetting.FightMode;
        Alive = characterSetting.Alive;
        Coin = characterSetting.Coin;
        Gem = characterSetting.Gem;
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
        Equipments = characterSetting.Equipments;
    }
    internal void Print()
    {
        Debug.Log("CharacterSetting = " + Id+"-"+Name+" ("+ Level+")");
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
            case "Health":
                return Health.ToString();
            case "Energy":
                return Energy.ToString();
            case "Experience":
                return Experience.ToString();
            case "Gem":
                return Gem.ToString();
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
                return "Undefined field in characterSetting";
        }
    }
}
