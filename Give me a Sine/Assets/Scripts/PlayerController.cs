using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    CharacterReaction kek;

    void Start ()
    {
        kek = GetComponent<CharacterReaction>();
	}

    void Update()
    {
        kek.hasFlipped = Input.GetKeyDown("joystick 1 button 1") || Input.GetKeyDown("1");
        kek.hasJumped = Input.GetKey("joystick 1 button 3") || Input.GetKey("2");
        kek.hasAttacked = Input.GetKey("joystick 1 button 2") || Input.GetKey("3");
        kek.hasDropped = Input.GetKey("joystick 1 button 0") || Input.GetKey("4");
    }
}
