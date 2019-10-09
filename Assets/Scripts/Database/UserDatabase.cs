using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserDatabase : MonoBehaviour
{
    private static UserDatabase _userDatabase;
    private ApiGatewayConfig _apiGatewayConfig;
    private XmlHelper _xmlHelper;
    private TerrainDatabase _terrainDatabase;

    private UserPlayer _userPlayer;
    private List<UserPlayer> _allUsers;
    private List<UserItem> _userInventory = new List<UserItem>();
    private List<UserCharacter> _userCharacters = new List<UserCharacter>();
    private List<Character> _myCharacters = new List<Character>();
    private CharacterMixture _characterMixture = new CharacterMixture();
    private List<CharacterResearch> _characterResearches = new List<CharacterResearch>();
    private CharacterResearching _characterResearching;
    private List<MailMessage> _mailMessages = new List<MailMessage>();

    private CharacterSetting _characterSetting;
    private List<UserRecipe> _userRecipes = new List<UserRecipe>();
    private List<Recipe> _myRecipes;

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
        _xmlHelper = XmlHelper.Instance();
        Debug.Log("***UDB*** Start!");
    }
    #region UserPlayer
    public UserPlayer GetUserPlayer()
    {
        return _userPlayer;
    }
    public void SaveUserPlayer(UserPlayer userPlayer)
    {
        _apiGatewayConfig.SaveUserPlayer(userPlayer);
    }
    public void UpdateUserPlayer(UserPlayer userPlayer)
    {
        _userPlayer = userPlayer;
        if (_userPlayer!= null)
        {
            _userPlayer.SetLocalTimes();
            _userPlayer.Print();
        }
    }
    public bool StartGameValidation()
    {
        if (_userPlayer == null)
        {
            //New User
            GoToStartScene();
            return false;
        }
        //Set Location Values
        var locationHelper = LocationHelper.Instance();
        var loc = locationHelper.GetLocation();
        _userPlayer.Latitude = loc.x;
        _userPlayer.Longitude = loc.y;
        _terrainDatabase.SetRegion(_userPlayer.Latitude, _userPlayer.Longitude);
        if (DateTime.Now < _userPlayer.GetLockUntil())
        {
            //Lock User
            print("UserPlayer is Locked now = " + Convert.ToDateTime(_userPlayer.LastLogin) + "(" + DateTime.Now + ") (" + _userPlayer.GetLastLogin() + ")"
                  + "AddMinutes = " + _userPlayer.LockMins
                  + "LockUntil = " + Convert.ToDateTime(_userPlayer.LockUntil) + "(" + _userPlayer.GetLockUntil() + ")");
            GoToWaitScene();
            return false;
        }
        _userPlayer.LockMins = 0;
        SaveUserPlayer(_userPlayer);
        Debug.Log("UD-UserPlayer Verified  = " + _userPlayer.MyInfo());

        if (_characterSetting == null)
            throw new Exception("UDB-Character Setting doesn't Exists!!!");
        if (_characterSetting.Id == 0)
        {
            //No active character
            GoToStartScene();
            return false;
        }
        if (!_characterSetting.Alive)
        {
            //Dead active character
            SceneManager.LoadScene(SceneSettings.SceneIdForGameOver);
            return false;
        }
        return true;
    }
    internal UserPlayer GetUserById(int id)
    {
        //todo: get all the users 
        if (_allUsers.Count<0)
        {
            _allUsers.Add(_userPlayer);
        }

        for (int i = 0; i < _allUsers.Count; i++)
            if (_allUsers[i].Id == id)
                return _allUsers[i];
        return null;
    }
    internal void UpdateAllUserPlayers(List<UserPlayer> userPlayers)
    {
        _allUsers = userPlayers;
        Debug.Log("UDB-AllUsers.Count = " + _allUsers.Count);
    }
    internal List<UserPlayer> GetAllUserPlayers()
    {
        if (_allUsers==null )
            _allUsers= new List<UserPlayer>();
        return _allUsers;
    }
    #endregion
    #region UserInventory
    public void UpdateUserInventory(List<UserItem> userInventory)
    {
        _userInventory = userInventory;
        Debug.Log("UDB-UserInventory.Count = " + _userInventory.Count);
    }
    internal void UpdateUserInventory(UserItem userItem=null)
    {
        if (userItem != null)
        {
            Debug.Log("UDB-Add item = " + userItem.MyInfo());
            _userInventory.Add(userItem);
        }
        SaveUserInventory();
    }
    internal void DeleteItemFromInventory(UserItem userItem)
    {
        Debug.Log("UDB-Remove item = " + userItem.MyInfo());
        _userInventory.Remove(userItem);
        SaveUserInventory();
    }
    internal List<UserItem> GetUserInventory()
    {
        return _userInventory;
    }
    public void SaveUserInventory()
    {
        _apiGatewayConfig.PutUserInventory(_userInventory);
    }
    #endregion
    #region UserCharacters    
    public List<UserCharacter> GetMyUserCharacters()
    {
        return _userCharacters;
    }
    public List<Character> GetMyCharacters()
    {
        return _myCharacters;
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
            if (characters[i].IsPublic)
            {
                myCharacters.Add(characters[i]);
                continue;
            }
            for (int j = 0; j < _userCharacters.Count; j++)
                if (characters[i].Id == _userCharacters[j].CharacterId)
                {
                    if (string.IsNullOrEmpty(_userCharacters[j].CharacterCode))
                        myCharacters.Add(characters[i]);
                    else{
                        characters[i].IsEnable = false;
                        myCharacters.Add(characters[i]);
                    }
                    break;
                }
        }
        _myCharacters = myCharacters;
        Debug.Log("UDB-BuiltUserCharacters.Count = " + _myCharacters.Count);
    }
    internal bool AddNewRandomUserCharacters()
    {
        var characterDatabase = CharacterDatabase.Instance();
        var characters = characterDatabase.GetCharacters();
        List<int> availableCharacters = new List<int>();
        int key = DateTime.Now.DayOfYear;
        var rarity = RandomHelper.Range(key, (int)DataTypes.Rarity.Common);
        bool userOwnedCharacter = false;
        for (int i = 0; i < characters.Count; i++)
            if (characters[i].IsEnable && !characters[i].IsPublic)
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
            var characterId = availableCharacters[RandomHelper.Range(key, availableCharacters.Count)];
            if (AddUserCharacters(characterId))
                return true;
        }
        return false;
    }
    public bool AddUserCharacters(int characterId)
    {
        try
        {
            var uc = _userCharacters.Find(c => c.CharacterId == characterId && !string.IsNullOrEmpty(c.CharacterCode));
            if (uc == null)
                uc = new UserCharacter(characterId, _userPlayer.Id);
            _apiGatewayConfig.PutUserCharacter(uc);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }
    public void UpdateUserCharacters(List<UserCharacter> userCharacters)
    {
        _userCharacters = userCharacters;
        Debug.Log("UDB-UserCharacters.Count = " + _userCharacters.Count);
        _userDatabase.BuildUserCharacters();
    }
    internal void UpdateUserCharacter(UserCharacter userCharacter)
    {
        for (int i = 0; i < _userCharacters.Count; i++)
            if (_userCharacters[i].Id == userCharacter.Id)
            {
                _userCharacters[i].CharacterCode = "";
                for (int j = 0; j < _myCharacters.Count; j++)
                    if (_myCharacters[j].Id == _userCharacters[i].CharacterId)
                    {
                        _myCharacters[j].IsEnable = true;
                        var characterHandler = FindObjectOfType(typeof(CharacterListHandler)) as CharacterListHandler;
                        if (characterHandler != null)
                            characterHandler.SceneRefresh = true;
                        return;
                    }
            }
        _userCharacters.Add(userCharacter);
        var characterDatabase = CharacterDatabase.Instance();
        Character character = characterDatabase.GetCharacterById(userCharacter.CharacterId);
        _myCharacters.Add(character);
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
        _xmlHelper.SaveCharacterMixture(_characterMixture);
        if (_characterMixture.StackCnt==0)
            _characterMixture.SetEmpty();
    }
    public void UpdateCharacterMixture(CharacterMixture characterMixture)
    {
        _characterMixture = characterMixture;
        Debug.Log("UDB-CharacterMixture = " + _characterMixture.MyInfo());
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
        Debug.Log("UDB-CharacterResearch = " +_characterResearching.MyInfo());
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
        foreach (var chResearch in _characterResearches)
            if (chResearch.ResearchId == research.Id)
            {
                if (level <= chResearch.Level)
                    throw new Exception("CharacterResearch Invalid <= ");
                if (chResearch.Level + 1 != level)
                    throw new Exception("CharacterResearch Invalid != ");
                chResearch.Level += 1;
                SaveCharacterResearch(chResearch);
                return;
            }
        var newResearch = new CharacterResearch(research.Id, _userPlayer.Id);
        _characterResearches.Add(newResearch);
        SaveCharacterResearch(newResearch);
    }
    private void SaveCharacterResearch(CharacterResearch characterResearch)
    {
        Debug.Log("UDB-characterResearch = " + characterResearch.MyInfo());
        _apiGatewayConfig.PutCharacterResearch(characterResearch);
    }
    #endregion
    #region CharacterSetting
    public CharacterSetting GetCharacterSetting()
    {
        return _characterSetting;
    }
    public void SaveCharacterSetting(CharacterSetting characterSetting)
    {
        _apiGatewayConfig.SaveCharacterSetting(characterSetting);
    }
    public void UpdateCharacterSetting(CharacterSetting characterSetting)
    {
        _characterSetting = characterSetting;
        _characterSetting.Print();
    }
    #endregion
    #region UserRecipe
    internal List<UserRecipe> GetMyUserRecipes()
    {
        return _userRecipes;
    }
    public List<Recipe> GetMyRecipes()
    {
        return _myRecipes;
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
                    else{
                        recipes[i].IsEnable = false;
                        myRecipes.Add(recipes[i]);
                    }
                    break;
                }
        }
        _myRecipes = myRecipes;
        Debug.Log("UDB-BuiltUserRecipes.Count = " + _myRecipes.Count);
    }
    internal bool AddNewRandomUserRecipe()
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
            var recipeId = availableRecipe[RandomHelper.Range(key, availableRecipe.Count)];
            var ur = _userRecipes.Find(c => c.RecipeId == recipeId && !string.IsNullOrEmpty(c.RecipeCode));
            if (ur == null)
                ur = new UserRecipe(recipeId, _userPlayer.Id);
            _apiGatewayConfig.PutUserRecipe(ur);
            return true;
        }
        return false;
    }
    public void UpdateUserRecipes(List<UserRecipe> userRecipes)
    {
        _userRecipes = userRecipes;
        Debug.Log("UDB-UserRecipes.Count = " + _userRecipes.Count);
        BuildUserRecipes();
    }
    internal void UpdateUserRecipe(UserRecipe userRecipe)
    {
        for (int i = 0; i < _userRecipes.Count; i++)
            if (_userRecipes[i].Id == userRecipe.Id)
            {
                _userRecipes[i].RecipeCode = "";
                for (int j = 0; j < _myRecipes.Count; j++)
                    if (_myRecipes[j].Id == _userRecipes[i].RecipeId)
                    {
                        _myRecipes[j].IsEnable = true;
                        var recipeHandler = FindObjectOfType(typeof(RecipeListHandler)) as RecipeListHandler;
                        if (recipeHandler != null)
                            recipeHandler.SceneRefresh = true;
                        return;
                    }
            }
        _userRecipes.Add(userRecipe);
        var itemDatabase = ItemDatabase.Instance();
        Recipe recipe = itemDatabase.GetRecipeById(userRecipe.RecipeId);
        _myRecipes.Add(recipe);
    }
    #endregion
    #region MailMessages    
    public List<MailMessage> GetMailMessages()
    {
        return _mailMessages;
    }
    internal void UpdateMailMessages(List<MailMessage> mailMessages)
    {
        _mailMessages = mailMessages;
        Debug.Log("UDB-MailMessages.Count = " + _mailMessages.Count);
        foreach (var msg in _mailMessages) msg.Print();
    }
    internal void AddMailMessage(MailMessage mailMessage)
    {
        _mailMessages.Add(mailMessage);
        SaveMailMessage(mailMessage);
    }
    private void SaveMailMessage(MailMessage mailMessage)
    {
        Debug.Log("UDB-MailMessage = " + mailMessage.MyInfo());
        _apiGatewayConfig.PutMailMessage(mailMessage);
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