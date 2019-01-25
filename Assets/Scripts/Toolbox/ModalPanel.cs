using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ModalPanel : MonoBehaviour
{
    public enum ModalPanelType
    {
        Ok,   
        YesNo, 
        YesCancel, 
        Cancel,
        YesNoCancel
    }

    private static ModalPanel _modalPanel; 

    private GameObject _modalPanelObject;
    private GameObject _messagePanel;
    private GameObject _warningPanel;

    private Text _question;
    private Button _buttonYes;
    private Button _buttonNo;
    private Button _buttonCancel;

    //Todo: add icon to the dialog  https://unity3d.com/learn/tutorials/modules/intermediate/live-training-archive/modal-windowhttps://unity3d.com/learn/tutorials/modules/intermediate/live-training-archive/modal-window-pt3?playlist=17111

    void Awake()
    {
        _modalPanel = ModalPanel.Instance();
        _modalPanelObject = GameObject.Find("ModalPanel");
        _messagePanel = _modalPanelObject.transform.Find("MessagePanel").gameObject;
        _messagePanel.transform.localPosition = Vector3.zero;

        _warningPanel = _modalPanelObject.transform.Find("WarningPanel").gameObject;
        _warningPanel.transform.localPosition = Vector3.zero;
    }

    // Yes/No/Cancel: A string, a Yes event, a No event and Cancel event
    public void Choice(string question, ModalPanelType modalPanelType, UnityAction firstEvent=null)
    {
        Debug.Log("Choice: " + question + modalPanelType);
        _modalPanelObject.SetActive(true);
        _messagePanel.SetActive(true);
        _warningPanel.SetActive(false);

        _question = _messagePanel.transform.Find("Question").gameObject.GetComponent<Text>();
        _buttonYes = _messagePanel.transform.GetChild(1).Find("OkButton").gameObject.GetComponent<Button>();
        _buttonNo = _messagePanel.transform.GetChild(1).Find("NoButton").gameObject.GetComponent<Button>();
        _buttonCancel = _messagePanel.transform.GetChild(1).Find("CancelButton").gameObject.GetComponent<Button>();

        //Set a default actions
        _buttonYes.onClick.RemoveAllListeners();
        _buttonYes.onClick.AddListener(ClosePanel);
        if (firstEvent != null)
            _buttonYes.onClick.AddListener(firstEvent);
        _buttonYes.gameObject.SetActive(false);
        _buttonNo.onClick.RemoveAllListeners();
        _buttonNo.onClick.AddListener(ClosePanel);
        _buttonNo.gameObject.SetActive(false);
        _buttonCancel.onClick.RemoveAllListeners();
        _buttonCancel.onClick.AddListener(ClosePanel);
        _buttonCancel.gameObject.SetActive(false);
        
        _question.text = question;
        switch (modalPanelType)
        {
            case ModalPanelType.YesNo:
                //  Yes/No: A string, a Yes event, a No event (No Cancel Button);
                _buttonYes.gameObject.SetActive(true);
                _buttonNo.gameObject.SetActive(true);
                break;
            case ModalPanelType.YesCancel:
                //  Yes/No: A string, a Yes event, a No event (No Cancel Button);
                _buttonYes.gameObject.SetActive(true);
                _buttonCancel.gameObject.SetActive(true);
                break;
            case ModalPanelType.Ok:
                _buttonYes.gameObject.SetActive(true);
                break;
            case ModalPanelType.Cancel:
                //  Announcement: A string and Cancel event;
                _buttonCancel.gameObject.SetActive(true);
                break;
            case ModalPanelType.YesNoCancel:
                //  Yes/No/Cancel: A string, a Yes event, a No event and Cancel event;
                _buttonYes.gameObject.SetActive(true);
                _buttonNo.gameObject.SetActive(true);
                _buttonCancel.gameObject.SetActive(true);
                break;
        }
    }

    public void Warning(string question, ModalPanelType modalPanelType, UnityAction firstEvent = null, UnityAction secondEvent = null)
    {
        Debug.Log("Warning: " + question + modalPanelType);
        _modalPanelObject.SetActive(true);
        _messagePanel.SetActive(false);
        _warningPanel.SetActive(true);

        _question = _warningPanel.transform.Find("Question").gameObject.GetComponent<Text>();
        _buttonYes = _warningPanel.transform.GetChild(1).Find("OkButton").gameObject.GetComponent<Button>();
        _buttonNo = _warningPanel.transform.GetChild(1).Find("NoButton").gameObject.GetComponent<Button>();
        _buttonCancel = _warningPanel.transform.GetChild(1).Find("CancelButton").gameObject.GetComponent<Button>();

        //Set a default actions
        _buttonNo.onClick.RemoveAllListeners();
        _buttonYes.onClick.RemoveAllListeners();
        _buttonCancel.onClick.RemoveAllListeners();
        if (firstEvent != null)
            _buttonYes.onClick.AddListener(firstEvent);
        if (secondEvent != null)
        {
            _buttonNo.onClick.AddListener(secondEvent);
            _buttonCancel.onClick.AddListener(secondEvent);
        }
        _buttonNo.onClick.AddListener(ClosePanel);
        _buttonCancel.onClick.AddListener(ClosePanel);
        _buttonYes.onClick.AddListener(ClosePanel);

        _buttonYes.gameObject.SetActive(false);
        _buttonNo.gameObject.SetActive(false);
        _buttonCancel.gameObject.SetActive(false);

        _question.text = question;
        switch (modalPanelType)
        {
            case ModalPanelType.YesNo:
                //  Yes/No: A string, a Yes event, a No event (No Cancel Button);
                _buttonYes.gameObject.SetActive(true);
                _buttonNo.gameObject.SetActive(true);
                break;
            case ModalPanelType.YesCancel:
                //  Yes/No: A string, a Yes event, a No event (No Cancel Button);
                _buttonYes.gameObject.SetActive(true);
                _buttonCancel.gameObject.SetActive(true);
                break;
            case ModalPanelType.Ok:
                _buttonYes.gameObject.SetActive(true);
                break;
            case ModalPanelType.Cancel:
                //  Announcement: A string and Cancel event;
                _buttonCancel.gameObject.SetActive(true);
                break;
            case ModalPanelType.YesNoCancel:
                //  Yes/No/Cancel: A string, a Yes event, a No event and Cancel event;
                _buttonYes.gameObject.SetActive(true);
                _buttonNo.gameObject.SetActive(true);
                _buttonCancel.gameObject.SetActive(true);
                break;
        }
    }

    void ClosePanel()
    {
        _modalPanelObject.SetActive(false);
    }

    public static ModalPanel Instance()
    {
        if (!_modalPanel)
        {
            _modalPanel = FindObjectOfType(typeof(ModalPanel)) as ModalPanel;
            if (!_modalPanel)
                Debug.LogError("There needs to be one active ModalPanel script on a GameObject in your scene.");
        }
        return _modalPanel;
    }
}
