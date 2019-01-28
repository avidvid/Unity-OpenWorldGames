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

    public string FirstName;
    public string LastName;
    public string FBid;
    public string Email;
    public Sprite ProfilePicture;
    public List<string> FriendList = new List<string>();

    public bool LoggedIn ;

    void Awake()
    {
        _facebook = FacebookHandler.Instance();
        _characterManager = CharacterManager.Instance();
        if (!FB.IsInitialized)
            FB.Init(SetInit, OnHideUnity);
        else
            FB.ActivateApp();
        GameObject.DontDestroyOnLoad(this);

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
            FetchFBProfile();
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
                    FriendList.Add(((Dictionary<string, object>) friend) ["name"].ToString());
            });
    }
    #endregion

    private void FetchFBProfile()
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

        var FBDictionary = (Dictionary<string, object>)result.ResultDictionary;
        //Shows Acquired Permission
        foreach (string perm in AccessToken.CurrentAccessToken.Permissions)
        {
            //Debug.Log(perm);
            switch (perm)
            {
                case "public_profile":
                    //Debug.Log("Profile: first name: " + FBUserDetails["first_name"]);
                    FirstName = FBDictionary["first_name"].ToString();
                    //Debug.Log("Profile: last name: " + FBUserDetails["last_name"]);
                    LastName = FBDictionary["first_name"].ToString();
                    //Debug.Log("Profile: id: " + FBUserDetails["id"]);
                    FBid = FBDictionary["id"].ToString();
                    _characterManager.SetFacebookLoggedIn(true, FBid);
                    break;
                case "user_friends":
                    break;
                case "email":
                    //Debug.Log("Profile: email: " + FBUserDetails["email"]);
                    Email = FBDictionary["email"].ToString();
                    break;

            }
        }
    }

    private void SetProfilePic(IGraphResult result)
    {
        if (result.Error != null)
            Debug.Log(result.Error);
        else
            ProfilePicture = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());
    }

    public string GetFirstName()
    {
        return FirstName;
    }
    public Sprite GetProfilePic()
    {
        return ProfilePicture;
    }
    public List<string> GetFriends()
    {
        return FriendList;
    }
    public void PrintFriends()
    {
        foreach (var friend in FriendList)
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
