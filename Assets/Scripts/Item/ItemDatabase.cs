using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

public class ItemDatabase : MonoBehaviour {

    public int _defaultDurationDays = 365;
    private static ItemDatabase _itemDatabase;
    private List<ItemContainer> _items = new List<ItemContainer>();
    private List<Recipe> _recipes = new List<Recipe>();
    private List<UserRecipe> _userRecipes = new List<UserRecipe>();

    public int Id;
    public string Name ;
    public string Description ;
    public string IconPath ;
    public int IconId ;
    public int Cost;
    public int Weight;
    public int MaxStackCnt;
    public int StackCnt;
    public Item.ItemType Type ;
    public Item.ItemRarity Rarity;
    public bool IsUnique;
    public int DurationDays;
    public int[] Values;

    void Awake()
    {
        _itemDatabase = ItemDatabase.Instance();

        ItemContainer tempItem =new ItemContainer();
        DateTime ExpirationTime = DateTime.Now.Add(new TimeSpan(_defaultDurationDays, 0, 0, 0, 0));

        LoadItems();
        Debug.Log("items.Count = " + _items.Count);
        //Consumable template
        tempItem = new ItemContainer(
            //Id
            _items.Count,                   
            //Name
            "Cherry",
            //Desc
            "One Cherry",
            //IconPath
            "Inventory/InventorySet1",  
            //IconID    
            1,  
            //Coat                            
            0,
            //Weight                            
            0,
            //MaxStackCnt
            20,
            //StackCnt
            1,
            //Type
            Item.ItemType.Consumable,
            //Rarity
            Item.ItemRarity.Common,
            //Uniqueness
            false,
            //DurationDays
            _defaultDurationDays,
            //ExpirationTime
            ExpirationTime,
            //###Extras
            new int[7] {                    
                1,  //Health
                0,  //Mana
                0,   //Energy
                0,   //coin
                0,   //gem
                0,   //Recipe 
                0   //Egg 
            }
        );
        if (!tempItem.Exist(_items))
            _items.Add(tempItem);

        //Equipment template 
        tempItem = new ItemContainer(
            //Id
            _items.Count,
            //Name
            "Basic Glove",
            //Desc
            "Basic glove from animal leather",
            //IconPath
            "Inventory/InventorySet1",
            //IconID    
            204,
            //Coat                            
            3,
            //Weight                            
            1,
            //MaxStackCnt
            2,
            //StackCnt
            1,
            //Type
            Item.ItemType.Equipment,
            //Rarity
            Item.ItemRarity.Common,
            //Uniqueness
            false,
            //DurationDays
            _defaultDurationDays,
            //ExpirationTime
            ExpirationTime,
            //###Extras
            new int[12] {
                9,//PlaceHolder = (PlaceType) values[0];
                0,//Agility = values[1];
                0,//Bravery = values[2];
                0,//Carry = values[3];
                0,//CarryCnt = values[4];
                0,//Charming = values[5];
                0,//Intellect = values[6];
                1,//Crafting = values[7];
                0,//Researching = values[8];
                0,//Speed = values[9];
                0,//Stamina = values[10];
                0//Strength = values[11];
            }
        );
        if (!tempItem.Exist(_items))
            _items.Add(tempItem);

        //Weapon template
        tempItem = new ItemContainer(
            //Id
            _items.Count,
            //Name
            "Small Sword",
            //Desc
            "Small Sword",
            //IconPath
            "Inventory/InventorySet1",  
            //IconID    
            73,  
            //Coat                            
            10,
            //Weight                            
            3,
            //MaxStackCnt
            1,
            //StackCnt
            1,
            //Type
            Item.ItemType.Weapon,
            //Rarity
            Item.ItemRarity.Common,
            //Uniqueness
            false,
            //DurationDays
            _defaultDurationDays,
            //ExpirationTime
            ExpirationTime,
            //###Extras
            new int[9] {
                0,  //CarryType
                1,  //SpeedAttack
                0,  //SpeedDefense
                1,  //AbilityAttack
                0,  //AbilityDefense
                0,  //MagicAttack
                0,  //MagicDefense
                0,  //PoisonAttack
                0   //PoisonDefense
            }
        );
        if (!tempItem.Exist(_items))
            _items.Add(tempItem);


        //Tools template   //Iron Metal Silver Golden
        tempItem = new ItemContainer(
            //Id
            _items.Count,
            //Name
            "Iron Axe",
            //Desc
            "Iron Axe",
            //IconPath
            "Inventory/InventoryTools",
            //IconID    
            2,
            //Coat                            
            10,
            //Weight                            
            5,
            //MaxStackCnt
            1,
            //StackCnt
            1,
            //Type
            Item.ItemType.Tool,
            //Rarity
            Item.ItemRarity.Common,
            //Uniqueness
            false,
            //DurationDays
            _defaultDurationDays,
            //ExpirationTime
            ExpirationTime,
            //###Extras //todo: ignore for now 
            new int[2] {
                100,  //FavouriteElement
                2  //FavouriteElement
            }
        );
        if (!tempItem.Exist(_items))
            _items.Add(tempItem);
        //Substance template
        tempItem = new ItemContainer(
            //Id
            _items.Count,
            //Name
            "Gem",
            //Desc
            "Gem",
            //IconPath
            "Inventory/gem",
            //IconID    
            0,
            //Coat                            
            10,
            //Weight                            
            0,
            //MaxStackCnt
            100,
            //StackCnt
            10,
            //Type
            Item.ItemType.Substance,
            //Rarity
            Item.ItemRarity.Common,
            //Uniqueness
            false,
            //DurationDays
            _defaultDurationDays,
            //ExpirationTime
            ExpirationTime,
            //###Extras //todo: ignore for now 
            new int[] { }
        );
        if (!tempItem.Exist(_items))
            _items.Add(tempItem);
        SaveItems();
        //PrintItems();
        LoadRecipes();
        Debug.Log("Recipes.Count = " + _recipes.Count);
        //PrintRecipes();
        LoadUserRecipes();
        Debug.Log("UserRecipes.Count = " + _userRecipes.Count);

    }

    //UserRecipe
    public List<Recipe> UserRecipeList()
    {
        List < Recipe > recipes = new List<Recipe>();
        for (int i = 0; i < _recipes.Count; i++)
        {
            if (!_recipes[i].IsEnable)
                continue;
            if (_recipes[i].IsPublic)
            {
                recipes.Add(_recipes[i]);
                continue;
            }
            for (int j = 0; j < _userRecipes.Count; j++)
                if (_recipes[i].Id == _userRecipes[j].RecipeId)
                {
                    if (string.IsNullOrEmpty(_userRecipes[j].RecipeCode))
                        recipes.Add(_recipes[i]);
                    else
                    {
                        _recipes[i].IsEnable = false;
                        recipes.Add(_recipes[i]);
                    }
                    break;
                }
        }
        return recipes;
    }

    internal bool ValidateRecipeCode(string recipeCode)
    {
        for (int j = 0; j < _userRecipes.Count; j++)
            if (_userRecipes[j].RecipeCode != null)
                if (_userRecipes[j].RecipeCode == recipeCode)
                {
                    _userRecipes[j].RecipeCode = "";
                    SaveUserRecipes();
                    return true;
                }
        return false;
    }

    private void LoadUserRecipes()
    {
        //Empty the Recipes DB
        _userRecipes.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "UserRecipe.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserRecipe>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _userRecipes = (List<UserRecipe>)serializer.Deserialize(fs);
        fs.Close();
    }
    public bool AddUserRecipe(UserRecipe ur)
    {
        try
        {
            var owned = _userRecipes.Find(c => c.RecipeId == ur.RecipeId && c.UserId == ur.UserId && !string.IsNullOrEmpty(c.RecipeCode));
            if (owned == null)
                _userRecipes.Add(ur);
            else
                owned.RecipeCode = "";
            SaveUserRecipes();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }
    internal bool AddNewRandomUserRecipe(int playerId)
    {
        List<int> availableRecipe = new List<int>();
        int key = DateTime.Now.DayOfYear;
        var rarity = RandomHelper.Range(key, (int)Item.ItemRarity.Common);
        bool userOwnedRecipe = false; 
        for (int i = 0; i < _recipes.Count; i++)
            if (_recipes[i].IsEnable && !_recipes[i].IsPublic)
            {
                if ((int)_recipes[i].Rarity < rarity)
                    continue;
                for (int j = 0; j < _userRecipes.Count; j++)
                    if (_recipes[i].Id == _userRecipes[j].RecipeId && string.IsNullOrEmpty(_userRecipes[j].RecipeCode))
                    {
                        userOwnedRecipe = true;
                        break;
                    }
                if (!userOwnedRecipe)
                        availableRecipe.Add(_recipes[i].Id);
                userOwnedRecipe = false;
            }
        if (availableRecipe.Count > 0)
        {
            UserRecipe ur = new UserRecipe(availableRecipe[RandomHelper.Range(key, availableRecipe.Count)], playerId);
            return AddUserRecipe(ur);
        }
        return false;
    }
    public Recipe FindUserRecipes(int first, int second)
    {
        for (int i = 0; i < _recipes.Count; i++)
        {
            if (!_recipes[i].IsEnable)
                continue;
            Recipe r = _recipes[i];
            if (_recipes[i].IsPublic)
            {
                if (r.IsEnable && first == r.FirstItemId && second == r.SecondItemId)
                    return r;
                if (r.IsEnable && first == r.SecondItemId && second == r.FirstItemId)
                    return Reverse(r);
                continue;
            }
            for (int j = 0; j < _userRecipes.Count; j++)
                if (_recipes[i].Id == _userRecipes[j].RecipeId)
                {
                    if (string.IsNullOrEmpty(_userRecipes[j].RecipeCode))
                    {
                        if (r.IsEnable && first == r.FirstItemId && second == r.SecondItemId)
                            return r;
                        if (r.IsEnable && first == r.SecondItemId && second == r.FirstItemId)
                            return Reverse(r);
                    }
                    break;
                }
        }
        return null;
    }

    private void SaveUserRecipes()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserRecipe.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserRecipe>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _userRecipes);
        fs.Close();
    }
    //Recipe
    private List<Recipe> RecipeList()
    {
        return _recipes;
    }
    public List<ItemContainer> RecipeItems(Recipe r)
    {
        return new List<ItemContainer> { FindItem(r.FirstItemId), FindItem(r.SecondItemId), FindItem(r.FinalItemId) };
    }
    private void LoadRecipes()
    {
        //Empty the Recipes DB
        _recipes.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Recipe.xml");
        //Read the Recipes from Recipe.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<Recipe>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _recipes = (List<Recipe>)serializer.Deserialize(fs);
        fs.Close();
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
    private Recipe Reverse(Recipe r)
    {
        int temp = r.FirstItemId;
        r.FirstItemId = r.SecondItemId;
        r.SecondItemId = temp;
        temp = r.FirstItemCnt;
        r.FirstItemCnt = r.SecondItemCnt;
        r.SecondItemCnt = temp;
        return r;
    }
    //Item
    public ItemContainer FindItem(int id)
    {
        for (int i = 0; i<_items.Count; i++)
            if (_items[i].Id == id)
                return _items[i];  
        return null;
    }
    private void SaveItems()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Item.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _items);
        fs.Close();
    }
    private void LoadItems()
    {
        //Empty the Items DB
        _items.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Item.xml");
        //Read the items from Item.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _items = (List<ItemContainer>)serializer.Deserialize(fs);
        fs.Close();
    }
    internal int GetItemBasedOnRarity(Vector3 position, string dropItems)
    {
        List<int> items = dropItems.Split(',').Select(Int32.Parse).ToList();
        List<int> availableItems = new List<int>();
        var rarity = RandomHelper.Range(position, DateTime.Now.DayOfYear, (int)Item.ItemRarity.Common);

        for (int i = 0; i < items.Count; i++)
            if ((int)FindItem(items[i]).Rarity >= rarity)
                availableItems.Add(items[i]);
        if (availableItems.Count > 0)
            return availableItems[RandomHelper.Range(position, 1, availableItems.Count)];
        return -1;
    }
    public void CreateItem()
    {   //Add item button call this function 
        LoadItems();
        //Add item through the public properties 
        _items.Add(new ItemContainer(Id, Name, Description,
            IconPath, IconId,
            Cost, Weight,
            MaxStackCnt, StackCnt,
            Type, Rarity, IsUnique,
            DurationDays, DateTime.Now.Add(new TimeSpan(DurationDays, 0, 0, 0, 0)),
            Values));
        //Save the new list back in Item.xml file in the streamingAssets folder
        SaveItems();
    }
    //Offers
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
    //Instance
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
}
