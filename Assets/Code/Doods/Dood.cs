using Code.Doods.AI;
using UnityEngine;

namespace Code.Doods {
	[RequireComponent (typeof (Walk))]
	public class Dood : MonoBehaviour {

		public Root Behavior { get; private set; }
		Walk _walk;

		public void Initialize ()
		{
			var seq = new Sequence (this);
			seq.AddChild (new LogMessage ("Hello", this));
			seq.AddChild (new Idle (this));
			Behavior = new Root (seq, this);
		}

		void Start ()
		{
			_walk = GetComponent<Walk> ();
		}

		void Update ()
		{
			var status = Behavior.Tick ();
		}
	}
}