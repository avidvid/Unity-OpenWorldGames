using System.Linq;
using UnityEngine;

public class MonsterIns
{
    private Character _monsterCharacter;
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public int CharacterSettingId { get; set; }
    public int Level { get; set; }
    public int MaxHealth { get; set; }
    public int Health { get; set; }
    public float SpeedAttack { get; set; }
    public float SpeedDefense { get; set; }
    public float AbilityAttack { get; set; }
    public float AbilityDefense { get; set; }
    public float MagicAttack { get; set; }
    public float MagicDefense { get; set; }
    public float PoisonAttack { get; set; }
    public float PoisonDefense { get; set; }
    public float Speed { get; set; }
    public float View { get; set; }
    public Character.Elements Element { get; set; }

    public MonsterIns(int id, int characterId, int characterSettingId, int level, int maxHealth, int health, float attackSpeed, float defenseSpeed, float abilityAttack, float abilityDefense, float magicAttack, float magicDefense, float poisonAttack, float poisonDefense, float speed, float view, Character.Elements element)
    {
        Id = id;
        CharacterId = characterId;
        CharacterSettingId = characterSettingId;
        Level = level;
        MaxHealth = maxHealth;
        Health = health;
        SpeedAttack = attackSpeed;
        SpeedDefense = defenseSpeed;
        AbilityAttack = abilityAttack;
        AbilityDefense = abilityDefense;
        MagicAttack = magicAttack;
        MagicDefense = magicDefense;
        PoisonAttack = poisonAttack;
        PoisonDefense = poisonDefense;
        Speed = speed;
        View = view;
        Element = element;
    }
    public MonsterIns(Character monsterCharacter, int level)
    {
        //Logic Should be align with CharacterManager:LevelCalculations()
        this._monsterCharacter = monsterCharacter;
        Id = 0;
        CharacterId = monsterCharacter.Id;
        CharacterSettingId = -1;
        if (level<3)
            level += 2;
        Level = Random.Range(level - 2, level + 3);
        //#Health/Mana/Energy
        MaxHealth = ((int)monsterCharacter.Body + Level ) * 100;
        Health = MaxHealth;
        //#Speed
        Speed = (int) monsterCharacter.Speed + Level/10;
        SpeedAttack = Speed;
        SpeedDefense = Speed;
        //#View
        View = (float)monsterCharacter.View;
        //#Attack/Defense
        AbilityAttack = monsterCharacter.CheckAttackType(monsterCharacter.AttackT, "Strength") ? monsterCharacter.BasicAttack + level / 5f : 0;
        AbilityDefense = monsterCharacter.CheckAttackType(monsterCharacter.DefenseT, "Strength") ? monsterCharacter.BasicDefense + level / 5f : 0;
        MagicAttack = monsterCharacter.CheckAttackType(monsterCharacter.AttackT, "Magic") ? monsterCharacter.BasicAttack + level / 5f : 0;
        MagicDefense = monsterCharacter.CheckAttackType(monsterCharacter.DefenseT, "Magic") ? monsterCharacter.BasicDefense + level / 5f : 0;
        PoisonAttack = monsterCharacter.CheckAttackType(monsterCharacter.AttackT, "Poison") ? monsterCharacter.BasicAttack + level / 5f : 0;
        PoisonDefense = monsterCharacter.CheckAttackType(monsterCharacter.DefenseT, "Poison") ? monsterCharacter.BasicDefense + level / 5f : 0;
        //#Element
        Element = Character.Elements.None;
    }

    public Character GetCharacter()
    {
        return _monsterCharacter;
    }

    internal void Print()
    {
        Debug.Log(
            " Monster:" + Id +
            " CharacterSettingId:" + CharacterSettingId +
            " CharacterId:" + CharacterId 
            );
    }

    internal Sprite GetSpellSprite()
    {
        var spells = Resources.LoadAll<Sprite>("Spells").ToList();
        if (PoisonAttack > AbilityAttack && PoisonAttack > MagicAttack)
            return spells[1];
        if (MagicAttack > PoisonAttack && MagicAttack > AbilityAttack)
            return spells[2];
        if (AbilityAttack > PoisonAttack && AbilityAttack > MagicAttack)
            return spells[0];
        return spells[3];
    }
}
