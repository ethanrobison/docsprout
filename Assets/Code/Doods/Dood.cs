using Code.Doods.AI;
using UnityEngine;

namespace Code.Doods {
	[RequireComponent (typeof (Walk))]
	public class Dood : MonoBehaviour {

		Walk _walk;
		Root _behavior;

		public void Initialize ()
		{
			var seq = new Sequence ();
			seq.AddChild (new LogMessage ("Hello"));
			seq.AddChild (new Idle ());
			_behavior = new Root (seq);
		}

		void Start ()
		{
			_walk = GetComponent<Walk> ();
		}

		void Update ()
		{
			var status = _behavior.Tick ();
		}
	}
}