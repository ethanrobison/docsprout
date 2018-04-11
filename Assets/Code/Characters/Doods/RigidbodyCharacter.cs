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
			_cap = GetComponent<CapsuleCollider> ();
		}

		// Update is called once per frame

		void CheckGrounded ()
		{
			RaycastHit hit;
			Vector3 origin = transform.position - Vector3.up * (_cap.height / 2f - _cap.radius);
			if (Physics.SphereCast (origin, _cap.radius, Vector3.down, out hit, 0.05f,
			                    GroundLayers, QueryTriggerInteraction.Ignore)) {


				groundNormal = hit.normal;
				if (hit.normal.y > .5f) {
					isOnGround = true;
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
			_rig.MovePosition (_rig.position + velocity * Time.deltaTime);
			CheckGrounded ();
		}


	}


}