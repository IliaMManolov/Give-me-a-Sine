using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicalSpawner : MonoBehaviour
{


    public enum ObstacleType
    {
        Spikes,
        Cliff,
        Enemy
    }

    [System.Serializable]
    public class ObstacleInfo
    {
        public ObstacleType type;
        public float spawnTime; 
    }

    //public GameObject obstaclePrefab;
    //public List<Transform> allObstacles;

    float lastSpawn;
    public float spawnDelta = 5;
    public float scrollingSpeed = 0.1f;
    public Vector2 newObstaclePosition;
    public float despawnPosX = -30.0f;
    UniversalWaveRender renderer;

    

	void Start ()
    {
        renderer = GetComponent<UniversalWaveRender>();
        lastSpawn = Time.time;
    }
	
	void FixedUpdate ()
    {
        if (Time.time > lastSpawn + spawnDelta)
        {
            spawnObstacle();
            lastSpawn = Time.time;
        }
	}

    public void spawnObstacle()
    {
        Debug.Log("Spawned one!");
        renderer.addObstacle((ObstacleType)Mathf.Floor(Random.Range(0, 3)));

    }
}
