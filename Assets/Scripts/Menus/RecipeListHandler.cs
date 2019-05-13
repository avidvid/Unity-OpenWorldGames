using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RecipeListHandler : MonoBehaviour {

    private CharacterManager _characterManager;
    private ItemDatabase _itemDatabase;

    //Recipe Prefab
    public GameObject RecipeContent;

    private List<Recipe> _recipes ;
    private GameObject _addRecipePanel;
    internal bool SceneRefresh;

    void Awake()
    {
        _itemDatabase = ItemDatabase.Instance();
        _characterManager = CharacterManager.Instance();
        _addRecipePanel = GameObject.Find("AddRecipePanel");
    }
    // Use this for initialization
    void Start ()
    {
        var contentPanel = GameObject.Find("ContentPanel");
        var recipeInfo = GameObject.Find("RecipeInfo").GetComponent<TextMeshProUGUI>();
        _addRecipePanel.SetActive(false);
        _recipes = _characterManager.UserRecipes;
        recipeInfo.text = "Your recipes: " + _recipes.Count(p => p.IsEnable);
        for (int i = 0; i < _recipes.Count; i++)
        {
            if (!_recipes[i].IsEnable)
            {
                _addRecipePanel.SetActive( true);
                continue;
            }
            List<ItemContainer> recipeItems = _itemDatabase.RecipeItems(_recipes[i]);
            List<int> recipeCnt = new List<int> { _recipes[i].FirstItemCnt, _recipes[i].SecondItemCnt, _recipes[i].FinalItemCnt };

            GameObject recipeObject = Instantiate(RecipeContent);
            recipeObject.transform.SetParent(contentPanel.transform);

            recipeObject.transform.name = "Recipe " + _recipes[i].Id;

            recipeObject.transform.localScale = Vector3.one;
            var items = recipeObject.GetComponentsInChildren<Image>();
            int itemNum = 0;
            for (int j = 0; j < items.Length; j++)
            {
                if (items[j].gameObject.tag != "Item")
                    continue;
                var texts = items[j].GetComponentsInChildren<TextMeshProUGUI>();
                texts[0].text = recipeCnt[itemNum].ToString();
                texts[1].text = recipeItems[itemNum].Name;
                items[j].sprite = recipeItems[itemNum].GetSprite();
                items[j].name = recipeItems[itemNum].Name;
                itemNum++;
            }
        }
    }

    void Update()
    {
        if (SceneRefresh)
            RefreshTheScene();
    }
    public void ValidateRecipeCode()
    {
        var recipeCode = _addRecipePanel.GetComponentInChildren<TMP_InputField>().text;
        if (!string.IsNullOrEmpty(recipeCode))
            ValidateRecipeCode(recipeCode);
    }

    internal void ValidateRecipeCode(string recipeCode)
    {
        var apiGatewayConfig = ApiGatewayConfig.Instance();
        if (apiGatewayConfig!= null)
        {
            var userRecipes = _characterManager.MyUserRecipes;
            for (int i = 0; i < userRecipes.Count; i++)
                if (userRecipes[i].RecipeCode == "0000")
                    apiGatewayConfig.PutUserRecipe(userRecipes[i], recipeCode);
        }
    }

    public void RefreshTheScene()
    {
        SceneManager.LoadScene(SceneSettings.SceneIdForRecipes);
    }

    public void BackToMainScene()
    {
        SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }
}
