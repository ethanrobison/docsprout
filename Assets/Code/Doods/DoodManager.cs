﻿using System.Collections.Generic;
using UnityEngine;

namespace Code.Doods {
	public class DoodManager : IContextManager {
		static Object Prefab;

		readonly List<Dood> _doods = new List<Dood> ();

		public void Initialize ()
		{
			string [] names = { "Sphere", "Capsule", "Cone", "Cube", "Cylinder" };
			for (int i = 0; i < 5; i++) {

				for (float y = 0; y < 12f; y += 1.2f) {
					MakeDood (new Vector3 (i, 1f, y), names [i]);
				}
			}

		}

		public void ShutDown () { }


		void MakeDood (Vector3 pos, string name)
		{
			var prefab = Resources.Load ("Doods/" + name + " Dood");
			var go = (GameObject)Object.Instantiate (prefab, pos, Quaternion.Euler (-90f, 0f, 0f));
			var dood = go.GetComponent<Dood> ();
			dood.Initialize ();

			_doods.Add (dood);
		}
	}
}