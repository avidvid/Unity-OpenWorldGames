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
        LoadCharacters();
        Character tempCharacter = new Character(
            _characters.Count,
            "Ring TV",
            "Ring TV",
            Character.CharacterType.Walk,
            Character.AttackRange.Short,
            Character.DefenseRange.Short,
            Character.AttackType.Magic,
            Character.AttackType.Magic,
            20, //Att
            20, //def
            Character.SpeedType.None,
            Character.BodyType.Tank,
            Character.CarryType.Normal,
            Character.ViewType.Medium,
            Character.CharRarity.Common,
            TerrainIns.TerrainType.Land,
            "0",                                //Drop Item
            0.7f                                //Chance
        );
        if (!tempCharacter.Exist(_characters))
        {
            _characters.Add(tempCharacter);
            SaveCharacters();
        }

        _researches = _characterDatabase.LoadResearches();
    }


    #region Characters
    public Character FindCharacter(int id)
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
    private void LoadCharacters()
    {
        //Empty the Characters DB
        _characters.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Character.xml");
        //Read the Characters from Character.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<Character>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _characters = (List<Character>)serializer.Deserialize(fs);
        fs.Close();
    }
    private void SaveCharacters()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Character.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<Character>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characters);
        fs.Close();
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
    internal List<Research> GetResearches()
    {
        return _researches;
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