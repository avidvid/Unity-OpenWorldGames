using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class ResearchListHandler : MonoBehaviour {

    private ModalPanel _modalPanel;
    private GameObject _contentPanel;
    private GameObject _clickedButton;
    private CharacterManager _characterManager;
    private List<UserResearch> _characterResearches = new List<UserResearch>();
    private List<Research> _researches ;  

    private string _backScene;

    void Awake()
    {
        _modalPanel = ModalPanel.Instance();
        _characterManager =CharacterManager.Instance();
        _contentPanel = GameObject.Find("ContentPanel");
    }
    void Start()
    {
        SetBackScene();
        _researches = _characterManager.Researches;
        _characterResearches = _characterManager.CharacterResearches;
        List<ResearchData> uiResearches = _contentPanel.GetComponentsInChildren<ResearchData>().ToList();
        List<GameObject> uiLinks = GameObject.FindGameObjectsWithTag("Line").ToList();
        foreach (var uiResearch in uiResearches)
        {
            int uiId = Int32.Parse(Regex.Match(uiResearch.name, @"\d+").Value);
            for (int i = 0; i < _researches.Count; i++)
            {
                if (!_researches[i].IsEnable)
                    continue;
                if (_researches[i].Id != uiId)
                    continue;
                //_researches[i].Id == uiId
                uiResearch.Research = _researches[i];
                uiResearch.Level = 1;
                foreach (var chResearch in _characterResearches)
                    if (chResearch.ResearchId == uiId)
                    {
                        uiResearch.Level = chResearch.Level+1;
                        break;
                    }
                //Coloring the lines
                if (uiResearch.Research.NeedLine())
                    foreach (var urLink in uiLinks)
                        if (urLink.name == "Link" + uiResearch.Research.RequiredResearchId1 + "-" + uiResearch.Research.Id)
                            if (ResearchLinkValid(uiResearch.Research.RequiredResearchId1, uiResearch.Research.RequiredResearchLevel1))
                                urLink.GetComponent<Image>().color = Color.green;
                            else
                                urLink.GetComponent<Image>().color = Color.red;
                //Purging the buttons
                var images = uiResearch.GetComponentsInChildren<Image>();
                var button = uiResearch.GetComponentInChildren<Button>();
                button.name = _researches[i].Id.ToString();
                //There is another active research progressing 
                if (_characterManager.CharacterResearching!= null)
                {
                    //blink the active building Research 
                    if (_characterManager.CharacterResearching.ResearchId == _researches[i].Id)
                        images[1].gameObject.AddComponent<BlinkMe>();
                    button.interactable = false;
                    images[1].color = new Color(images[1].color.r, images[1].color.g, images[1].color.b, 0.5f);   
                    continue;
                }
                if (ResearchUpgradeIsValid(_researches[i]))
                {
                    button.onClick.AddListener(DoResearch);
                }
                else
                {
                    button.interactable = false;
                    images[1].color = new Color(images[1].color.r, images[1].color.g, images[1].color.b, 0.5f);
                }
            }
        }
    }
    void Update()
    {

    }
    private void DoResearch()
    {
        //Set the Target Research 
        _clickedButton = EventSystem.current.currentSelectedGameObject;
        string buttonName = _clickedButton.name;
        int researchId = Int32.Parse(buttonName);
        
        Research research = null;
        UserResearch characterResearch = null;
        for (int i = 0; i < _researches.Count; i++)
            if (_researches[i].Id == researchId)
            {
                research = _researches[i];
                break;
            }
        if (research == null)
            throw new Exception("Research is not available ");
        research.Print();
        for (int i = 0; i < _characterResearches.Count; i++)
            if (_characterResearches[i].ResearchId == researchId)
            {
                characterResearch = _characterResearches[i];
                break;
            }
        int nextLevel = characterResearch != null ? characterResearch.Level + 1 : 1;
        //Check criteria
        if (!ResearchUpgradeIsValid(research))
        {
            _modalPanel.Choice("Invalid Research!", ModalPanel.ModalPanelType.Ok);
            return;
        }
        //Process the pay 
        var payAmount = research.CalculatePrice(nextLevel);
        if (_characterManager.CharacterSetting.Coin < payAmount) 
        {
            _modalPanel.Choice("You don't have enough Coin ! ", ModalPanel.ModalPanelType.Ok);
            return;
        }
        _characterManager.AddCharacterSetting("Coin", -payAmount);
        MakeResearching(research, nextLevel );
        NewResearchInProgress(researchId);
        _modalPanel.Choice(research.Name + " is in progress ! ", ModalPanel.ModalPanelType.Ok);
    }

    private void NewResearchInProgress(int researchId)
    {
        List<ResearchData> uiResearches = _contentPanel.GetComponentsInChildren<ResearchData>().ToList();
        foreach (var uiResearch in uiResearches)
        {
            var button = uiResearch.GetComponentInChildren<Button>();
            button.interactable = false;
            var images = uiResearch.GetComponentsInChildren<Image>();
            images[1].color = new Color(images[1].color.r, images[1].color.g, images[1].color.b, 0.5f);
            int uiId = Int32.Parse(Regex.Match(uiResearch.name, @"\d+").Value);
            if (uiId == researchId)
                images[1].gameObject.AddComponent<BlinkMe>();
        }
    }
    
    internal void MakeResearching(Research  research, int nextLevel)
    {
        float speed = _characterManager.GetCharacterAttribute("Researching");
        int durationMinutes = research.CalculateTime(nextLevel);
        DateTime time = DateTime.Now.AddMinutes(durationMinutes * (1 - speed));
        _characterManager.SaveCharacterResearching(research.Id, nextLevel, time);
    }
    private bool ResearchUpgradeIsValid(Research research)
    {
        var research1 = false;
        var research2 = false;
        var research3 = false;
        var item = false;
        if (!research.IsEnable)
            return false;
        if (research.RequiredResearchId1 == -1)
            research1 = true;
        if (research.RequiredResearchId2 == -1)
            research2 = true;
        if (research.RequiredResearchId3 == -1)
            research3 = true;
        //Research Requirement 
        foreach (var chResearch in _characterResearches)
        {
            if (research.RequiredResearchId1 == chResearch.ResearchId && research.RequiredResearchLevel1 >= chResearch.Level)
                research1 = true;
            if (research.RequiredResearchId2 == chResearch.ResearchId && research.RequiredResearchLevel2 >= chResearch.Level)
                research2 = true;
            if (research.RequiredResearchId3 == chResearch.ResearchId && research.RequiredResearchLevel3 >= chResearch.Level)
                research3 = true;
        }
        //Item Requirement 
        if (research.RequiredItem == -1)
            item = true;
        else
        if (_characterManager.ItemIsInInventory(research.RequiredItem))
            item = true;
        return research1 && research2 && research3 && item;
    }
    private bool ResearchLinkValid(int requiredResearchId, int requiredResearchLevel)
    {  
        foreach (var chResearch in _characterResearches)
            if (requiredResearchId == chResearch.ResearchId && requiredResearchLevel <= chResearch.Level)
                return true ;
        return false;
    }
    private void SetBackScene()
    {
        var starter = FindObjectOfType<SceneStarter>();
        if (starter != null)
            _backScene = starter.Content;
    }
    public void BackToMainScene()
    {
        switch (_backScene)
        {
            case "Wait":
                SceneManager.LoadScene(SceneSettings.SceneIdForWait);
                return;
            case "GameOver":
                SceneManager.LoadScene(SceneSettings.SceneIdForGameOver);
                return;
            case "Terrain":
                SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
                return;
        }
        //Todo: delete this part 2019-03-15 Give it 2 months
        //if (_characterManager.CharacterSetting.Alive)
        //    SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
        //else
        //    SceneManager.LoadScene(SceneSettings.SceneIdForGameOver);
    }
}
