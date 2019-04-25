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


        FirstWave();
        StartCoroutine(SecondWave());









    }


    // Update is called once per frame
    void Update()
    {
    }
    #region FirstWave
    private void FirstWave()
    {
        print("*** FirstWave Started ");
        //Call Random
        var apiGate = "GetRandom";
        var uri = String.Format(ApiPath + ApiStage + apiGate + "?min={0}&max={1}", "999", "10000");
        StartCoroutine(GetRequest(uri, ReadRandomJson));
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

        //Call Characters
        apiGate = "GetCharacters";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadCharactersJson));
        //Call Researches
        apiGate = "GetResearches";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(uri, ReadResearchesJson));

        //Call Regions
        apiGate = "GetRegion";
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
    private void ReadRandomJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.RandomNum == 0)
            return;
        print("*** Updating RandomNum");
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
        print("*** Updating Items");
        _itemDatabase.UpdateItems(response.Body.Items);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    private void ReadRecipesJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.Recipes.Count == 0)
             throw new Exception("API-Recipes Failed!!!");
        print("*** Updating Recipes");
        _itemDatabase.UpdateRecipes(response.Body.Recipes);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    private void ReadOffersJson(string result)
    {
        //todo: move it out of loading  
        var response = TranslateResponse(result);
        if (response.Body.Offers.Count == 0)
          throw new Exception("API-Offers Failed!!!");
        print("*** Updating Offers");
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
        print("*** Updating Characters");
        _characterDatabase.UpdateCharacters(response.Body.Characters);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    private void ReadResearchesJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.Researches.Count == 0)
            throw new Exception("API-Researches Failed!!!");
        print("*** Updating Researches");
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
        print("*** Updating Regions");
        _terrainDatabase.UpdateRegions(response.Body.Regions);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    private void ReadTerrainsJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.Terrains.Count == 0)
            throw new Exception("API-Terrains Failed!!!");
        print("*** Updating Terrains");
        _terrainDatabase.UpdateTerrains(response.Body.Terrains);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    private void ReadElementsJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.Elements.Count == 0)
            throw new Exception("API-Elements Failed!!!");
        print("*** Updating Elements");
        _terrainDatabase.UpdateElements(response.Body.Elements);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    private void ReadInsideStoriesJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.InsideStories.Count == 0)
            throw new Exception("API-InsideStories Failed!!!");
        print("*** Updating InsideStories");
        _terrainDatabase.UpdateInsideStories(response.Body.InsideStories);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    #endregion
    #region SecondWave    
    IEnumerator SecondWave()
    {
        yield return new WaitUntil(() => _firstWaveTarget >= 10);
        print("*** SecondWave Started ");
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
        print("*** Updating UserPlayer");
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
        print("*** ThirdWave Started ");
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
        print("*** Updating CharacterSetting");
        _userDatabase.UpdateCharacterSetting(response.Body.CharacterSetting);
        _gameLoadHelper.LoadingThumbsUp();
    }
    private void ReadCharacterMixtureJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.CharacterMixture.Id == 0)
            throw new Exception("API-CharacterMixture Failed!!!");
        print("*** Updating CharacterMixture");
        _userDatabase.UpdateCharacterMixture(response.Body.CharacterMixture);
        _gameLoadHelper.LoadingThumbsUp();
    }
    private void ReadCharacterResearchingJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.CharacterResearching.Id == 0)
            throw new Exception("API-CharacterResearching Failed!!!");
        print("*** Updating CharacterResearching");
        _userDatabase.UpdateCharacterResearching(response.Body.CharacterResearching);
        _gameLoadHelper.LoadingThumbsUp();
    }
    private void ReadUserInventoryJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserInventory.Count == 0)
            throw new Exception("API-UserInventory Failed!!!");
        print("*** Updating UserInventory");
        _userDatabase.UpdateUserInventory(response.Body.UserInventory);
        _gameLoadHelper.LoadingThumbsUp();
    }
    private void ReadUserCharactersJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserCharacters.Count == 0)
            throw new Exception("API-UserCharacters Failed!!!");
        print("*** Updating UserCharacters");
        _userDatabase.UpdateUserCharacters(response.Body.UserCharacters);
        _gameLoadHelper.LoadingThumbsUp();
        ForthWave();
    }
    private void ReadCharacterResearchesJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.CharacterResearches.Count == 0)
            throw new Exception("API-CharacterResearches Failed!!!");
        print("*** Updating CharacterResearches");
        _userDatabase.UpdateCharacterResearches(response.Body.CharacterResearches);
        _gameLoadHelper.LoadingThumbsUp();
    }
    private void ReadUserRecipesJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserRecipes.Count == 0)
            throw new Exception("API-UserRecipes Failed!!!");
        print("*** Updating UserRecipes");
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
    #region ApiRequest
    private ApiResponse TranslateResponse(string result)
    {
        ApiResponse response = JsonUtility.FromJson<ApiResponse>(result);
        if (response == null) throw new ArgumentNullException("Api Response is null");
        response.Print();
        return response;
    }
    //https://www.youtube.com/watch?v=UUQydC0IimI
    //you can make start function IEnumerator
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
            print("$$$$$$$$$$$$$$$$$$$$ Json sting In Co-routine " + result);
            callback(result);
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