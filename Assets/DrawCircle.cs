using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    public float radiusX;
    public float radiusY;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] points = new Vector3[361];
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 361;
        for (int i = 0; i < 361; i++)
        {
            float rad = Mathf.Deg2Rad * i;
            points[i] = new Vector3(Mathf.Sin(rad) * radiusY, Mathf.Cos(rad) * radiusX, 0.0f);
        }
        lineRenderer.SetPositions(points);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
