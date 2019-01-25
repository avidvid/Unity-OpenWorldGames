using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSettings {

    public const int SceneIdForTerrainView = 0;
    public const int SceneIdForInsideBuilding = 1;
    public const int SceneIdForRecipes = 2;
    public const int SceneIdForStore = 3;
    public const int SceneIdForCredits = 4;
    public const int SceneIdForMiniMap = 5;
    public const int SceneIdForMenu = 6;
    public const int SceneIdForProfile = 7;
    public const int SceneIdForGameOver = 8;
    public const int SceneIdForStart = 9;
    public const int SceneIdForWait = 10;
    public const int SceneIdForResearch = 11;

    public static void BuildStarter(string domain = null, string content = null)
    {
        //Preparing to return to terrain
        GameObject go = new GameObject();
        //Make go unDestroyable
        GameObject.DontDestroyOnLoad(go);
        var starter = go.AddComponent<SceneStarter>();
        Transform player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        starter.PreviousPosition = player.position;
        starter.ShowInventory = true;
        go.name = domain ?? "Unknown";
        starter.Content = content ?? "Unknown";
    }

    internal static void GoToResearchScene()
    {
        BuildStarter();
        SceneManager.LoadScene(SceneSettings.SceneIdForResearch);
    }

    internal static void GoToShopScene(string reason = null)
    {
        BuildStarter(reason, reason);
        SceneManager.LoadScene(SceneSettings.SceneIdForStore);
    }

    internal static void GoToRecipeScene()
    {
        BuildStarter();
        SceneManager.LoadScene(SceneSettings.SceneIdForRecipes);
    }
}
