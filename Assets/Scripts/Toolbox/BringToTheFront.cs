using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringToTheFront : MonoBehaviour {

    void OnEnable()
    {
        transform.SetAsLastSibling();
    }
}
