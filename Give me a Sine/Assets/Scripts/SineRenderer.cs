using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineRenderer : MonoBehaviour {

    [Range(0.001f, 20.0f)]
    public float spacing;
    [Range(0.001f, 50.0f)]
    public float length;
    [Range(0.001f, 20.0f)]
    public float frequency;
    public float amplitude;
    public float x;
    [Range(0.1f, 20.0f)]
    public float speed;
    LineRenderer sine;

    public Vector2 positionOffset;

	void Start ()
    {
        sine = GetComponent<LineRenderer>();

        renderSine(sine);
	}
	
	
	void FixedUpdate ()
    {
        renderSine(sine);
	}

    void renderSine(LineRenderer target)
    {
        //target.numPositions = 0;
        Vector3[] positions = new Vector3[(int)(length/(spacing*(1/frequency)))];
        
        for(int i=0;i<(int)(length/(spacing * (1/frequency)));i++)
        {
            Vector2 currPoint;
            currPoint = new Vector2((x + i * spacing * (1/frequency) + positionOffset.x),
                                    (Mathf.Sin(Time.time * speed + x + i * spacing) * amplitude) + positionOffset.y);
            positions[i]=currPoint;
        }
        target.numPositions = positions.Length;
        target.SetPositions(positions);
    }
}
