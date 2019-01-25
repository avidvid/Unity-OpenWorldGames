using System;

[Serializable]
public class Offer
{
    public int Id { get; set; }
    public string SellProd { get; set; }
    public int SellAmount { get; set; }
    public string PayProd { get; set; }
    public int PayAmount { get; set; }
    public bool IsSpecial { get; set; }
    public bool IsEnable { get; set; }

}
