using Code.Doods.AI;
using UnityEngine;

namespace Code.Doods {
	[RequireComponent (typeof (Characters.Movement.Walk))]
	public class Dood : MonoBehaviour {

		public Root Behavior { get; private set; }
		Characters.Movement.Walk _walk;

		public void Initialize ()
		{
			var seq = new Sequence (this);
			seq.AddChild (new LogMessage ("Hello", this));
			seq.AddChild (new Idle (this));
			Behavior = new Root (seq, this);
		}

		void Start ()
		{
			_walk = GetComponent<Characters.Movement.Walk> ();
		}

		void Update ()
		{
			var status = Behavior.Tick ();
		}
	}
}