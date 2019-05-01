using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDatabase : MonoBehaviour {

    private static ItemDatabase _itemDatabase;
    private List<ItemContainer> _items ;
    private List<Recipe> _recipes;
    private List<Offer> _offers ;

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
    void Start()
    {
        _itemDatabase = ItemDatabase.Instance();
        Debug.Log("***IDB*** Start!");
    }
    #region Item
    public List<ItemContainer> GetItems()
    {
        return _items;
    }
    internal ItemContainer GetItemById(int id)
    {
        for (int i = 0; i < _items.Count; i++)
            if (_items[i].Id == id)
                return _items[i];
        return null;
    }
    internal void UpdateItems(List<ItemContainer> items)
    {
        _items = new List<ItemContainer>(items.FindAll(s => s.IsEnable));
        Debug.Log("IDB-Items.Count = " + _items.Count);
    }
    internal int GetItemIdBasedOnRarity(Vector3 position, string dropItems = null)
    {
        if (dropItems == null) //Drop coin/Gem/Recipe 
            dropItems = "1,2,3";
        List<int> items = dropItems.Split(',').Select(Int32.Parse).ToList();
        List<int> availableItems = new List<int>();
        var rarity = RandomHelper.Range(position, DateTime.Now.DayOfYear, (int)DataTypes.Rarity.Common);
        for (int i = 0; i < items.Count; i++)
            if ((int)GetItemById(items[i]).Rarity >= rarity)
                availableItems.Add(items[i]);
        if (availableItems.Count > 0)
            return availableItems[RandomHelper.Range(position, 1, availableItems.Count)];
        return -1;
    }
    #endregion
    #region Recipe
    internal List<Recipe> GetRecipes()
    {
        return _recipes;
    }
    internal void UpdateRecipes(List<Recipe> recipes)
    {
        _recipes = new List<Recipe>(recipes.FindAll(s => s.IsEnable));
        Debug.Log("IDB-Recipes.Count = " + _recipes.Count);
    }
    public List<ItemContainer> RecipeItems(Recipe r)
    {
        return new List<ItemContainer> { GetItemById(r.FirstItemId), GetItemById(r.SecondItemId), GetItemById(r.FinalItemId) };
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
    #region Offers
    public List<Offer> GetOffers()
    {
        return _offers;
    }
    internal void UpdateOffers(List<Offer> offers)
    {
        _offers = new List<Offer>(offers.FindAll(s => s.IsEnable));
        Debug.Log("IDB-Offers.Count = " + _offers.Count);
    }
    #endregion
}