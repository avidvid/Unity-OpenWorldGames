using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;

[Serializable]
public class TerrainIns
{
    public enum TerrainType
    {
        Land,
        Desert,
        Rock,
        Water,
        Snow,
        Death,
        Lava
    }
    public int Id;
    public string Name;
    public bool Digable;
    public bool Walkable;
    public bool Flyable;
    public bool Swimable;
    public bool HasElement;
    public bool HasMonster;
    public string DropItems;
    public float DropChance;
    public TerrainType Type;
    public int Tiles;
    public int AnimationController;
    public bool IsEnable;
    private List<Sprite> _tiles = new List<Sprite>();
    private List<RuntimeAnimatorController> _rac = new List<RuntimeAnimatorController>();
    public Sprite GetTile (float x,float y,int key)
    {
        if (_tiles.Count == 0)
            GetSprites();
        int iconId = RandomHelper.Range(x, y, key, Tiles);
        return _tiles[iconId];
    }
    public void GetSprites()
    {
        _tiles = Resources.LoadAll<Sprite>("Terrain/" + Name).ToList();
    }
    public Sprite GetSprite()
    {
        Sprite[] terrainSprites = Resources.LoadAll<Sprite>("Terrain/" + Name);
        return terrainSprites[Random.Range(0,terrainSprites.Length)];
    }
    public RuntimeAnimatorController GetAnimation(float x, float y, int key)
    {
        if (_rac.Count == 0)
            GetAnimations();
        int iconId = RandomHelper.Range(x, y, key, AnimationController);
        return _rac[iconId];
    }
    public void GetAnimations()
    {
        _rac = Resources.LoadAll<RuntimeAnimatorController>("Terrain/" + Name).ToList();
    }
    internal void Print()
    {
        Debug.Log("Terrain = " + MyInfo());
    }
    internal string MyInfo()
    {
        return Id +
               " Name:" + Name +
               " Type:" + Type +
               " Tiles:" + Tiles +
               " HasElement:" + HasElement +
               " IsEnable:" + IsEnable +
               " Digable:" + Digable +
               " AnimationController:" + AnimationController;
    }
}