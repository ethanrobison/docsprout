using UnityEngine;
using Code.Environment.Advertising;
using Code.Characters.Doods;

namespace Code.environment
{
	public class Treat : MonoBehaviour
	{
		private float _lifetime = 100f;
		private float _decayRate = 5f;
		private Satisfier _satisfier;

		void Start () {
			_satisfier = GetComponentInChildren<Satisfier>();
			_satisfier.OnInteract += Snacking;
		}

		private void Snacking (Dood dood) {
			_lifetime -= _decayRate * Time.deltaTime;
			if (_lifetime <= 0f) {
				//_satisfier.OnInteract -= Snacking;
				Object.Destroy(gameObject);
			}
		}
	}
}
