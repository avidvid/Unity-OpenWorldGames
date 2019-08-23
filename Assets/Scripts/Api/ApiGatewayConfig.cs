using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;

public class ApiGatewayConfig : MonoBehaviour
{
    private static ApiGatewayConfig _apiGatewayConfig;
    private XmlHelper _xmlHelper;
    //Database
    private ItemDatabase _itemDatabase;
    private UserDatabase _userDatabase;
    private CharacterDatabase _characterDatabase;
    private TerrainDatabase _terrainDatabase;

    private GameLoadHelper _gameLoadHelper;

    private const string ApiKey = "AOlOnm2C4394k8QZHqkLl8xYcCEWRSND5WtAclWq";
    private const string ApiPath = "https://h28ve9pjh5.execute-api.us-west-2.amazonaws.com/";
    private const string ApiStage = "prod/";
    private string _apiGate = "";
    private string _uri =  "";
    private int _userId;

    private List<UserItem> _oldUserInventory =new List<UserItem>();
    private int _firstWaveTarget;
    private int _secondWaveTarget;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("***API*** Start!");
        _xmlHelper = XmlHelper.Instance();
        _itemDatabase = ItemDatabase.Instance();
        _userDatabase =UserDatabase.Instance();
        _characterDatabase=CharacterDatabase.Instance();
        _terrainDatabase=TerrainDatabase.Instance();
        _gameLoadHelper = GameObject.Find("GameStarter").GetComponent<GameLoadHelper>();
        //Call UserPlayer
        _apiGate = "GetUserPlayer";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", DeviceHandler.FetchMacId());
        StartCoroutine(GetRequest(_uri, ReadUserPlayerJson));

    }
    #region ReadDB
    private void FirstWave()
    {
        //###Item Database
        //Call Items
        //_apiGate = "GetItems";
        //_uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        //StartCoroutine(GetRequest(_uri, ReadItemsJson));
        ReadItemsXml();
        //Call Recipe
        _apiGate = "GetRecipes";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadRecipesJson));
        //Call Offer
        _apiGate = "GetOffers";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadOffersJson));
        //###Character Database
        //Call Characters
        _apiGate = "GetCharacters";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadCharactersJson));
        //Call Researches
        _apiGate = "GetResearches";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadResearchesJson));
        //###Terrain Database
        //Call Regions
        _apiGate = "GetRegions";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadRegionsJson));
        //Call Terrains
        _apiGate = "GetTerrains";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadTerrainsJson));
        //Call Elements
        _apiGate = "GetElements";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadElementsJson));
        //Call InsideStories
        _apiGate = "GetInsideStories";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadInsideStoriesJson));
        //Call UserPlayerSecure
        _apiGate = "GetUserPlayerSecure";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadUserPlayerSecureJson));
    }



    IEnumerator SecondWave()
    {
        yield return new WaitUntil(() => _firstWaveTarget >= 10);        
        //###User Database
        //Call CharacterMixture
        _apiGate = "GetCharacterMixture";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadCharacterMixtureJson));
        //Call CharacterResearching
        _apiGate = "GetCharacterResearching";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadCharacterResearchingJson));
        //Call UserInventory
        _apiGate = "GetUserInventory";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadUserInventoryJson));
        //Call CharacterResearches
        _apiGate = "GetCharacterResearches";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadCharacterResearchesJson));
        //Call UserRecipes
        _apiGate = "GetUserRecipes";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadUserRecipesJson));
        //Call UserCharacters
        _apiGate = "GetUserCharacters";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadUserCharactersJson));
        //Call MailMessages
        _apiGate = "GetMailMessage";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadMailMessagesJson));
    }
    IEnumerator ThirdWave()
    {
        yield return new WaitUntil(() => _secondWaveTarget >= 7);
        //Call CharacterSetting
        _apiGate = "GetCharacterSetting";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", _userId.ToString());
        StartCoroutine(GetRequest(_uri, ReadCharacterSettingJson));
    }
    //ItemDatabase
    private void ReadItemsJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.Items.Count == 0)
            throw new Exception("API-Items Failed!!!");
        _itemDatabase.UpdateItems(response.Body.Items);
        _xmlHelper.SaveItems(response.Body.Items.OrderBy(o => o.Id).ToList());
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    private void ReadItemsXml()
    {
        _itemDatabase.UpdateItems(_xmlHelper.GetItems());
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
    private void ReadUserPlayerSecureJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserPlayers.Count == 0)
            throw new Exception("API-UserPlayerSecure Failed!!!");
        _userDatabase.UpdateAllUserPlayers(response.Body.UserPlayers);
        _gameLoadHelper.LoadingThumbsUp();
        _firstWaveTarget++;
    }
    private void ReadCharacterMixtureJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.CharacterMixture==null)
            Debug.LogWarning("####API-CharacterMixture Is Empty!!!");
        _userDatabase.UpdateCharacterMixture(response.Body.CharacterMixture);
        _gameLoadHelper.LoadingThumbsUp();
        _secondWaveTarget++;
    }
    private void ReadCharacterResearchingJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.CharacterResearching == null)
            Debug.LogWarning("####API-CharacterResearching Is Empty!!!");
        _userDatabase.UpdateCharacterResearching(response.Body.CharacterResearching);
        _gameLoadHelper.LoadingThumbsUp();
        _secondWaveTarget++;
    }
    private void ReadUserInventoryJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserInventory.Count == 0)
            Debug.LogWarning("####API-UserInventory Is Empty!!!");
        SavedUserInventory(response.Body.UserInventory);
        _userDatabase.UpdateUserInventory(response.Body.UserInventory);
        _gameLoadHelper.LoadingThumbsUp();
        _secondWaveTarget++;
    }
    private void ReadUserCharactersJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserCharacters.Count == 0)
            Debug.LogWarning("####API--UserCharacters Is Empty!!!");
        _userDatabase.UpdateUserCharacters(response.Body.UserCharacters);
        _gameLoadHelper.LoadingThumbsUp();
        _secondWaveTarget++;
    }
    private void ReadCharacterResearchesJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.CharacterResearches.Count == 0)
            Debug.LogWarning("####API--CharacterResearches Is Empty!!!");
        _userDatabase.UpdateCharacterResearches(response.Body.CharacterResearches);
        _gameLoadHelper.LoadingThumbsUp();
        _secondWaveTarget++;
    }
    private void ReadUserRecipesJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserRecipes.Count == 0)
            Debug.LogWarning("####API--UserRecipes Is Empty!!!");
        _userDatabase.UpdateUserRecipes(response.Body.UserRecipes);
        _gameLoadHelper.LoadingThumbsUp();
        _secondWaveTarget++;
    }
    private void ReadCharacterSettingJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.CharacterSetting.Id == 0)
            Debug.LogWarning("####API-CharacterSetting Is Empty!!!");
        _userDatabase.UpdateCharacterSetting(response.Body.CharacterSetting);
        if (_userDatabase.StartGameValidation())
            _gameLoadHelper.LoadingThumbsUp();
        //Instantiate Important Game Objects
        print("***###*** Instantiate Important Game Objects ***###***");
        var characterManager = Resources.Load<GameObject>("Prefabs/CharacterManager");
        Instantiate(characterManager);
        var musicBox = Resources.Load<GameObject>("Prefabs/MusicBox");
        Instantiate(musicBox);
        var cache = Resources.Load<GameObject>("Prefabs/Cache");
        Instantiate(cache);
    }
    private void ReadUserPlayerJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.UserPlayer == null)
        {
            Debug.LogWarning("####API-UserPlayer Is Empty!!!");
            _userId = 0;
        }
        else
            _userId = response.Body.UserPlayer.Id;
        _userDatabase.UpdateUserPlayer(response.Body.UserPlayer);
        _gameLoadHelper.LoadingThumbsUp();
        FirstWave();
        StartCoroutine(SecondWave());
        StartCoroutine(ThirdWave());
    }
    private void ReadMailMessagesJson(string result)
    {
        var response = TranslateResponse(result);
        if (response.Body.MailMessages.Count == 0)
            Debug.LogWarning("####API--MailMessages Is Empty!!!");
        _userDatabase.UpdateMailMessages(response.Body.MailMessages);
        _gameLoadHelper.LoadingThumbsUp();
        _secondWaveTarget++;
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
         _apiGate = "GetUserPlayer";
        print("Saving" + userPlayer.MyInfo());
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", userPlayer.Id.ToString());
        if (userPlayer.Latitude == 0 && userPlayer.Longitude == 0)
        {
            var locationHelper = LocationHelper.Instance();
            var loc =locationHelper.GetLocation();
            userPlayer.Latitude = loc.x;
            userPlayer.Longitude = loc.y;
            _terrainDatabase.SetRegion(userPlayer.Latitude, userPlayer.Longitude);
        }
        ApiRequest ap = new ApiRequest
        {
            UserPlayer = userPlayer
        };
        StartCoroutine(PutRequest(_uri, ap));
    }
    internal void SaveCharacterSetting(CharacterSetting characterSetting)
    {
        _apiGate = "GetCharacterSetting";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", characterSetting.Id.ToString());
        ApiRequest ap = new ApiRequest
        {
            CharacterSetting = characterSetting
        };
        StartCoroutine(PutRequest(_uri, ap));
    }
    internal void PutUserInventory(List<UserItem> userInventory)
    {
        //Todo: delete
        Debug.Log("PutUserInventory   _oldUserInventory=" + _oldUserInventory.Count + " userInventory = "+ userInventory.Count);
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
        _apiGate = "GetUserInventory";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", item.UserId.ToString());
        ApiRequest ap = new ApiRequest
        {
            Action = action,
            UserInventory = item
        };
        Debug.Log(ap.Action + " UserInventory : " + ap.UserInventory.MyInfo());
        StartCoroutine(PutRequest(_uri, ap, true));
    }
    internal void PutCharacterMixture(CharacterMixture characterMixture)
    {
        _apiGate = "GetCharacterMixture";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", characterMixture.Id.ToString());
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
        StartCoroutine(PutRequest(_uri, ap));
    }
    internal void PutUserRecipe(UserRecipe userRecipe,string code=null)
    {
        _apiGate = "GetUserRecipes";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", userRecipe.Id.ToString());
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
        StartCoroutine(PutRequest(_uri, ap, true));
    }
    internal void PutUserCharacter(UserCharacter userCharacter, string code = null)
    {
        _apiGate = "GetUserCharacters";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", userCharacter.Id.ToString());
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
        StartCoroutine(PutRequest(_uri, ap, true));
    }
    internal void PutCharacterResearching(CharacterResearching characterResearching)
    {
        _apiGate = "GetCharacterResearching";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", characterResearching.Id.ToString());
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
        StartCoroutine(PutRequest(_uri, ap));
    }
    internal void PutCharacterResearch(CharacterResearch characterResearch)
    {
        _apiGate = "GetCharacterResearches";
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", characterResearch.Id.ToString());
        string action = "Insert";
        if (characterResearch.Level != 1)
            action = "Update";
        ApiRequest ap = new ApiRequest
        {
            Action = action,
            CharacterResearch = characterResearch
        };
        StartCoroutine(PutRequest(_uri, ap));
    }
    internal void PutMailMessage(MailMessage mailMessage)
    {
        //mins is the minutes that the user should stay locked 
        _apiGate = "GetMailMessage";
        print("Saving" + mailMessage.MyInfo());
        _uri = String.Format(ApiPath + ApiStage + _apiGate + "?id={0}", mailMessage.Id.ToString());
        ApiRequest ap = new ApiRequest
        {
            Action = "Insert",
            MailMessage = mailMessage
        };
        StartCoroutine(PutRequest(_uri, ap));
    }
    #endregion
    #region ApiRequest
    private ApiResponse TranslateResponse(string result)
    {
        ApiResponse response = JsonUtility.FromJson<ApiResponse>(result);
        if (response == null) throw new ArgumentNullException("API Response is null");
        return response;
    }
    private IEnumerator GetRequest(string _uri,Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(_uri))
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
    private IEnumerator PutRequest(string _uri, ApiRequest apiRequest, bool refresh=false)
    {
        string json = JsonUtility.ToJson(apiRequest);
        Debug.Log("json: " + json);
        using (UnityWebRequest request = UnityWebRequest.Put(_uri, json))
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
                Debug.LogError("There needs to be one active _apiGatewayConfig script on a GameObject in your scene.");
        }
        return _apiGatewayConfig;
    }
    #endregion
}