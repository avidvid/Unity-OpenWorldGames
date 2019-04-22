using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserDatabase : MonoBehaviour
{

    private static UserDatabase _userDatabase;

    private UserPlayer _userPlayer;
    private List<UserItem> _userInventory = new List<UserItem>();
    private List<UserCharacter> _userCharacters = new List<UserCharacter>();
    private List<Character> _myCharacters = new List<Character>();
    private CharacterMixture _characterMixture;
    private List<UserResearch> _characterResearches = new List<UserResearch>();
    private CharacterResearching _characterResearching;
    private CharacterSetting _characterSetting;
    private List<Recipe> _myRecipes;
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
    void Start()
    {
        _userDatabase = Instance();
        Debug.Log("***UDB*** Start!");
        //UserPlayer
        _userPlayer = LoadUserPlayer();
        if (_userPlayer == null)
        {
            print("UserPlayer is empty");
            GoToStartScene();
            return;
            //throw new Exception("UDB-User Player doesn't Exists!!!");
        }
        _userPlayer.Print();
        _userPlayer.HealthCheck = _userPlayer.CalculateHealthCheck();
        _userPlayer.LastLogin = DateTime.Now;
        SaveUserPlayer();
        if (DateTime.Now < _userPlayer.LockUntil)
        {
            GoToWaitScene();
            return;
        }
        //CharacterSetting
        _characterSetting = LoadCharacterSetting();
        if (_characterSetting == null)
        {
            print("CharacterSetting is empty");
            GoToStartScene();
            return;
            //throw new Exception("UDB-Character Setting doesn't Exists!!!");
        }
        _characterSetting.HealthCheck = _characterSetting.CalculateHealthCheck();
        _characterSetting.Print();
        if (_characterSetting != null)
        {        
            //UserInventory
            _userInventory = LoadUserInventory();
            Debug.Log("UDB-UserInventory.Count = " + _userInventory.Count);
            //UserCharacters
            _userCharacters = LoadUserCharacters();
            Debug.Log("UDB-UserCharacters.Count = " + _userCharacters.Count);
            _myCharacters =  BuildUserCharacters();
            Debug.Log("UDB-BuiltUserCharacters.Count = " + _myCharacters.Count);
            //CharacterMixture
            _characterMixture = LoadCharacterMixture();
            Debug.Log("UDB-CharacterMixture = " +  (_characterMixture == null ? "Empty" : _characterMixture.MyInfo()) );
            //CharacterResearch
            _characterResearches=LoadCharacterResearches();
            Debug.Log("UDB-CharacterResearch.Count = " + _characterResearches.Count);
            _characterResearching = LoadCharacterResearching();
            Debug.Log("UDB-CharacterResearch = " + (_characterResearching == null ? "Empty" : _characterResearching.MyInfo()));
            //UserRecipe
            _userRecipes =LoadUserRecipes();
            Debug.Log("UDB-UserRecipes.Count = " + _userRecipes.Count);
        }
        Debug.Log("***UDB*** Success!");
    }
    // Update is called once per frame
    void Update()
    {
    }
    #region UserPlayer
    public UserPlayer GetUserPlayer()
    {
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
    public void UpdateUserPlayer(UserPlayer userPlayer)
    {
        _userPlayer = userPlayer;
        SaveUserPlayer();
    }
    private void HealthCheckUserPlayer()
    {
        var healthCheck = _userPlayer.CalculateHealthCheck();
        if (healthCheck != _userPlayer.HealthCheck)
            throw new Exception("Health Check User Player Failed. It is off by " + (healthCheck - _userPlayer.HealthCheck));
    }
    #endregion
    #region UserInventory
    private List<UserItem> LoadUserInventory()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserInventory.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserItem>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var userInv = (List<UserItem>)serializer.Deserialize(fs);
        fs.Close();
        return userInv;
    }
    public void SaveUserInventory()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserInventory.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserItem>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _userInventory);
        fs.Close();
    }
    public void UpdateUserInventory(List<UserItem> userInventory)
    {
        _userInventory = userInventory;
        SaveUserInventory();
    }
    internal void UpdateUserInventory(List<ItemIns> invCarry, List<ItemIns> invEquipment = null)
    {
        _userInventory.Clear();
        foreach (var userItem in invCarry)
            if (userItem.UserItem.StackCnt!=0)
                _userInventory.Add(userItem.UserItem);
        if (invEquipment!=null)
            foreach (var userItem in invEquipment)
                _userInventory.Add(userItem.UserItem);
        _userInventory = _userInventory.OrderBy(x => !x.Equipped).ThenBy(x => !x.Stored).ThenBy(x => x.Order).ToList();
        SaveUserInventory();
    }
    internal List<UserItem> GetUserInventory()
    {
        return _userInventory;
    }
    #endregion
    #region UserCharacters
    public List<Character> GetMyCharacters()
    {
        return _myCharacters;
    }
    public List<UserCharacter> GetUserCharacters()
    {
        return _userCharacters;
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
    public void UpdateUserCharacters(List<UserCharacter> userCharacters)
    {
        _userCharacters = userCharacters;
        SaveUserCharacters();
    }
    private List<Character> BuildUserCharacters()
    {
        var characterDatabase = CharacterDatabase.Instance();
        var characters = characterDatabase.GetCharacters();
        List<Character> myCharacters = new List<Character>();
        for (int i = 0; i < characters.Count; i++)
        {
            if (!characters[i].IsEnable)
                continue;
            for (int j = 0; j < _userCharacters.Count; j++)
                if (characters[i].Id == _userCharacters[j].CharacterId)
                {
                    if (string.IsNullOrEmpty(_userCharacters[j].CharacterCode))
                        myCharacters.Add(characters[i]);
                    else
                        //if not purchased yet :This might cause problem later because all of the other items are by reference and this  one is by value 
                        myCharacters.Add(new Character(characters[i], false));
                    break;
                }
        }
        return myCharacters;
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
    internal bool AddNewUserCharacters(int characterId=-1)
    {
        //add a New specific Character
        if (characterId != -1)
        {
            UserCharacter uc = new UserCharacter(characterId, _userPlayer.Id);
            return AddUserCharacters(uc);
        }
        //add a New Random Character
        var characterDatabase = CharacterDatabase.Instance();
        var characters = characterDatabase.GetCharacters();
        List<int> availableCharacters = new List<int>();
        int key = DateTime.Now.DayOfYear;
        var rarity = RandomHelper.Range(key, (int)DataTypes.Rarity.Common);
        bool userOwnedCharacter = false;
        for (int i = 0; i < characters.Count; i++)
            if (characters[i].IsEnable)
            {
                if ((int)characters[i].Rarity < rarity)
                    continue;
                for (int j = 0; j < _userCharacters.Count; j++)
                    if (characters[i].Id == _userCharacters[j].CharacterId && string.IsNullOrEmpty(_userCharacters[j].CharacterCode))
                    {
                        userOwnedCharacter = true;
                        break;
                    }
                if (!userOwnedCharacter)
                    availableCharacters.Add(characters[i].Id);
                userOwnedCharacter = false;
            }
        if (availableCharacters.Count > 0)
        {
            UserCharacter uc = new UserCharacter(availableCharacters[RandomHelper.Range(key, availableCharacters.Count)], _userPlayer.Id);
            return AddUserCharacters(uc);
        }
        return false;
    }
    #endregion
    #region CharacterMixture
    internal CharacterMixture GetCharacterMixture()
    {
        return _characterMixture;
    }
    public void SaveCharacterMixture(int itemId, int stackCnt, DateTime durationMinutes)
    {
        if (stackCnt == 0)
            _characterMixture = null;
        else
            _characterMixture = new CharacterMixture(_userPlayer.Id, itemId, stackCnt, durationMinutes);
        SaveCharacterMixture();
    }
    private CharacterMixture LoadCharacterMixture()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterMixture.xml");
        //Read the CharacterMixture from CharacterMixture.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterMixture));
        FileStream fs = new FileStream(path, FileMode.Open);
        var characterMixture = (CharacterMixture)serializer.Deserialize(fs);
        fs.Close();
        return characterMixture;
    }
    public void SaveCharacterMixture()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterMixture.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterMixture));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characterMixture);
        fs.Close();
    }
    public void UpdateCharacterMixture(CharacterMixture characterMixture)
    {
        _characterMixture = characterMixture;
        SaveCharacterMixture();
    }
    #endregion
    #region CharacterResearch
    internal CharacterResearching GetCharacterResearching()
    {
        return _characterResearching;
    }
    internal void SaveCharacterResearching(int researchId, int level, DateTime durationMinutes)
    {
        _characterResearching = new CharacterResearching(_userPlayer.Id,researchId, level, durationMinutes);
        SaveCharacterResearching();
    }
    internal void SaveCharacterResearching(CharacterResearching characterResearching)
    {
        SaveCharacterResearching( characterResearching.ResearchId,characterResearching.Level, characterResearching.ResearchTime);
    }
    public void EmptyCharacterResearching()
    {
        _characterResearching = null;
        SaveCharacterResearching();
    }
    private CharacterResearching LoadCharacterResearching()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearching.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterResearching));
        FileStream fs = new FileStream(path, FileMode.Open);
        var characterResearching = (CharacterResearching)serializer.Deserialize(fs);
        fs.Close();
        return characterResearching;
    }
    private void SaveCharacterResearching()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearching.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterResearching));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characterResearching);
        fs.Close();
    }
    public void UpdateCharacterResearching(CharacterResearching characterResearching)
    {
        _characterResearching = characterResearching;
        SaveCharacterResearching();
    }
    internal List<UserResearch> GetCharacterResearches()
    {
        return _characterResearches;
    }
    private List<UserResearch> LoadCharacterResearches()
    {
        //Empty the Items DB
        _characterResearches.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearch.xml");
        //Read the CharacterResearch from CharacterResearch.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserResearch>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var characterResearches = (List<UserResearch>)serializer.Deserialize(fs);
        fs.Close();
        return characterResearches;
    }
    public void SaveCharacterResearches()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearch.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserResearch>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characterResearches);
        fs.Close();
    }
    public void UpdateCharacterResearches(List<UserResearch> userResearches)
    {
        _characterResearches = userResearches;
        SaveCharacterResearches();
    }
    internal void AddCharacterResearch(Research research, int level)
    {
        bool updated = false;
        foreach (var chResearch in _characterResearches)
            if (chResearch.ResearchId == research.Id)
            {
                if (level <= chResearch.Level)
                    throw new Exception("CharacterResearch Invalid <= ");
                if (chResearch.Level + 1 != level)
                    throw new Exception("CharacterResearch Invalid != ");
                chResearch.Level = level;
                updated = true;
                break;
            }
        if (!updated)
            _characterResearches.Add(new UserResearch(research.Id,_userPlayer.Id ,level));
        SaveCharacterResearches();
    }
    #endregion
    #region CharacterSetting
    public CharacterSetting GetCharacterSetting()
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
    public void UpdateCharacterSetting(CharacterSetting characterSetting)
    {
        _characterSetting = characterSetting;
        SaveCharacterSetting();
    }
    private void HealthCheckCharacterSetting()
    {
        var healthCheck = _characterSetting.CalculateHealthCheck();
        if (healthCheck != _characterSetting.HealthCheck)
            throw new Exception("Health Check CharacterSetting Failed. It is off by " + (healthCheck - _characterSetting.HealthCheck));
    }
    #endregion
    #region UserRecipe
    public List<UserRecipe> GetUserRecipes()
    {
        return _userRecipes;
    }
    public void BuildUserRecipes()
    {
        var itemDatabase = ItemDatabase.Instance();
        var recipes = itemDatabase.GetRecipes();
        List<Recipe> myRecipes = new List<Recipe>();
        for (int i = 0; i < recipes.Count; i++)
        {
            if (!recipes[i].IsEnable)
                continue;
            if (recipes[i].IsPublic)
            {
                myRecipes.Add(recipes[i]);
                continue;
            }
            for (int j = 0; j < _userRecipes.Count; j++)
                if (recipes[i].Id == _userRecipes[j].RecipeId)
                {
                    if (string.IsNullOrEmpty(_userRecipes[j].RecipeCode))
                        myRecipes.Add(recipes[i]);
                    else
                    {
                        recipes[i].IsEnable = false;
                        myRecipes.Add(recipes[i]);
                    }
                    break;
                }
        }
        _myRecipes = myRecipes;
        Debug.Log("UDB-BuiltUserRecipes.Count = " + _myRecipes.Count);
    }
    public List<Recipe> GetMyRecipes()
    {
        return _myRecipes;
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
        var itemDatabase = ItemDatabase.Instance();
        var recipes = itemDatabase.GetRecipes();
        List<int> availableRecipe = new List<int>();
        int key = DateTime.Now.DayOfYear;
        var rarity = RandomHelper.Range(key, (int)DataTypes.Rarity.Common);
        bool userOwnedRecipe = false;
        for (int i = 0; i < recipes.Count; i++)
            if (recipes[i].IsEnable && !recipes[i].IsPublic)
            {
                if ((int)recipes[i].Rarity < rarity)
                    continue;
                for (int j = 0; j < _userRecipes.Count; j++)
                    if (recipes[i].Id == _userRecipes[j].RecipeId && string.IsNullOrEmpty(_userRecipes[j].RecipeCode))
                    {
                        userOwnedRecipe = true;
                        break;
                    }
                if (!userOwnedRecipe)
                    availableRecipe.Add(recipes[i].Id);
                userOwnedRecipe = false;
            }
        if (availableRecipe.Count > 0)
        {
            UserRecipe ur = new UserRecipe(availableRecipe[RandomHelper.Range(key, availableRecipe.Count)], playerId);
            return AddUserRecipe(ur);
        }
        return false;
    }
    private List<UserRecipe> LoadUserRecipes()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserRecipe.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserRecipe>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var userRecipes = (List<UserRecipe>)serializer.Deserialize(fs);
        fs.Close();
        return userRecipes;
    }
    public void SaveUserRecipes()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserRecipe.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserRecipe>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _userRecipes);
        fs.Close();
    }
    public void UpdateUserRecipes(List<UserRecipe> userRecipes)
    {
        _userRecipes = userRecipes;
        SaveUserRecipes();
    }
    internal bool ValidateRecipeCode(string recipeCode)
    {
        for (int j = 0; j < _userRecipes.Count; j++)
            if (_userRecipes[j].RecipeCode != null)
                if (_userRecipes[j].RecipeCode == recipeCode)
                {
                    _userRecipes[j].RecipeCode = "";
                    _userDatabase.SaveUserRecipes();
                    return true;
                }
        return false;
    }
    #endregion
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
}