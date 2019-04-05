using System;
using System.Collections.Generic;
using Facebook.MiniJSON;
using UnityEngine;

[Serializable]
public class Headers
{
    public string Header1;

    internal string MyInfo()
    {
        return "Header1=" + Header1 ;
    }
}

[Serializable]
public class BodyResponse
{
    public string Message;
    public int RandomNum=0;
    public List<Offer> Offers;

    internal string MyInfo()
    {
        string info = " Message=" +  Message;
        if (RandomNum!=0)
            info += " RandomNum=" + RandomNum;
        if (Offers != null)
            info += " Offers=" + Offers.Count;
        return info;
    }
}

[Serializable]
public class ApiResponse 
{
    public string StatusCode;
    public Headers Headers;
    public BodyResponse Body;
    internal void Print()
    {
        Debug.Log("ApiResponse(StatusCode=" + StatusCode + "): Headers: " + Headers.MyInfo() +
                  " Body: " + Body.MyInfo());
    }
}