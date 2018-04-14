using UnityEngine;

namespace Code.Environment {
	public class ScreenDoorTransparency : MonoBehaviour {
		Renderer[] _renderers;
		Renderer _renderer;
		MaterialPropertyBlock _prop;

		[SerializeField] float _alpha = 1;
		public float Alpha {
			get {
				return _alpha;
			}
			set {
				_alpha = Mathf.Clamp01(value);
				_prop.SetFloat ("_Alpha", _alpha);
				if(_renderer) {
					_renderer.SetPropertyBlock (_prop);	
				}
				foreach(Renderer r in _renderers) {
					r.SetPropertyBlock (_prop);	
				}
			}
		}
		// Use this for initialization
		void Start ()
		{
			_renderers = GetComponentsInChildren<Renderer>();
			_renderer = GetComponent<Renderer> ();
			_prop = new MaterialPropertyBlock ();
			_prop.SetFloat ("_Alpha", _alpha);
			if(_renderer) {
				_renderer.SetPropertyBlock (_prop);	
			}
			foreach(Renderer r in _renderers) {
				r.SetPropertyBlock (_prop);	
			}
		}

		// Update is called once per frame
		void Update ()
		{
			if(_renderer) {
				_renderer.sharedMaterial.SetVector ("_ScreenSize", new Vector2 (Screen.width, Screen.height));
			}
			foreach(Renderer r in _renderers) {
				r.sharedMaterial.SetVector ("_ScreenSize", new Vector2 (Screen.width, Screen.height));
			}
		}
	}
}
