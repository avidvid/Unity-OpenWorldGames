using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _characterManager;
    private ItemDatabase _itemDatabase;
    private CharacterDatabase _characterDatabase;
    private TerrainDatabase _terrainDatabase;

    public Character Character;
    public UserPlayer UserPlayer;

    private Sprite _spellSprite;
    public CharacterSetting CharacterSetting;
    public CharacterMixture CharacterMixture;
    public CharacterResearching CharacterResearching=new CharacterResearching();
    public List<ItemContainer> CharacterInventory = new List<ItemContainer>();
    public List<CharacterResearch> CharacterResearches = new List<CharacterResearch>();
    public List<Research> Researches = new List<Research>();

    private float _nextActionTime = 100;
    private GameObject _levelUp;
    private GameObject _gameOver;

    void Awake()
    {
        _characterDatabase = CharacterDatabase.Instance();
        _characterManager = CharacterManager.Instance();
        _itemDatabase = ItemDatabase.Instance();
        _levelUp = GameObject.Find("LevelUp");
        _gameOver = GameObject.Find("GameOver");
        //UserPlayer
        UserPlayer = _characterDatabase.FindUserPlayer(0);
        UserPlayer.Print();
        if (UserPlayer.Id == -1)
        {
            print("UserPlayer is empty");
            GoToStartScene();
            return;
        }
        UserPlayer.LastLogin = DateTime.Now;
        SaveUserPlayer();
        //CharacterSetting
        CharacterSetting = _characterDatabase.FindCharacterSetting(UserPlayer.Id);
        if (CharacterSetting.Id == -1)
        {
            print("CharacterSetting is empty");
            GoToStartScene();
            return;
        }
        CharacterSetting.Print();
        if (DateTime.Now < UserPlayer.LockUntil)
        {
            GoToWaitScene();
            return;
        }
        Character = _characterDatabase.FindCharacter(CharacterSetting.CharacterId);
        Character.Print();
        CharacterMixture = _characterDatabase.FindCharacterMixture(CharacterSetting.Id);
        CharacterMixture.Print();
        CharacterInventory = _characterDatabase.FindCharacterInventory(CharacterSetting.Id);
        if (CharacterInventory.Count == 0)
            throw new Exception("CharacterInventory count is ZERO");
    }

    void Start()
    {
        
        //CharacterResearch
        Researches = _characterDatabase.LoadResearches();
        if (Researches.Count == 0)
            throw new Exception("Researches count is ZERO");
        CharacterResearches = _characterDatabase.LoadCharacterResearches(UserPlayer.Id);
        print("CharacterResearches count == "+ CharacterResearches.Count);
        CharacterResearching = _characterDatabase.FindCharacterResearching(UserPlayer.Id);
        CharacterResearching.Print();
        if (SettingBasicChecks()) 
            return;
        LoginCalculations();
    }
    void Update()
    {
        if (SettingBasicChecks())
            return;

        //Refresh User stats Health Mana Energy Level
        float period = 100 - CharacterSetting.Level;

        if (Time.time > _nextActionTime)
        {
            _nextActionTime += period;
            print("Executed Increase Health Mana Energy next time =" + _nextActionTime);
            if (CharacterSetting.Energy < CharacterSetting.MaxEnergy)
                //todo: CharacterSetting.Energy += 100;
                CharacterSetting.Energy = CharacterSetting.MaxEnergy;

            if (CharacterSetting.Health < CharacterSetting.MaxHealth)
                CharacterSetting.Health += 1;
            if (CharacterSetting.Mana < CharacterSetting.MaxMana)
                CharacterSetting.Mana += 1;
            CharacterSetting.Updated = true;
            SaveCharacterSetting();
        }
        //Level Up
        if (CharacterSetting.Experience >= CharacterSetting.MaxExperience)
        {
            StartCoroutine(LevelUpFadeInOut());
            CharacterSetting.Experience -= CharacterSetting.MaxExperience;
            //Todo: Level upgrade is limited to 10
            if (CharacterSetting.Level < 10)
            {
                CharacterSetting.Level += 1;
                //Calculate all the values based on the new level
                LevelCalculations();
            }
        }
    }
    internal Vector2 GetMapLocation(int key)
    {
        //Latitudes range from 0 to 90. Longitudes range from 0 to 180
        _terrainDatabase = TerrainDatabase.Instance();
        Vector2 regionLocation = _terrainDatabase.GetRegionLocation(key);
        int x =(int) (UserPlayer.Latitude * 1000 - regionLocation.x) ;
        int y =(int) (UserPlayer.Longitude * 1000 - regionLocation.y) ;
        var mapLocation = new Vector2(x,y);
        return mapLocation;
    }

    public void SaveCharacterInventory()
    {
        _characterDatabase.SaveCharacterInventory();
    }
    internal bool HaveAvailableSlot()
    {
        for (int i = 0; i < CharacterSetting.CarryCnt; i++)
            if (CharacterInventory[i].Id == -1)
                return true;
        return false;
    }
    internal bool AddItemToInventory(ItemContainer item)
    {
        //Should match with the AddItemToInventory in InventoryHandler
        //CheckUniqueness
        if (item.IsUnique)
            if (_characterManager.InventoryExists(item.Id))
                return false;
        for (int i = 0; i < CharacterSetting.CarryCnt; i++)
        {
            if (CharacterInventory[i].Id == item.Id)
            {
                if (CharacterInventory[i].StackCnt + item.StackCnt <= CharacterInventory[i].MaxStackCnt)
                {
                    CharacterInventory[i].setStackCnt(CharacterInventory[i].StackCnt + item.StackCnt);
                    break;
                }
            }

            if (CharacterInventory[i].Id != -1)
                continue;
            CharacterInventory[i] = item;
            CharacterInventory[i].Print();
            break;
        }

        _characterDatabase.SaveCharacterInventory();
        return true;
    }

    internal void CharacterSettingApplyResearch(CharacterResearch charResearch, Research research)
    {
        if (charResearch == null || research == null)
            return;
        float value = research.CalculateValue(charResearch.Level);
        _characterDatabase.AddCharacterResearch(charResearch);
        _characterDatabase.EmptyCharacterResearching();
        AddCharacterSetting(research.Target, value);
    }

    public bool InventoryExists(int id)
    {
        //checkInventory for the item
        if (_characterManager.InventoryIndexOf(id) > -1)
            return true;
        if (_characterManager.EquipmentIndexOf(id) > -1)
            return true;
        return false;
    }
    private int EquipmentIndexOf(int id)
    {
        for (int i = 0; i < CharacterSetting.Equipments.Count; i++)
            if (CharacterSetting.Equipments[i].Id == id)
                return i;
        return -1;
    }
    private int InventoryIndexOf(int id)
    {
        for (int i = 0; i < CharacterSetting.CarryCnt; i++)
            if (CharacterInventory[i].Id == id)
                return i;
        return -1;
    }
    internal bool UseEnergy(int amount)
    {
        if (amount <= 0)
            return true;
        if (CharacterSetting.Energy > amount)
        {
            AddCharacterSetting("Energy", -amount);
            return true;
        }
        return false;
    }
    public void AddCharacterSetting(string field, float value)
    {
        print(field + "= CharacterSetting." + CharacterSetting.FieldValue(field) + " " + value);
        switch (field)
        {
            case "Health":
                CharacterSetting.Health += (int)value;
                if (CharacterSetting.Health < 0)
                    GameOverCalculations();
                CharacterSetting.Updated = true;
                break;
            case "Energy":
                CharacterSetting.Energy += (int)value;
                CharacterSetting.Updated = true;
                break;
            case "Experience":
                CharacterSetting.Experience += (int)value;
                CharacterSetting.Updated = true;
                break;
            case "Gem":
                CharacterSetting.Gem += (int)value;
                CharacterSetting.Updated = true;
                break;
            case "Coin":
                CharacterSetting.Coin += (int)value;
                CharacterSetting.Updated = true;
                break;
            case "CarryCnt":
                CharacterSetting.CarryCnt += (int)value;
                break;
            case "Life":
                CharacterSetting.Life += (int)value;
                break;
            case "Alive":
                CharacterSetting.Alive = (value > 0) ;  
                break;
            case "Agility":
                CharacterSetting.Agility += value;
                break;
            case "Crafting":
                CharacterSetting.Crafting += value;
                break;
        }
        SaveCharacterSetting();
    }
    public string CharacterSettingUseItem(ItemContainer item,bool save)
    {
        string message = "";
        if (item == null)
            return message;
        item.Print();

        switch (item.Type)
        {
            case Item.ItemType.Consumable:
                if (item.Consumable.Recipe == 1)
                {
                    if (_itemDatabase.AddNewRandomUserRecipe(UserPlayer.Id))
                        message = "You found a Recipe!!";
                    else return "Recipe you found is not readable!! ";
                }
                CharacterSetting.Health += item.Consumable.Health* item.StackCnt;
                CharacterSetting.Mana += item.Consumable.Mana * item.StackCnt;
                CharacterSetting.Energy += item.Consumable.Energy * item.StackCnt;
                CharacterSetting.Coin += item.Consumable.Coin * item.StackCnt;
                CharacterSetting.Gem += item.Consumable.Gem * item.StackCnt;
                if (CharacterSetting.Health > CharacterSetting.MaxHealth)
                    CharacterSetting.Health = CharacterSetting.MaxHealth;
                if (CharacterSetting.Mana > CharacterSetting.MaxMana)
                    CharacterSetting.Mana = CharacterSetting.MaxMana;
                if (CharacterSetting.Energy > CharacterSetting.MaxEnergy)
                    CharacterSetting.Energy = CharacterSetting.MaxEnergy;
                break;
            case Item.ItemType.Equipment:
                CharacterSetting.Agility += item.Equipment.Agility;
                CharacterSetting.Bravery += item.Equipment.Bravery;
                CharacterSetting.Carry += item.Equipment.Carry;
                CharacterSetting.CarryCnt += item.Equipment.CarryCnt;
                CharacterSetting.Charming += item.Equipment.Charming;
                CharacterSetting.Intellect += item.Equipment.Intellect;
                CharacterSetting.Crafting += item.Equipment.Crafting;
                CharacterSetting.Researching += item.Equipment.Researching;
                CharacterSetting.Speed += item.Equipment.Speed;
                CharacterSetting.Stamina += item.Equipment.Stamina;
                CharacterSetting.Strength += item.Equipment.Strength;
                break;
            case Item.ItemType.Weapon:
                CharacterSetting.SpeedAttack += item.Weapon.SpeedAttack;
                CharacterSetting.SpeedDefense += item.Weapon.SpeedDefense;
                CharacterSetting.AbilityAttack += item.Weapon.AbilityAttack;
                CharacterSetting.AbilityDefense += item.Weapon.AbilityDefense;
                CharacterSetting.MagicAttack += item.Weapon.MagicAttack;
                CharacterSetting.MagicDefense += item.Weapon.MagicDefense;
                CharacterSetting.PoisonAttack += item.Weapon.PoisonAttack;
                CharacterSetting.PoisonDefense += item.Weapon.PoisonDefense;
                break;
            case Item.ItemType.Tool:
                return message;
        }
        CharacterSetting.Updated = true;
        if (save)
            SaveCharacterSetting();
        return message;
    }
    internal void CharacterSettingUnuseItem(ItemContainer item, bool save)
    {
        if (item == null)
            return;
        switch (item.Type)
        {
            case Item.ItemType.Consumable:
                return;
            case Item.ItemType.Equipment:
                CharacterSetting.Agility -= item.Equipment.Agility;
                CharacterSetting.Bravery -= item.Equipment.Bravery;
                CharacterSetting.Carry -= item.Equipment.Carry;
                CharacterSetting.CarryCnt -= item.Equipment.CarryCnt;
                CharacterSetting.Charming -= item.Equipment.Charming;
                CharacterSetting.Intellect -= item.Equipment.Intellect;
                CharacterSetting.Crafting -= item.Equipment.Crafting;
                CharacterSetting.Researching -= item.Equipment.Researching;
                CharacterSetting.Speed -= item.Equipment.Speed;
                CharacterSetting.Stamina -= item.Equipment.Stamina;
                CharacterSetting.Strength -= item.Equipment.Strength;
                break;
            case Item.ItemType.Weapon:
                CharacterSetting.SpeedAttack -= item.Weapon.SpeedAttack;
                CharacterSetting.SpeedDefense -= item.Weapon.SpeedDefense;
                CharacterSetting.AbilityAttack -= item.Weapon.AbilityAttack;
                CharacterSetting.AbilityDefense -= item.Weapon.AbilityDefense;
                CharacterSetting.MagicAttack -= item.Weapon.MagicAttack;
                CharacterSetting.MagicDefense -= item.Weapon.MagicDefense;
                CharacterSetting.PoisonAttack -= item.Weapon.PoisonAttack;
                CharacterSetting.PoisonDefense -= item.Weapon.PoisonDefense;
                break;
            case Item.ItemType.Tool:
                return;
        }
        CharacterSetting.Updated = true;
        if (save)
            SaveCharacterSetting();
    }
    internal List<Character> BuildCharacterList(string characterList=null)
    {
        if (characterList == null)
            characterList = UserPlayer.CharacterList;
        List<int> characterIds = characterList.Split(',').Select(Int32.Parse).ToList();
        List<Character> characters = new List<Character>();
        for (int i = 0; i < characterIds.Count; i++)
            characters.Add(_characterDatabase.FindCharacter(characterIds[i]));
        return characters;
    }
    //Calculations
    private void LoginCalculations()
    {
        var diffInSeconds = (CharacterSetting.LastUpdated - DateTime.Now).TotalSeconds;
        if (diffInSeconds > 200)
        {
            var period = diffInSeconds / (100f / CharacterSetting.Level);
            CharacterSetting.Energy += (int)period;
            if (CharacterSetting.Energy > CharacterSetting.MaxEnergy)
                CharacterSetting.Energy = CharacterSetting.MaxEnergy;
            CharacterSetting.Health += (int)period;
            if (CharacterSetting.Health > CharacterSetting.MaxHealth)
                CharacterSetting.Health = CharacterSetting.MaxHealth;
            CharacterSetting.Mana += (int)period;
            if (CharacterSetting.Mana > CharacterSetting.MaxMana)
                CharacterSetting.Mana = CharacterSetting.MaxMana;
        }
    }
    internal void LevelCalculations()
    {
        if (Character.Id == -1)
            Character = _characterDatabase.FindCharacter(CharacterSetting.CharacterId);
        //Logic Should be align with MonsterIns: MonsterIns()
        var level = CharacterSetting.Level;
        CharacterSetting.MaxExperience = CalculateXp(level);
        //#Health/Mana/Energy
        float lastPercentage = (float)CharacterSetting.Health / CharacterSetting.MaxHealth;
        CharacterSetting.MaxHealth = ((int)Character.Body + level) * 100;
        CharacterSetting.Health = (int)lastPercentage * CharacterSetting.MaxHealth;
        lastPercentage = (float)CharacterSetting.Mana / CharacterSetting.MaxMana;
        CharacterSetting.MaxMana = CharacterSetting.MaxHealth / 2;
        CharacterSetting.Mana = (int)lastPercentage * CharacterSetting.MaxMana;
        lastPercentage = (float)CharacterSetting.Energy / CharacterSetting.MaxEnergy;
        CharacterSetting.MaxEnergy = CharacterSetting.MaxHealth * 2;
        CharacterSetting.Energy = (int)lastPercentage * CharacterSetting.MaxEnergy;
        if (level == 0)
        {
            CharacterSetting.Health = CharacterSetting.MaxHealth;
            CharacterSetting.Mana = CharacterSetting.MaxMana;
            CharacterSetting.Energy = CharacterSetting.MaxEnergy;
        }
        //#Speed
        CharacterSetting.Speed = (int)Character.Speed + level / 10;
        CharacterSetting.SpeedAttack = CharacterSetting.Speed;
        CharacterSetting.SpeedDefense = CharacterSetting.Speed;
        //#Attack/Defense
        CharacterSetting.AbilityAttack = Character.CheckAttackType(Character.AttackT, "Strength") ? Character.BasicAttack + level / 5f : 0;
        CharacterSetting.AbilityDefense = Character.CheckAttackType(Character.DefenseT, "Strength") ? Character.BasicDefense + level / 5f : 0;
        CharacterSetting.MagicAttack = Character.CheckAttackType(Character.AttackT, "Magic") ? Character.BasicAttack + level / 5f : 0;
        CharacterSetting.MagicDefense = Character.CheckAttackType(Character.DefenseT, "Magic") ? Character.BasicDefense + level / 5f : 0;
        CharacterSetting.PoisonAttack = Character.CheckAttackType(Character.AttackT, "Poison") ? Character.BasicAttack + level / 5f : 0;
        CharacterSetting.PoisonDefense = Character.CheckAttackType(Character.DefenseT, "Poison") ? Character.BasicDefense + level / 5f : 0;
        //#Element
        CharacterSetting.Element = Character.Elements.None;
        //#Carry
        var newCarryCnt = (int)Character.Carry + level / 10;
        //considering Slot sells which was pre acquisition  
        if (newCarryCnt > CharacterSetting.CarryCnt)
            CharacterSetting.CarryCnt = newCarryCnt;
        //Weight to carry
        CharacterSetting.Carry = CharacterSetting.CarryCnt * (int)Character.Carry;
        //#Equipment setting
        //Reset all to get value from equipments  
        CharacterSetting.Agility = CharacterSetting.Bravery =
                CharacterSetting.Charming = CharacterSetting.Intellect =
                    CharacterSetting.Crafting = CharacterSetting.Researching =
                        CharacterSetting.Stamina = CharacterSetting.Strength = 0;
        //todo: Get values from research too
        if (CharacterSetting.Equipments != null)
            foreach (var item in CharacterSetting.Equipments)
            {
                if (item.Id == -1)
                    continue;
                CharacterSettingUseItem(item, false);
            }
        CharacterSetting.Updated = true;
        SaveCharacterSetting();
    }

    internal float GetCharacterAttribute(string field)
    {
        //Return the value saved in CharacterSetting in 1*1000
        switch (field)
        {
            case "Crafting":
                return (_characterManager.CharacterSetting.Crafting % 1000) / 1000;
            case "Researching":
                return (_characterManager.CharacterSetting.Researching % 1000) / 1000;
            default:
                return 0;
        }
    }

    private void GameOverCalculations()
    {
        StartCoroutine(GameOverFadeInOut());
        //Todo: Drop an item on other players screen 
        CharacterSetting.Alive = false;
        //Goto Game over scene in 5 seconds 
        Invoke("OpenGameOver", 5.0f);
    }
    private int CalculateXp(int level)
    {
        if (level == 0)
            return 500;
        if (level == 1)
            return 1000;
        return CalculateXp(level - 1) + CalculateXp(level - 2);
    }
    //Start Ups
    internal void ReviveCharacter()
    {
        CharacterSetting.Alive = true;
        CharacterSetting.Life = 0;
        CharacterSetting.Health = CharacterSetting.MaxHealth;
        CharacterSetting.Updated = true;
        SetLockTill(DateTime.Now.AddMinutes(Mathf.Pow(CharacterSetting.Level, 3) + 3) );
        SaveCharacterSetting();
    }
    private bool SettingBasicChecks()
    {
        if (CharacterSetting.Id == -1)
            return true;
        if (CharacterSetting.Health < 0 && CharacterSetting.Alive)
            AddCharacterSetting("Alive", 0);
        if (!CharacterSetting.Alive)
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.buildIndex == SceneSettings.SceneIdForStore ||
                scene.buildIndex == SceneSettings.SceneIdForGameOver)
                return true;
            OpenGameOver();
            return true;
        }
        return false;
    }
    IEnumerator LevelUpFadeInOut()
    {
        _levelUp.SetActive(true);
        yield return new WaitForSeconds(1);
        _levelUp.SetActive(false);
    }
    IEnumerator GameOverFadeInOut()
    {
        _gameOver.SetActive(true);
        yield return new WaitForSeconds(5);
        _gameOver.SetActive(false);
    }
    internal Sprite GetSpellSprite()
    {
        if (_spellSprite == null)
            _spellSprite = CharacterSetting.GetSpellSprite();
        return _spellSprite;
    }
    //Scene manager
    public void OpenGameOver()
    {
        SceneManager.LoadScene(SceneSettings.SceneIdForGameOver);
    }
    private void GoToStartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.buildIndex == SceneSettings.SceneIdForStart)
            return;
        SceneManager.LoadScene(SceneSettings.SceneIdForStart);
    }
    private void GoToWaitScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.buildIndex == SceneSettings.SceneIdForWait)
            return;
        if (scene.buildIndex == SceneSettings.SceneIdForStore)
            return;
        SceneManager.LoadScene(SceneSettings.SceneIdForWait);
    }
    //Middle man to CharacterDatabase
    internal Research FindResearch(int id)
    {
        for (int i = 0; i < Researches.Count; i++)
        {
            if (Researches[i].Id == id)
                return Researches[i];
        }
        return null;
    }

    internal void SetLockTill(DateTime time )
    {
        UserPlayer.LockUntil = time;
        SaveUserPlayer();
    }
    internal void SetFacebookLoggedIn(bool Status,string id=null)
    {
        UserPlayer.FBLoggedIn = Status;
        if (id!=null)
            UserPlayer.FBid = id;
        SaveUserPlayer();
    }
    public void SaveCharacterMixture(ItemContainer item, DateTime time)
    {
        _characterDatabase.SaveCharacterMixture(item, time);
    }
    internal void SetCharacterMixtureTime(DateTime time)
    {
        CharacterMixture.Time = time;
        _characterDatabase.SaveCharacterMixture(CharacterMixture.Item, time);
    }
    public void SaveCharacterSetting()
    {
        _characterDatabase.SaveCharacterSetting(CharacterSetting);
    }
    internal void InitInventory()
    {
        _characterDatabase.InitInventory(CharacterSetting);
    }
    public void SaveUserPlayer()
    {
        _characterDatabase.SaveUserPlayer(UserPlayer);
    }
    public void SaveCharacterEquipments(List<ItemContainer> equipments)
    {
        CharacterSetting.Equipments = equipments.ToList();
        //Todo: or this one = > _characterSetting.Equipments = new List<Int32>(equipments);
        CharacterSetting.Updated = true;
        SaveCharacterSetting();
    }
    public void SaveCharacterResearching(CharacterResearch charResearch, DateTime durationMinutes)
    {
        _characterDatabase.SaveCharacterResearching(charResearch, durationMinutes);
    }
    internal void SetCharacterResearchingTime(DateTime time)
    {
        CharacterResearching.Time = time;
        _characterDatabase.SaveCharacterResearching(CharacterResearching.CharResearch, time);
    }
    //Instance
    public static CharacterManager Instance()
    {
        if (!_characterManager)
        {
            _characterManager = FindObjectOfType(typeof(CharacterManager)) as CharacterManager;
            if (!_characterManager)
                Debug.LogError("There needs to be one active ItemDatabase script on a GameObject in your scene.");
        }
        return _characterManager;
    }
}
