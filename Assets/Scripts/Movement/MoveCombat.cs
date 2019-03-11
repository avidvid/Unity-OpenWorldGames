using UnityEngine;

public class MoveCombat : MonoBehaviour
{
    private Vector3 _previousPosition;
    private float _screenLimit = 2.7f;

    void Start()
    {
        var pos = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        _screenLimit = pos.y * 9 / 10;
    }
    void Update()
    {
        Vector3 currentPos = transform.position;
        if (IsBlocked(currentPos))
            transform.position = _previousPosition;
        else
            _previousPosition = currentPos;
    }
    private bool IsBlocked(Vector3 currentPos)
    {
        if (Mathf.Abs(currentPos.x) > _screenLimit || Mathf.Abs(currentPos.y) > _screenLimit)
            return true;
        return false;
    }
}