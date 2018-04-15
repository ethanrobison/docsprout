using System.Collections.Generic;
using UnityEngine;

namespace Code.Doods {
	public class DoodManager : IContextManager {
		static Object Prefab;

		public readonly List<Dood> DoodList = new List<Dood> ();

		public void Initialize ()
		{
			// fixme what is going on here
			string [] names = { "Sphere", "Capsule", "Cone", "Cube", "Cylinder" };
			for (int i = 0; i < 5; i++) {

				for (float y = 0; y < 12f; y += 1.2f) {
					MakeDood (new Vector3 (i, 5f, y), names [i]);
				}
			}
		}

		public void ShutDown () { }


		void MakeDood (Vector3 pos, string name)
		{
			var prefab = Resources.Load ("Doods/" + name + " Dood");
			var go = (GameObject)Object.Instantiate (prefab, pos, Quaternion.Euler (-90f, 0f, 0f));
			var dood = go.GetComponent<Dood> ();
			//dood.Initialize ();

			DoodList.Add (dood);
		}
	}
}