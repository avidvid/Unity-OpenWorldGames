using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameHandler : MonoBehaviour {

    private CharacterManager _characterManager;
    private MessagePanelHandler _messagePanelHandler;
    private UserPlayer _userPlayer;
    private int _selection;
    private Character _selectedCharacter;
    private TMP_InputField _name;
    private TMP_InputField _desc;

    private GameObject _menuCharacter;
    private GameObject _menuUser;


    private Image _characterImage;
    private Button _right;
    private Button _left;
    private List<Character> _characters ;


    void Awake()
    {
        _characterManager = CharacterManager.Instance();
        _messagePanelHandler = MessagePanelHandler.Instance();
        _menuCharacter = GameObject.Find("MenuCharacter");
        _menuUser = GameObject.Find("MenuUser");
    }

    void Start () {
        if (_characterManager.UserPlayer==null)
        {
            _menuCharacter.SetActive(false);
            _menuUser.SetActive(true);
            _desc = GameObject.Find("InputFieldDesc").GetComponent<TMP_InputField>();
            if (_desc == null)
                throw new Exception("_desc is null");
            _name = GameObject.Find("InputField").GetComponent<TMP_InputField>();
            if (_name == null)
                throw new Exception("_name is null");
        } else if (_characterManager.CharacterSetting.Id == 0 || !_characterManager.CharacterSetting.Alive )
            ActivateCharacterMenu();
        else
            SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }

    private void ActivateCharacterMenu()
    {
        _menuCharacter.SetActive(true);
        _menuUser.SetActive(false);

        _characterImage = GameObject.Find("SelectingCharacter").GetComponent<Image>();
        _right = GameObject.Find("RightButton").GetComponent<Button>();
        _left = GameObject.Find("LeftButton").GetComponent<Button>();
        _characters = _characterManager.MyCharacters;
        if (_characters.Count == 0)
            throw new Exception("Character count is ZERO");
        _name = GameObject.Find("InputField").GetComponent<TMP_InputField>();
        if (_name == null)
            throw new Exception("_name is null");
        //Set the default Character sprite 
        _selection = 0;
        SetCharacter();
        DisableNavigators();
    }

    public void NavigateRight()
    {
        _selection++;
        SetCharacter();
        DisableNavigators();
    }
    public void NavigateLeft()
    {
        _selection--;
        SetCharacter();
        DisableNavigators();
    }
    private void SetCharacter()
    {
        _selectedCharacter = _characters[_selection];
        _characterImage.sprite = _selectedCharacter.GetSprite();
    }
    private void DisableNavigators()
    {
        _left.interactable = _selection != 0;
        _right.interactable = _selection + 1 != _characters.Count;
    }

    public void StartTheGame()
    {
        if (!ValidateText(_name.text))
            return;
        Debug.Log("Creating a New CharacterSetting");
        _characterManager.CharacterSetting = 
                new CharacterSetting(_characterManager.UserPlayer.Id,
                    _selectedCharacter.Id, 
                    _name.text);
        _characterManager.CharacterSetting.Life = 100;
        _characterManager.SetMyCharacter(_characterManager.CharacterSetting.CharacterId);
        _characterManager.LevelCalculations();
        SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }

    public void CreateUser()
    {
        if (!ValidateText(_name.text))
            return;
        if (!ValidateText(_desc.text,100))
            return;
        Debug.Log("Create New UserPlayer");
        _characterManager.UserPlayer = new UserPlayer(_name.text, _desc.text);
        _characterManager.SaveUserPlayer();
        SceneManager.LoadScene(SceneSettings.SceneIdForStart );
    }

    private bool ValidateText(string str,int maxLength = 12)
    {
        if (str.Length < 3)
        {
            _messagePanelHandler.ShowMessage(str + " is a bit too short !!!  ", MessagePanel.PanelType.Ok);
            return false;
        }
        if (str.Length > maxLength)
        {
            _messagePanelHandler.ShowMessage(str + " is a bit too Long (less than "+ maxLength + ") !!!  ", MessagePanel.PanelType.Ok);
            return false;
        }
        return true;
    }
}