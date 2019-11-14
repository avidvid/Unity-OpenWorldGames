using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    internal UserPlayer GetUserPlayer(string macId)
    {
        Debug.Log("Mac Id = "+ macId);
        string path = Path.Combine(Application.streamingAssetsPath, "UserPlayer.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(UserPlayer));
        FileStream fs = new FileStream(path, FileMode.Open);
        var userPlayer = (UserPlayer)serializer.Deserialize(fs);
        fs.Close();
        try
        {
            if (userPlayer.MacId == macId)
                return userPlayer;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return null;

    }
    internal void SaveUserPlayer(UserPlayer userPlayer)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserPlayer.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(UserPlayer));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, userPlayer);
        fs.Close();
    }
    internal CharacterSetting GetCharacterSetting(int userId)
    {
        if (userId == 0)
            return null;
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterSetting.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterSetting));
        FileStream fs = new FileStream(path, FileMode.Open);
        var characterSetting = (CharacterSetting)serializer.Deserialize(fs);
        fs.Close();
        return characterSetting;
    }
    internal void SaveCharacterSetting(CharacterSetting characterSetting)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterSetting.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterSetting));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, characterSetting);
        fs.Close();
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
    internal CharacterMixture GetCharacterMixture(int userId)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterMixture.xml");
        //Read the CharacterMixture from CharacterMixture.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterMixture));
        FileStream fs = new FileStream(path, FileMode.Open);
        var characterMixture = (CharacterMixture)serializer.Deserialize(fs);
        fs.Close();
        return userId==0 ? new CharacterMixture() : characterMixture;
    }
    internal void SaveCharacterResearching(CharacterResearching characterResearching)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearching.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterResearching));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, characterResearching);
        fs.Close();
    }
    internal CharacterResearching GetCharacterResearching(int userId)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearching.xml");
        //Read the CharacterResearching from CharacterResearching.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterResearching));
        FileStream fs = new FileStream(path, FileMode.Open);
        var characterResearching = (CharacterResearching)serializer.Deserialize(fs);
        fs.Close();
        return userId == 0 ? new CharacterResearching() : characterResearching;
    }
    internal void SaveUserInventory(List<UserItem> list)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserInventory.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserItem>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, list);
        fs.Close();
    }
    internal List<UserItem> GetUserInventory(int userId)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserInventory.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserItem>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var list = (List<UserItem>)serializer.Deserialize(fs);
        fs.Close();
        return list.Where(item => item.UserId == userId).ToList();
    }
    internal void SaveUserCharacters(List<UserCharacter> list)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserCharacters.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserCharacter>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, list);
        fs.Close();
    }
    internal List<UserCharacter> GetUserCharacters(int userId)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserCharacters.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserCharacter>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var list = (List<UserCharacter>)serializer.Deserialize(fs);
        fs.Close();
        return list.Where(item => item.UserId == userId).ToList();
    }
    internal void SaveCharacterResearches(List<CharacterResearch> list)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearches.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<CharacterResearch>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, list);
        fs.Close();
    }
    internal List<CharacterResearch> GetCharacterResearches(int userId)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearches.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<CharacterResearch>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var list = (List<CharacterResearch>)serializer.Deserialize(fs);
        fs.Close();
        return list.Where(item => item.UserId == userId).ToList();
    }
    internal void SaveUserRecipes(List<UserRecipe> list)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserRecipes.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserRecipe>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, list);
        fs.Close();
    }
    internal List<UserRecipe> GetUserRecipes(int userId)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserRecipes.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserRecipe>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var list = (List<UserRecipe>)serializer.Deserialize(fs);
        fs.Close();
        return list.Where(item => item.UserId == userId).ToList();
    }
    internal void SaveMailMessages(List<MailMessage> list)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "MailMessages.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<MailMessage>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, list);
        fs.Close();
    }
    internal List<MailMessage> GetMailMessages(int userId)
    {
        string path = Path.Combine(Application.streamingAssetsPath, "MailMessages.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<MailMessage>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var list = (List<MailMessage>)serializer.Deserialize(fs);
        fs.Close();
        return list.Where(item => item.SenderId == userId || item.ReceiverId == userId).ToList();
    }
}