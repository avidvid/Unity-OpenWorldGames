using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

public class TerrainDatabase : MonoBehaviour {

    private static TerrainDatabase _terrainDatabase;
    private CharacterManager _characterManager;
    private CharacterDatabase _characterDatabase;

    internal List<Region> _regions = new List<Region>();
    public Region Region;

    private List<TerrainIns> _terrains = new List<TerrainIns>();
    public List<TerrainIns> Terrains = new List<TerrainIns>();

    private List<ElementIns> _elements = new List<ElementIns>();
    public List<ElementIns> Elements = new List<ElementIns>();


    internal List<InsideStory> _insideStories = new List<InsideStory>();
    public List<InsideStory> InsideStories = new List<InsideStory>();
    public HashSet<int> InsideMonsterIds = new HashSet<int>();
    public List<Character> InsideMonsters = new List<Character>();

    public List<Character> Monsters = new List<Character>();

    void Awake()
    {
        _terrainDatabase = TerrainDatabase.Instance();
        _characterDatabase = CharacterDatabase.Instance();
        _characterManager = CharacterManager.Instance();

        //Load Regions from database 
        LoadRegions();
        //Set Region based on the user
        Region = GetRegion(_characterManager.UserPlayer.Latitude, _characterManager.UserPlayer.Longitude);

        Region.Print();

        //Load Terrains from database 
        LoadTerrains();
        //Set Terrains based on the user region
        SetTerrainTypes();
        if (Terrains.Count == 0)
            throw new Exception("Terrains count is ZERO");

        //Load Elements from database 
        LoadElements();
        //Set Elements based on the user region
        SetElementTypes();
        if (Elements.Count == 0)
            throw new Exception("Elements count is ZERO");

        //Load InsideStories from database 
        LoadInsideStories();
        //Set InsideStories based on the user region
        SetInsideStoryTypes();
        if (InsideStories.Count == 0)
            throw new Exception("InsideStories count is ZERO");

        //Set Monsters based on the user region
        SetMonsterTypes();
        if (Monsters.Count == 0)
            throw new Exception("Monsters count is ZERO");
        if (InsideMonsters.Count == 0)
            throw new Exception("InsideMonsters count is ZERO");
        Debug.Log("Terrain Database Success!");
    }
    //Regions
    public Region GetRegion(float latitude, float longitude)
    {
        for (int i = 0; i < _regions.Count; i++)
            if (Mathf.Abs(_regions[i].Latitude - latitude) < 10 && Mathf.Abs(_regions[i].Longitude - longitude) < 10)
                return _regions[i];
        return _regions[0];
    }
    private void LoadRegions()
    {
        //Empty the Characters DB
        _regions.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Region.xml");
        //Read the Characters from Character.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<Region>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _regions = (List<Region>)serializer.Deserialize(fs);
        fs.Close();
    }
    private void SaveRegions()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Region.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Region>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _regions);
        fs.Close();
    }
    internal Vector2 GetRegionLocation(int key)
    {
        for (int i = 0; i < _regions.Count; i++)
            if (_regions[i].Key == key)
                return new Vector2(_regions[i].Latitude*1000, _regions[i].Longitude * 1000);
                // Vector2 only accept 2 decimal points
        return Vector2.zero;
    }



    //Terrains
    private void LoadTerrains()
    {
        _terrains.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Terrain.xml");
        //Read the Recipes from Terrain.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<TerrainIns>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _terrains = (List<TerrainIns>)serializer.Deserialize(fs);
        fs.Close();
    }
    private void SetTerrainTypes()
    {
        List<int> regionTerrains = Region.Terrains.Split(',').Select(Int32.Parse).ToList();
        foreach (var terrain in _terrains)
            if (regionTerrains.IndexOf(terrain.Id) != -1  && terrain.IsEnable)
                Terrains.Add(terrain);
    }
    //Elements
    private void LoadElements()
    {
        _elements.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Element.xml");
        //Read the Recipes from Element.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<ElementIns>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _elements = (List<ElementIns>)serializer.Deserialize(fs);
        fs.Close();
    }
    private void SetElementTypes()
    {
        List<int> regionElements = Region.Elements.Split(',').Select(Int32.Parse).ToList();
        foreach (var element in _elements)
            if (regionElements.IndexOf(element.Id) != -1 && element.IsEnable)
                Elements.Add(element);
        //Terrain Element HealthCheck 
        foreach (var terrain in Terrains)
        {
            var healthCheck = false;
            if (!terrain.HasElement)
                continue;
            foreach (var element in Elements)
                if (terrain.Type == element.FavoriteTerrainTypes)
                {
                    healthCheck = true;
                    break;
                }
            if (!healthCheck)
                throw new Exception("Terrain with no available element");
        }
    }
    //InsideStories
    private void LoadInsideStories()
    {
        //Empty the InsideStory DB
        _insideStories.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "InsideStory.xml");
        //Read the InsideStories from InsideStory.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<InsideStory>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _insideStories = (List<InsideStory>)serializer.Deserialize(fs);
        fs.Close();
    }
    private void SetInsideStoryTypes()
    {
        List<int> regionInsideStories = Region.InsideStories.Split(',').Select(Int32.Parse).ToList();
        foreach (var insideStory in _insideStories)
            if (regionInsideStories.IndexOf(insideStory.Id) != -1 && insideStory.IsEnable)
            {
                InsideStories.Add(insideStory);
                foreach (var actor in insideStory.Actors)
                    InsideMonsterIds.Add(actor.CharacterId);
            }
    }
    internal Character GetMonsterById(int id)
    {
        for (int i = 0; i < InsideMonsters.Count; i++)
            if (InsideMonsters[i].Id == id)
                return InsideMonsters[i];
        return null;
    }
    internal InsideStory GetStoryBasedOnRarity(Vector2 position, int key)
    {
        var rarity = RandomHelper.Range(position, DateTime.Now.DayOfYear, (int)InsideStory.InsideStoryRarity.Common);
        List<InsideStory> insideStories = new List<InsideStory>();
        for (int i = 0; i < InsideStories.Count; i++)
            if ((int)InsideStories[i].Rarity >= rarity)
                insideStories.Add(InsideStories[i]);
        print("Rarity=" + rarity + " CNT=" + insideStories.Count + " InsideStory =" + insideStories[RandomHelper.Range(position, key, insideStories.Count)].Name);
        return insideStories[RandomHelper.Range(position, key, insideStories.Count)];
    }
    //Monsters
    private void SetMonsterTypes()
    {
        List<int> regionMonsters = Region.Monsters.Split(',').Select(Int32.Parse).ToList();
        foreach (var monster in _characterDatabase.GetCharacters())
        {
            if (regionMonsters.IndexOf(monster.Id) != -1 && monster.IsEnable)
                Monsters.Add(monster);
            if (InsideMonsterIds.Contains(monster.Id) && monster.IsEnable)
                InsideMonsters.Add(monster);
        }
        //Terrain Monster HealthCheck 
        foreach (var terrain in Terrains)
        {
            var healthCheck = false;
            if (!terrain.HasMonster)
                continue;
            foreach (var monster in Monsters)
                if (terrain.Type == monster.FavoriteTerrainTypes)
                {
                    healthCheck = true;
                    break;
                }
            if (!healthCheck)
                throw new Exception("Terrain with no available monster");
        }

        //InsideStory Monster HealthCheck 
        foreach (var insideStory in InsideStories)
        {
            if (insideStory.Actors.Count <= 0)
                continue;
            foreach (var actor in insideStory.Actors)
            {
                if (InsideMonsters.Any(m => m.Id == actor.CharacterId))
                    continue;
                throw new Exception("InsideStory with no available monster. Inside Story: "+ insideStory.Name+ insideStory.Id);
            }
        }

    }
    //Being called by building Interior
    internal List<Character> GetMonstersByStoryId(InsideStory story)
    {
        List<Character> storyActors = new List<Character>();
        foreach (var monster in Monsters)
        foreach (var actor in story.Actors)
            if (actor.CharacterId == monster.Id)
                storyActors.Add(monster);
        return storyActors;
    }
    public static TerrainDatabase Instance()
    {
        if (!_terrainDatabase)
        {
            _terrainDatabase = FindObjectOfType(typeof(TerrainDatabase)) as TerrainDatabase;
            if (!_terrainDatabase)
                Debug.LogError("There needs to be one active TerrainDatabase script on a GameObject in your scene.");
        }
        return _terrainDatabase;
    }
}
