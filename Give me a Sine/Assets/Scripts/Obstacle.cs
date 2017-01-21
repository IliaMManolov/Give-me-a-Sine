using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    // Use this for initialization

    public ClassicalSpawner.ObstacleType type;
    public Transform sineTransform;
    public static float scrollingSpeed = 0.1f;
    public static float despawnPosX = -30;
    public bool isFlipped = false;
    public bool isFailed = false;


	void Start ()
    {
        sineTransform = transform.parent.gameObject.GetComponent<LineRenderer>().transform;
        Debug.Log(sineTransform);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void FixedUpdate()
    {
        transform.position = transform.position - new Vector3(scrollingSpeed, 0, 0);

        if (transform.position.x < despawnPosX)
            Destroy(gameObject);

    }

    public void changeType(ClassicalSpawner.ObstacleType newType)
    {
        type = newType;

        switch (type)
        {
            case ClassicalSpawner.ObstacleType.Cliff:
                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
                break;
            case ClassicalSpawner.ObstacleType.Enemy:
                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
                break;
            case ClassicalSpawner.ObstacleType.Spikes:
                transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
                break;
        }
    }

    /*void stickToSine()
    {
        LineRenderer sineRenderer = sineTransform.gameObject.GetComponent<LineRenderer>();
        int l = 0, r = sineRenderer.numPositions;

        while (l<r-1)
        {
            int m = (l + r) / 2;
            if (sineRenderer.GetPosition(m).x < transform.position.x)
                l = m;
            else
                r = m;
        }

        transform.position = new Vector3(transform.position.x, sineRenderer.GetPosition(l).y, transform.position.y);
    }*/
}
