using UnityEngine;
using System.Collections;

public class LineDrawScript : MonoBehaviour {


    class Segment
    {
        public Vector2 relativeStartingPosition;
        public float timeStamp;     //in seconds
        public float frequency;
        Segment(Vector2 a, float b, float c)
        {
            relativeStartingPosition = a;
            timeStamp = b;
            frequency = c;
        }
    };

    LineRenderer waveRenderer;
    public float movementSpeed;

    void Start ()
    {
        waveRenderer = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        /*for (int i=0;i<waveRenderer.numPositions;i++)
        {
            Vector3 currentPosition = waveRenderer.GetPosition(i);
            currentPosition.x = currentPosition.x - movementSpeed;
            waveRenderer.SetPosition(i, currentPosition);
        }*/
        //int index = 0;
	}
}
