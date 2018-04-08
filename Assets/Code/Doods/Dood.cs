using Code.Doods.AI;
using UnityEngine;

namespace Code.Doods {
	[RequireComponent (typeof (Walk))]
	[RequireComponent (typeof (BehaviorTree))]
	public class Dood : MonoBehaviour {
		DoodManager _manager;

		Walk _walk;
		BehaviorTree _behavior;

		public void Initialize (DoodManager manager)
		{
			_manager = manager;

		}

		void Start ()
		{
			_walk = GetComponent<Walk> ();
			_behavior = GetComponent<BehaviorTree> ();
		}

		void Update ()
		{
			_behavior.Tick ();
		}
	}
}