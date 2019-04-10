using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ApiGatewayConfig : MonoBehaviour
{
    //Database
    private ItemDatabase _itemDatabase;
    private UserDatabase _userDatabase;
    private CharacterDatabase _characterDatabase;
    private TerrainDatabase _terrainDatabase;

    private const string ApiKey = "4OguhEhkYy3dFBRw7oaIK6Q5WEzVZLGa23DXeJet";
    private const string ApiPath = "https://c34irxibui.execute-api.us-west-2.amazonaws.com/";
    private const string ApiStage = "default/";
    private string _random ;

    private int _loading = 0;
    private const int Targets = 1;

    // Start is called before the first frame update
    void Start()
    {
        _itemDatabase = ItemDatabase.Instance();
        _userDatabase =UserDatabase.Instance();
        _characterDatabase=CharacterDatabase.Instance();
        _terrainDatabase=TerrainDatabase.Instance();
        //Todo: get user id  from the device info 
        string apiGate = "";
        string uri="";
        //Call Random
        apiGate = "TestRandomNumber";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?min={0}&max={1}", "999", "10000");
        StartCoroutine(GetRequest(uri, ReadRandomJson));
        //Call Offer
        apiGate = "TestRandomNumber";
        uri = String.Format(ApiPath + ApiStage + apiGate + "?min={0}&max={1}", "999", "10000");
        StartCoroutine(GetRequest(uri, ReadOffersJson));
    }
    // Update is called once per frame
    void Update()
    {
        if (_loading>=100)
            SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }
    private void LoadingThumbsUp()
    {
        _loading += (int)Mathf.Ceil(100 / (float)Targets);
    }
    private void ReadRandomJson(string result)
    {
        var response = TranslateResponse(result);
    }

    #region PublicRequests
    //ItemDatabase
    private void ReadItemsJson(string result)
    {
        List<ItemContainer> items = _itemDatabase.GetItems();
        var response = TranslateResponse(result);
        if (response.Body.Items.Count != items.Count)
        {
            print("Updating Items");
            //_itemDatabase.UpdateItems(response.Body.Items);
        }
        LoadingThumbsUp();
    }
    private void ReadRecipesJson(string result)
    {
        List<Recipe> recipes = _itemDatabase.GetRecipes();
        var response = TranslateResponse(result);
        if (response.Body.Recipes.Count != recipes.Count)
        {
            print("Updating Recipes");
            //_itemDatabase.UpdateRecipes(response.Body.Recipes);
        }
        LoadingThumbsUp();
    }
    private void ReadOffersJson(string result)
    {
        List<Offer> offers = _itemDatabase.GetOffers();
        var response = TranslateResponse(result);
        if (response.Body.Offers.Count != offers.Count)
        {
            print("Updating Offers");
            //_itemDatabase.UpdateOffers(response.Body.Offers);
        }
        LoadingThumbsUp();
    }
    //CharacterDatabase
    private void ReadCharactersJson(string result)
    {
        List<Character> characters = _characterDatabase.GetCharacters();
        var response = TranslateResponse(result);
        if (response.Body.Characters.Count != characters.Count)
        {
            print("Updating Characters");
            //_characterDatabase.UpdateCharacters(response.Body.Characters);
        }
        LoadingThumbsUp();
    }
    private void ReadResearchesJson(string result)
    {
        List<Research> researches = _characterDatabase.GetResearches();
        var response = TranslateResponse(result);
        if (response.Body.Researches.Count != researches.Count)
        {
            print("Updating Researches");
            //_characterDatabase.UpdateResearches(response.Body.Researches);
        }
        LoadingThumbsUp();
    }
    //TerrainDatabase
    private void ReadRegionsJson(string result)
    {
        List<Region> regions = _terrainDatabase.GetRegions();
        var response = TranslateResponse(result);
        if (response.Body.Regions.Count != regions.Count)
        {
            print("Updating Regions");
            //_terrainDatabase.UpdateRegions(response.Body.Regions);
        }
        LoadingThumbsUp();
    }
    private void ReadTerrainsJson(string result)
    {
        List<TerrainIns> terrains = _terrainDatabase.GetTerrains();
        var response = TranslateResponse(result);
        if (response.Body.Terrains.Count != terrains.Count)
        {
            print("Updating Terrains");
            //_terrainDatabase.UpdateTerrains(response.Body.Terrains);
        }
        LoadingThumbsUp();
    }
    private void ReadElementsJson(string result)
    {
        List<ElementIns> elements = _terrainDatabase.GetElements();
        var response = TranslateResponse(result);
        if (response.Body.Elements.Count != elements.Count)
        {
            print("Updating Elements");
            //_terrainDatabase.UpdateElements(response.Body.Elements);
        }
        LoadingThumbsUp();
    }
    private void ReadInsideStoriesJson(string result)
    {
        List<InsideStory> insideStories = _terrainDatabase.GetInsideStories();
        var response = TranslateResponse(result);
        if (response.Body.InsideStories.Count != insideStories.Count)
        {
            print("Updating InsideStories");
            //_terrainDatabase.UpdateInsideStories(response.Body.InsideStories);
        }
        LoadingThumbsUp();
    }
    #endregion
    #region UserRequests
    //public CharacterMixture CharacterMixture;
    //public CharacterResearching ;
    private void ReadUserPlayerJson(string result)
    {
        UserPlayer userPlayer = _userDatabase.GetUserPlayer();
        var response = TranslateResponse(result);
        if (response.Body.UserPlayer != userPlayer)
        {
            print("Updating UserPlayer");
            _userDatabase.UpdateUserPlayer(response.Body.UserPlayer);
        }
        LoadingThumbsUp();
    }
    private void ReadCharacterSettingJson(string result)
    {
        CharacterSetting characterSetting = _userDatabase.GetCharacterSetting();
        var response = TranslateResponse(result);
        if (response.Body.CharacterSetting != characterSetting)
        {
            print("Updating CharacterSetting");
            _userDatabase.UpdateCharacterSetting(response.Body.CharacterSetting);
        }
        LoadingThumbsUp();
    }
    private void ReadCharacterMixtureJson(string result)
    {
        CharacterMixture characterMixture = _userDatabase.GetCharacterMixture();
        var response = TranslateResponse(result);
        if (response.Body.CharacterMixture != characterMixture)
        {
            print("Updating CharacterMixture");
            _userDatabase.UpdateCharacterMixture(response.Body.CharacterMixture);
        }
        LoadingThumbsUp();
    }
    private void ReadCharacterResearchingJson(string result)
    {
        CharacterResearching characterResearching = _userDatabase.GetCharacterResearching();
        var response = TranslateResponse(result);
        if (response.Body.CharacterResearching != characterResearching)
        {
            print("Updating CharacterResearching");
            _userDatabase.UpdateCharacterResearching(response.Body.CharacterResearching);
        }
        LoadingThumbsUp();
    }
    private void ReadUserInventoryJson(string result)
    {
        List<UserItem> items = _userDatabase.GetUserInventory();
        var response = TranslateResponse(result);
        if (response.Body.UserInventory.Count != items.Count)
        {
            print("Updating UserInventory");
            _userDatabase.UpdateUserInventory(response.Body.UserInventory);
        }
        LoadingThumbsUp();
    }
    private void ReadUserCharactersJson(string result)
    {
        List<UserCharacter> userCharacters = _userDatabase.GetUserCharacters();
        var response = TranslateResponse(result);
        if (response.Body.UserCharacters.Count != userCharacters.Count)
        {
            print("Updating UserCharacters");
            _userDatabase.UpdateUserCharacters(response.Body.UserCharacters);
        }
        LoadingThumbsUp();
    }
    private void ReadCharacterResearchesJson(string result)
    {
        List<UserResearch> researches = _userDatabase.GetCharacterResearches();
        var response = TranslateResponse(result);
        if (response.Body.CharacterResearches.Count != researches.Count)
        {
            print("Updating CharacterResearches");
            _userDatabase.UpdateCharacterResearches(response.Body.CharacterResearches);
        }
        LoadingThumbsUp();
    }
    //public List<UserRecipe> UserRecipes;
    private void ReadUserRecipesJson(string result)
    {
        List<UserRecipe> recipes = _userDatabase.GetUserRecipes();
        var response = TranslateResponse(result);
        if (response.Body.UserRecipes.Count != recipes.Count)
        {
            print("Updating UserRecipes");
            _userDatabase.UpdateUserRecipes(response.Body.UserRecipes);
        }
        LoadingThumbsUp();
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
}