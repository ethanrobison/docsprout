using System.Collections.Generic;
using UnityEngine;

namespace Code.Doods {
	public class DoodManager : IContextManager {
		static Object Prefab;

		readonly List<Dood> _doods = new List<Dood> ();

		public void Initialize ()
		{
			Prefab = Resources.Load ("Dood");
			MakeDood ();
		}

		public void ShutDown () { }


		void MakeDood ()
		{
			var go = (GameObject)Object.Instantiate (Prefab);
			var dood = go.GetComponent<Dood> ();
			dood.Initialize ();

			_doods.Add (dood);
		}
	}
}