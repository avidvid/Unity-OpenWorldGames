using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using System;

public class ProfileHandler : MonoBehaviour {

    private FacebookHandler _facebook;
    private CharacterManager _characterManager;
    private CharacterSetting _settings;
    private Character _character; 
    private UserPlayer _player;

    private BarHandler _health;
    private BarHandler _mana;
    private BarHandler _energy;
    private BarHandler _experience;
    private ContainerValueHandler _coin;
    private ContainerValueHandler _gem; 
    private Button _profilePic;
    private Button _characterPic;
    private GameObject _panelStats;

    //private bool _showStats = false;
    //private bool _showTooltip = false;
    //private string _tooltip = "";

    // Use this for initialization
    void Awake()
    {
        _characterManager = CharacterManager.Instance();
        _settings = _characterManager.CharacterSetting;
        _player = _characterManager.UserPlayer;
        _character = _characterManager.Character;

        if (_characterManager.UserPlayer.FBLoggedIn)
            _facebook = FacebookHandler.Instance();
    }

    void Start()
    {
        if (_health == null)
            _health = GameObject.FindGameObjectWithTag("Health").GetComponent<BarHandler>();
        if (_mana == null)
            _mana = GameObject.FindGameObjectWithTag("Mana").GetComponent<BarHandler>();
        if (_energy == null)
            _energy = GameObject.FindGameObjectWithTag("Energy").GetComponent<BarHandler>();
        if (_experience == null)
            _experience = GameObject.FindGameObjectWithTag("Experience").GetComponent<BarHandler>();
        if (_coin == null)
            _coin = GameObject.FindGameObjectWithTag("Coin").GetComponent<ContainerValueHandler>();
        if (_gem == null)
            _gem = GameObject.FindGameObjectWithTag("Gem").GetComponent<ContainerValueHandler>();
        if (_profilePic == null)
            _profilePic = GameObject.Find("PlayerPic").GetComponent<Button>();
        if (_characterPic == null)
            _characterPic = GameObject.Find("CharacterPic").GetComponent<Button>();
        if (_panelStats == null)
            _panelStats = GameObject.Find("PanelStats");

        //Set big Items in the profile 
        _health.UpdateValues(_settings.Health, _settings.MaxHealth);
        _mana.UpdateValues(_settings.Mana, _settings.MaxMana);
        _energy.UpdateValues(_settings.Energy, _settings.MaxEnergy);
        _experience.UpdateValues(_settings.Experience, _settings.MaxExperience, _settings.Level);

        _coin.UpdateValue(_settings.Coin);
        _gem.UpdateValue(_settings.Gem);

        _characterPic.image.sprite = _profilePic.image.sprite = _characterManager.Character.GetSprite();
        


        int i = 0;

        var profileTexts = _panelStats.GetComponentsInChildren<TextMeshProUGUI>();
        profileTexts[i++].text = _character.AttackR + " / " + _character.DefenseR;
        profileTexts[i++].text = _character.AttackT + " / " + _character.DefenseT;

        string[] settingActions = new string[] { "Ability", "Magic", "Poison", "Speed" };
        foreach (var act in settingActions)
        {
            profileTexts[i++].text = act +" "+GetPropertyValue(_settings, act+"Attack" ) +" / " + GetPropertyValue(_settings, act + "Defense");
        }

        string[] settingChar = new string[] {
            "Speed","Body",
            "Carry","Move"};
        foreach (var chars in settingChar)
        {
            profileTexts[i++].text = chars + " :";
            profileTexts[i++].text = GetPropertyValue(_character, chars).ToString();
        }

        string[] userStats = new string[] {
            "ClanRank","Rank"
        };
        foreach (var stat in userStats)
        {
            profileTexts[i++].text = stat + " :";
            profileTexts[i++].text = GetPropertyValue(_player, stat).ToString();
        }

        string[] settingStats = new string[] {
            "Speed",
            "Intellect","Agility",
            "Strength","Stamina",
            "Crafting","Researching",
            "Bravery", "Charming",
            "Carry","CarryCnt",
             };
        foreach (var stat in settingStats)
        {
            profileTexts[i++].text = stat + " :";
            profileTexts[i++].text = GetPropertyValue(_settings, stat).ToString();
        }

        LoadProfilePicture();

    }

    private void LoadProfilePicture()
    {
        if (_facebook != null )
            if (FB.IsLoggedIn)
                _profilePic.image.sprite = _facebook.GetProfilePic();
        else
        {
            StartCoroutine("LoadImages");
        }
    }

    public object GetPropertyValue(object obj, string propertyName)
    {
        return obj.GetType().GetProperties()
            .Single(pi => pi.Name == propertyName)
            .GetValue(obj, null);
    }

    private IEnumerator LoadImages()
    {
        var directory = "https://www.gravatar.com/avatar/205e460b479e2e5b48aec07710c08d50?s=200";
        WWW www = new WWW(directory);
        yield return www;
        _profilePic.image.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));

    }

    public void GoToMenuSceneSocial()
    {
        ReBuildStarter("Social");
        //switch the scene
        SceneManager.LoadScene(SceneSettings.SceneIdForMenu);
    }
    

    public void BackToMainScene()
    {
        SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }

    private void ReBuildStarter(string content = null)
    {
        var starter = GameObject.FindObjectOfType<SceneStarter>();
        if (starter != null)
            starter.Content = content;
    }
}
