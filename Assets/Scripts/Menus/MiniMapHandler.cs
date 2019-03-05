using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniMapHandler : MonoBehaviour {

    private TerrainDatabase _terrainDatabase;
    private CharacterManager _characterManager;

    private int _key;
    private List<TerrainIns> _availableTerrainTypes;
    private List<Marker> _markers;
    private Vector3 _previousPosition = Vector3.zero;
    private Vector2 _mapPosition = Vector2.zero;
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
        Debug.Log("Mini Map-Available TerrainTypes.Count = " + _availableTerrainTypes.Count);
        _markers = GetAllMarkers(_mapPosition.x, _mapPosition.y, _key);
        Debug.Log("Mini Map-Map Markers.Count = " + _markers.Count);
        Debug.Log("Mini Map-" + "currentPos =" + _previousPosition + "mapPos =" + _mapPosition + "Key =" + _key);
        _mapPanel = GameObject.Find("MapPanel");
        for (int i = 0; i < 13; i++)  //25/49/81
        {
            for (int j = 0; j < 9; j++)
            {
                GameObject pixelMap = new GameObject(_markers[i * 9 + j].MarkerIndex + "-" + i + "x" + j + " " + _markers[i * 9 + j].Terrain.Name + _markers[i * 9 + j].Location);
                pixelMap.transform.SetParent(_mapPanel.transform);
                pixelMap.transform.localScale = new Vector3(1.70f, 1.70f, 0);
                pixelMap.AddComponent<Image>().sprite = _markers[i * 9 + j].Terrain.GetSprite();
                if (i == 6 && j == 4) //"Center" 
                {
                    GameObject characterPin = new GameObject("Character Pin " + _characterManager.MyCharacter.Name);
                    characterPin.transform.SetParent(pixelMap.transform);
                    characterPin.transform.localScale = new Vector3(0.3f, 0.3f, 0);
                    characterPin.transform.localPosition = new Vector3(5, 5, 0);
                    characterPin.AddComponent<Image>().sprite = _characterManager.MyCharacter.GetSprite();
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