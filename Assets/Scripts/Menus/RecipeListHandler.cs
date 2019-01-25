using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RecipeListHandler : MonoBehaviour {


    private ItemDatabase _itemDatabase;
    public GameObject RecipeContent;

    private GameObject _contentPanel;
    private List<Recipe> _recipes = new List<Recipe>();
    void Awake()
    {
        _itemDatabase = ItemDatabase.Instance();

        _contentPanel = GameObject.Find("ContentPanel");
    }


    // Use this for initialization
    void Start ()
    {
        _recipes = _itemDatabase.UserRecipeList();
        for (int i = 0; i < _recipes.Count; i++)
        {
            List<ItemContainer> recipeItems = _itemDatabase.RecipeItems(_recipes[i]);
            List<int> recipeCnt = new List<int> { _recipes[i].FirstItemCnt, _recipes[i].SecondItemCnt, _recipes[i].FinalItemCnt };

            GameObject recipeObject = Instantiate(RecipeContent);
            recipeObject.transform.SetParent(_contentPanel.transform);

            recipeObject.transform.name = "Recipe " + _recipes[i].Id;

            recipeObject.transform.localScale = Vector3.one;
            var items = recipeObject.GetComponentsInChildren<Image>();
            for (int j = 0; j < items.Length; j++)
            {
                var texts = items[j].GetComponentsInChildren<Text>();
                texts[0].text = recipeCnt[j].ToString();
                texts[1].text = recipeItems[j].Name;
                items[j].sprite = recipeItems[j].GetSprite();
                items[j].name = recipeItems[j].Name;
            }
        }
    }

    public void BackToMainScene()
    {
        SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }
}
