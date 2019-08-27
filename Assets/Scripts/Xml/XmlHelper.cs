using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class XmlHelper : MonoBehaviour
{
    private static XmlHelper _xmlHelper;
    //Database
    private ItemDatabase _itemDatabase;
    private UserDatabase _userDatabase;
    private CharacterDatabase _characterDatabase;
    private TerrainDatabase _terrainDatabase;

    #region XmlHelper Instance
    public static XmlHelper Instance()
    {
        if (!_xmlHelper)
        {
            _xmlHelper = FindObjectOfType(typeof(XmlHelper)) as XmlHelper;
            if (!_xmlHelper)
                Debug.LogWarning("There needs to be one active _xmlHelper script on a GameObject in your scene.");
        }
        return _xmlHelper;
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("***XML*** Start!");
        _xmlHelper = XmlHelper.Instance();
    }
    internal void SaveItems(List<ItemContainer> list)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "ItemContainer.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, list);
        fs.Close();
    }
    internal List<ItemContainer> GetItems()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "ItemContainer.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var itemContainer = (List<ItemContainer>)serializer.Deserialize(fs);
        fs.Close();
        return itemContainer;
    }
    internal void SaveRecipes(List<Recipe> list)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Recipe.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Recipe>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, list);
        fs.Close();
    }
    internal List<Recipe> GetRecipes()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Recipe.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Recipe>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var recipes = (List<Recipe>)serializer.Deserialize(fs);
        fs.Close();
        return recipes;
    }
    internal void SaveOffers(List<Offer> list)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Offer.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Offer>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, list);
        fs.Close();
    }
    internal List<Offer> GetOffers()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Offer.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Offer>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var offers = (List<Offer>)serializer.Deserialize(fs);
        fs.Close();
        return offers;
    }
    internal void SaveCharacters(List<Character> list)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Character.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Character>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, list);
        fs.Close();
    }
    internal List<Character> GetCharacters()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Character.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Character>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var characters = (List<Character>)serializer.Deserialize(fs);
        fs.Close();
        return characters;
    }
    internal void SaveResearches(List<Research> list)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Research.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Research>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, list);
        fs.Close();
    }
    internal List<Research> GetResearches()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Research.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Research>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var researches = (List<Research>)serializer.Deserialize(fs);
        fs.Close();
        return researches;
    }
    internal void SaveRegions(List<Region> list)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Region.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Region>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, list);
        fs.Close();
    }
    internal List<Region> GetRegions()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Region.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Region>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var regions = (List<Region>)serializer.Deserialize(fs);
        fs.Close();
        return regions;
    }
    internal void SaveTerrains(List<TerrainIns> list)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "TerrainIns.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<TerrainIns>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, list);
        fs.Close();
    }
    internal List<TerrainIns> GetTerrains()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "TerrainIns.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<TerrainIns>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var terrains = (List<TerrainIns>)serializer.Deserialize(fs);
        fs.Close();
        return terrains;
    }
    internal void SaveElements(List<ElementIns> list)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "ElementIns.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<ElementIns>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, list);
        fs.Close();
    }
    internal List<ElementIns> GetElements()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "ElementIns.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<ElementIns>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var elements = (List<ElementIns>)serializer.Deserialize(fs);
        fs.Close();
        return elements;
    }
    internal void SaveInsideStories(List<InsideStory> list)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "InsideStory.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<InsideStory>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, list);
        fs.Close();
    }
    internal List<InsideStory> GetInsideStories()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "InsideStory.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<InsideStory>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var insideStories = (List<InsideStory>)serializer.Deserialize(fs);
        fs.Close();
        return insideStories;
    }
    internal void SaveCharacterMixture(CharacterMixture characterMixture)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterMixture.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterMixture));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, characterMixture);
        fs.Close();
    }
    internal CharacterMixture GetCharacterMixture()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterMixture.xml");
        //Read the CharacterMixture from CharacterMixture.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterMixture));
        FileStream fs = new FileStream(path, FileMode.Open);
        var characterMixture = (CharacterMixture)serializer.Deserialize(fs);
        fs.Close();
        return characterMixture;
    }
}