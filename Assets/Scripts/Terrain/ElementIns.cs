using System;
using UnityEngine;

[Serializable]
public class ElementIns {
    public enum ElementType
    {
        Hole,
        Building,
        Tree,
        Bush,
        Rock
    }
    public int Id;
    public string Name;
    public string MultipleName;
    public int MultipleId;
    public bool Walkable;
    public bool Flyable;
    public bool Swimable;
    public bool Enterable;
    public bool Destroyable;
    public string DropItems;
    public float DropChance;
    public ElementType Type;
    public TerrainIns.TerrainType FavoriteTerrainTypes;
    public bool IsEnable;

    private Sprite _tile ;
    public Sprite GetSprite()
    {
        if (_tile == null)
            BuildSprite();
        return _tile;
    }
    public void BuildSprite()
    {
        if (MultipleId!=-1)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Elements/" + MultipleName);
            // Get specific sprite
            _tile = sprites[MultipleId];
        }
        else
            _tile = Resources.Load<Sprite>("Elements/"+ Name);
    }
    internal void Print()
    {
        Debug.Log(
            " Element:" + Id + 
            " Name:" + Name + 
            " Type:" + Type + 
            " Destroyable:" + Destroyable +
            " Enterable :" + Enterable +
            " IsEnable:" + IsEnable + 
            " FavoriteTerrainTypes:" + FavoriteTerrainTypes
            );
    }
}