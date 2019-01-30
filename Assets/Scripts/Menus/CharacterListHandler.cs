using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterListHandler : MonoBehaviour
{
    private CharacterManager _characterManager;
    private ModalPanel _modalPanel;


    //Character Prefab
    public GameObject CharacterContent;

    private List<Character> _characters = new List<Character>();
    private GameObject _addCharacterPanel;

    void Awake()
    {
        _characterManager = CharacterManager.Instance();
        _modalPanel = ModalPanel.Instance();
        _addCharacterPanel = GameObject.Find("AddCharacterPanel");
    }
    void Start()
    {
        var contentPanel = GameObject.Find("ContentPanel");
        var characterInfo = GameObject.Find("CharacterInfo").GetComponent<TextMeshProUGUI>();
        _addCharacterPanel.SetActive(false);
        _characters = _characterManager.UserCharacters;
        characterInfo.text = "Your Characters: " + _characters.Count(p => p.IsEnable);
        for (int i = 0; i < _characters.Count; i++)
        {
            if (!_characters[i].IsEnable)
            {
                _addCharacterPanel.SetActive(true);
                continue;
            }
            GameObject characterObject = Instantiate(CharacterContent);
            characterObject.transform.SetParent(contentPanel.transform);

            characterObject.transform.name = "Character " + _characters[i].Name + _characters[i].Id;
            characterObject.transform.localScale = Vector3.one;
            characterObject.GetComponentsInChildren<Image>()[1].sprite = _characters[i].GetSprite();
            characterObject.GetComponentInChildren<TextMeshProUGUI>().text = _characters[i].Name;
        }
    }

    public void ValidateRecipeCode()
    {
        var characterCode = _addCharacterPanel.GetComponentInChildren<TMP_InputField>().text;
        if (!string.IsNullOrEmpty(characterCode))
            if (_characterManager.ValidateCharacterCode(characterCode))
                _modalPanel.Choice("New Character added to your list!", ModalPanel.ModalPanelType.Ok, RefreshTheScene);
            else
                _modalPanel.Choice("The Character Code is Wrong! ", ModalPanel.ModalPanelType.Ok);
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
