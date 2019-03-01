using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TerrainActions : MonoBehaviour {
    private TerrainManager _terrainManager;
    private CharacterManager _characterManager;
    private GUIManager _GUIManager;
    private InventoryHandler _inv;

    public KeyCode KeyToDrop = KeyCode.D;
    private static GameObject _popupAction;

    // Use this for initialization
    void Start () {
        _inv = InventoryHandler.Instance();
        _characterManager = CharacterManager.Instance();
        _terrainManager = TerrainManager.Instance();
        _GUIManager = GUIManager.Instance();
        _popupAction = GameObject.Find("Popup Action");
    }
	
	// Update is called once per frame
    void Update()
    {
        if (_inv.InventoryPanelStat())
            return;

        InLineOfSight();
        var pos = transform.position;
        pos.z += 0.01f;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            _popupAction.SetActive(false);
            var touchLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distance = Vector2.Distance(touchLocation, pos);

            //AttackTarget Also check AttackInside
            var monster = _terrainManager.GetMonster(touchLocation, 0.4f);
            if (monster != null)
            {
                if (distance < (int) _characterManager.MyCharacter.AttackR)
                {
                    _popupAction.GetComponent<ActionHandler>().SetActiveMonster(monster, transform,"Terrain");
                    _popupAction.GetComponent<ActionHandler>().AttackMonster();
                    //Less click approach 
                    //CreateFloatingAction(touchLocation, "Monster");
                }
                else
                    _GUIManager.PrintMessage("Too Far from this Target!",Color.yellow);
                return;
            }
            //Consume Element
            var currentElement = _terrainManager.GetElement(touchLocation);
            if (currentElement != null)
            {
                if (distance < 1)
                {
                    _popupAction.GetComponent<ActionHandler>().SetActiveElement(currentElement);
                    CreateFloatingAction(touchLocation, "Element");
                    return;
                }
            }                
            //Pick Up Item
            var currentItem = _terrainManager.GetDropItem(touchLocation);
            if (currentItem != null)
            {
                print("currentItem happening :" + distance + " _item:" + currentItem.ItemTypeInUse.Name);
                if (distance < 1)
                {
                    _popupAction.GetComponent<ActionHandler>().SetActiveItem(currentItem,"Terrain");
                    _popupAction.GetComponent<ActionHandler>().GrabItem();
                    //Less click approach 
                    //CreateFloatingAction(touchLocation, "Item");
                }
                else
                    _GUIManager.PrintMessage("Too Far from this Target!", Color.yellow);
                return;
            }
            if (distance < 1)
            {
                TerrainIns currentTerrain = _terrainManager.SelectTerrain(touchLocation);
                if (currentTerrain != null)
                {
                    if (currentTerrain.Diggable)
                    {
                        touchLocation.z = pos.z;
                        _popupAction.GetComponent<ActionHandler>().SetActiveTerrain(currentTerrain);
                        CreateFloatingAction(touchLocation, "Dig");
                    }
                    return;
                }
            }
            else
                CreateFloatingAction(touchLocation, "Map");
        }
        //todo:delete
        if (Input.GetKeyDown(KeyToDrop))
        {
            //print("###inside TerrainAction: " + pos);
            TerrainIns currentTerrain = _terrainManager.SelectTerrain(pos.x, pos.y);
            if (currentTerrain != null)
                if (currentTerrain.Walkable)
                    _terrainManager.TerrainDropItem(pos, 1, "0,1,2,3,4");
        }
    }
    private void InLineOfSight()
    {
        var myPos = transform.position;
        var monster3 = _terrainManager.GetMonster(myPos, 3);
        //monster is in the player's line of sight
        if (monster3!= null)
            monster3.gameObject.SetActive(true); 
    }
    public static void CreateFloatingAction( Vector3 location,string action)
    {
        _popupAction.SetActive(true);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(location);
        _popupAction.transform.position = screenPosition;
        _popupAction.GetComponent<ActionHandler>().SetAction(location,action);
    }


}
