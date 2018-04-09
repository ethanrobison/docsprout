﻿using System.Collections.Generic;
using UnityEngine;

namespace Code.Doods {
	public class DoodManager : IContextManager {
		static Object Prefab;

		public readonly List<Dood> DoodList = new List<Dood> ();

		public void Initialize ()
		{
			Prefab = Resources.Load ("Dood");
			for (int x = 0; x < 5; x++) {
				for (int y = 0; y < 6; y++) {

					MakeDood (new Vector3 (x, 0f, y));
				}
			}
		}

		public void ShutDown () { }


		void MakeDood (Vector3 pos)
		{
			var go = (GameObject)Object.Instantiate (Prefab, pos, Quaternion.identity);
			var dood = go.GetComponent<Dood> ();
			dood.Initialize ();

			DoodList.Add (dood);
		}
	}
}