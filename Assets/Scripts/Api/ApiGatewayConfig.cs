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

        string ApiGate;
        string uri;
        //Call Random
        ApiGate = "TestRandomNumber";
        uri = String.Format(ApiPath + ApiStage + ApiGate + "?min={0}&max={1}", "999", "10000");
        StartCoroutine(GetRequest(uri, ReadRandomJson));
        //Call Offer
        ApiGate = "TestRandomNumber";
        uri = String.Format(ApiPath + ApiStage + ApiGate + "?min={0}&max={1}", "999", "10000");
        StartCoroutine(GetRequest(uri, ReadOffersJson));
    }

    // Update is called once per frame
    void Update()
    {
        if (_loading>=100)
            SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }
    private void ReadRandomJson(string result)
    {
        var response = TranslateResponse(result);
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

    private void LoadingThumbsUp()
    {
        _loading += (int) Mathf.Ceil(100/ (float)Targets);
    }

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
}