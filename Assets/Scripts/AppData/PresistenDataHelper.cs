using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresistenDataHelper
{
    //Todo: is not being used yet 
    private readonly string _path= Application.persistentDataPath + "//";

    public Sprite LoadSprite(string name)
    {
        var path = _path + name;
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }
}