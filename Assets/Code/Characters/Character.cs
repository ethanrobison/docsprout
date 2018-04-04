using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour {
    /// <summary>
    /// The Character's CharacterController.
    /// </summary>
    CharacterController characterController;

    /// <summary>
    /// The Character's velocity. The Character moves every physics update based off the velocity
    /// </summary>
    [HideInInspector] public Vector3 velocity = Vector3.zero;

    /// <summary>
    /// Whether or not the Character is on the ground
    /// </summary>
    [HideInInspector] public bool isOnGround;

    /// <summary>
    /// The collision flags returned after moving the Character
    /// </summary>
    [HideInInspector] public CollisionFlags collisionFlags;


	void Start () {
        characterController = GetComponent<CharacterController>();
	}

	void FixedUpdate()
	{
        velocity += Physics.gravity*Time.fixedDeltaTime;
        collisionFlags = characterController.Move(velocity*Time.fixedDeltaTime);
        isOnGround = (collisionFlags & CollisionFlags.Below) !=0;
        if(isOnGround) {
            velocity.y = 0f;
        }
        Debug.Log(velocity.y);
	}
}
