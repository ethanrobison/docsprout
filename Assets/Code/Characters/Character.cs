using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour {
    
    /// <summary>
    /// The character's character controller.
    /// </summary>
    CharacterController characterController;

    /// <summary>
    /// The character's velocity. The character moves every physics update based off the velocity
    /// </summary>
    [HideInInspector] public Vector3 velocity = Vector3.zero;

    /// <summary>
    /// Whether or not the character is on the ground
    /// </summary>
    [HideInInspector] public bool isOnGround;

    /// <summary>
    /// The collision flags returned after moving the Character
    /// </summary>
    [HideInInspector] public CollisionFlags collisionFlags;

    /// <summary>
    /// The surface normal of the ground the chracter is standing on
    /// </summary>
    [HideInInspector] public Vector3 groundNormal = Vector3.up;


	void Start () {
        characterController = GetComponent<CharacterController>();
	}

    void setGroundNormal(){
        
    }

	void FixedUpdate()
	{
        collisionFlags = characterController.Move(velocity*Time.fixedDeltaTime);
        isOnGround = characterController.isGrounded;
        if(isOnGround) {
            velocity.y = 0f;
            setGroundNormal();
        }
	}
}
