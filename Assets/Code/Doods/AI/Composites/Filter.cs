using System;
using System.Collections.Generic;

namespace Code.Doods.AI {
	/// <summary>
	/// Adds preconditions to an ordinary sequence.
	/// Note that these preconditions are checked every tick.
	/// </summary>
	public class Filter : Sequence {
		readonly List<Func<bool>> _preconditions = new List<Func<bool>> {
			() => {return true; } // todo I should learn if this is the right way to do this
		};

		public void AddPrecondition (Func<bool> precondition)
		{
			_preconditions.Add (precondition);
		}

		protected override Status Update ()
		{
			// todo should this just get calculated once?
			for (int i = 0, c = _preconditions.Count; i < c; i++) {
				if (!_preconditions [i] ()) { return Status.Failure; }
			}

			return base.Update ();
		}

	}
}