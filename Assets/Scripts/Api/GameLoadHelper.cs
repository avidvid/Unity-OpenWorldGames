using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoadHelper : MonoBehaviour
{
    private const int Targets = 19;
    private Slider _slider;
    void Start()
    {
        _slider = GameObject.Find("LoadingSlider").GetComponent<Slider>();
        _slider.value =0;
    }
    void Update()
    {
        if (_slider.value + 0.001 >= _slider.maxValue)
        {
            print("***###*** Welcome ***###***");
            SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
        }
    }
    internal void LoadingThumbsUp()
    {
        _slider.value += _slider.maxValue / Targets;
    }
    //TODO: Add Story Writing For Users
}