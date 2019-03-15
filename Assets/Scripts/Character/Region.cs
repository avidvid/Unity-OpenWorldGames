using System;
using UnityEngine;

[Serializable]
public class Region
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public int Key { get; set; }
    public string Elements { get; set; }
    public string Monsters { get; set; }
    public string InsideStories { get; set; }
    public string Terrains { get; set; }
    public bool IsEnable { get; set; }
    public int HealthCheck { get; set; }

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