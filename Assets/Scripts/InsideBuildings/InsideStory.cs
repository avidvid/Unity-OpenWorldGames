using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Actor
{
    public int CharacterId;
    public int CharacterCnt;
    public int MinLevel;
    public int LocationIndex;
}

[Serializable]
public class InsideStory {



    public int Id;
    public string Name;
    public string Description;
    public DataTypes.Rarity Rarity;
    public bool IsEnable;
    public List<Actor> Actors;

    internal void Print()
    {
        if (Id == -1)
        {
            Debug.Log("InsideStory Id:" + Id);
            return;
        }
        Debug.Log("InsideStory Id:" + Id + " Name:" + Name + " Description:" + Description + " IsEnable:" + IsEnable + " Actors:" + Actors.Count );
    }
}
