using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour {
    private TerrainManager _terrainManager;
    private GUIManager _GUIManager;

    private Vector3 previousPosition = Vector3.zero;
    
    private CharacterManager _characterManager;
    private SpriteRenderer _renderer;
    private Cache _cache;



    void Awake()
    {
        _characterManager = CharacterManager.Instance();
        _terrainManager = TerrainManager.Instance();
        _GUIManager = GUIManager.Instance();

    }

    // Use this for initialization
    void Start ()
    {
        _renderer = transform.GetComponentsInChildren<SpriteRenderer>()[0];
        /*foreach (var item in _cache.Find("Player",true))
        {
            if (IsBlocked(item.Location))
                continue;
            else
                previousPosition = item.Location;
        }*/
    }

    private bool IsBlocked(Vector3 itemLocation, ActiveElementType element)
    {
        Vector3 currentPos = itemLocation;
        Vector2 mapPos = _terrainManager.WorldToMapPosition(currentPos);
        var terrain = _terrainManager.SelectTerrain(mapPos.x, mapPos.y);
        if (
            //Terrain Types and charcter type 
            (!terrain.Walkable && _characterManager.MyCharacter.Move == Character.CharacterType.Walk) ||
            (!terrain.Flyable && _characterManager.MyCharacter.Move == Character.CharacterType.Fly) ||
            (!terrain.Swimable && _characterManager.MyCharacter.Move == Character.CharacterType.Swim) ||
            //element + character
            (element != null && !element.ElementTypeInUse.Enterable && _characterManager.MyCharacter.Move != Character.CharacterType.Fly)
            )
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
	void Update ()
	{
	    Vector3 currentPos = transform.position;
        Vector2 mapPos = _terrainManager.WorldToMapPosition(currentPos);
        var element = _terrainManager.GetElement(mapPos);
	    if (element != null)
	        if(element.transform.position.y> currentPos.y)
	            element.transform.GetComponent<SpriteRenderer>().sortingOrder = _renderer.sortingOrder -1;
            else
	            element.transform.GetComponent<SpriteRenderer>().sortingOrder = _renderer.sortingOrder + 1;
        if (IsBlocked(currentPos, element))
        { 
            transform.position = currentPos = previousPosition;
        }
        if (element != null && element.ElementTypeInUse.Enterable)
        {
            //Use 1/4 of Max energy to enter a room
            if (_characterManager.UseEnergy(_characterManager.CharacterSetting.MaxEnergy / 1000))
            {
                //Preparing to switch the scene
                GameObject go = new GameObject();
                //Make go unDestroyable
                GameObject.DontDestroyOnLoad(go);
                var starter = go.AddComponent<SceneStarter>();
                starter.Key = TerrainManager.Key;
                starter.MapPosition = mapPos;
                starter.PreviousPosition = previousPosition;
                starter.PreviousPosition = previousPosition;
                starter.LastScene = "Terrain";
                go.name = "Move SceneStarter";
                //switch the scene
                SceneManager.LoadScene(SceneSettings.SceneIdForInsideBuilding);
            }
            else
                _GUIManager.PrintMessage("Not enough energy to enter this place", Color.yellow);
        }
        previousPosition = currentPos;
    }
}
