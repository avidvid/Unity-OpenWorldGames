using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

public class ItemDatabase : MonoBehaviour {

    private static ItemDatabase _itemDatabase;
    private List<OupItem> _items = new List<OupItem>();
    private List<Recipe> _recipes = new List<Recipe>();

    #region ItemDatabase Instance
    public static ItemDatabase Instance()
    {
        if (!_itemDatabase)
        {
            _itemDatabase = FindObjectOfType(typeof(ItemDatabase)) as ItemDatabase;
            if (!_itemDatabase)
                Debug.LogWarning("There needs to be one active ItemDatabase script on a GameObject in your scene.");
        }
        return _itemDatabase;
    }
    #endregion
    void Awake()
    {
        _itemDatabase = ItemDatabase.Instance();
        Debug.Log("***IDB*** Start!");
        _items = LoadItems();
        Debug.Log("IDB-Items.Count = " + _items.Count);
        _recipes = LoadRecipes();
        Debug.Log("IDB-Recipes.Count = " + _recipes.Count);
        Debug.Log("***IDB*** Success!");
    }
    #region Recipe
    internal List<Recipe> GetRecipes()
    {
        return _recipes;
    }
    public List<OupItem> RecipeItems(Recipe r)
    {
        return new List<OupItem> { GetItemById(r.FirstItemId), GetItemById(r.SecondItemId), GetItemById(r.FinalItemId) };
    }
    private List<Recipe> LoadRecipes()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Recipe.xml");
        //Read the Recipes from Recipe.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<Recipe>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var recipes = (List<Recipe>)serializer.Deserialize(fs);
        fs.Close();
        return recipes;
    }
    public Recipe FindRecipe(int recipeId)
    {
        for (int i = 0; i < _recipes.Count; i++)
        {
            if (recipeId == _recipes[i].Id)
                return _recipes[i];
        }
        return null;
    }
    #endregion
    #region Item
    internal OupItem GetItemById(int id)
    {
        for (int i = 0; i < _items.Count; i++)
            if (_items[i].Id == id)
                return _items[i];
        return null;
    }
    private void SaveItems()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Item.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<OupItem>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _items);
        fs.Close();
    }
    private List<OupItem> LoadItems()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Item.xml");
        //Read the items from Item.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<OupItem>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var items = (List<OupItem>)serializer.Deserialize(fs);
        fs.Close();
        return items;
    }
    internal int GetItemIdBasedOnRarity(Vector3 position, string dropItems)
    {
        List<int> items = dropItems.Split(',').Select(Int32.Parse).ToList();
        List<int> availableItems = new List<int>();
        var rarity = RandomHelper.Range(position, DateTime.Now.DayOfYear, (int)OupItem.ItemRarity.Common);
        for (int i = 0; i < items.Count; i++)
            if ((int)GetItemById(items[i]).Rarity >= rarity)
                availableItems.Add(items[i]);
        if (availableItems.Count > 0)
            return availableItems[RandomHelper.Range(position, 1, availableItems.Count)];
        return -1;
    }
    #endregion
    #region Offers
    public List<Offer> LoadOffers()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Offer.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Offer>));
        FileStream fs = new FileStream(path, FileMode.Open);
        List<Offer> offers = (List<Offer>)serializer.Deserialize(fs);
        fs.Close();
        return offers;
    }
    private void SaveOffersJson()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Offer.json");
        Offers offers = new Offers(LoadOffers());
        using (StreamWriter stream = new StreamWriter(path))
        {
            string jsonData = JsonUtility.ToJson(offers);
            print(offers.OfferList.Count + jsonData);
            stream.Write(jsonData);
        }
    }
    public List<Offer> LoadOffersJson()
    {
        Offers offers = new Offers();
        string path = Path.Combine(Application.streamingAssetsPath, "Offer.json");
        try
        {
            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);
                offers = JsonUtility.FromJson<Offers>(jsonData);
            }
            else
                Debug.LogError("Error in Load Data");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
        return offers.OfferList;
    }
    #endregion
}
