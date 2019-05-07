﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserDatabase : MonoBehaviour
{
    private static UserDatabase _userDatabase;
    private ApiGatewayConfig _apiGatewayConfig;
    private TerrainDatabase _terrainDatabase;

    private UserPlayer _userPlayer;
    private List<UserItem> _userInventory = new List<UserItem>();
    private List<UserCharacter> _userCharacters = new List<UserCharacter>();
    private List<Character> _myCharacters = new List<Character>();
    private CharacterMixture _characterMixture;
    private List<CharacterResearch> _characterResearches = new List<CharacterResearch>();
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
        _terrainDatabase = TerrainDatabase.Instance();
        _apiGatewayConfig =ApiGatewayConfig.Instance();
        Debug.Log("***UDB*** Start!");
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
    public void SaveUserPlayer(UserPlayer userPlayer, int days)
    {
        _apiGatewayConfig.SaveUserPlayer(userPlayer, days);
    }
    public bool UpdateUserPlayer(UserPlayer userPlayer)
    {
        _userPlayer = userPlayer;
        if (_userPlayer == null)
        {
            //todo: Unknown path
            print("UserPlayer is empty");
            GoToStartScene();
            return false;
            //throw new Exception("UDB-User Player doesn't Exists!!!");
        }
        _userPlayer.Print();
        _terrainDatabase.SetRegion(_userPlayer.Latitude, _userPlayer.Longitude);
        if (Convert.ToDateTime(_userPlayer.LastLogin) < Convert.ToDateTime(_userPlayer.LockUntil))
        {
            //todo: Unknown path
            print("UserPlayer is Locked now = " + Convert.ToDateTime(_userPlayer.LastLogin) + "now = " + Convert.ToDateTime(_userPlayer.LockUntil)  );
            GoToWaitScene();
        }
        return true;
    }
    #endregion
    #region UserInventory
    public void UpdateUserInventory(List<UserItem> userInventory)
    {
        _userInventory = userInventory;
        Debug.Log("UDB-UserInventory.Count = " + _userInventory.Count);
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
        //SaveUserInventory();
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
    internal bool ValidateCharacterCode(string characterCode)
    {
        for (int j = 0; j < (_userCharacters).Count; j++)
            if (_userCharacters[j].CharacterCode != null)
                if (_userCharacters[j].CharacterCode == characterCode)
                {
                    _userCharacters[j].CharacterCode = "";
                    //SaveUserCharacters();
                    return true;
                }
        return false;
    }
    public void UpdateUserCharacters(List<UserCharacter> userCharacters)
    {
        _userCharacters = userCharacters;
        Debug.Log("UDB-UserCharacters.Count = " + _userCharacters.Count);
        _userDatabase.BuildUserCharacters();
    }
    internal void BuildUserCharacters()
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
        _myCharacters = myCharacters;
        Debug.Log("UDB-BuiltUserCharacters.Count = " + _myCharacters.Count);
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
            //SaveUserCharacters();
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
    internal void SaveCharacterMixture(int itemId, int stackCnt, DateTime time)
    {
        if (stackCnt == 0)
            _characterMixture.StackCnt = 0;
        else
            _characterMixture.SetValues(_userPlayer.Id, itemId, stackCnt, time);
        SaveCharacterMixture();
    }
    internal void SaveCharacterMixture()
    {
        _apiGatewayConfig.PutCharacterMixture(_characterMixture);
        if (_characterMixture.StackCnt==0)
            _characterMixture.SetEmpty();
    }
    public void UpdateCharacterMixture(CharacterMixture characterMixture)
    {
        _characterMixture = characterMixture;
        Debug.Log("UDB-CharacterMixture = " +  (_characterMixture == null ? "Empty" : _characterMixture.MyInfo()) );
    }
    #endregion
    #region CharacterResearching
    internal CharacterResearching GetCharacterResearching()
    {
        return _characterResearching;
    }
    internal void SaveCharacterResearching(int researchId, int level, DateTime time)
    {
        if (level == 0)
            _characterResearching.Level = 0;
        else
            _characterResearching.SetValues(_userPlayer.Id, researchId, level, time);
        SaveCharacterResearching();
    }
    internal void SaveCharacterResearching()
    {
        _apiGatewayConfig.PutCharacterResearching(_characterResearching);
        if (_characterResearching.Level == 0)
            _characterResearching.SetEmpty();
    }

    public void UpdateCharacterResearching(CharacterResearching characterResearching)
    {
        _characterResearching = characterResearching;
        Debug.Log("UDB-CharacterResearch = " + (_characterResearching == null ? "Empty" : _characterResearching.MyInfo()));
    }
    #endregion
    #region CharacterResearch
    internal List<CharacterResearch> GetCharacterResearches()
    {
        return _characterResearches;
    }
    public void UpdateCharacterResearches(List<CharacterResearch> userResearches)
    {
        _characterResearches = userResearches;
        Debug.Log("UDB-CharacterResearch.Count = " + _characterResearches.Count);
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
            _characterResearches.Add(new CharacterResearch(research.Id,_userPlayer.Id ,level));
        //SaveCharacterResearches();
    }
    #endregion
    #region CharacterSetting
    public CharacterSetting GetCharacterSetting()
    {
        return _characterSetting;
    }
    public void SaveCharacterSetting()
    {
        _apiGatewayConfig.SaveCharacterSetting(_characterSetting);
    }
    public void UpdateCharacterSetting(CharacterSetting characterSetting)
    {
        _characterSetting = characterSetting;
        if (_characterSetting == null)
        {
            //Unknown path
            print("CharacterSetting is empty");
            GoToStartScene();
            return;
            //throw new Exception("UDB-Character Setting doesn't Exists!!!");
        }
        _characterSetting.Print();
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
            //SaveUserRecipes();
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
    public void UpdateUserRecipes(List<UserRecipe> userRecipes)
    {
        _userRecipes = userRecipes;
        Debug.Log("UDB-UserRecipes.Count = " + _userRecipes.Count);
        BuildUserRecipes();
    }
    internal bool ValidateRecipeCode(string recipeCode)
    {
        for (int j = 0; j < _userRecipes.Count; j++)
            if (_userRecipes[j].RecipeCode != null)
                if (_userRecipes[j].RecipeCode == recipeCode)
                {
                    _userRecipes[j].RecipeCode = "";
                    //SaveUserRecipes();
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