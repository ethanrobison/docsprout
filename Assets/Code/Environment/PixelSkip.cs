using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Code.Environment {
	[ExecuteInEditMode]
	public class PixelSkip : MonoBehaviour {
		Renderer _renderer;
		MaterialPropertyBlock _prop;

		[SerializeField] int _skip = 0;
		public int Skip {
			get {
				return _skip;
			}
			set {
				_skip = value;
				_prop.SetFloat ("_PixelSkip", _skip);
				_renderer.SetPropertyBlock (_prop);
			}
		}
		// Use this for initialization
		void Start ()
		{
			_renderer = GetComponent<Renderer> ();
			_prop = new MaterialPropertyBlock ();
			_prop.SetFloat ("_PixelSkip", _skip);
			_renderer.SetPropertyBlock (_prop);
		}

		// Update is called once per frame
		void Update ()
		{
			_prop.SetFloat ("_PixelSkip", _skip);
			_renderer.SetPropertyBlock (_prop);
			_renderer.sharedMaterial.SetVector ("_ScreenSize", new Vector2 (Screen.width, Screen.height));
		}
	}
}
