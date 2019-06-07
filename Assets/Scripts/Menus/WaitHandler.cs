using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitHandler : MonoBehaviour {

    private CharacterManager _characterManager;
    private MessagePanelHandler _messagePanelHandler;

    private Button _startButton;
    private TextMeshProUGUI _startText;
    private GameObject _buyButton;
    private TextMeshProUGUI _buyText;
    private GameObject _storeButton;

    // Use this for initialization
    void Start () {
        _characterManager = CharacterManager.Instance();
        _messagePanelHandler = MessagePanelHandler.Instance();

        _startButton = GameObject.Find("ButtonStart").GetComponent<Button>();
        _startButton.interactable = false;
        _startText = GameObject.Find("ButtonStart").GetComponentInChildren<TextMeshProUGUI>();

        _buyButton = GameObject.Find("ButtonBuyStart");
        _buyText = _buyButton.GetComponentInChildren<TextMeshProUGUI>();

        _storeButton = GameObject.Find("ButtonStore");
        _storeButton.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
	    if (_characterManager.UserPlayer.GetLockUntil() > DateTime.Now)
	    {
	        var timer = _characterManager.UserPlayer.GetLockUntil() - DateTime.Now;

            _startText.text = TimeHandler.PrintTime(timer); 
	        _buyText.text = "Use " + TimeHandler.GemTimeValue(timer)+" Gem(s)";
        }
	    else
	    {
	        _startText.text = "Now";
            _startButton.interactable = true;
	        _buyButton.SetActive(false);
        }
    }

    public void BuyTime()
    {
        _messagePanelHandler.ShowMessage("Are you sure you want to buy out your wait time " + _buyText.text + " gem(s)?", 
                MessagePanel.PanelType.YesNo, 
                () => { SpendGem();
        });
    }

    private void SpendGem()
    {
        var timer = _characterManager.UserPlayer.GetLockUntil() - DateTime.Now;
        var gem = TimeHandler.GemTimeValue(timer);
        print("Process GEM " + gem   + " of  " + _characterManager.UserPlayer.Gem);
        if (_characterManager.UserPlayer.Gem > gem)
            _characterManager.AddCharacterSetting("Gem", -gem);
        else
        {
            _messagePanelHandler.ShowMessage("You don't have enough Gem ! ", MessagePanel.PanelType.Ok);
            _storeButton.SetActive(true);
            _buyButton.SetActive(false);
            return;
        }
        _characterManager.SetLockTill();
    }

    public void GoToStoreScene()
    {
        //Preparing to go to Store
        GameObject go = new GameObject();
        //Make go unDestroyable
        GameObject.DontDestroyOnLoad(go);
        var starter = go.AddComponent<SceneStarter>();
        starter.Content = "Wait";
        go.name = "Wait StoreSceneStarter";
        //switch the scene
        SceneManager.LoadScene(SceneSettings.SceneIdForStore);
    }

    public void GoToGame()
    {
        //switch the scene
        SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }


}
