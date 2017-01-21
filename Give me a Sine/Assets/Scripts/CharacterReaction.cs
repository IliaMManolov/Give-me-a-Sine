using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterReaction : MonoBehaviour {

    public float collisionDistance;

    public bool isMidair = false;
    public bool isAttacking = false;
    public bool isFlipped = false;
    public GameObject spawnerObject;

	// Use this for initialization
	void Start ()
    {
        //spawnerObject = GameObject.FindGameObjectWithTag("spawner");

	}
	
	void FixedUpdate ()
    {
        Transform nextObstacle = getNextObstacleTransform();

        if (nextObstacle != null && 
            nextObstacle.position.x - transform.position.x < collisionDistance &&
            nextObstacle.GetComponent<Obstacle>().isFailed == false)
        {
            switch(nextObstacle.gameObject.GetComponent<Obstacle>().type)
            {
                case ClassicalSpawner.ObstacleType.Cliff:
                    if (nextObstacle.gameObject.GetComponent<Obstacle>().isFlipped == isFlipped)
                        TakeDamage();
                    break;
                case ClassicalSpawner.ObstacleType.Enemy:
                    if (isAttacking == false)
                        TakeDamage();
                    break;
                case ClassicalSpawner.ObstacleType.Spikes:
                    if (isMidair == false)
                        TakeDamage();
                    break;
            }

            nextObstacle.GetComponent<Obstacle>().isFailed = true;
        }
	}

    Transform getNextObstacleTransform()
    {
        int l = 0, r = spawnerObject.transform.childCount;
        while (l<r-1)
        {
            int m = (l + r) / 2;
            if (spawnerObject.transform.GetChild(m).transform.position.x < transform.position.x)
                l = m;
            else
                r = m;
        }
        if (l >= spawnerObject.transform.childCount)
            return null;
        return spawnerObject.transform.GetChild(l).transform;
    }

    void TakeDamage()
    {
        Debug.Log("Ouch!");
    }
}
