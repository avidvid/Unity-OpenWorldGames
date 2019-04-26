using System;
using UnityEngine;

[Serializable]
public class CacheContent  {
    
    public Vector3 Location { get; set; }
    public string ObjectType { get; set; }
    public string Content { get; set; }
    public DateTime ExpirationTime { get; set; }
}