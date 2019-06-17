using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System;
using TMPro;
using Object = System.Object;

public class FacebookHandler : MonoBehaviour
{
    private CharacterManager _characterManager;
    private static FacebookHandler _facebook;
    private int _counter =0 ;

    private string _firstName;
    private string _lastName;
    private string _fBid;
    private string _email;
    private Sprite _profilePicture;
    private List<string> _friendList = new List<string>();

    internal bool LoggedIn ;

    void Start()
    {
        Debug.Log("***FB*** Start!");
        _facebook = FacebookHandler.Instance();
        _characterManager = CharacterManager.Instance();
        if (!FB.IsInitialized)
            FB.Init(SetInit, OnHideUnity);
        else
            FB.ActivateApp();
    }

    private void SetInit()
    {
        if (FB.IsInitialized)
            FB.ActivateApp();
        else
            Debug.Log("Fb couldn't Initialized");
        CheckLoginStatus();
    }
    private void OnHideUnity(bool isGameShown)
    {
        if (isGameShown)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
    private void CheckLoginStatus()
    {
        if (_counter >3)
        {
            Debug.Log("Fb Log in Failed");
            return;
        }
        if (FB.IsLoggedIn)
        {
            Debug.Log("Fb is Logged in");
            LoggedIn = true;
            FetchFbProfile();
            //FacebookGetFriends();
            //Debug.Log("FriendList.count = "+ FriendList.Count);
            //PrintFriends();
        }
        else
        {
            Debug.Log("Fb is not Logged in or user cancelled it");
        }
    }
    #region Login / LogOut
    public void FacebookLogin()
    {
        if (!FB.IsInitialized)
        {
            Debug.Log("Fb is not Initialized");
            return;
        }
        if (FB.IsLoggedIn)
        {
            Debug.Log("Fb is already Logged in");
            return;
        }
        _counter++;
        List<string> permissions = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }
    public void FacebookLogout()
    {
        if (FB.IsLoggedIn)
            FB.LogOut();
        else
            Debug.Log("Fb is already Logged out");
        _characterManager.SetFacebookLoggedIn(false);
    }
    private void AuthCallBack(ILoginResult result)
    {
        if (result.Error != null)
            Debug.Log(result.Error);
        else
            CheckLoginStatus();
    }
    #endregion
    #region Friends/Sharing
    public void FacebookShare()
    {
        FB.ShareLink(
            new System.Uri("Http://goofle.com"),
            "Check This Out!",
            "Some descriptions",
            new System.Uri("Logo.pnj")
        );
    }
    public void FacebookGameRequest()
    {
        FB.AppRequest("Hey! come and play this awesome Game!", title: "Hello World");
    }
    public void FacebookInvite()
    {
        FB.Mobile.AppInvite(new System.Uri("Appstore link "));
    }
    private void FacebookGetFriends()
    {
        FB.API("/me/friends", HttpMethod.GET,
            result =>
            {
                var dictionary = (Dictionary<string, object>) Facebook.MiniJSON.Json.Deserialize(result.RawResult);
                var friendList = (List<object>) dictionary["data"];
                Debug.Log("friendList. count = "+ friendList.Count);
                foreach (var friend in friendList)
                    _friendList.Add(((Dictionary<string, object>) friend) ["name"].ToString());
            });
    }
    #endregion
    private void FetchFbProfile()
    {
        FB.API("/me?fields=first_name,last_name,email", HttpMethod.GET, FetchProfileCallback, new Dictionary<string, string>() { });
        FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, SetProfilePic);
    }
    private void FetchProfileCallback(IGraphResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
            return;
        }
        //Debug.Log(result.RawResult);
        var fBDictionary = (Dictionary<string, object>)result.ResultDictionary;
        //Shows Acquired Permission
        foreach (string perm in AccessToken.CurrentAccessToken.Permissions)
        {
            //Debug.Log(perm);
            switch (perm)
            {
                case "public_profile":
                    //Debug.Log("Profile: first name: " + FBUserDetails["first_name"]);
                    _firstName = fBDictionary["first_name"].ToString();
                    //Debug.Log("Profile: last name: " + FBUserDetails["last_name"]);
                    _lastName = fBDictionary["first_name"].ToString();
                    //Debug.Log("Profile: id: " + FBUserDetails["id"]);
                    _fBid = fBDictionary["id"].ToString();
                    _characterManager.SetFacebookLoggedIn(true, _fBid);
                    break;
                case "user_friends":
                    break;
                case "email":
                    //Debug.Log("Profile: email: " + FBUserDetails["email"]);
                    _email = fBDictionary["email"].ToString();
                    break;
            }
        }
    }
    private void SetProfilePic(IGraphResult result)
    {
        if (result.Error != null)
            Debug.Log(result.Error);
        else
            _profilePicture = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
    }
    public string GetFirstName()
    {
        return _firstName;
    }
    public Sprite GetProfilePic()
    {
        return _profilePicture;
    }
    public List<string> GetFriends()
    {
        return _friendList;
    }
    public void PrintFriends()
    {
        foreach (var friend in _friendList)
            Debug.Log(friend);
    }
    public static FacebookHandler Instance()
    {
        if (!_facebook)
        {
            _facebook = FindObjectOfType(typeof(FacebookHandler)) as FacebookHandler;
            if (!_facebook)
                Debug.LogWarning("#### There needs to be one active FacebookHandler script on a GameObject in your scene.");
        }
        return _facebook;
    }
}