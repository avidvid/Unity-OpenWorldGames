using System;
using UnityEngine;

[Serializable]
public class Region
{
    public int Id;
    public string Name;
    public string Description;
    public float Longitude;
    public float Latitude;
    public int Key;
    public string Elements;
    public string Monsters;
    public string InsideStories;
    public string Terrains;
    public bool IsEnable;

    internal void Print()
    {
        Debug.Log("Region = " + MyInfo());
    }
    internal string MyInfo()
    {
        return Id + "-" + Name + " (" + Key + ")";
    }
    internal string MapInfo()
    {
        return Name + " (" + Description + ")";
    }
}