using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoadHelper : MonoBehaviour
{
    private const int Targets = 20;
    private int _loading = 0;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (_loading >= 100)
            SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
    }
    internal void LoadingThumbsUp()
    {
        _loading += (int)Mathf.Ceil(100 / (float)Targets);
        print("_loading = "+ _loading);
    }
}