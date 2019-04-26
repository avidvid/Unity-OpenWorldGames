using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TerrainActions : MonoBehaviour {
    private TerrainManager _terrainManager;
    private GUIManager _GUIManager;
    private InventoryHandler _inv;

    private static GameObject _popupAction;

    // Use this for initialization
    void Start () {
        _inv = InventoryHandler.Instance();
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
            foreach (var monster in _terrainManager.GetMonster(touchLocation, 0.4f))
            {
                _popupAction.GetComponent<ActionHandler>().SetActiveMonster(monster, transform, "Terrain");
                CreateFloatingAction(touchLocation, "Monster");
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
                    //Performance: Less click approach 
                    //CreateFloatingAction(touchLocation, "Item");
                }
                else
                    _GUIManager.PrintMessage("Too Far from this Target!", Color.yellow);
                return;
            }
            //Digging
            if (distance < 1)
            {
                TerrainIns currentTerrain = _terrainManager.SelectTerrain(touchLocation);
                //print("TerrainIns distance < 1" +currentTerrain.MyInfo());
                if (currentTerrain.Digable)
                {
                    touchLocation.z = pos.z;
                    _popupAction.GetComponent<ActionHandler>().SetActiveTerrain(currentTerrain);
                    CreateFloatingAction(touchLocation, "Dig");
                    return;
                }
            }
            CreateFloatingAction(touchLocation, "Map");
        }
    }
    private void InLineOfSight()
    {
        var pos = transform.position;
        //monster is in the player's line of sight
        foreach (var monster in _terrainManager.GetMonster(pos, 3))
            monster.gameObject.SetActive(true);
    }
    public static void CreateFloatingAction( Vector3 location,string action)
    {
        _popupAction.SetActive(true);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(location);
        _popupAction.transform.position = screenPosition;
        _popupAction.GetComponent<ActionHandler>().SetAction(location,action);
    }
}