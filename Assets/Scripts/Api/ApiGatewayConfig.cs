using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ApiGatewayConfig : MonoBehaviour
{
    private static ApiGatewayConfig _apiGatewayConfig;
    //Database
    private ItemDatabase _itemDatabase;
    private UserDatabase _userDatabase;
    private CharacterDatabase _characterDatabase;
    private TerrainDatabase _terrainDatabase;

    private GameLoadHelper _gameLoadHelper;

    private const string ApiKey = "AOlOnm2C4394k8QZHqkLl8xYcCEWRSND5WtAclWq";
    private const string ApiPath = "https://h28ve9pjh5.execute-api.us-west-2.amazonaws.com/";
    private const string ApiStage = "prod/";
    private int _userId=22;

    private List<UserItem> _oldUserInventory =new List<UserItem>();
    private int _firstWaveTarget;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("***API*** Start!");
        _itemDatabase = ItemDatabase.Instance();
        _userDatabase =UserDatabase.Instance();
        _characterDatabase=CharacterDatabase.Instance();
        _terrainDatabase=TerrainDatabase.Instance();
        _gameLoadHelper = GameObject.Find("GameStarter").GetComponent<GameLoadHelper>();
        //Call Random
        string apiGate = "GetRandom";
        string uri = String.Format(ApiPath + ApiStage + apiGate + "?min={0}&max={1}", "999", "10000");
        StartCoroutine(GetRequest(uri, GetUserIdForThisDevice));
        
    }
    #region ReadDB
    private void FirstWave()
    {
        string apiGate;
        string uri;
        //###Item Database
        //Call Items
        apiGate = "GetItems";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadItemsJson));
        //Call Recipe
        apiGate = "GetRecipes";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadRecipesJson));
        //Call Offer
        apiGate = "GetOffers";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadOffersJson));
        //###Character Database
        //Call Characters
        apiGate = "GetCharacters";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadCharactersJson));
        //Call Researches
        apiGate = "GetResearches";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadResearchesJson));
        //###Terrain Database
        //Call Regions
        apiGate = "GetRegions";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadRegionsJson));
        //Call Terrains
        apiGate = "GetTerrains";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadTerrainsJson));
        //Call Elements
        apiGate = "GetElements";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadElementsJson));
        //Call InsideStories
        apiGate = "GetInsideStories";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadInsideStoriesJson));

    }
    IEnumerator SecondWave()
    {
        yield return new WaitUntil(() => _firstWaveTarget >= 9);        
        string apiGate;
        string uri;
        //###User Database
        //Call CharacterMixture
        apiGate = "GetCharacterMixture";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadCharacterMixtureJson));
        //Call CharacterResearching
        apiGate = "GetCharacterResearching";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadCharacterResearchingJson));
        //Call UserInventory
        apiGate = "GetUserInventory";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadUserInventoryJson));
        //Call CharacterResearches
        apiGate = "GetCharacterResearches";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadCharacterResearchesJson));
        //Call UserRecipes
        apiGate = "GetUserRecipes";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadUserRecipesJson));
        //Call UserCharacters
        apiGate = "GetUserCharacters";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadUserCharactersJson));
        //Call CharacterSetting
        apiGate = "GetCharacterSetting";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadCharacterSettingJson));
        //Call UserPlayer
        apiGate = "GetUserPlayer";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadUserPlayerJson));
    }
    private void GetUserIdForThisDevice(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.RandomNum == 0)
            return;
        //Todo: get the userid
        _userId = 33;
        _gameLoadHelper.LoadingThumbsUp();
        FirstWave();
        StartCoroutine(SecondWave());
    }
    //ItemDatabase
    private void ReadItemsJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.Items.Count == 0)
            throw new Exception("API-Items Failed!!!");
        _itemDatabase.UpdateItems(response.Body.Items);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    private void ReadRecipesJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.Recipes.Count == 0)
             throw new Exception("API-Recipes Failed!!!");
        _itemDatabase.UpdateRecipes(response.Body.Recipes);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    private void ReadOffersJson(string result)
    { 
        var response = TranslateResponse(result);
        if (response.Body.Offers.Count == 0)
          throw new Exception("API-Offers Failed!!!");
        _itemDatabase.UpdateOffers(response.Body.Offers);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    //CharacterDatabase
    private void ReadCharactersJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.Characters.Count == 0)
            throw new Exception("API-Characters Failed!!!");
        _characterDatabase.UpdateCharacters(response.Body.Characters);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    private void ReadResearchesJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.Researches.Count == 0)
            throw new Exception("API-Researches Failed!!!");
        _characterDatabase.UpdateResearches(response.Body.Researches);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    //TerrainDatabase
    private void ReadRegionsJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.Regions.Count == 0)
             throw new Exception("API-Regions Failed!!!");
        _terrainDatabase.UpdateRegions(response.Body.Regions);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    private void ReadTerrainsJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.Terrains.Count == 0)
            throw new Exception("API-Terrains Failed!!!");
        _terrainDatabase.UpdateTerrains(response.Body.Terrains);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    private void ReadElementsJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.Elements.Count == 0)
            throw new Exception("API-Elements Failed!!!");
        _terrainDatabase.UpdateElements(response.Body.Elements);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    private void ReadInsideStoriesJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.InsideStories.Count == 0)
            throw new Exception("API-InsideStories Failed!!!");
        _terrainDatabase.UpdateInsideStories(response.Body.InsideStories);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    //UserDatabase
    private void ReadCharacterMixtureJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.CharacterMixture==null)
            Debug.LogWarning("####API-CharacterMixture Is Empty!!!");
        _userDatabase.UpdateCharacterMixture(response.Body.CharacterMixture);
        _gameLoadHelper.LoadingThumbsUp();
    }
    private void ReadCharacterResearchingJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.CharacterResearching == null)
            Debug.LogWarning("####API-CharacterResearching Is Empty!!!");
        _userDatabase.UpdateCharacterResearching(response.Body.CharacterResearching);
        _gameLoadHelper.LoadingThumbsUp();
    }
    private void ReadUserInventoryJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserInventory.Count == 0)
            Debug.LogWarning("####API-UserInventory Is Empty!!!");
        SavedUserInventory(response.Body.UserInventory);
        _userDatabase.UpdateUserInventory(response.Body.UserInventory);
        _gameLoadHelper.LoadingThumbsUp();
    }
    private void ReadUserCharactersJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserCharacters.Count == 0)
            Debug.LogWarning("####API--UserCharacters Is Empty!!!");
        _userDatabase.UpdateUserCharacters(response.Body.UserCharacters);
        _gameLoadHelper.LoadingThumbsUp();
    }
    private void ReadCharacterResearchesJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.CharacterResearches.Count == 0)
            Debug.LogWarning("####API--CharacterResearches Is Empty!!!");
        _userDatabase.UpdateCharacterResearches(response.Body.CharacterResearches);
        _gameLoadHelper.LoadingThumbsUp();
    }
    private void ReadUserRecipesJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserRecipes.Count == 0)
            Debug.LogWarning("####API--UserRecipes Is Empty!!!");
        _userDatabase.UpdateUserRecipes(response.Body.UserRecipes);
        _gameLoadHelper.LoadingThumbsUp();
    }
    private void ReadCharacterSettingJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.CharacterSetting.Id == 0)
            Debug.LogWarning("####API-CharacterSetting Is Empty!!!");
        _userDatabase.UpdateCharacterSetting(response.Body.CharacterSetting);
        _gameLoadHelper.LoadingThumbsUp();
    }
    private void ReadUserPlayerJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserPlayer == null)
            Debug.LogWarning("####API-UserPlayer Is Empty!!!");
        if (_userDatabase.UpdateUserPlayer(response.Body.UserPlayer))
            _gameLoadHelper.LoadingThumbsUp();
        var characterManager = Resources.Load<GameObject>("Prefabs/CharacterManager");
        Instantiate(characterManager);
        var musicBox = Resources.Load<GameObject>("Prefabs/MusicBox");
        Instantiate(musicBox);
        var cache = Resources.Load<GameObject>("Prefabs/Cache");
        Instantiate(cache);
    }
    #endregion
    #region Updates
    private void SavedUserInventory(List<UserItem> userInventory)
    {
        _oldUserInventory.Clear();
        userInventory.ForEach((item) =>
        {
            _oldUserInventory.Add(new UserItem(item));
        });
    }
    internal void SaveUserPlayer(UserPlayer userPlayer)
    {
        //mins is the minutes that the user should stay locked 
        var apiGate = "GetUserPlayer";
        var uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", userPlayer.Id.ToString());
        var action = "Update";
        if (userPlayer.Latitude == 0 && userPlayer.Longitude == 0)
        {
            action = "Insert";
            _terrainDatabase.SetRegion(userPlayer.Latitude, userPlayer.Longitude);
        }
        ApiRequest ap = new ApiRequest
        {
            Action = action,
            UserPlayer = userPlayer
        };
        StartCoroutine(PutRequest(uri, ap));
    }
    internal void SaveCharacterSetting(CharacterSetting characterSetting)
    {
        var healthCheck = characterSetting.CalculateHealthCheck();
        var apiGate = "GetCharacterSetting";
        var uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", characterSetting.Id.ToString());
        ApiRequest ap = new ApiRequest
        {
            Action = "UpdateCharacterSetting",
            HealthCheck = healthCheck,
            CharacterSetting = characterSetting
        };
        StartCoroutine(PutRequest(uri, ap));
    }
    internal void PutUserInventory(List<UserItem> userInventory)
    {
        Debug.Log("PutUserInventory   _oldUserInventory=" + _oldUserInventory.Count + " userInventory = "+ userInventory.Count);
        //Todo: delete
        //foreach (var item in _oldUserInventory) item.Print();
        //foreach (var item in userInventory) item.Print();
        foreach (var dbItem in _oldUserInventory)
        {
            bool delete = true;
            foreach (var item in userInventory)
            {
                if (dbItem.Id == item.Id)
                {
                    delete = false;
                    if (!dbItem.Equals(item))
                        PutUserInventory(item, "Update");
                    break;
                }
            }
            if (delete)
                PutUserInventory(dbItem, "Delete");
        }
        foreach (var item in userInventory)
        {
            bool newItem = true;
            foreach (var exItem in _oldUserInventory)
            {
                if (exItem.Id == item.Id)
                {
                    newItem = false;
                    break;
                }
            }
            if (newItem)
                PutUserInventory(item, "Insert");
        }
        SavedUserInventory(userInventory);
    }
    private void PutUserInventory(UserItem item, string action)
    {
        var apiGate = "GetUserInventory";
        var uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", item.UserId.ToString());
        ApiRequest ap = new ApiRequest
        {
            Action = action,
            UserInventory = item
        };
        Debug.Log(ap.Action + " UserInventory : " + ap.UserInventory.MyInfo());
        StartCoroutine(PutRequest(uri, ap, true));
    }
    internal void PutCharacterMixture(CharacterMixture characterMixture)
    {
        var apiGate = "GetCharacterMixture";
        var uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", characterMixture.Id.ToString());
        int time = -1;
        string action;
        if (characterMixture.StackCnt == 0)
            action = "Delete";
        else if (characterMixture.MixTime == "Now")
        {
            action = "Update";
            time = 0;
        }
        else
            action = "Insert";
        ApiRequest ap = new ApiRequest
        {
            Action = action,
            Time = time,
            CharacterMixture = characterMixture 
        };
        StartCoroutine(PutRequest(uri, ap));
    }
    internal void PutUserRecipe(UserRecipe userRecipe,string code=null)
    {
        var apiGate = "GetUserRecipes";
        var uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", userRecipe.Id.ToString());
        ApiRequest ap = new ApiRequest
        {
            Action = "Insert",
            UserRecipe = userRecipe
        };
        if (code != null)
        {
            ap.Action = "Update";
            ap.Code = code;
        }
        Debug.Log(ap.Action + " UserRecipe : " + userRecipe.MyInfo());
        StartCoroutine(PutRequest(uri, ap, true));
    }
    internal void PutUserCharacter(UserCharacter userCharacter, string code = null)
    {
        var apiGate = "GetUserCharacters";
        var uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", userCharacter.Id.ToString());
        ApiRequest ap = new ApiRequest
        {
            Action = "Insert",
            UserCharacter = userCharacter
        };
        if (code != null)
        {
            ap.Action = "Update";
            ap.Code = code;
        }
        Debug.Log(ap.Action + " UserCharacter : " + userCharacter.MyInfo());
        StartCoroutine(PutRequest(uri, ap, true));
    }
    internal void PutCharacterResearching(CharacterResearching characterResearching)
    {
        var apiGate = "GetCharacterResearching";
        var uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", characterResearching.Id.ToString());
        int time = -1;
        string action;
        if (characterResearching.Level == 0)
            action = "Delete";
        else if (characterResearching.ResearchTime == "Now")
        {
            action = "Update";
            time = 0;
        }
        else
            action = "Insert";
        ApiRequest ap = new ApiRequest
        {
            Action = action,
            Time = time,
            CharacterResearching = characterResearching
        };
        StartCoroutine(PutRequest(uri, ap));
    }
    internal void PutCharacterResearch(CharacterResearch characterResearch)
    {
        var apiGate = "GetCharacterResearches";
        var uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", characterResearch.Id.ToString());
        string action = "Insert";
        if (characterResearch.Level != 1)
            action = "Update";
        ApiRequest ap = new ApiRequest
        {
            Action = action,
            CharacterResearch = characterResearch
        };
        StartCoroutine(PutRequest(uri, ap));
    }
    #endregion
    #region ApiRequest
    private ApiResponse TranslateResponse(string result)
    {
        ApiResponse response = JsonUtility.FromJson<ApiResponse>(result);
        if (response == null) throw new ArgumentNullException("API Response is null");
        return response;
    }
    private IEnumerator GetRequest(string uri,Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("x-api-key", ApiKey);
            yield return request.SendWebRequest();
            string result;
            if (request.isNetworkError)
                result = request.error;
            else
                result = request.downloadHandler.text;
            print(result);
            callback(result);
        }
    }
    private IEnumerator PutRequest(string uri, ApiRequest apiRequest, bool refresh=false)
    {
        string json = JsonUtility.ToJson(apiRequest);
        Debug.Log("json: " + json);
        using (UnityWebRequest request = UnityWebRequest.Put(uri, json))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("x-api-key", ApiKey);
            yield return request.SendWebRequest();
            if (request.isNetworkError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log("request.downloadHandler.text = " + request.downloadHandler.text);
                if (refresh)
                {
                    var response = TranslateResponse(request.downloadHandler.text);
                    Refresh(apiRequest, response);
                }
            }
        }
    }
    private void Refresh(ApiRequest apiRequest, ApiResponse apiResponse)
    {
        //UserRecipe
        if (apiRequest.UserRecipe.Id != 0)
        {
            if (apiResponse.Body.UserRecipe!=null)
            {
                //Response Item has value && (Insert/Update)
                if (apiResponse.Body.UserRecipe.Id != 0)
                {
                    _userDatabase.UpdateUserRecipe(apiResponse.Body.UserRecipe);
                    return;
                }
            }
            //Update && Response Item has 0 value or not 
            if (apiRequest.Action == "Update")
            {
                Debug.Log("Update UserRecipe==null");
                return;
            }
            //Insert && Response empty
            _userDatabase.UpdateUserRecipe(apiRequest.UserRecipe);
        }
        if (apiRequest.UserCharacter.Id != 0)
        {
            if (apiResponse.Body.UserCharacter != null)
            {
                //Response Item has value && (Insert/Update)
                if (apiResponse.Body.UserCharacter.Id != 0)
                {
                    _userDatabase.UpdateUserCharacter(apiResponse.Body.UserCharacter);
                    return;
                }
            }
            //Update && Response Item has 0 value or not 
            if (apiRequest.Action == "Update")
            {
                Debug.Log("Update UserCharacter==null");
                return;
            }
            //Insert && Response empty
            _userDatabase.UpdateUserCharacter(apiRequest.UserCharacter);
        }
    }
    #endregion
    #region ApiGatewayConfig Instance
    public static ApiGatewayConfig Instance()
    {
        if (!_apiGatewayConfig)
        {
            _apiGatewayConfig = FindObjectOfType(typeof(ApiGatewayConfig)) as ApiGatewayConfig;
            if (!_apiGatewayConfig)
                Debug.LogError("There needs to be one active ApiGatewayConfig script on a GameObject in your scene.");
        }
        return _apiGatewayConfig;
    }
    #endregion
}