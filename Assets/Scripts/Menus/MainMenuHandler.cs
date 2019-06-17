using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour {

    private FacebookHandler _facebook;
    private CharacterManager _characterManager;
    private AudioManager _audioManager;
    private GameObject _menuSocial;
    private GameObject _loggedIn;
    private GameObject _loggedOut;
    private TextMeshProUGUI _username;
    private Image _profilePicture;
    private Slider _sound;
    private TMP_InputField _desc;
    private string _menuSelector ="Main";

    void Start()
    {
        var starter = GameObject.FindObjectOfType<SceneStarter>();
        if (starter != null)
            _menuSelector = starter.Content;

        _facebook = FacebookHandler.Instance();
        _loggedIn = GameObject.Find("FBLoggedIn");
        _loggedOut = GameObject.Find("FBLoggedOut");
        _menuSocial = GameObject.Find("MenuSocial");
        _characterManager = CharacterManager.Instance();
        _audioManager = AudioManager.Instance();

        _desc = GameObject.Find("InputFieldDesc").GetComponent<TMP_InputField>();
        _desc.text = _characterManager.UserPlayer.Description;
        _sound = GameObject.Find("SoundSlider").GetComponent<Slider>();
        _sound.value = _characterManager.UserPlayer.SoundVolume;

        switch (_menuSelector)
        {
            case "Option":
                GameObject.Find("MenuMain").SetActive(false);
                GameObject.Find("MenuOption").SetActive(true);
                _menuSocial.SetActive(false);
                break;
            case "Social":
                GameObject.Find("MenuMain").SetActive(false);
                GameObject.Find("MenuOption").SetActive(false);
                _menuSocial.SetActive(true);
                break;
            default:
                GameObject.Find("MenuMain").SetActive(true);
                GameObject.Find("MenuOption").SetActive(false);
                _menuSocial.SetActive(false);
                break;
        }

        _username = GameObject.Find("Username").GetComponent<TextMeshProUGUI>();
        _profilePicture = GameObject.Find("ProfilePicture").GetComponent<Image>();
        _loggedIn.SetActive(false);
        _loggedOut.SetActive(false);
    }
    void Update()
    {
        if (_menuSocial.activeSelf)
            DealWithFBMenus(FB.IsLoggedIn);
    }
    private void DealWithFBMenus(bool isLoggedIn)
    {
        _menuSelector = "Social";
        if (isLoggedIn)
        {
            _loggedOut.SetActive(false);
            _loggedIn.SetActive(true);
            _username.text = "Hi there, " + _facebook.GetFirstName();
            _profilePicture.sprite = _facebook.GetProfilePic();
        }
        else
        {
            _loggedIn.SetActive(false);
            _loggedOut.SetActive(true);
        }
    }

    public void AdjustVolume()
    {
        if (_audioManager != null)
            _audioManager.UpdateSoundVolume(_sound.value);
    }
    public void SaveSetting()
    {
        _characterManager.UserPlayer.Description = _desc.text;
        _characterManager.UserPlayer.SoundVolume = _sound.value;
        _characterManager.SaveUserPlayer();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }
    public void ExitGame()
    {
        Debug.Log("Exit!!");
        //todo: Application.Quit();
        SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }
    public void FacebookLogin()
    {
        _facebook.FacebookLogin();
    }
    public void FacebookLogout()
    {
        _facebook.FacebookLogout();
    }
}