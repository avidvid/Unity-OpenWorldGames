using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour {

    private CharacterManager _characterManager;

    GameObject _buttonUseLife;
    GameObject _buttonBuyLife;
    CharacterSetting _settings;

    void Awake()
    {
        _characterManager = CharacterManager.Instance();
    }

    // Use this for initialization
    void Start ()
    {
        _settings = _characterManager.CharacterSetting;
        _buttonUseLife = GameObject.Find("ButtonUseLife");
        _buttonBuyLife =GameObject.Find("ButtonBuyLife");
        if (_settings.Life > 0)
            _buttonBuyLife.SetActive(false);
        else
            _buttonUseLife.SetActive(false);
    }
    public void GoToStartScene()
    {
        SceneManager.LoadScene(SceneSettings.SceneIdForStart);
    }
    public void GoToStoreScene()
    {
        //Preparing to go to Store
        GameObject go = new GameObject();
        //Make go unDestroyable
        GameObject.DontDestroyOnLoad(go);
        var starter = go.AddComponent<SceneStarter>();
        starter.Content = "GameOver";
        go.name = "GameOver StoreSceneStarter";
        //switch the scene
        SceneManager.LoadScene(SceneSettings.SceneIdForStore);
    }
    public void UseLife()
    {
        _characterManager.ReviveCharacter();
        //todo: go to base 
        SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }
}