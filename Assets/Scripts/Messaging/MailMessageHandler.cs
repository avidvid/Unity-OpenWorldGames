using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MailMessageHandler : MonoBehaviour
{
    public Sprite InSprite;
    public Sprite OutSprite;

    private CharacterManager _characterManager;
    private MessagePanelHandler _messagePanelHandler;
    private UserDatabase _userDatabase;

    public GameObject MailContentPrefab;

    private MailMessage _activeMessage;
    private bool _loadNew =true;

    public TextMeshProUGUI Title;
    public TextMeshProUGUI Body;
    public TextMeshProUGUI Info;
    public TextMeshProUGUI ReTitle;
    public TMP_InputField InputFieldBody;
    public Button Reply;


    void Awake()
    {
        _characterManager = CharacterManager.Instance();
        _messagePanelHandler = MessagePanelHandler.Instance();
        _userDatabase = UserDatabase.Instance();
    }
    void Start()
    {
        var contentPanel = GameObject.Find("ContentPanel");

        var messages = _characterManager.MailMessages;
        _activeMessage = messages[0];
        for (int i = 0; i < messages.Count; i++)
        {
            GameObject messageObject = Instantiate(MailContentPrefab);
            messageObject.transform.SetParent(contentPanel.transform);
            messageObject.transform.name = "Message " + messages[i].Id;
            messageObject.transform.localScale = Vector3.one;
            var messageData = messageObject.GetComponentInChildren<MessageData>();
            messageData.MessageIns = messages[i];
            messageData.InComing = (messages[i].ReceiverId == _characterManager.UserPlayer.Id);
            messageData.TheOtherPlayer = (messageData.InComing? 
                                                _userDatabase.GetUserById(messages[i].SenderId): 
                                                _userDatabase.GetUserById(messages[i].ReceiverId));
            messageObject.GetComponentInChildren<Image>().sprite = (messageData.InComing ? InSprite: OutSprite); ;
            messageObject.GetComponentInChildren<TextMeshProUGUI>().text = messages[i].Title;
        }
    }
    void Update()
    {
        if (_loadNew)
        {
            Title.text = _activeMessage.Title;
            Body.text = _activeMessage.Body;
            Info.text = _activeMessage.GetInfo();
            ReTitle.text = "Re: " + _activeMessage.Title;
            _loadNew = false;
        }
    }
    public void RefreshTheScene()
    {
        SceneManager.LoadScene(SceneSettings.SceneIdForMailMessage);
    }
    public void BackToMainScene()
    {
        SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }
    public void SendTheMail()
    {
        if (!ValidateText(InputFieldBody.text))
            return;
        Debug.Log("Send The respond!!!");
        MailMessage newMail = new MailMessage(Title.text, 
                                                InputFieldBody.text,
                                                _characterManager.UserPlayer.Id, 
                                                _activeMessage.SenderId);
        newMail.Print();
    }

    private bool ValidateText(string str, int maxLength = 40)
    {
        if (str.Length < 3)
        {
            _messagePanelHandler.ShowMessage(str + " is a bit too short !!!  ", MessagePanel.PanelType.Ok);
            return false;
        }
        if (str.Length > maxLength)
        {
            _messagePanelHandler.ShowMessage(str + " is a bit too Long (less than " + maxLength + ") !!!  ", MessagePanel.PanelType.Ok);
            return false;
        }
        return true;
    }
}