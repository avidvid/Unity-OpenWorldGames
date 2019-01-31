using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour {


    private static CharacterDatabase _characterDatabase;
    private List<Character> _characters = new List<Character>();
    private List<ItemContainer> _characterInventory = new List<ItemContainer>();
    private List<CharacterResearch> _characterResearches = new List<CharacterResearch>();
    private List<UserCharacter> _userCharacters = new List<UserCharacter>();

    private UserPlayer _userPlayer;
    private CharacterSetting _characterSetting;
    private CharacterMixture _characterMixture;
    private CharacterResearching _characterResearching;

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

        LoadUserPlayer();
        if (_userPlayer != null)
        {
            LoadUserCharacters();
            LoadCharacterSetting();
            if (_characterSetting != null)
            {
                //If the Character exists 
                LoadCharacterInventory();
                LoadCharacterMixture();
                LoadCharacterResearches();
                LoadCharacterResearching();
            }
        }
    }
    //CharacterInventory
    public List<ItemContainer> FindCharacterInventory(int playerId)
    {
        return _characterInventory;
    }
    private void LoadCharacterInventory()
    {
        //Empty the Items DB
        _characterInventory.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterInventory.xml");
        //Read the items from Item.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _characterInventory = (List<ItemContainer>)serializer.Deserialize(fs);
        fs.Close();
    }
    public void SaveCharacterInventory()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterInventory.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<ItemContainer>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characterInventory);
        fs.Close();
    }
    internal void InitInventory(CharacterSetting characterSetting)
    {
        _characterInventory.Clear();
        for (int i = 0; i < 30; i++)
            _characterInventory.Add(new ItemContainer());
        SaveCharacterInventory();
    }
    //CharacterResearch
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
    internal CharacterResearching FindCharacterResearching(int playerId)
    {
        return _characterResearching;
    }
    public void SaveCharacterResearching(CharacterResearch charResearch,  DateTime durationMinutes)
    {
        _characterResearching = new CharacterResearching(charResearch,  durationMinutes);
        SaveCharacterResearching();
    }
    public void EmptyCharacterResearching()
    {
        _characterResearching = new CharacterResearching();
        SaveCharacterResearching();
    }
    public void SaveCharacterResearching()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearching.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterResearching));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characterResearching);
        fs.Close();
    }
    private void LoadCharacterResearching()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearching.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterResearching));
        FileStream fs = new FileStream(path, FileMode.Open);
        _characterResearching = (CharacterResearching)serializer.Deserialize(fs);
        fs.Close();
    }
    internal List<CharacterResearch> LoadCharacterResearches(int playerId)
    {
        return _characterResearches;
    }
    private void LoadCharacterResearches()
    {
        //Empty the Items DB
        _characterResearches.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearch.xml");
        //Read the CharacterResearch from CharacterResearch.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<CharacterResearch>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _characterResearches = (List<CharacterResearch>)serializer.Deserialize(fs);
        fs.Close();
    }
    public void SaveCharacterResearches()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterResearch.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<CharacterResearch>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characterResearches);
        fs.Close();
    }
    internal void AddCharacterResearch(CharacterResearch charResearch)
    {
        bool updated = false;
        foreach (var chResearch in _characterResearches)
            if (chResearch.ResearchId == charResearch.ResearchId)
            {
                if ( charResearch.Level <= chResearch.Level )
                    throw new Exception("CharacterResearch Invalid <= ");
                if (chResearch.Level +1 != charResearch.Level)
                    throw new Exception("CharacterResearch Invalid != ");
                chResearch.Level = charResearch.Level;
                updated = true;
                break;
            }
        if (!updated)
            _characterResearches.Add(charResearch);
        SaveCharacterResearches(); 
    }
    //UserCharacters
    public List<Character> GetUserCharacters()
    {
        List<Character> characters = new List<Character>();
        for (int i = 0; i < _characters.Count; i++)
        {
            if (!_characters[i].IsEnable)
                continue;
            for (int j = 0; j < _userCharacters.Count; j++)
                if (_characters[i].Id == _userCharacters[j].CharacterId)
                {
                    if (string.IsNullOrEmpty(_userCharacters[j].CharacterCode))
                        characters.Add(_characters[i]);
                    else
                        //if not purchased yet :This might cause problem later because all of the other items are by reference and this  one is by value 
                        characters.Add(new Character(_characters[i],false));
                    break;
                }
        }
        return characters;
    }
    internal bool ValidateCharacterCode(string characterCode)
    {
        for (int j = 0; j < (_userCharacters).Count; j++)
            if (_userCharacters[j].CharacterCode != null)
                if (_userCharacters[j].CharacterCode == characterCode)
                {
                    _userCharacters[j].CharacterCode = "";
                    SaveUserCharacters();
                    return true;
                }
        return false;
    }
    private void LoadUserCharacters()
    {
        _userCharacters.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "UserCharacter.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserCharacter>));
        FileStream fs = new FileStream(path, FileMode.Open);
        _userCharacters = (List<UserCharacter>)serializer.Deserialize(fs);
        fs.Close();
    }
    private void SaveUserCharacters()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserCharacter.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<UserCharacter>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _userCharacters);
        fs.Close();
    }
    //Characters
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
    //MonsterIns
    internal MonsterIns GenerateMonster(Character monsterCharacter)
    {
        return new MonsterIns(monsterCharacter, _characterSetting.Level);
    }
    //CharacterSetting
    public CharacterSetting FindCharacterSetting(int userPlayerId)
    {
        return _characterSetting;
    }
    public void SaveCharacterSetting(CharacterSetting characterSetting)
    {
        _characterSetting = new CharacterSetting(characterSetting);
        SaveCharacterSetting();
    }
    private void LoadCharacterSetting()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterSetting.xml");
        //Read the CharacterSetting from CharacterSetting.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterSetting));
        FileStream fs = new FileStream(path, FileMode.Open);
        _characterSetting = (CharacterSetting)serializer.Deserialize(fs);
        fs.Close();
    }
    public void SaveCharacterSetting()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterSetting.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterSetting));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characterSetting);
        fs.Close();
    }
    //UserPlayer
    public UserPlayer FindUserPlayer(int id)
    {
        //todo: update on login : long lat last login 
        return _userPlayer;
    }
    public void SaveUserPlayer(UserPlayer userPlayer)
    {
        _userPlayer = new UserPlayer(userPlayer);
        SaveUserPlayer();
    }
    private void LoadUserPlayer()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserPlayer.xml");
        //Read the UserPlayer from UserPlayer.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(UserPlayer));
        FileStream fs = new FileStream(path, FileMode.Open);
        _userPlayer = (UserPlayer)serializer.Deserialize(fs);
        fs.Close();
    }
    public void SaveUserPlayer()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "UserPlayer.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(UserPlayer));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _userPlayer);
        fs.Close();
    }
    //CharacterMixture
    internal CharacterMixture FindCharacterMixture(int id)
    {
        return _characterMixture;
    }
    public void SaveCharacterMixture(ItemContainer item, DateTime durationMinutes)
    {
        _characterMixture = new CharacterMixture(item, durationMinutes);
        SaveCharacterMixture();
    }
    private void LoadCharacterMixture()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterMixture.xml");
        //Read the CharacterMixture from CharacterMixture.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterMixture));
        FileStream fs = new FileStream(path, FileMode.Open);
        _characterMixture = (CharacterMixture)serializer.Deserialize(fs);
        fs.Close();
    }
    public void SaveCharacterMixture()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "CharacterMixture.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(CharacterMixture));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, _characterMixture);
        fs.Close();
    }
    //Instance
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
}