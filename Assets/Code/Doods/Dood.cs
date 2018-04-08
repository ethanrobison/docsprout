using Code.Doods.AI;
using UnityEngine;

namespace Code.Doods {
	[RequireComponent (typeof (Walk))]
	public class Dood : MonoBehaviour {

		Walk _walk;
		Root _behavior;

		public void Initialize ()
		{
			_behavior = new Root (new Idle ());
		}

		void Start ()
		{
			_walk = GetComponent<Walk> ();
		}

		void Update()
		{
			var status = _behavior.Tick ();
		}
	}
}