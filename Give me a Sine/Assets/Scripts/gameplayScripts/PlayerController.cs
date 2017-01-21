using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public bool isFlipped = false;
    public bool isAttacking = false;
    public bool isJumped = false;
    public bool isDropped = false;

	
	void Update ()
    {
        if (Input.GetKeyDown("joystick 1 button 1")||Input.GetKeyDown("1"))
        {
            isFlipped = !isFlipped;
        }

        isJumped = Input.GetKey("joystick 1 button 3");
        isJumped = Input.GetKey("2");
        isAttacking = Input.GetKey("joystick 1 button 2");
        isAttacking = Input.GetKey("3");
        isDropped = Input.GetKey("joystick 1 button 0");
        isDropped = Input.GetKey("4");




    }
}