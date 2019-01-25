using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public Animator Animator;
    private Text _damageText;

	// Use this for initialization
	void Awake () {
	    var clipInfo = Animator.GetCurrentAnimatorClipInfo(0);
        _damageText = Animator.GetComponent<Text>();
	    Destroy(gameObject, clipInfo[0].clip.length);
	}


    void Start()
    {
        transform.SetSiblingIndex(0);
        name = "Damage " + _damageText;
    }

    public void SetText(string text)
    {
        _damageText.text = text;
    }
}
