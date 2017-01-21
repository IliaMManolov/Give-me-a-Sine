using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterReaction : MonoBehaviour {

    public float collisionDistance;

    public float jumpTime = 1, attackTime = 1, dropTime = 0.2f, flipTime = 0.1f;

    [HideInInspector]
    public bool hasJumped = false;
    [HideInInspector]
    public bool hasAttacked = false;
    [HideInInspector]
    public bool hasFlipped = false;
    [HideInInspector]
    public bool hasDropped = false;
    public GameObject spawnerObject;

    bool isFlipped = false;

    float lastAttackMoment;
    float lastJumpMoment;
    float lastFlippedMoment;
    float lastDroppedMoment;

    [System.Serializable]
    public enum CharacterState
    {
        Normal,
        Midair,
        Attacking,
        Flipping,
        Dropping
    }
    public CharacterState state;

	// Use this for initialization
	void Start ()
    {
        //spawnerObject = GameObject.FindGameObjectWithTag("spawner");

	}

    void Update()
    {
        if (hasJumped == true && state == CharacterState.Normal)
            jump();
        if (hasAttacked == true && state == CharacterState.Normal)
            attack();
        if (hasFlipped == true && state == CharacterState.Normal)
            flip();
        if (hasDropped == true && state == CharacterState.Normal)
            drop();

        if ((state == CharacterState.Midair && Time.time - lastJumpMoment > jumpTime) ||
            (state == CharacterState.Attacking && Time.time - lastAttackMoment > attackTime) ||
            (state == CharacterState.Flipping && Time.time - lastFlippedMoment > flipTime) ||
            (state == CharacterState.Dropping && Time.time - lastDroppedMoment > dropTime))
            state = CharacterState.Normal;
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
                    if (state != CharacterState.Attacking)
                        TakeDamage();
                    break;
                case ClassicalSpawner.ObstacleType.Spikes:
                    if (state != CharacterState.Midair)
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

    void jump()
    {
        state = CharacterState.Midair;
        lastJumpMoment = Time.time;
    }

    void attack()
    {
        state = CharacterState.Attacking;
        lastAttackMoment = Time.time;
    }

    void flip()
    {
        state = CharacterState.Flipping;
        lastFlippedMoment = Time.time;

        isFlipped = !isFlipped;

    }

    void drop()
    {
        state = CharacterState.Dropping;
        lastDroppedMoment = Time.time;
    }
}
