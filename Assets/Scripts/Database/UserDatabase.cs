using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class UserDatabase : MonoBehaviour
{

    private static UserDatabase _userDatabase;
    private CharacterDatabase _characterDatabase;
    private ItemDatabase _itemDatabase;

    private UserPlayer _userPlayer;
    private List<UserItem> _userInventory = new List<UserItem>();
    private List<Character> _characters = new List<Character>();
    private List<UserCharacter> _userCharacters = new List<UserCharacter>();
    private CharacterMixture _characterMixture;
    private List<CharacterResearch> _characterResearches = new List<CharacterResearch>();
    private CharacterResearching _characterResearching;
    private CharacterSetting _characterSetting;
    private List<Recipe> _recipes = new List<Recipe>();
    private List<UserRecipe> _userRecipes = new List<UserRecipe>();

    #region UserDatabase Instance
    public static UserDatabase Instance()
    {
        if (!_userDatabase)
        {
            _userDatabase = FindObjectOfType(typeof(UserDatabase)) as UserDatabase;
            if (!_userDatabase)
                Debug.LogError("There needs to be one active ItemDatabase script on a GameObject in your scene.");
        }
        return _userDatabase;
    }
    #endregion

    void Awake()
    {
        _userDatabase = Instance();
        _characterDatabase=CharacterDatabase.Instance();
        _itemDatabase = ItemDatabase.Instance();
    }

    void Start()
    {
        //UserPlayer
        _userPlayer = LoadUserPlayer();
        if (_userPlayer == null)
            throw new Exception("User Player doesn't Exists!!!");

        _characterSetting = LoadCharacterSetting();
        if (_characterSetting != null)
        {        
            //UserInventory
            _userInventory = LoadUserInventory();
            Debug.Log("UserInventory.Count = " + _userInventory.Count);
            //UserCharacters
            _characters = _characterDatabase.GetCharacters();
            _userCharacters = _userCharacters = LoadUserCharacters();
            Debug.Log("UserInventory.Count = " + _userInventory.Count);
            //CharacterMixture
            LoadCharacterMixture();
            //CharacterResearch
            LoadCharacterResearches();
            LoadCharacterResearching();
            //UserRecipe
            _recipes = _itemDatabase.GetRecipes();
            LoadUserRecipes();
            Debug.Log("UserRecipes.Count = " + _userRecipes.Count);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region UserPlayer
    public UserPlayer FindUserPlayer(int id)
    {
        //todo: update on login : long lat last login 
        return _userPlayer;
    }
    public void SaveUserPlayer(UserPlayer userPlayer)
    {
        _userPlayer = new UserPlayer(userPlayer);
        SaveUserPlayer();
    }
    private UserPlayer LoadUserPlayer()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserPlayer.xml");
        //Read the UserPlayer from UserPlayer.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(UserPlayer));
        FileStream fs = new FileStream(path, FileMode.Open);
        var userPlayer = (UserPlayer)serializer.Deserialize(fs);
        fs.Close();
        return userPlayer;
    }
    public void SaveUserPlayer()
    {
        HealthCheckUserPlayer();
        string path = Path.Combine(Application.streamingAssetsPath, "UserPlayer.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(UserPlayer));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _userPlayer);
        fs.Close();
    }
    private void HealthCheckUserPlayer()
    {
        var oldUserPlayer = LoadUserPlayer();
        int healthCheck = 0;
        healthCheck +=
            oldUserPlayer.CalculateHealthCheck(oldUserPlayer.Gem - _userPlayer.Gem, "Gem");
        if (oldUserPlayer.HealthCheck + healthCheck != _userPlayer.HealthCheck)
            throw new Exception("Health Check User Player Failed. It is off by " + (healthCheck - _userPlayer.HealthCheck));
    }
    #endregion
    #region UserInventory
    private List<UserItem> LoadUserInventory()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserInventory.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserItem>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var userRecipes = (List<UserItem>)serializer.Deserialize(fs);
        fs.Close();
        return userRecipes;
    }
    public void SaveUserInventory()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserInventory.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserItem>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _userInventory);
        fs.Close();
    }
    internal void UpdateUserInventory(List<ItemIns> characterInventory)
    {
        _userInventory.Clear();
        foreach (var userItem in characterInventory)
        {
            _userInventory.Add(userItem.UserItem);
        }
        SaveUserInventory();
    }
    internal List<UserItem> GetUserInventory()
    {
        return _userInventory;
    }
    #endregion
    #region UserCharacters
    public List<Character> GetUserCharacters()
    {
        List<Character> characters = new List<Character>();
        for (int i = 0; i < _characters.Count; i++)
        {
            if (!_characters[i].IsEnable)
                continue;
            for (int j = 0; j < _userCharacters.Count; j++)
                if (_characters[i].Id == _userCharacters[j].CharacterId)
                {
                    if (string.IsNullOrEmpty(_userCharacters[j].CharacterCode))
                        characters.Add(_characters[i]);
                    else
                        //if not purchased yet :This might cause problem later because all of the other items are by reference and this  one is by value 
                        characters.Add(new Character(_characters[i], false));
                    break;
                }
        }
        return characters;
    }
    internal bool ValidateCharacterCode(string characterCode)
    {
        for (int j = 0; j < (_userCharacters).Count; j++)
            if (_userCharacters[j].CharacterCode != null)
                if (_userCharacters[j].CharacterCode == characterCode)
                {
                    _userCharacters[j].CharacterCode = "";
                    SaveUserCharacters();
                    return true;
                }
        return false;
    }
    private List<UserCharacter> LoadUserCharacters()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserCharacter.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserCharacter>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var userCharacters = (List<UserCharacter>)serializer.Deserialize(fs);
        fs.Close();
        return userCharacters;
    }
    private void SaveUserCharacters()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserCharacter.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserCharacter>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _userCharacters);
        fs.Close();
    }
    public bool AddUserCharacters(UserCharacter uc)
    {
        try
        {
            var owned = _userCharacters.Find(c => c.CharacterId == uc.CharacterId && c.UserId == uc.UserId && !string.IsNullOrEmpty(c.CharacterCode));
            if (owned == null)
                _userCharacters.Add(uc);
            else
                owned.CharacterCode = "";
            SaveUserCharacters();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }
    internal bool AddNewRandomUserCharacters(int playerId)
    {
        List<int> availableCharacters = new List<int>();
        int key = DateTime.Now.DayOfYear;
        var rarity = RandomHelper.Range(key, (int)Item.ItemRarity.Common);
        bool userOwnedRecipe = false;
        for (int i = 0; i < _characters.Count; i++)
            if (_characters[i].IsEnable)
            {
                if ((int)_characters[i].Rarity < rarity)
                    continue;
                for (int j = 0; j < _userCharacters.Count; j++)
                    if (_characters[i].Id == _userCharacters[j].CharacterId && string.IsNullOrEmpty(_userCharacters[j].CharacterCode))
                    {
                        userOwnedRecipe = true;
                        break;
                    }
                if (!userOwnedRecipe)
                    availableCharacters.Add(_characters[i].Id);
                userOwnedRecipe = false;
            }
        if (availableCharacters.Count > 0)
        {
            UserCharacter uc = new UserCharacter(availableCharacters[RandomHelper.Range(key, availableCharacters.Count)], playerId);
            return AddUserCharacters(uc);
        }
        return false;
    }
    #endregion
    #region CharacterMixture
    internal CharacterMixture GetCharacterMixture(int id)
    {
        return _characterMixture;
    }
    public void SaveCharacterMixture(int itemId, int stackCnt, DateTime durationMinutes)
    {
        _characterMixture = new CharacterMixture(_userPlayer.Id, itemId, stackCnt, durationMinutes);
        SaveCharacterMixture();
    }
    private void LoadCharacterMixture()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterMixture.xml");
        //Read the CharacterMixture from CharacterMixture.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterMixture));
        FileStream fs = new FileStream(path, FileMode.Open);
        _characterMixture = (CharacterMixture)serializer.Deserialize(fs);
        fs.Close();
    }
    public void SaveCharacterMixture()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterMixture.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterMixture));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characterMixture);
        fs.Close();
    }
    #endregion
    #region CharacterResearch
    internal CharacterResearching FindCharacterResearching(int playerId)
    {
        return _characterResearching;
    }
    public void SaveCharacterResearching(CharacterResearch charResearch, DateTime durationMinutes)
    {
        _characterResearching = new CharacterResearching(charResearch, durationMinutes);
        SaveCharacterResearching();
    }
    public void EmptyCharacterResearching()
    {
        _characterResearching = new CharacterResearching();
        SaveCharacterResearching();
    }
    public void SaveCharacterResearching()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearching.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterResearching));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characterResearching);
        fs.Close();
    }
    private void LoadCharacterResearching()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearching.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterResearching));
        FileStream fs = new FileStream(path, FileMode.Open);
        _characterResearching = (CharacterResearching)serializer.Deserialize(fs);
        fs.Close();
    }
    internal List<CharacterResearch> LoadCharacterResearches(int playerId)
    {
        return _characterResearches;
    }
    private void LoadCharacterResearches()
    {
        //Empty the Items DB
        _characterResearches.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearch.xml");
        //Read the CharacterResearch from CharacterResearch.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<CharacterResearch>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _characterResearches = (List<CharacterResearch>)serializer.Deserialize(fs);
        fs.Close();
    }
    public void SaveCharacterResearches()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearch.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<CharacterResearch>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characterResearches);
        fs.Close();
    }
    internal void AddCharacterResearch(CharacterResearch charResearch)
    {
        bool updated = false;
        foreach (var chResearch in _characterResearches)
            if (chResearch.ResearchId == charResearch.ResearchId)
            {
                if (charResearch.Level <= chResearch.Level)
                    throw new Exception("CharacterResearch Invalid <= ");
                if (chResearch.Level + 1 != charResearch.Level)
                    throw new Exception("CharacterResearch Invalid != ");
                chResearch.Level = charResearch.Level;
                updated = true;
                break;
            }
        if (!updated)
            _characterResearches.Add(charResearch);
        SaveCharacterResearches();
    }
    #endregion
    #region CharacterSetting
    public CharacterSetting GetCharacterSetting(int userPlayerId)
    {
        return _characterSetting;
    }
    public void SaveCharacterSetting(CharacterSetting characterSetting)
    {
        _characterSetting = new CharacterSetting(characterSetting);
        SaveCharacterSetting();
    }
    private CharacterSetting LoadCharacterSetting()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterSetting.xml");
        //Read the CharacterSetting from CharacterSetting.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterSetting));
        FileStream fs = new FileStream(path, FileMode.Open);
        var characterSetting = (CharacterSetting)serializer.Deserialize(fs);
        fs.Close();
        return characterSetting;
    }
    public void SaveCharacterSetting()
    {
        HealthCheckCharacterSetting();
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterSetting.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterSetting));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characterSetting);
        fs.Close();
    }
    private void HealthCheckCharacterSetting()
    {
        var oldCharacterSetting = LoadCharacterSetting();
        int healthCheck = 0;
        healthCheck +=
            oldCharacterSetting.CalculateHealthCheck(oldCharacterSetting.Coin - _characterSetting.Coin, "Coin");

        if (oldCharacterSetting.HealthCheck + healthCheck != _characterSetting.HealthCheck)
            throw new Exception("Health Check CharacterSetting Failed. It is off by " + (healthCheck - _characterSetting.HealthCheck));
    }
    #endregion
    #region UserRecipe
    public List<Recipe> UserRecipeList()
    {
        List<Recipe> recipes = new List<Recipe>();
        for (int i = 0; i < _recipes.Count; i++)
        {
            if (!_recipes[i].IsEnable)
                continue;
            if (_recipes[i].IsPublic)
            {
                recipes.Add(_recipes[i]);
                continue;
            }
            for (int j = 0; j < _userRecipes.Count; j++)
                if (_recipes[i].Id == _userRecipes[j].RecipeId)
                {
                    if (string.IsNullOrEmpty(_userRecipes[j].RecipeCode))
                        recipes.Add(_recipes[i]);
                    else
                    {
                        _recipes[i].IsEnable = false;
                        recipes.Add(_recipes[i]);
                    }
                    break;
                }
        }
        return recipes;
    }
    internal bool ValidateRecipeCode(string recipeCode)
    {
        for (int j = 0; j < _userRecipes.Count; j++)
            if (_userRecipes[j].RecipeCode != null)
                if (_userRecipes[j].RecipeCode == recipeCode)
                {
                    _userRecipes[j].RecipeCode = "";
                    SaveUserRecipes();
                    return true;
                }
        return false;
    }
    private void LoadUserRecipes()
    {
        //Empty the Recipes DB
        _userRecipes.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "UserRecipe.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserRecipe>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _userRecipes = (List<UserRecipe>)serializer.Deserialize(fs);
        fs.Close();
    }
    public bool AddUserRecipe(UserRecipe ur)
    {
        try
        {
            var owned = _userRecipes.Find(c => c.RecipeId == ur.RecipeId && c.UserId == ur.UserId && !string.IsNullOrEmpty(c.RecipeCode));
            if (owned == null)
                _userRecipes.Add(ur);
            else
                owned.RecipeCode = "";
            SaveUserRecipes();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }
    internal bool AddNewRandomUserRecipe(int playerId)
    {
        List<int> availableRecipe = new List<int>();
        int key = DateTime.Now.DayOfYear;
        var rarity = RandomHelper.Range(key, (int)Item.ItemRarity.Common);
        bool userOwnedRecipe = false;
        for (int i = 0; i < _recipes.Count; i++)
            if (_recipes[i].IsEnable && !_recipes[i].IsPublic)
            {
                if ((int)_recipes[i].Rarity < rarity)
                    continue;
                for (int j = 0; j < _userRecipes.Count; j++)
                    if (_recipes[i].Id == _userRecipes[j].RecipeId && string.IsNullOrEmpty(_userRecipes[j].RecipeCode))
                    {
                        userOwnedRecipe = true;
                        break;
                    }
                if (!userOwnedRecipe)
                    availableRecipe.Add(_recipes[i].Id);
                userOwnedRecipe = false;
            }
        if (availableRecipe.Count > 0)
        {
            UserRecipe ur = new UserRecipe(availableRecipe[RandomHelper.Range(key, availableRecipe.Count)], playerId);
            return AddUserRecipe(ur);
        }
        return false;
    }
    public Recipe FindUserRecipes(int first, int second)
    {
        for (int i = 0; i < _recipes.Count; i++)
        {
            if (!_recipes[i].IsEnable)
                continue;
            Recipe r = _recipes[i];
            if (_recipes[i].IsPublic)
            {
                if (r.IsEnable && first == r.FirstItemId && second == r.SecondItemId)
                    return r;
                if (r.IsEnable && first == r.SecondItemId && second == r.FirstItemId)
                    return r.Reverse();
                continue;
            }
            for (int j = 0; j < _userRecipes.Count; j++)
                if (_recipes[i].Id == _userRecipes[j].RecipeId)
                {
                    if (string.IsNullOrEmpty(_userRecipes[j].RecipeCode))
                    {
                        if (r.IsEnable && first == r.FirstItemId && second == r.SecondItemId)
                            return r;
                        if (r.IsEnable && first == r.SecondItemId && second == r.FirstItemId)
                            return r.Reverse();
                    }
                    break;
                }
        }
        return null;
    }
    private void SaveUserRecipes()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserRecipe.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserRecipe>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _userRecipes);
        fs.Close();
    }
    #endregion
}