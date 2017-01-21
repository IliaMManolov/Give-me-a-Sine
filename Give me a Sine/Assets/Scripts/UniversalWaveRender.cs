using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalWaveRender : MonoBehaviour {


    [System.Serializable]
    public class WaveProperties
    {
        public Vector2 offsetFromLast;
        [Range(0.001f, 20.0f)]
        public float spacing = 5.0f;
        [Range(0.01f, 100.0f)]
        public float frequency = 1;
        [Range(0.01f, 10.0f)]
        public float amplitude = 1;
        [Range(1.0f, 100.0f)]
        public float length = 30;
        [Range(0.001f, 20.0f)]
        public float speed = 1;

        public void copy(WaveProperties other)
        {
            offsetFromLast = other.offsetFromLast;
            spacing = other.spacing;
            length = other.length;
            frequency = other.frequency;
            amplitude = other.amplitude;
            speed = other.speed;
        }

        public bool same(WaveProperties other)
        {
            return !(offsetFromLast != other.offsetFromLast ||
                   spacing != other.spacing ||
                   length != other.length ||
                   frequency != other.frequency ||
                   amplitude != other.amplitude ||
                   speed != other.speed);
        }
    }

    public float speed = 1;
    public float scrollingSpeed = 1;
    public List<WaveProperties> allProperties;
    public WaveProperties startingProperty;
    public WaveProperties spikesProperty;
    public WaveProperties cliffProperty;
    public WaveProperties enemyProperty;
    WaveProperties currentProperty;
    public LineRenderer lineRenderer;
    public float offScreenLeft, offScreenRight;

    Camera gameCamera;
    //public Vector2 startingPosition;

    void Start()
    {
        currentProperty = startingProperty;
        lineRenderer = GetComponent<LineRenderer>();

        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x - scrollingSpeed, transform.position.y, transform.position.z);
        if (allProperties.Count == 0)
        {
            allProperties.Add(new WaveProperties());
            allProperties[0].copy(currentProperty);
        }

        while (isVisibleEnd(allProperties.Count-1))
        {
            allProperties.Add(new WaveProperties());
            allProperties[allProperties.Count - 1].copy(currentProperty);
        }
        //Debug.Log(allProperties.Count);
        //Debug.Log(isVisibleEnd(0));
        while (allProperties.Count > 0 && !isVisible(0))
        {
            int propPointCnt = Mathf.FloorToInt(allProperties[0].length * allProperties[0].spacing);
            transform.position += new Vector3((float)propPointCnt/allProperties[0].spacing, 0, 0);
            allProperties.RemoveAt(0);
        }

        
        renderAllWaves();

    }

    public bool isVisibleEnd(int index)
    {
        float startX = transform.position.x;
        for (int i = 0; i <= index; i++)
            startX += allProperties[i].length;
        
        //Vector2 screenPoint = gameCamera.WorldToViewportPoint(new Vector3(startX, 0, 0));

        
        if (startX<offScreenLeft || startX>offScreenRight)
        {
            return false;
        }
        return true;
    }

    public bool isVisible(int index)
    {
        float startX = transform.position.x;
        for (int i = 0; i <= index; i++)
            startX += allProperties[i].length;

        //Vector2 screenPoint = gameCamera.WorldToViewportPoint(new Vector3(startX, 0, 0));

        //Debug.Log(index);
        //Debug.Log(screenPoint);
        //Debug.Log(startX);
        if (startX < offScreenLeft)
        {
            return false;
        }
        return true;
    }

    void renderAllWaves()
    {
        lineRenderer.numPositions = 0;
        Vector2 lastOffset = transform.position;
        for (int i=0;i<allProperties.Count;i++)
        {
            renderWave(allProperties[i], lastOffset);
            lastOffset = lineRenderer.GetPosition(lineRenderer.numPositions-1);
        }
    }

    void renderWave(WaveProperties properties, Vector2 lastOffset)
    {
        Vector2 startingPosition = new Vector2();
        startingPosition.x = lastOffset.x + properties.offsetFromLast.x;
        startingPosition.y = lastOffset.y + properties.offsetFromLast.y;
        float deltaY = Mathf.Sin(Time.time * speed * properties.frequency) * properties.amplitude;

        int pointCount = Mathf.FloorToInt(properties.length * properties.spacing);
        if (pointCount == 0)
            return;
        List<Vector2> points = new List<Vector2>();

        for (int i=0;i<pointCount;i++)
        {
            Vector2 finalPoint = new Vector2();

            finalPoint.x = startingPosition.x + (float)i / properties.spacing;
            finalPoint.y = startingPosition.y + Mathf.Sin(((float)i / properties.spacing + Time.time * speed) * properties.frequency) * properties.amplitude - deltaY;
            points.Add(finalPoint); 
        }

        if (!properties.same(cliffProperty))
        {
            //Debug.Log(points.Count);
            points.Add(new Vector2(points[pointCount - 1].x, points[0].y));
            //Debug.Log("Passed: " + points[0].ToString() + " " + points[points.Count - 1].ToString());

        }

        int oldSize = lineRenderer.numPositions;
        lineRenderer.numPositions = lineRenderer.numPositions + points.Count;
        for (int i = oldSize; i < lineRenderer.numPositions; i++)
            lineRenderer.SetPosition(i, points[i - oldSize]);
    }

    public void addObstacle(ClassicalSpawner.ObstacleType type)
    {
        int index = 0;
        float len = transform.position.x;
        while (index < allProperties.Count && len < offScreenRight)
        {
            len += allProperties[index].length;
            index++;
        }

        Debug.Log(index);
        Debug.Log(allProperties.Count);

        if (len > offScreenRight && index + 1 < allProperties.Count)
        {
            while (allProperties.Count != index + 1)
                allProperties.RemoveAt(index + 1);
        }

        if (len > offScreenLeft)
            allProperties[allProperties.Count - 1].length -= len - offScreenRight;


        allProperties.Add(new WaveProperties());
        switch (type)
        {
            case ClassicalSpawner.ObstacleType.Cliff:
                allProperties[allProperties.Count - 1].copy(cliffProperty);
                break;
            case ClassicalSpawner.ObstacleType.Enemy:
                allProperties[allProperties.Count - 1].copy(enemyProperty);
                break;
            case ClassicalSpawner.ObstacleType.Spikes:
                allProperties[allProperties.Count - 1].copy(spikesProperty);
                break;
        }
    }
        /*[System.Serializable]
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

            public void copy(WaveProperties other)
            {
                offset = other.offset;
                spacing = other.spacing;
                length = other.length;
                frequency = other.frequency;
                amplitude = other.amplitude;
                speed = other.speed;
            }
        }

        public List<WaveProperties> allProperties;
        public LineRenderer lineRenderer;
        public float scrollingSpeed;
        public Vector2 globalOffset;
        public WaveProperties defaultProperties;

        void Start ()
        {
            if (allProperties.Count == 0)
            {
                allProperties.Add(new WaveProperties());
                allProperties[allProperties.Count - 1].copy(defaultProperties);
            }
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

            Debug.Log(lastPosition);

            for (int i = 0; i < pointCount; i++)
            {
                Vector2 currPoint;
                currPoint = new Vector2(properties.x + lastPosition.x + (i * properties.spacing / properties.frequency + properties.offset.x),
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
            Debug.Log("First position: " + lineRenderer.GetPosition(0).ToString());
            int l = 0, r = lineRenderer.numPositions;
            while (l < r - 1)
            {
                int m = (l + r) / 2;
                if (lineRenderer.GetPosition(m).x < globalOffset.x)
                    l = m;
                else
                    r = m;
            }

            Vector2 referencePoint = lineRenderer.GetPosition(l) ;

            Debug.Log("Reference Point: " + referencePoint.ToString() + ' ' + l.ToString());

            float deltaY = globalOffset.y - referencePoint.y;

            for (int i = 0; i < lineRenderer.numPositions; i++)
                lineRenderer.SetPosition(i, lineRenderer.GetPosition(i) + new Vector3(globalOffset.x, deltaY, 0));


            /*int l = 0, r = lineRenderer.numPositions;
            while (l<r-1)
            {
                int m = (l + r) / 2;
                if (lineRenderer.GetPosition(m).x < globalOffset.x)
                    l = m;
                else
                    r = m;
            }

            if (l == lineRenderer.numPositions - 1)
                return;

            float targetHeight;
            Vector2 pointA = lineRenderer.GetPosition(l), pointB = lineRenderer.GetPosition(l + 1);

            Debug.Log("A: " + pointA);
            Debug.Log("B: " + pointB);

            targetHeight = pointA.y;

            //targetHeight = pointA.y + (pointB.y - pointA.y) * (-globalOffset.x-pointA.x) / (pointB.x - pointA.x);



            for (int i = 0; i < lineRenderer.numPositions; i++)
                lineRenderer.SetPosition(i,
                    lineRenderer.GetPosition(i) - new Vector3(globalOffset.x, targetHeight + globalOffset.y, 0));

        }

        */
}