using System;
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

    public MailMessage ActiveMessage;
    public bool LoadNew = true;

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
        var messages = _characterManager.MailMessages.OrderByDescending(x => Convert.ToDateTime(x.SendTime)).ToList();
        ActiveMessage = messages[0];
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
        if (LoadNew)
        {
            //Todo: when loading new mail save it as read and delivered in api
            Title.text = ActiveMessage.Title;
            Body.text = ActiveMessage.Body;
            Info.text = ActiveMessage.GetInfo();
            if (ActiveMessage.IsPublic || ActiveMessage.SenderId == _characterManager.UserPlayer.Id)
            {
                SetReplyActive(false);
            }
            else
            {
                SetReplyActive(true);
                ReTitle.text = "Re: " + ActiveMessage.Title;
            }
            LoadNew = false;
        }
    }
    protected void SetReplyActive(bool value)
    {
        Reply.interactable = value;
        Reply.gameObject.SetActive(value);
        InputFieldBody.gameObject.SetActive(value);
        ReTitle.gameObject.SetActive(value);
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
                                                ActiveMessage.SenderId);
        newMail.Print();
        _userDatabase.AddMailMessage(newMail);
        RefreshTheScene();
    }

    private bool ValidateText(string str, int maxLength = 100)
    {
        if (str.Length < 3)
        {
            _messagePanelHandler.ShowMessage(str + " is a bit too short !!!  ", MessagePanel.PanelType.Ok);
            return false;
        }
        if (str.Length > maxLength)
        {
            _messagePanelHandler.ShowMessage("Your reply is a bit too Long remove" + (str.Length-maxLength) + " characters) !!!  ", MessagePanel.PanelType.Ok);
            return false;
        }
        return true;
    }
}