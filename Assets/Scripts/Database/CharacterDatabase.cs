using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour {

    private static CharacterDatabase _characterDatabase;
    private List<Character> _characters = new List<Character>();
    private List<Research> _researches = new List<Research>();

    void Awake()
    {
        _characterDatabase = CharacterDatabase.Instance();
        Debug.Log("***CDB*** Start!");
        _characters = LoadCharacters();
        Debug.Log("CDB-Characters.Count = " + _characters.Count);
        _researches = LoadResearches();
        Debug.Log("CDB-Researches.Count = " + _researches.Count);
        Debug.Log("***CDB*** Success!");
    }

    #region Characters
    public Character GetCharacterById(int id)
    {
        for (int i = 0; i < _characters.Count; i++)
        {
            if (_characters[i].Id == id)
                return _characters[i];
        }
        return null;
    }
    internal List<Character> GetCharacters()
    {
        return _characters;
    }
    private List<Character> LoadCharacters()
    {
        //Empty the Characters DB
        _characters.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Character.xml");
        //Read the Characters from Character.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<Character>));
        FileStream fs = new FileStream(path, FileMode.Open);
        var characters = (List<Character>)serializer.Deserialize(fs);
        fs.Close();
        return characters;
    }
    private void SaveCharacters()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Character.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Character>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characters);
        fs.Close();
    }
    internal void UpdateCharacters(List<Character> characters)
    {
        _characters = characters;
        SaveCharacters();
    }
    #endregion
    #region Research
    public List<Research> LoadResearches()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Research.xml");
        //Read the Recipes from Recipe.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<Research>));
        FileStream fs = new FileStream(path, FileMode.Open);
        List<Research> researches = (List<Research>)serializer.Deserialize(fs);
        fs.Close();
        return researches;
    }
    private void SaveResearches()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Research.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Research>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _researches);
        fs.Close();
    }
    internal void UpdateResearches(List<Research> researches)
    {
        _researches = researches;
        SaveResearches();
    }
    internal List<Research> GetResearches()
    {
        return _researches;
    }
    internal Research GetResearchById(int id)
    {
        for (int i = 0; i < _researches.Count; i++)
            if (_researches[i].Id == id)
                return _researches[i];
        return null;
    }
    #endregion
    #region CharacterDatabase Instance
    public static CharacterDatabase Instance()
    {
        if (!_characterDatabase)
        {
            _characterDatabase = FindObjectOfType(typeof(CharacterDatabase)) as CharacterDatabase;
            if (!_characterDatabase)
                Debug.LogError("There needs to be one active ItemDatabase script on a GameObject in your scene.");
        }
        return _characterDatabase;
    }
    #endregion
}