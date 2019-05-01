using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        GameObject.DontDestroyOnLoad(this);
    }
}