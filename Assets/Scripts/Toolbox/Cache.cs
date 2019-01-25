using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Linq;

public class Cache : MonoBehaviour {

    private static Cache _cache;

    private readonly List<CacheContent> _contents = new List<CacheContent>();
    public List<CacheContent> PersistContents = new List<CacheContent>();

    void Start()
    {
        _cache = Cache.Instance();
        LoadPersistContents();
        PurgePersistContents();
    }

    private void PurgePersistContents()
    {
        if (PersistContents.Count == 0)
            return;
        bool updated=false;
        foreach (var c in new List<CacheContent>(PersistContents) )
            if (c.ExpirationTime < DateTime.Now)
            {
                PersistContents.Remove(c);
                updated = true;
            }
        if (updated)
            SavePersistContents();
    }

    public void Add(CacheContent content)
    {
        _contents.Add(content);
    }

    public void PersistAdd(CacheContent content)
    {
        PersistContents.Add(content);
        SavePersistContents();
    }
    private void LoadPersistContents()
    {
        //Empty the PersistContents
        PersistContents.Clear();
        string path = Path.Combine(Application.streamingAssetsPath, "Cache.xml");
        //Read the CacheContents from Cache.xml file in the streamingAssets folder
        XmlSerializer serializer = new XmlSerializer(typeof(List<CacheContent>));
        FileStream fs = new FileStream(path, FileMode.Open);
        PersistContents = (List<CacheContent>)serializer.Deserialize(fs);
        fs.Close();
    }

    private void SavePersistContents()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Cache.xml");
        XmlSerializer serializer = new XmlSerializer(typeof(List<CacheContent>));
        FileStream fs = new FileStream(path, FileMode.Create);
        serializer.Serialize(fs, PersistContents);
        fs.Close();
    }

    internal IEnumerable<CacheContent> FindPersist(
                                string objectType
                            )
    {
        foreach (var c in PersistContents)
        {
            if (c.ObjectType != objectType)
                continue;
            yield return c;
        }
    }

    public IEnumerable<CacheContent> Find(
                        string objectType, 
                        Vector3 near, // Location
                        float radius, //in what range we need to look for the object 
                        bool destroy //destroy the rest if true
                            )
    {
        //print("Find and destroy: " + destroy + " - " + objectType + " around " + radius + " of " + near + "_contents: "+ _contents.Count);
        //int cnt = 0;
        List<CacheContent> toDelete = new List<CacheContent>();
        foreach (var c in _contents)
        {
            //print("_contents item number: " + cnt++ + " -Content " + c.Content +
            //      " -ExpirationTime " + c.ExpirationTime + " -Location " + c.Location +
            //      " -ObjectType " + c.ObjectType + " -Distance " + Vector3.Distance(near, c.Location));
            if (c.ObjectType != objectType)
                continue;
            if (Vector2.Distance(near, c.Location) < radius)
            {
                //lets the loop continue and return all the objects in that radius
                yield return c;
                if (destroy)
                    toDelete.Add(c);
            }
        }
        if (destroy)
            foreach (var c in toDelete)
                _contents.Remove(c);
    }
    //todo: delete 2018-10-15 ?? Error around here keep it longer 
    //public void SyncItems(string objectType, List<ActiveItemType> items)
    //{
    //    List<CacheContent> toDelete = new List<CacheContent>();
    //    foreach (var c in _contents)
    //    {
    //        if (c.ObjectType != objectType)
    //            continue;
    //        bool objectExists = false;
    //        for (int i = 0; i < items.Count; i++)
    //        {
    //            if (items[i].ItemTypeInUse.Id == Int32.Parse(c.Content)
    //                && items[i].Location == c.Location)
    //            {
    //                objectExists = true;
    //                break;
    //            }
    //        }
    //        if (!objectExists)
    //            toDelete.Add(c);
    //    }
    //    foreach (var c in toDelete)
    //        _contents.Remove(c);
    //}
    public string SerializeCache()
    {
        List<string> cacheArray = new List<string>();
        foreach (var item in _contents)
        {
            cacheArray.Add(Serialize(item));
        }
        return Serialize(cacheArray.ToArray());
    }
    public string Serialize(object target)
    {
        var serializer = new XmlSerializer(target.GetType());
        using (var writer = new StringWriter())
        {
            serializer.Serialize(writer, target);
            return writer.ToString();
        }
    }
    public void DeserializeCache(string content)
    {
        string[] serList = Deserialize<string[]>(content);
        _contents.Clear();
        foreach (var item in serList)
        {
            _contents.Add(Deserialize<CacheContent>(item));
        }
    }
    public T Deserialize<T>(string content)
    {
        var serializer = new XmlSerializer(typeof(T));
        using (var stream = new StringReader(content))
        {
            return (T)serializer.Deserialize(stream);
        }
    }
    public void Print()
    {
        print("Cache count: " + _contents.Count);
        foreach (var c in _contents)
            print("Cache count: " + c.ObjectType + c.Location + c.Content);
    }
    //To use in the scenes dat don't have the Cache object already 
    public static Cache Get()
    {
        var cache = GameObject.FindObjectOfType<Cache>();
        if (cache == null)
        {
            GameObject go = new GameObject
            {
                name = "Cache"
            };
            GameObject.DontDestroyOnLoad(go);
            cache = go.AddComponent<Cache>();
        }
        return cache;
    }
    public static Cache Instance()
    {
        if (!_cache)
        {
            _cache = FindObjectOfType(typeof(Cache)) as Cache;
            if (!_cache)
                Debug.LogError("There needs to be one active Cache script on a GameObject in your scene.");
        }
        return _cache;
    }

    internal int GetCacheCount()
    {
        return _contents.Count;
    }
}
