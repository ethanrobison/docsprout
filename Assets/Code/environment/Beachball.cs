using Code.Characters.Doods;
using Code.Environment.Advertising;
using Code.Utils;
using UnityEngine;

namespace Code.environment
{
	public class Beachball : MonoBehaviour {
	
		public Vector3 Force = new Vector3(0f, 5f, 5f);
		
		private Rigidbody _rig;
		private float _cooldown;
		private const float COOLDOWN_TIME = .1f;

		private void Start () {
			_rig = gameObject.GetRequiredComponent<Rigidbody>();
			gameObject.GetRequiredComponentInChildren<Satisfier>().OnInteract += Kick;
		}

		private void Update () {
			if(_cooldown > 0f) {
				_cooldown -= Time.deltaTime;
			}
		}


		private void Kick(Dood dood) {
			if(_cooldown > 0f) return;
			_cooldown += COOLDOWN_TIME;
			_rig.AddForce(dood.transform.TransformVector(Force), ForceMode.VelocityChange);
		}
	}
}
