using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

[Serializable]
public class Offers
{
    public List<Offer> OfferList;
    public Offers() {}
    public Offers(List<Offer> list)
    {
        OfferList = list;
    }
}

[Serializable]
public class Offer
{
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
