using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalWaveRender : MonoBehaviour {


    [System.Serializable]
    public class WaveProperties
    {
        public Vector2 offset;
        [Range(0.001f, 20.0f)]
        public float spacing = 0.1f;
        [Range(0.001f, 50.0f)]
        public float length = 10;
        [Range(0.001f, 20.0f)]
        public float frequency = 1;
        public float amplitude = 1;
        public float x = 0;
        [Range(0.001f, 20.0f)]
        public float speed = 1;
    }

    public List<WaveProperties> allProperties;
    public LineRenderer lineRenderer;
    public float scrollingSpeed;
    public Vector2 globalOffset;

    void Start ()
    {
        if (allProperties.Count == 0)
            allProperties.Add(new WaveProperties());
        lineRenderer = GetComponent<LineRenderer>();
	}
	
	void FixedUpdate ()
    {
        renderComplex();
	}


    void renderComplex()
    {
        lineRenderer.numPositions = 0;
        Vector2 lastPoint = new Vector2(0, 0);
        allProperties[0].offset.x -= scrollingSpeed;
        for (int i = 0; i < allProperties.Count; i++)
        {
            renderSimple(lastPoint, allProperties[i]);
            lastPoint = new Vector2(lineRenderer.GetPosition(lineRenderer.numPositions - 1).x,
                lineRenderer.GetPosition(lineRenderer.numPositions - 1).y);
        }
        offsetHeight();

    }

    void renderSimple(Vector2 lastPosition, WaveProperties properties)
    {
        lastPosition.y -= (Mathf.Sin(Time.time * properties.speed + properties.x) * properties.amplitude) + properties.offset.y;
        int pointCount = (int)(properties.length * properties.frequency / properties.spacing);
        Vector3[] positions = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            Vector2 currPoint;
            currPoint = new Vector2(lastPosition.x + (properties.x + i * properties.spacing / properties.frequency + properties.offset.x),
                                    lastPosition.y + (Mathf.Sin(Time.time * properties.speed + properties.x + i * properties.spacing) * properties.amplitude) + properties.offset.y);

            positions[i] = currPoint;
        }
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        int oldSize = lineRenderer.numPositions;
        lineRenderer.numPositions = lineRenderer.numPositions + positions.Length;
        for (int i = oldSize; i < lineRenderer.numPositions; i++)
            lineRenderer.SetPosition(i, positions[i - oldSize]);
        
    }

    void offsetHeight()
    {
        int l = 0, r = lineRenderer.numPositions;
        while (l<r-1)
        {
            int m = (l + r) / 2;
            if (lineRenderer.GetPosition(m).x + globalOffset.x < 0)
                l = m;
            else
                r = m;
        }

        if (l == lineRenderer.numPositions - 1)
            return;

        float targetHeight;
        Vector2 pointA = lineRenderer.GetPosition(l), pointB = lineRenderer.GetPosition(l + 1);

        //Debug.Log("A: " + pointA);
        //Debug.Log("B: " + pointB);

        targetHeight = pointA.y + (pointB.y - pointA.y) * (-globalOffset.x-pointA.x) / (pointB.x - pointA.x);



        for (int i = 0; i < lineRenderer.numPositions; i++)
            lineRenderer.SetPosition(i,
                lineRenderer.GetPosition(i) - new Vector3(globalOffset.x, targetHeight + globalOffset.y, 0));
    }
}
