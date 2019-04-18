using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

[Serializable]
public class JsonOfferList
{
    public List<Offer> List;
    public JsonOfferList() {}
    public JsonOfferList(List<Offer> list)
    {
        List = list;
    }
}

[Serializable]
public class Offer
{
    public int id;
    public string SellProd;
    public int SellAmount;
    public string PayProd;
    public int PayAmount;
    public bool IsSpecial;
    public bool IsEnable;
    public int HealthCheck;

    internal string NameGenerator()
    {
        string offerName = "Offer:"+ SellAmount + "-";
        if (Regex.IsMatch(SellProd, @"\d"))
            offerName += "Item(" + SellProd + ")";
        else
            offerName += SellProd;
        offerName += " For " + PayAmount+ "-" + PayProd; 
        return offerName;
    }
}
