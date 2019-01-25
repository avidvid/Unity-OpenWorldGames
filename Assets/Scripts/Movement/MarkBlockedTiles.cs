using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkBlockedTiles : MonoBehaviour {


    private float radius = 0.25f;
    private float tileRange = 4;

    private TerrainManager _terrainManager;
    void Start()
    {
        _terrainManager = TerrainManager.Instance();
    }

    public bool NotWalkable()
    {
        return true;
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
            for (float x = transform.position.x - tileRange ; x <= transform.position.x + tileRange; x+= radius)
            {
                for (float y = transform.position.y - tileRange; y <= transform.position.y + tileRange; y += radius)
                {
                    Vector3 worldPos= new Vector3(x, y, 0);
                    var mapPos = _terrainManager.WorldToMapPosition(worldPos);
                    var terrain = _terrainManager.SelectTerrain(mapPos.x, mapPos.y);

                    //UnityEditor.Handles.color = Color.red;
                    //UnityEditor.Handles.DrawWireDisc(worldPos, Vector3.back, radius);
                    var element = _terrainManager.GetElement(mapPos);
                    if (!terrain.Walkable || (element != null && !element.ElementTypeInUse.Enterable))
                    {
                        UnityEditor.Handles.color = Color.white;
                        UnityEditor.Handles.DrawWireDisc(worldPos, Vector3.back, radius);
                    }
                }
            }
    }


}
