using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Doods {
	public class DoodColor : MonoBehaviour {

		[SerializeField] Color _color;
		[SerializeField] float _happiness;
		[SerializeField] Texture _texture;
		MaterialPropertyBlock _propertyBlock;
		Renderer _renderer;
		public Color Color {
			get {
				return _color;
			}
			set {
				_color = value;
				_propertyBlock.SetColor ("_Color", _color);
				_renderer.SetPropertyBlock (_propertyBlock);
			}
		}

		public float Happiness {
			get {
				return _happiness;
			}
			set {
				_happiness = value;
				_propertyBlock.SetFloat ("_Happiness", _happiness);
				_renderer.SetPropertyBlock (_propertyBlock);
			}
		}

		public Texture Texture {
			get {
				return _texture;
			}
			set {
				_texture = value;
				_propertyBlock.SetTexture ("_MainTex", _texture);
				_renderer.SetPropertyBlock (_propertyBlock);
			}
		}

		void Awake ()
		{
			_propertyBlock = new MaterialPropertyBlock ();
			_propertyBlock.SetColor ("_Color", _color);
			_propertyBlock.SetFloat ("_Happiness", _happiness);
			if (_texture) {
				_propertyBlock.SetTexture ("_MainTex", _texture);
			}
			_renderer = GetComponent<Renderer> ();
			_renderer.SetPropertyBlock (_propertyBlock);
		}

	}
}