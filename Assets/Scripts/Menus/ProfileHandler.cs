using System.Collections;
using System.Linq;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
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
    private GameObject _profileStats;
    private TextMeshProUGUI _characterName;



    //private bool _showStats = false;
    //private bool _showTooltip = false;
    //private string _tooltip = "";

    // Use this for initialization
    void Awake()
    {
        _characterManager = CharacterManager.Instance();
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
        if (_profileStats == null)
            _profileStats = GameObject.Find("ProfileStats");
        if (_characterName == null)
            _characterName = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        _settings = _characterManager.CharacterSetting;
        _player = _characterManager.UserPlayer;
        _character = _characterManager.MyCharacter;
        if (_characterManager.UserPlayer.FBLoggedIn)
            _facebook = FacebookHandler.Instance();

        

        //Set big Items in the profile 
        _health.UpdateValues(_settings.Health, _settings.MaxHealth);
        _mana.UpdateValues(_settings.Mana, _settings.MaxMana);
        _energy.UpdateValues(_settings.Energy, _settings.MaxEnergy);
        _experience.UpdateValues(_settings.Experience, _settings.MaxExperience, _settings.Level);

        _coin.UpdateValue(_settings.Coin);
        _gem.UpdateValue(_player.Gem);

        _characterPic.image.sprite = _characterManager.MyCharacter.GetSprite();
        StartCoroutine("LoadProfilePicture");

        int i = 0;

        _characterName.text = _settings.Name ;
        var profileTexts = _profileStats.GetComponentsInChildren<TextMeshProUGUI>();
        profileTexts[i++].text = "Attack: " + 
                                 _character.AttackR + " (A " + 
                                 _settings.AbilityAttack + " /M " +
                                 _settings.MagicAttack + " /P " +
                                 _settings.PoisonAttack + ")" ;
        profileTexts[i++].text = "Defense: " + 
                                 _character.DefenseR + " (A " +
                                 _settings.AbilityDefense + " /M " +
                                 _settings.MagicDefense + " /P " +
                                 _settings.PoisonDefense + ")" ;

        //Row1
        profileTexts[i++].text = "Attack Speed: " + " (" +
                                 _settings.SpeedAttack + ") ";
        profileTexts[i++].text = "Defense Speed: " + " (" +
                                 _settings.SpeedDefense + ") ";

        //Row2
        profileTexts[i++].text = "Move: " +
                                 _character.Move + " (" +
                                 _settings.Speed + ") ";
        profileTexts[i++].text = "Carry: " +
                                 _character.Carry + " (" +
                                 _settings.CarryCnt + ") ";

        //Row3-6
        string[] settingStats = new string[] {
            "Intellect","Agility",
            "Strength","Stamina",
            "Crafting","Researching",
            "Bravery", "Charming"
             };
        foreach (var stat in settingStats)
            profileTexts[i++].text = stat + " : "+ GetPropertyValue(_settings, stat) +"    ";
        //Row7
        profileTexts[i++].text = "Clan: " +
                                 (_player.ClanId==-1?"Solo": _player.ClanId.ToString());
        profileTexts[i++].text = "Rank: " +
                                 _player.ClanRank;
        //Last Login
        profileTexts[i++].text = "Last Login: " +
                                 (_player.LastLogin > DateTime.Now.AddMinutes(-30) ? "Now" : _player.LastLogin.ToString());
    }

    private IEnumerator LoadProfilePicture()
    {
        if (_facebook != null )
            if (FB.IsLoggedIn)
                _profilePic.image.sprite = _facebook.GetProfilePic();
        yield return "Done";
    }

    private object GetPropertyValue(object obj, string propertyName)
    {
        return obj.GetType().GetProperties()
            .Single(pi => pi.Name == propertyName)
            .GetValue(obj, null);
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

    public void GoToCharacterScene()
    {
        SceneManager.LoadScene(SceneSettings.SceneIdForCharacterScene);
    }

    private void ReBuildStarter(string content = null)
    {
        var starter = GameObject.FindObjectOfType<SceneStarter>();
        if (starter != null)
            starter.Content = content;
    }
}
