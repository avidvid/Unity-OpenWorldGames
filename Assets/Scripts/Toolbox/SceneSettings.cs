using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSettings {

    public const int SceneIdForCharacterScene = 0;
    public const int SceneIdForFightMonster = 1;
    public const int SceneIdForGameOver = 2;
    public const int SceneIdForTerrainView = 3;
    public const int SceneIdForInsideBuilding = 4;
    //public const int SceneIdForLoading = 5;
    public const int SceneIdForMailMessage = 6;
    //public const int SceneIdForMain = 7;
    public const int SceneIdForMenu = 8;
    public const int SceneIdForMiniMap = 9;
    public const int SceneIdForProfile = 10;
    public const int SceneIdForRecipes = 11;
    public const int SceneIdForResearch = 12;
    public const int SceneIdForCredits = 13;
    public const int SceneIdForStart = 14;
    public const int SceneIdForStore = 15;
    public const int SceneIdForWait = 16;

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
