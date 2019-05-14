using System;
using System.Collections;
using System.Collections.Generic;
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

        //todo: delete ##############################
        //Call UserPlayer
        var apiGate = "GetUserPlayer";
        var uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        //StartCoroutine(GetRequest(uri, TestUserPlayer));
        //###########################################

        FirstWave();
        StartCoroutine(SecondWave());
    }

    //todo: delete ##############################
    private void TestUserPlayer(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserPlayer.Id == 0)
            throw new Exception("API-UserPlayer Failed!!!");
        UserPlayer up = response.Body.UserPlayer;
        up.Print();
        up.Gem += 70;
        up.Description +=  up.Gem;
        up.FBLoggedIn = !up.FBLoggedIn;
        up.SoundVolume *= 2;
        up.Print();
        int days = 10;
        SaveUserPlayer(up, days);
    }
    //###########################################

    // Update is called once per frame
    void Update()
    {
    }
    #region FirstWave
    private void FirstWave()
    {
        string apiGate;
        string uri;
        //Call Random *
        apiGate = "GetRandom";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?min={0}&max={1}", "999", "10000");
        StartCoroutine(GetRequest(uri, ReadRandomJson));
        //Call Items *
        apiGate = "GetItems";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?uid={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadItemsJson));
        //Call Recipe *
        apiGate = "GetRecipes";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?uid={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadRecipesJson));
        //Call Offer *
        apiGate = "GetOffers";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?uid={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadOffersJson));

        //Call Characters *
        apiGate = "GetCharacters";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?uid={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadCharactersJson));
        //Call Researches *
        apiGate = "GetResearches";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?uid={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadResearchesJson));

        //Call Regions
        apiGate = "GetRegions";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?uid={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadRegionsJson));
        //Call Terrains *
        apiGate = "GetTerrains";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?uid={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadTerrainsJson));
        //Call Elements *
        apiGate = "GetElements";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?uid={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadElementsJson));
        //Call InsideStories *
        apiGate = "GetInsideStories";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?uid={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadInsideStoriesJson));
    }
    private void ReadRandomJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.RandomNum == 0)
            return;
        //Todo: get the userid
        _userId = 22;
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
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
    #endregion
    #region SecondWave    
    IEnumerator SecondWave()
    {
        yield return new WaitUntil(() => _firstWaveTarget >= 10);
        //Call UserPlayer
        var apiGate = "GetUserPlayer";
        var uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadUserPlayerJson));
    }
    private void ReadUserPlayerJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserPlayer.Id == 0)
            throw new Exception("API-UserPlayer Failed!!!");
        if (_userDatabase.UpdateUserPlayer(response.Body.UserPlayer))
        {
            _gameLoadHelper.LoadingThumbsUp();
            ThirdWave();
        }
    }
    #endregion
    #region ThirdWave
    private void ThirdWave()
    {
        //Call CharacterMixture
        string apiGate = "GetCharacterMixture";
        string uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
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
    }
    private void ReadCharacterSettingJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.CharacterSetting.Id == -1)
            throw new Exception("API-CharacterSetting Failed!!!");
        _userDatabase.UpdateCharacterSetting(response.Body.CharacterSetting);
        _gameLoadHelper.LoadingThumbsUp();
    }
    private void ReadCharacterMixtureJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.CharacterMixture.Id == 0)
            Debug.LogWarning("####API-CharacterMixture Is Empty!!!");
        _userDatabase.UpdateCharacterMixture(response.Body.CharacterMixture);
        _gameLoadHelper.LoadingThumbsUp();
    }
    private void ReadCharacterResearchingJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.CharacterResearching.Id == 0)
            Debug.LogWarning("####API-CharacterResearching Is Empty!!!");
        _userDatabase.UpdateCharacterResearching(response.Body.CharacterResearching);
        _gameLoadHelper.LoadingThumbsUp();
    }



    private void ReadUserInventoryJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserInventory.Count == 0)
            throw new Exception("API-UserInventory Failed!!!");
        _userDatabase.UpdateUserInventory(response.Body.UserInventory);
        _gameLoadHelper.LoadingThumbsUp();
    }
    private void ReadUserCharactersJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserCharacters.Count == 0)
            throw new Exception("API-UserCharacters Failed!!!");
        _userDatabase.UpdateUserCharacters(response.Body.UserCharacters);
        _gameLoadHelper.LoadingThumbsUp();
        ForthWave();
    }
    private void ReadCharacterResearchesJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.CharacterResearches.Count == 0)
            throw new Exception("API-CharacterResearches Failed!!!");
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
    #endregion
    #region ForthWave
    private void ForthWave()
    {
        //Call CharacterSetting
        string apiGate = "GetCharacterSetting";
        string uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", "11");
        StartCoroutine(GetRequest(uri, ReadCharacterSettingJson));
    }
    #endregion
    #region Updates
    internal void SaveUserPlayer(UserPlayer userPlayer, int days)
    {
        var healthCheck =userPlayer.CalculateHealthCheck();
        var apiGate = "GetUserPlayer";
        var uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", userPlayer.Id.ToString());
        ApiRequest ap = new ApiRequest
        {
            Action = "UpdateUserPlayer",
            HealthCheck = healthCheck,
            Time = days,
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
        if (response == null) throw new ArgumentNullException("Api Response is null");
        return response;
    }
    private IEnumerator GetRequest(string uri,System.Action<string> callback)
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