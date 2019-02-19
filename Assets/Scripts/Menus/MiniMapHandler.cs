using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniMapHandler : MonoBehaviour {

    private Vector3 _previousPosition = Vector3.zero;

    private List<TerrainIns> _availableTerrainTypes = new List<TerrainIns>();
    private int _key;
    private TerrainDatabase _terrainDatabase;
    private CharacterManager _characterManager;
    
    public Vector2 _mapPosition = Vector2.zero;

    private List<Marker> _markers;
    public GameObject PixelMap;
    private GameObject _mapPanel;
    
    void Start()
    {
        _terrainDatabase = TerrainDatabase.Instance();
        _characterManager = CharacterManager.Instance();
        var starter = GameObject.FindObjectOfType<SceneStarter>();
        if (starter != null)
        {
            _mapPosition = starter.MapPosition;
            _key = starter.Key;
            _previousPosition = starter.PreviousPosition;
        }
        _availableTerrainTypes = _terrainDatabase.GetTerrains();

        print("_availableTerrainTypes count = " + _availableTerrainTypes.Count);
        _markers = GetAllMarkers(_mapPosition.x, _mapPosition.y, _key);
        print("_terrains count = " + _markers.Count);

        Debug.Log("OpenMiniMap " + "currentPos =" + _previousPosition + "mapPos =" + _mapPosition + "Key =" + _key);


        _mapPanel = GameObject.Find("MapPanel");
        for (int i = 0; i < 13; i++)  //25/49/81
        {
            for (int j = 0; j < 9; j++)
            {
                GameObject pixelMap = Instantiate(PixelMap);
                pixelMap.transform.SetParent(_mapPanel.transform);
                pixelMap.transform.localScale = new Vector3(2, 2, 0);
                pixelMap.transform.name = "PixelMap "+ _markers[i * 9 + j].MarkerIndex+"-"  + (i + 1) + "x" + (j + 1) + _markers[i * 9 + j].Terrain.Name+ _markers[i * 9 + j].Location;

                var images = pixelMap.GetComponentsInChildren<Image>();
                images[0].sprite = _markers[i * 9 + j].Terrain.GetSprite();
                if (i == 6 && j == 4) //"Center" 
                {
                    images[1].sprite =_characterManager.MyCharacter.GetSprite();
                }
            }
        }
    }

    public List<Marker> GetAllMarkers(float x, float y, int key)
    {
        var markers = new List<Marker>();
        x = (int)x >> 4;
        y = (int)y >> 4;
        int mIndex = 0;
        for (int iX = 6; iX >= -6; iX--)
        {
            for (int iY = 4; iY >= -4; iY--)
            {
                var terrain = _availableTerrainTypes[RandomHelper.Range(x + iX, y + iY, key, _availableTerrainTypes.Count)];
                Vector2 location = new Vector2((int)(x + iX) << 4, (int)(y + iY) << 4);
                markers.Add(new Marker(terrain, location, mIndex++));
                //todo: delete
                //if (iX== iY && iX== 0)
                //    Debug.Log("Center-" + terrain.Name + " " + location);
                //else
                //Debug.Log(mIndex +"-"+ terrain.Name +" " + location );
            }
        }
        return markers;
    }

    public void BackToMainScene()
    {
        SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }
}
