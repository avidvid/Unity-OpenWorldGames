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

    void Start()
    {
        _characterDatabase = Instance();
        Debug.Log("***CDB*** Start!");
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
    internal void UpdateCharacters(List<Character> characters)
    {
        _characters = new List<Character>(characters.FindAll(s => s.IsEnable));
        Debug.Log("CDB-Characters.Count = " + _characters.Count);
    }
    #endregion
    #region Research
    internal void UpdateResearches(List<Research> researches)
    {
        _researches = new List<Research>(researches.FindAll(s => s.IsEnable));
        Debug.Log("CDB-Researches.Count = " + _researches.Count);
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