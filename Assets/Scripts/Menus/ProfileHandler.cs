using System.Collections;
using System.Linq;
using Facebook.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using System.Security;

public class ProfileHandler : MonoBehaviour {

    private FacebookHandler _facebook;
    private CharacterManager _characterManager;
    private CharacterDatabase _characterDatabase;

    private string _monsterInfo;
    private Vector2 _monsterPosition;
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

    // Use this for initialization
    void Awake()
    {
        _characterManager = CharacterManager.Instance();
        _characterDatabase = CharacterDatabase.Instance();
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
        var starter = GameObject.FindObjectOfType<SceneStarter>();
        if (starter != null)
        {
            //starter.Print();
            if (starter.Content.IndexOf(',') > 0)
            {
                var monsterInfo = starter.Content;
                var monsterPosition = starter.MapPosition;
                LoadMonsterProfile(monsterInfo, monsterPosition);
            }
            else
            {
                if (_characterManager.UserPlayer.FBLoggedIn)
                    _facebook = FacebookHandler.Instance();
                var settings = _characterManager.CharacterSetting;
                var player = _characterManager.UserPlayer;
                var character = _characterManager.MyCharacter;
                LoadUserProfile(player, settings, character);
            }
        }
    }
    private void LoadUserProfile(UserPlayer player,CharacterSetting settings,Character character)
    {
        //Set big Items in the profile 
        _health.UpdateValues(settings.Health, settings.MaxHealth);
        _mana.UpdateValues(settings.Mana, settings.MaxMana);
        _energy.UpdateValues(settings.Energy, settings.MaxEnergy);
        _experience.UpdateValues(settings.Experience, settings.MaxExperience, settings.Level);

        _coin.UpdateValue(settings.Coin);
        _gem.UpdateValue(player.Gem);

        _characterPic.image.sprite = character.GetSprite();
        StartCoroutine("LoadProfilePicture");
        int i = 0;
        _characterName.text = settings.Name;
        var profileTexts = _profileStats.GetComponentsInChildren<TextMeshProUGUI>();
        profileTexts[i++].text = "Attack: " +
                                 character.AttackR + " (A " +
                                 settings.AbilityAttack + " /M " +
                                 settings.MagicAttack + " /P " +
                                 settings.PoisonAttack + ")";
        profileTexts[i++].text = "Defense: " +
                                 character.DefenseR + " (A " +
                                 settings.AbilityDefense + " /M " +
                                 settings.MagicDefense + " /P " +
                                 settings.PoisonDefense + ")";

        //Row1
        profileTexts[i++].text = "Attack Speed: " + " (" +
                                 settings.SpeedAttack + ") ";
        profileTexts[i++].text = "Defense Speed: " + " (" +
                                 settings.SpeedDefense + ") ";

        //Row2
        profileTexts[i++].text = "Move: " +
                                 character.Move + " (" +
                                 settings.Speed + ") ";
        profileTexts[i++].text = "Carry: " +
                                 character.Carry + " (" +
                                 settings.CarryCnt + ") ";

        //Row3-6
        string[] settingStats = new string[] {
            "Intellect","Agility",
            "Strength","Stamina",
            "Crafting","Researching",
            "Bravery", "Charming"
             };
        foreach (var stat in settingStats)
            profileTexts[i++].text = stat + " : " + GetPropertyValue(settings, stat) + "    ";
        //Row7
        profileTexts[i++].text = "Clan: " +
                                 (player.ClanId == -1 ? "Solo" : player.ClanId.ToString());
        profileTexts[i++].text = "Rank: " +
                                 player.ClanRank;
        //Last Login
        profileTexts[i++].text = "Last Login: " +
                                 (player.LastLogin > DateTime.Now.AddMinutes(-30) ? "Now" : player.LastLogin.ToString());
    }

    private void LoadMonsterProfile(string monsterInfo, Vector2 monsterLoc )
    {
        List<int> monsterData = monsterInfo.Split(',').Select(Int32.Parse).ToList();
        //monsterData[0]=CharacterId
        //monsterData[1]=Level
        Character character = _characterDatabase.GetCharacterById(monsterData[0]);
        var monster = _characterManager.GenerateMonster(character, monsterData[1]);
        //Set big Items in the profile 
        _health.UpdateValues(monster.Health, monster.MaxHealth);
        _mana.gameObject.SetActive(false);
        _energy.gameObject.SetActive(false);
        _experience.gameObject.SetActive(false);
        _coin.gameObject.SetActive(false);
        _gem.gameObject.SetActive(false);
        _characterPic.image.sprite = character.GetSprite();
        _profilePic.gameObject.SetActive(false);
        _characterName.text = character.Name + " Level " + monster.Level;
        int i = 0;
        var profileTexts = _profileStats.GetComponentsInChildren<TextMeshProUGUI>();
        profileTexts[i++].text = "Attack: " +
                                 character.AttackR + " (A " +
                                 monster.AbilityAttack + " /M " +
                                 monster.MagicAttack + " /P " +
                                 monster.PoisonAttack + ")";
        profileTexts[i++].text = "Defense: " +
                                 character.DefenseR + " (A " +
                                 monster.AbilityDefense + " /M " +
                                 monster.MagicDefense + " /P " +
                                 monster.PoisonDefense + ")";
        //Row1
        profileTexts[i++].text = "Attack Speed: " + " (" +
                                 monster.SpeedAttack + ") ";
        profileTexts[i++].text = "Defense Speed: " + " (" +
                                 monster.SpeedDefense + ") ";
        //Row2
        profileTexts[i++].text = "Move: " +
                                 character.Move + " (" +
                                 monster.Speed + ") ";
        profileTexts[i++].gameObject.SetActive(false);
        //Row3-6
        string[] settingStats = new string[] {
            "Intellect","Agility",
            "Strength","Stamina",
            "Crafting","Researching",
            "Bravery", "Charming"
             };
        foreach (var stat in settingStats)
            profileTexts[i++].gameObject.SetActive(false);
        //Row7
        profileTexts[i++].gameObject.SetActive(false);
        profileTexts[i++].gameObject.SetActive(false);
        //Last Login
        profileTexts[i++].text = "Location: " + monsterLoc;

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