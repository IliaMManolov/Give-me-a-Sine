using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositioning : MonoBehaviour {

    UniversalWaveRender renderer;
	void Start ()
    {
        renderer = GameObject.FindGameObjectWithTag("sine").GetComponent<UniversalWaveRender>();
		
	}
	
	void Update ()
    {
        int l = 0, r = renderer.lineRenderer.numPositions;

        while (l<r-1)
        {
            int m = (l + r) / 2;
            if (renderer.lineRenderer.GetPosition(m).x < transform.position.x)
                l = m;
            else
                r = m;

        }

        transform.position = new Vector3(transform.position.x, renderer.lineRenderer.GetPosition(l).y);

	}
}
