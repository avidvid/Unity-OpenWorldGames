using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MessagePanel : MonoBehaviour
{
    public enum PanelType
    {
        Ok,
        YesNo,
        YesCancel,
        Cancel,
        YesNoCancel
    }

    public GameObject MessageBoard;
    public GameObject MessageBox;
    public TextMeshProUGUI Message;
    public Button ButtonYes;
    public Button ButtonNo;
    public Button ButtonCancel;

    //Todo: add icon to the dialog  https://unity3d.com/learn/tutorials/modules/intermediate/live-training-archive/modal-windowhttps://unity3d.com/learn/tutorials/modules/intermediate/live-training-archive/modal-window-pt3?playlist=17111

    void Start()
    {
        var canvas = GameObject.Find("Canvas");
        MessageBoard.transform.SetParent(canvas.transform, false);
        MessageBox.transform.localPosition = Vector3.zero;
        MessageBoard.transform.localPosition = Vector3.zero;
    }


    public void ShowMessage(string question, PanelType modalPanelType, UnityAction firstEvent = null, UnityAction secondEvent = null)
    {
        Debug.Log("ShowMessage: " + question + modalPanelType);


        //Set a default actions
        ButtonNo.onClick.RemoveAllListeners();
        ButtonCancel.onClick.RemoveAllListeners();
        ButtonYes.onClick.RemoveAllListeners();

        if (firstEvent != null)
            ButtonYes.onClick.AddListener(firstEvent);
        if (secondEvent != null)
        {
            ButtonNo.onClick.AddListener(secondEvent);
            ButtonCancel.onClick.AddListener(secondEvent);
        }
        ButtonNo.onClick.AddListener(ClosePanel);
        ButtonCancel.onClick.AddListener(ClosePanel);
        ButtonYes.onClick.AddListener(ClosePanel);

        ButtonNo.gameObject.SetActive(false);
        ButtonCancel.gameObject.SetActive(false);
        ButtonYes.gameObject.SetActive(false);

        Message.text = question;
        switch (modalPanelType)
        {
            case PanelType.YesNo:
                //  Yes/No: A string, a Yes event, a No event (No Cancel Button);
                ButtonYes.gameObject.SetActive(true);
                ButtonNo.gameObject.SetActive(true);
                break;
            case PanelType.YesCancel:
                //  Yes/No: A string, a Yes event, a No event (No Cancel Button);
                ButtonYes.gameObject.SetActive(true);
                ButtonCancel.gameObject.SetActive(true);
                break;
            case PanelType.Ok:
                ButtonYes.gameObject.SetActive(true);
                break;
            case PanelType.Cancel:
                //  Announcement: A string and Cancel event;
                ButtonCancel.gameObject.SetActive(true);
                break;
            case PanelType.YesNoCancel:
                //  Yes/No/Cancel: A string, a Yes event, a No event and Cancel event;
                ButtonYes.gameObject.SetActive(true);
                ButtonNo.gameObject.SetActive(true);
                ButtonCancel.gameObject.SetActive(true);
                break;
        }
    }
    void ClosePanel()
    {
        Destroy(gameObject);
    }
}

