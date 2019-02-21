using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Start()
    {
        GameObject.DontDestroyOnLoad(this);
    }
}