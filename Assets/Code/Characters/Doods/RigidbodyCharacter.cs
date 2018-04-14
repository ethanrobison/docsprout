using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Code.Characters.Doods {
	[RequireComponent (typeof (Rigidbody))]
	[RequireComponent (typeof (CapsuleCollider))]
	public class RigidbodyCharacter : Character {


		Rigidbody _rig;
		CapsuleCollider _cap;

		// Use this for initialization
		void Start ()
		{
			_rig = GetComponent<Rigidbody> ();
			_rig.useGravity = false;
			_cap = GetComponent<CapsuleCollider> ();
		}

		// Update is called once per frame

		void CheckGrounded ()
		{
			RaycastHit hit;
			Vector3 origin = transform.position + (Vector3.down * (_cap.height / 2f - _cap.radius) + transform.rotation * _cap.center) * transform.lossyScale.z + Vector3.up*.1f;
			if (Physics.SphereCast (origin, _cap.radius * transform.lossyScale.z*.95f, Vector3.down, out hit, .4f,
								GroundLayers, QueryTriggerInteraction.Ignore)) {


				groundNormal = hit.normal;
				if (hit.normal.y > .5f) {
					isOnGround = true;
					velocity.y = 0f;
					velocity -= Vector3.Dot (velocity, groundNormal) * groundNormal;
				} else {
					groundNormal = Vector3.up;
				}
			} else {
				isOnGround = false;
				groundNormal = Vector3.up;
			}
		}

		void FixedUpdate ()
		{
			//Vector3 dS = velocity * Time.deltaTime;
			_rig.MovePosition (_rig.position + velocity * Time.deltaTime);
			CheckGrounded ();
			//_rig.AddForce(-_rig.velocity, ForceMode.VelocityChange);
			_rig.velocity = Vector3.zero;
			//_rig.velocity = velocity;
		}


	}


}