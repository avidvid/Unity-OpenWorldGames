using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterManager : MonoBehaviour
{
    //Database
    private static CharacterManager _characterManager;
    private ItemDatabase _itemDatabase;
    private UserDatabase _userDatabase;
    private CharacterDatabase _characterDatabase;
    //UserPlayer
    internal UserPlayer UserPlayer;
    //CharacterSetting
    internal CharacterSetting CharacterSetting;
    //Character
    internal Character MyCharacter;
    internal List<Character> UserCharacters ;
    //Research
    internal List<Research> Researches;
    internal List<CharacterResearch> CharacterResearches = new List<CharacterResearch>();
    internal CharacterResearching CharacterResearching;
    //Recipes
    internal List<Recipe> UserRecipes;
    //Inventory
    internal List<ItemIns> CharacterInventory = new List<ItemIns>();
    internal CharacterMixture CharacterMixture;

    private float _nextActionTime = 100;
    private Sprite _spellSprite;
    private GameObject _levelUp;
    private GameObject _gameOver;

    #region CharacterManager Instance
    public static CharacterManager Instance()
    {
        if (!_characterManager)
        {
            _characterManager = FindObjectOfType(typeof(CharacterManager)) as CharacterManager;
            if (!_characterManager)
                Debug.LogError("There needs to be one active ItemDatabase script on a GameObject in your scene.");
        }
        //CheckDBAvailability
        var database = GameObject.FindGameObjectWithTag("Database");
        if (database == null)
        {
            database = Resources.Load<GameObject>("Prefabs/Database");
            Instantiate(database);
        }
        return _characterManager;
    }
    #endregion

    void Awake()
    {
        _characterManager = Instance();
        _characterDatabase = CharacterDatabase.Instance();
        _itemDatabase = ItemDatabase.Instance();
        _userDatabase = UserDatabase.Instance();
        _levelUp = GameObject.Find("LevelUp");
        _gameOver = GameObject.Find("GameOver");
    }
    void Start()
    {
        //UserPlayer
        UserPlayer = _userDatabase.GetUserPlayer();
        Debug.Log("CM-UserPlayer = " + UserPlayer.MyInfo());
        //CharacterSetting
        CharacterSetting = _userDatabase.GetCharacterSetting(UserPlayer.Id);
        Debug.Log("CM-CharacterSetting = " + CharacterSetting.MyInfo());
        //Character
        MyCharacter = _characterDatabase.GetCharacterById(CharacterSetting.CharacterId);
        Debug.Log("CM-MyCharacter = " + MyCharacter.MyInfo());
        UserCharacters = _userDatabase.GetUserCharacters();
        Debug.Log("CM-UserCharacters.Count = " + UserCharacters.Count);
        //Research
        Researches = _characterDatabase.GetResearches();
        CharacterResearches = _userDatabase.GetCharacterResearches();
        CharacterResearching = _userDatabase.GetCharacterResearching();
        //Recipes
        UserRecipes = _userDatabase.GetMyRecipes();
        Debug.Log("CM-UserRecipes.Count = " + UserRecipes.Count);
        //Inventory
        InitUserInventory();
        Debug.Log("CharacterInventory.Count = " + CharacterInventory.Count);
        CharacterMixture = _userDatabase.GetCharacterMixture();
        LoginCalculations();
    }


    internal bool ValidateRecipeCode(string recipeCode)
    {
        return _userDatabase.ValidateRecipeCode(recipeCode);
    }

    void Update()
    {
        //Refresh User stats Health Mana Energy Level
        if (Time.time > _nextActionTime)
        {
            if (CharacterSettingHealthCheck())
                throw new Exception("CM-Character Setting Health Check Failed!!!");
            float period = 100 - CharacterSetting.Level;
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
    private bool CharacterSettingHealthCheck()
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
    public Recipe FindUserRecipes(int first, int second)
    {
        foreach (var r in UserRecipes)
        {
            if (first == r.FirstItemId && second == r.SecondItemId)
                return r;
            if (first == r.SecondItemId && second == r.FirstItemId)
                return r.Reverse();
        }
        return null;
    }
    #region MonsterIns
    internal MonsterIns GenerateMonster(Character monsterCharacter, int level)
    {
        return new MonsterIns(monsterCharacter, level);
    }
    #endregion
    internal Vector2 GetMapLocation(int key)
    {
        //Latitudes range from 0 to 90. Longitudes range from 0 to 180
        var terrainDatabase = TerrainDatabase.Instance();
        Vector2 regionLocation = terrainDatabase.GetRegionLocation(key);
        int x =(int) (UserPlayer.Latitude * 1000 - regionLocation.x) ;
        int y =(int) (UserPlayer.Longitude * 1000 - regionLocation.y) ;
        var mapLocation = new Vector2(x,y);
        return mapLocation;
    }
    #region Inventory
    private void InitUserInventory()
    {
        List<UserItem> userInventory = _userDatabase.GetUserInventory();
        foreach (var userItem in userInventory)
        {
            var item = _itemDatabase.GetItemById(userItem.ItemId);
            CharacterInventory.Add(new ItemIns(item, userItem));
        }
    }
    public ItemIns ItemInInventory(int itemId)
    {
        foreach (var itemIns in CharacterInventory)
            if (itemIns.Item.Id == itemId)
                return itemIns;
        return null;
    }
    public bool ItemIsInInventory(int itemId)
    {
        var item = ItemInInventory(itemId);
        return item!=null ;
    }
    internal bool AddItemToInventory(OupItem item,int stcCnt=1)
    {
        //Todo: make sure about the carry count of user 
        //Should match with the AddItemToInventory in InventoryHandler
        //CheckUniqueness
        var existingItem = ItemInInventory(item.Id);
        if (existingItem!=null)
        {
            if (item.MaxStackCnt == 1)
                return false;
            if (existingItem.UserItem.StackCnt + stcCnt <= item.MaxStackCnt)
            {
                existingItem.UserItem.StackCnt++;
                _userDatabase.UpdateUserInventory(CharacterInventory);
                return true;
            }
        }
        var userItem = new UserItem(item);
        var newItem = new ItemIns(item, userItem);
        newItem.UserItem.StackCnt = stcCnt;
        newItem.Print();
        CharacterInventory.Add(newItem);
        _userDatabase.UpdateUserInventory(CharacterInventory);
        return true;
    }
    #endregion
    #region ItemUse
    internal void CharacterSettingUnuseItem(ItemIns itemIns, bool save)
    {
        if (itemIns == null)
            return;
        var item = itemIns.Item;
        switch (itemIns.Item.Type)
        {
            case OupItem.ItemType.Consumable:
                return;
            case OupItem.ItemType.Equipment:
                CharacterSetting.Agility -= item.Agility;
                CharacterSetting.Bravery -= item.Bravery;
                CharacterSetting.Carry -= item.Carry;
                CharacterSetting.CarryCnt -= item.CarryCnt;
                CharacterSetting.Charming -= item.Charming;
                CharacterSetting.Intellect -= item.Intellect;
                CharacterSetting.Crafting -= item.Crafting;
                CharacterSetting.Researching -= item.Researching;
                CharacterSetting.Speed -= item.Speed;
                CharacterSetting.Stamina -= item.Stamina;
                CharacterSetting.Strength -= item.Strength;
                break;
            case OupItem.ItemType.Weapon:
                CharacterSetting.SpeedAttack -= item.SpeedAttack;
                CharacterSetting.SpeedDefense -= item.SpeedDefense;
                CharacterSetting.AbilityAttack -= item.AbilityAttack;
                CharacterSetting.AbilityDefense -= item.AbilityDefense;
                CharacterSetting.MagicAttack -= item.MagicAttack;
                CharacterSetting.MagicDefense -= item.MagicDefense;
                CharacterSetting.PoisonAttack -= item.PoisonAttack;
                CharacterSetting.PoisonDefense -= item.PoisonDefense;
                break;
            case OupItem.ItemType.Tool:
                return;
        }
        CharacterSetting.Updated = true;
        if (save)
            SaveCharacterSetting();
    }
    public string CharacterSettingUseItem(ItemIns itemIns, bool save)
    {
        string message = "";
        if (itemIns == null)
            return message;
        Debug.Log("ItemIns = " + itemIns.MyInfo());  
        var item = itemIns.Item;
        var userItem = itemIns.UserItem;
        switch (itemIns.Item.Type)
        {
            case OupItem.ItemType.Consumable:
                if (item.Recipe > 0)
                {
                    if (_userDatabase.AddNewRandomUserRecipe(UserPlayer.Id))
                        message = "You found a Recipe!!";
                    else return "Recipe you found is not readable!! ";
                }
                if (itemIns.Item.Egg > 0)
                {
                    if (_userDatabase.AddNewRandomUserCharacters())
                        message = "You Hatched a New Character!!";
                    else return "The Egg you found is already rotten !! ";
                }
                CharacterSetting.Health += item.Health * userItem.StackCnt;
                CharacterSetting.Mana += item.Mana * userItem.StackCnt;
                CharacterSetting.Energy += item.Energy * userItem.StackCnt;
                CharacterSetting.Coin += item.Coin * userItem.StackCnt;
                UserPlayer.Gem += item.Gem * userItem.StackCnt;
                if (CharacterSetting.Health > CharacterSetting.MaxHealth)
                    CharacterSetting.Health = CharacterSetting.MaxHealth;
                if (CharacterSetting.Mana > CharacterSetting.MaxMana)
                    CharacterSetting.Mana = CharacterSetting.MaxMana;
                if (CharacterSetting.Energy > CharacterSetting.MaxEnergy)
                    CharacterSetting.Energy = CharacterSetting.MaxEnergy;
                if (save)
                    SaveUserPlayer();
                break;
            case OupItem.ItemType.Equipment:
                CharacterSetting.Agility += item.Agility;
                CharacterSetting.Bravery += item.Bravery;
                CharacterSetting.Carry += item.Carry;
                CharacterSetting.CarryCnt += item.CarryCnt;
                CharacterSetting.Charming += item.Charming;
                CharacterSetting.Intellect += item.Intellect;
                CharacterSetting.Crafting += item.Crafting;
                CharacterSetting.Researching += item.Researching;
                CharacterSetting.Speed += item.Speed;
                CharacterSetting.Stamina += item.Stamina;
                CharacterSetting.Strength += item.Strength;
                break;
            case OupItem.ItemType.Weapon:
                CharacterSetting.SpeedAttack += item.SpeedAttack;
                CharacterSetting.SpeedDefense += item.SpeedDefense;
                CharacterSetting.AbilityAttack += item.AbilityAttack;
                CharacterSetting.AbilityDefense += item.AbilityDefense;
                CharacterSetting.MagicAttack += item.MagicAttack;
                CharacterSetting.MagicDefense += item.MagicDefense;
                CharacterSetting.PoisonAttack += item.PoisonAttack;
                CharacterSetting.PoisonDefense += item.PoisonDefense;
                break;
            case OupItem.ItemType.Tool:
                return message;
        }
        CharacterSetting.Updated = true;
        if (save)
            SaveCharacterSetting();
        return message;
    }
    #endregion
    //############################# OLD CODE
    internal void CharacterSettingApplyResearch(Research research, int level)
    {
        if (research == null)
            return;
        float value = research.CalculateValue(level);
        _userDatabase.AddCharacterResearch(research, level);
        _userDatabase.EmptyCharacterResearching();
        AddCharacterSetting(research.Target, value);
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
        print("CharacterSetting." + field + "=" + CharacterSetting.FieldValue(field) + " + " + value);
        switch (field)
        {
            case "MaxHealth":
                CharacterSetting.MaxHealth += (int)value;
                CharacterSetting.Updated = true;
                break;
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
                UserPlayer.Gem += (int)value;
                UserPlayer.HealthCheck += UserPlayer.CalculateHealthCheck((int)value, "Gem");
                CharacterSetting.Updated = true;
                SaveUserPlayer();
                return;
            case "Coin":
                CharacterSetting.Coin += (int)value;
                CharacterSetting.HealthCheck += CharacterSetting.CalculateHealthCheckByField((int)value, "Coin");
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
        if (MyCharacter.Id == -1)
            MyCharacter = _characterDatabase.GetCharacterById(CharacterSetting.CharacterId);
        //Logic Should be align with MonsterIns: MonsterIns()
        var level = CharacterSetting.Level;
        CharacterSetting.MaxExperience = CalculateXp(level);
        //#Health/Mana/Energy
        float lastPercentage = (float)CharacterSetting.Health / CharacterSetting.MaxHealth;
        CharacterSetting.MaxHealth = ((int)MyCharacter.Body + level) * 100;
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
        CharacterSetting.Speed = (int)MyCharacter.Speed + level / 10;
        CharacterSetting.SpeedAttack = CharacterSetting.Speed;
        CharacterSetting.SpeedDefense = CharacterSetting.Speed;
        //#Attack/Defense
        CharacterSetting.AbilityAttack = MyCharacter.CheckAttackType(MyCharacter.AttackT, "Strength") ? MyCharacter.BasicAttack + level / 5f : 0;
        CharacterSetting.AbilityDefense = MyCharacter.CheckAttackType(MyCharacter.DefenseT, "Strength") ? MyCharacter.BasicDefense + level / 5f : 0;
        CharacterSetting.MagicAttack = MyCharacter.CheckAttackType(MyCharacter.AttackT, "Magic") ? MyCharacter.BasicAttack + level / 5f : 0;
        CharacterSetting.MagicDefense = MyCharacter.CheckAttackType(MyCharacter.DefenseT, "Magic") ? MyCharacter.BasicDefense + level / 5f : 0;
        CharacterSetting.PoisonAttack = MyCharacter.CheckAttackType(MyCharacter.AttackT, "Poison") ? MyCharacter.BasicAttack + level / 5f : 0;
        CharacterSetting.PoisonDefense = MyCharacter.CheckAttackType(MyCharacter.DefenseT, "Poison") ? MyCharacter.BasicDefense + level / 5f : 0;
        //#Element
        CharacterSetting.Element = Character.Elements.None;
        //#Carry
        var newCarryCnt = (int)MyCharacter.Carry + level / 10;
        //considering Slot sells which was pre acquisition  
        if (newCarryCnt > CharacterSetting.CarryCnt)
            CharacterSetting.CarryCnt = newCarryCnt;
        //Weight to carry
        CharacterSetting.Carry = CharacterSetting.CarryCnt * (int)MyCharacter.Carry;
        //#Equipment setting
        //Reset all to get value from equipments  
        CharacterSetting.Agility = CharacterSetting.Bravery =
                CharacterSetting.Charming = CharacterSetting.Intellect =
                    CharacterSetting.Crafting = CharacterSetting.Researching =
                        CharacterSetting.Stamina = CharacterSetting.Strength = 0;
        //todo: Get values from research too
        foreach (var itemIns in CharacterInventory)
        {
            if (itemIns.UserItem.Equipped)
                CharacterSettingUseItem(itemIns, false);
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

    //Middle man to CharacterDatabase
    internal bool ValidateCharacterCode(string characterCode)
    {
        return _userDatabase.ValidateCharacterCode(characterCode);
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
    public void SaveCharacterMixture(int itemId, int stackCnt, DateTime time)
    {
        _userDatabase.SaveCharacterMixture(itemId, stackCnt, time);
    }
    internal void SetCharacterMixtureTime(DateTime time)
    {
        CharacterMixture.MixTime = time;
        _userDatabase.SaveCharacterMixture(CharacterMixture.ItemId, CharacterMixture.StackCnt, time);
    }
    public void SaveCharacterSetting()
    {
        _userDatabase.SaveCharacterSetting(CharacterSetting);
    }
    public void SaveUserPlayer()
    {
        _userDatabase.SaveUserPlayer(UserPlayer);
    }
    public void SaveCharacterResearching(int researchId, int level, DateTime durationMinutes)
    {
        _userDatabase.SaveCharacterResearching( researchId,  level , durationMinutes);
    }
    internal void SetCharacterResearchingTime(DateTime time)
    {
        CharacterResearching.ResearchTime = time;
        _userDatabase.SaveCharacterResearching(CharacterResearching);
    }

}
