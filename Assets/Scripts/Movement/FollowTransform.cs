using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//todo: delete 
public class FollowTransform : MonoBehaviour 
{
    private Transform _target;

    private Vector3 offsetPosition = Vector3.zero ;

    private Space offsetPositionSpace = Space.Self;


    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (_target == null)
        {
            Debug.LogWarning("Missing target ref !", this);
            return;
        }

        // compute position
        if (offsetPositionSpace == Space.Self)
        {
            transform.position = _target.TransformPoint(offsetPosition);
        }
        else
        {
            transform.position = _target.position + offsetPosition;
        }

        //// compute rotation
        //if (lookAt)
        //    transform.LookAt(_target);
        //else
        //    transform.rotation = _target.rotation;
    }
}