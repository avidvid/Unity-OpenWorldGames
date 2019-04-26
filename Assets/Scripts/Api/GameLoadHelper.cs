using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoadHelper : MonoBehaviour
{
    private const int Targets = 18;
    private Slider _slider;
    // Start is called before the first frame update
    void Start()
    {
        _slider = GameObject.Find("LoadingSlider").GetComponent<Slider>();
        _slider.value =0;
    }
    // Update is called once per frame
    void Update()
    {
        if (_slider.value >= _slider.maxValue)
        {
            SceneManager.LoadScene(SceneSettings.SceneIdForTerrainView);
            print("***###*** Welcome ***###***");
        }
    }
    internal void LoadingThumbsUp()
    {
        _slider.value += _slider.maxValue / Targets;
    }
}