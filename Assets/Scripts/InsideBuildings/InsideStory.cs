using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InsideStory {
    public class Actor
    {
        public int CharacterId;
        public int CharacterCnt;
        public int MinLevel;
        public int LocationIndex;
        public int HealthCheck;
    }
    public enum InsideStoryRarity
    {
        Sacred = 0,
        Legendary = 1,
        Saga = 3,
        Rare = 10,
        Uncommon = 40,
        Common = 100
    }
    public int Id { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public InsideStoryRarity Rarity { get; set; }
    public bool IsEnable { get; set; }
    public List<Actor> Actors { get; set; }

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
