using System;
using System.Globalization;
using System.Linq;
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

    public CharacterSetting(int userId, int characterId, string name)
    {
        Id = UnityEngine.Random.Range(0, 1999999999);
        UserId = userId;
        CharacterId = characterId;
        Name = name;
        Level = 0;
        Life = 0;
        Updated = true;
        LastUpdated = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        IsEnable = true;
        FightMode = false;
        Alive = true;
        Coin = 100;
        Experience = MaxExperience = 0;
        HandsCnt = 2;
        MaxHealth =Health = 0;
        MaxMana = Mana = 0;
        MaxEnergy = Energy = 0;
        SpeedAttack = SpeedDefense = 0;
        AbilityAttack = AbilityDefense = MagicAttack = 0;
        MagicDefense = PoisonAttack = PoisonDefense = 0;
        Element = 0;
        Carry = CarryCnt = 0;
        Agility = Bravery = Charming = Intellect = Crafting = Researching = Speed = Stamina = Strength = 0;
    }
    internal void Print()
    {
        Debug.Log("CharacterSetting = " + MyInfo());
    }
    internal string MyInfo()
    {
        try
        {
            return Id + "-" + Name + " (" + Level + ")"
                   + " Energy:" + Energy
                   + " Health:" + Health
                   + " Coin:" + Coin
                   + " Mana:" + Mana
                   + " Intellect:" + Intellect
                   + " IsEnable=" + IsEnable
                ;
        }
        catch (Exception e)
        {
            return "Empty CharacterSetting";
        }
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