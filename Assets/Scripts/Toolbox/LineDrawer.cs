using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    private Camera _camera;

    public Material LineMaterial;
    public float LineWidth;
    public float Depth;

    private Vector3? _lineStartPoint = null; 

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            _lineStartPoint = GetMouseClickedPoint();
        else if (Input.GetMouseButtonUp(0))
        {
            if (!_lineStartPoint.HasValue)
                return; 
            var lineEndPoint = GetMouseClickedPoint();
            GameObject gameObject = new GameObject();
            LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = LineMaterial;
            lineRenderer.SetPositions(new Vector3[] { _lineStartPoint.Value, lineEndPoint } );
            lineRenderer.startWidth = LineWidth;
            lineRenderer.endWidth = LineWidth;
            //reset the Start
            _lineStartPoint = null;
        }
    }

    private Vector3 GetMouseClickedPoint()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * Depth;
    }


    private void DrawResearchLine(Vector3 startPoint, Vector3 endPoint)
    {
        GameObject gameObject = new GameObject();
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        //lineRenderer.material = LineMaterial;

        print("CastRayLines: Original " + startPoint + " " + endPoint + " PointToRay: " + PointToRay(startPoint) + " " + PointToRay(endPoint));
        lineRenderer.SetPositions(new Vector3[] { PointToRay(startPoint), PointToRay(endPoint) });
        lineRenderer.startWidth = 0.2f;
        lineRenderer.startColor = Color.blue;
        lineRenderer.endWidth = 0.2f;
        lineRenderer.endColor = Color.blue;
    }
    private Vector3 PointToRay(Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(point);
        return ray.origin + ray.direction * 100;
    }
}
