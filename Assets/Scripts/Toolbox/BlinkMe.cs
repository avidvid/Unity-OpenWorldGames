using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkMe : MonoBehaviour
{
    private float _offInterval = 0.5f;
    private float _onInterval = 1f;
    private Image _blinkImage;
    // Start is called before the first frame update
    void Start()
    {
        _blinkImage = this.GetComponent<Image>();
        StartBlinking();
    }
    void StartBlinking()
    {
        StopAllCoroutines();
        StartCoroutine("Blink");
    }
    IEnumerator Blink()
    {
        while (true)
        {
            switch ((int)_blinkImage.color.a)
            {
                case 0:
                    _blinkImage.color = new Color(_blinkImage.color.r, _blinkImage.color.g, _blinkImage.color.b, 1);
                    yield return new WaitForSeconds(_onInterval);
                    break;
                case 1:
                    _blinkImage.color = new Color(_blinkImage.color.r, _blinkImage.color.g, _blinkImage.color.b, 0);
                    yield return new WaitForSeconds(_offInterval);
                    break;
            }
        }
    }
}
