using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

public class TerrainDatabase : MonoBehaviour {

    private static TerrainDatabase _terrainDatabase;
    private CharacterDatabase _characterDatabase;

    private List<Region> _regions;
    private Region _region;

    private List<TerrainIns> _terrains = new List<TerrainIns>();
    private List<TerrainIns> _myTerrains = new List<TerrainIns>();

    private List<ElementIns> _elements = new List<ElementIns>();
    private List<ElementIns> _myElements = new List<ElementIns>();

    private List<InsideStory> _insideStories = new List<InsideStory>();
    private List<InsideStory> _myInsideStories = new List<InsideStory>();

    private List<Character> _monsters = new List<Character>();
    private List<Character> _insideMonsters = new List<Character>();


    #region TerrainDatabase Instance
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
    #endregion

    void Start()
    {
        _terrainDatabase = TerrainDatabase.Instance(); 
         _characterDatabase = CharacterDatabase.Instance();
        Debug.Log("***TDB*** Start!");
    }
    #region Regions    
    internal void UpdateRegions(List<Region> regions)
    {
        _regions = new List<Region>(regions.FindAll(s => s.IsEnable));
        Debug.Log("TDB-Regions.Count = " + _regions.Count);
    }
    internal List<Region> GetRegions()
    {
        return _regions;
    }
    internal int GetRegionKey()
    {
        return _region.Key;
    }
    internal Region GetRegion()
    {
        return _region;
    }
    internal void SetRegion(float latitude, float longitude)
    {
        for (int i = 0; i < _regions.Count; i++)
            if (Mathf.Abs(_regions[i].Latitude - latitude) < 10 && Mathf.Abs(_regions[i].Longitude - longitude) < 10)
                _region= _regions[i];
        _region = _regions[0];
        _region.Print();
        //Set Terrains based on the user region
        SetRegionTerrainTypes();
        Debug.Log("TDB-RegionTerrains.Count = " + _myTerrains.Count);
        SetRegionMonsterTypes();
        Debug.Log("TDB-RegionMonsters.Count = " + _monsters.Count);
        //Set InsideStories based on the user region
        SetRegionInsideStoryTypes();
        Debug.Log("TDB-RegionInsideStories.Count = " + _myInsideStories.Count);
        Debug.Log("TDB-RegionInsideMonsters.Count = " + _insideMonsters.Count);
    }
    internal Vector2 GetRegionLocation(int key)
    {
        for (int i = 0; i < _regions.Count; i++)
            if (_regions[i].Key == key)
                return new Vector2(_regions[i].Latitude * 1000, _regions[i].Longitude * 1000);
        // Vector2 only accept 2 decimal points
        return Vector2.zero;
    }
    #endregion
    #region Terrains
    private List<TerrainIns> LoadTerrains()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Terrain.xml");
        //Read the Recipes from Terrain.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<TerrainIns>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var terrains = (List<TerrainIns>)serializer.Deserialize(fs);
        fs.Close();
        return terrains;
    }
    internal void UpdateTerrains(List<TerrainIns> terrains)
    {
        //Load Terrains from database 
        _terrains = terrains;
        Debug.Log("TDB-Terrains.Count = " + _terrains.Count);
    }
    private void SetRegionTerrainTypes()
    {
        List<int> regionTerrains = _region.Terrains.Split(',').Select(Int32.Parse).ToList();
        foreach (var terrain in _terrains)
            if (regionTerrains.IndexOf(terrain.Id) != -1  && terrain.IsEnable)
                _myTerrains.Add(terrain);
    }
    internal List<TerrainIns> GetTerrains()
    {
        return _myTerrains;
    }
    #endregion
    #region Elements
    internal void UpdateElements(List<ElementIns> elements)
    {
        //Load Elements from database 
        _elements = elements;
        Debug.Log("TDB-Elements.Count = " + _elements.Count);
        //Set Elements based on the user region
        SetRegionElementTypes();
        Debug.Log("TDB-RegionElements.Count = " + _myElements.Count);
    }
    private void SetRegionElementTypes()
    {
        List<int> regionElements = _region.Elements.Split(',').Select(Int32.Parse).ToList();
        foreach (var element in _elements)
            if (regionElements.IndexOf(element.Id) != -1 && element.IsEnable)
            {
                //Terrain Element HealthCheck 
                var healthCheck = false;
                foreach (var terrain in _terrains)
                {
                    if (!terrain.HasElement)
                        continue;
                    if (terrain.Type == element.FavoriteTerrainTypes)
                    {
                        healthCheck = true;
                        break;
                    }
                }
                if (!healthCheck)
                    throw new Exception("Terrain with no available element");
                _myElements.Add(element);
            }
    }
    internal List<ElementIns> GetElements()
    {
        return _myElements;
    }
    #endregion
    #region InsideStories    
    internal List<InsideStory> GetInsideStories()
    {
        return _insideStories;
    }
    internal void UpdateInsideStories(List<InsideStory> insideStories)
    {
        //Load InsideStories from database 
        _insideStories = insideStories;
        Debug.Log("TDB-InsideStories.Count = " + _insideStories.Count);
    }
    private void SetRegionInsideStoryTypes()
    {
        List<int> regionInsideStories = _region.InsideStories.Split(',').Select(Int32.Parse).ToList();
        foreach (var inStory in _insideStories)
            if (regionInsideStories.IndexOf(inStory.Id) != -1 && inStory.IsEnable)
            {
                _myInsideStories.Add(inStory);
                foreach (var actor in inStory.Actors)
                    _insideMonsters.Add(_characterDatabase.GetCharacterById(actor.CharacterId));
            }
    }
    internal InsideStory GetStoryBasedOnRarity(Vector2 position, int key)
    {
        var rarity = RandomHelper.Range(position, DateTime.Now.DayOfYear, (int)DataTypes.Rarity.Common);
        List<InsideStory> insideStories = new List<InsideStory>();
        foreach (var inStory in _myInsideStories)
        {
            if ((int)inStory.Rarity >= rarity)
                insideStories.Add(inStory);
        }
        print("Rarity=" + rarity + " CNT=" + insideStories.Count + " InsideStory =" + insideStories[RandomHelper.Range(position, key, insideStories.Count)].Name);
        return insideStories[RandomHelper.Range(position, key, insideStories.Count)];
    }
    internal Character GetMonsterById(int id)
    {
        foreach (var monster in _monsters)
            if (monster.Id == id)
                return monster;
        return null;
    }
    internal Character GetInsMonsterById(int id)
    {
        foreach (var inMonster in _insideMonsters)
            if (inMonster.Id == id)
                return inMonster;
        return null;
    }
    #endregion
    #region Monsters    
    internal List<Character> GetMonsters()
    {
        return _monsters;
    }
    internal void SetRegionMonsterTypes()
    {
        //Set Monsters based on the user region
        List<int> regionMonsters = _region.Monsters.Split(',').Select(Int32.Parse).ToList();
        foreach (var monster in _characterDatabase.GetCharacters())
            if (regionMonsters.IndexOf(monster.Id) != -1 && monster.IsEnable)
            {
                //Terrain Monster HealthCheck 
                var healthCheck = false;
                foreach (var terrain in _myTerrains)
                {
                    if (!terrain.HasMonster)
                        continue;
                    if (terrain.Type == monster.FavoriteTerrainTypes)
                    {
                        healthCheck = true;
                        break;
                    }
                }
                if (!healthCheck)
                    throw new Exception("Terrain with no available monster");
                _monsters.Add(monster);
            }
    }
    #endregion
}