using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterListHandler : MonoBehaviour
{
    private CharacterManager _characterManager;


    //Character Prefab
    public GameObject CharacterContent;

    private GameObject _addCharacterPanel;
    internal bool SceneRefresh;

    void Awake()
    {
        _characterManager = CharacterManager.Instance();
        _addCharacterPanel = GameObject.Find("AddCharacterPanel");
    }
    void Start()
    {
        var contentPanel = GameObject.Find("ContentPanel");
        var characterInfo = GameObject.Find("CharacterInfo").GetComponent<TextMeshProUGUI>();
        _addCharacterPanel.SetActive(false);
        var characters = _characterManager.MyCharacters;
        characterInfo.text = "Your Characters: " + characters.Count(p => p.IsEnable);
        for (int i = 0; i < characters.Count; i++)
        {
            if (!characters[i].IsEnable)
            {
                _addCharacterPanel.SetActive(true);
                continue;
            }
            GameObject characterObject = Instantiate(CharacterContent);
            characterObject.transform.SetParent(contentPanel.transform);
            characterObject.transform.name = "Character " + characters[i].Name + characters[i].Id;
            characterObject.transform.localScale = Vector3.one;
            characterObject.GetComponentInChildren<CharacterData>().SlotCharacter = characters[i];
            characterObject.GetComponentsInChildren<Image>()[1].sprite = characters[i].GetSprite();
            characterObject.GetComponentInChildren<TextMeshProUGUI>().text = characters[i].Name;
        }
    }
    void Update()
    {
        if (SceneRefresh)
            RefreshTheScene();
    }
    public void ValidateCharacterCode()
    {
        var characterCode = _addCharacterPanel.GetComponentInChildren<TMP_InputField>().text;
        if (!string.IsNullOrEmpty(characterCode))
            ValidateCharacterCode(characterCode);
    }
    internal void ValidateCharacterCode(string characterCode)
    {
        var userCharacters = _characterManager.MyUserCharacters;
        for (int i = 0; i < userCharacters.Count; i++)
            if (userCharacters[i].CharacterCode == characterCode)
            {
                userCharacters[i].CharacterCode = "";
            }
        var xmlHelper = XmlHelper.Instance();
        if (xmlHelper != null)
        {
            xmlHelper.SaveUserCharacters(userCharacters);
        }
        //var apiGatewayConfig = ApiGatewayConfig.Instance();
        //if (apiGatewayConfig != null)
        //{
        //    var userCharacters = _characterManager.MyUserCharacters;
        //    for (int i = 0; i < userCharacters.Count; i++)
        //        if (userCharacters[i].CharacterCode == "0000")
        //            apiGatewayConfig.PutUserCharacter(userCharacters[i], characterCode);
        //}
    }

    public void RefreshTheScene()
    {
        SceneManager.LoadScene(SceneSettings.SceneIdForCharacterScene);
    }

    public void BackToMainScene()
    {
        SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }
}
