using System;
using System.Collections.Generic;
using UnityEngine;

//TODO: add this later maybe for now just have it as an anchor 
[Serializable]
public class BaseStory
{
    public class Capture
    {
        public int CharacterId;
        public int LocationIndex;
    }

    public int Id { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public bool Locked { get; set; }
    public int CageCnt { get; set; }
    public DateTime LockUntil { get; set; }
    public List<Capture> Captures { get; set; }

    internal void Print()
    {
        if (Id == -1)
        {
            Debug.Log("BaseStory Id:" + Id);
            return;
        }
        Debug.Log("BaseStory Id:" + Id + " Name:" + Name + " Description:" + Description +
                  " Locked:" + Locked + " Captures:"+ Captures.Count);
    }

}
