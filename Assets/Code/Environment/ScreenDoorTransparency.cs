using UnityEngine;

namespace Code.Environment
{
    public class ScreenDoorTransparency : MonoBehaviour
    {
        private Renderer[] _renderers;
        private Renderer _renderer;
        private MaterialPropertyBlock _prop;

        [SerializeField] private float _alpha = 1;

        public float Alpha {
            set {
                _alpha = Mathf.Clamp01(value);
                _prop.SetFloat("_Alpha", _alpha);
                if (_renderer) {
                    _renderer.SetPropertyBlock(_prop);
                }

                foreach (var r in _renderers) {
                    r.SetPropertyBlock(_prop);
                }
            }
        }

        private void Start () {
            _renderers = GetComponentsInChildren<Renderer>();
            _renderer = GetComponent<Renderer>();
            _prop = new MaterialPropertyBlock();
            _prop.SetFloat("_Alpha", _alpha);
            if (_renderer) {
                _renderer.SetPropertyBlock(_prop);
            }

            foreach (var r in _renderers) {
                r.SetPropertyBlock(_prop);
            }
        }

        private void Update () {
            if (_renderer) {
                _renderer.sharedMaterial.SetVector("_ScreenSize", new Vector2(Screen.width, Screen.height));
            }

            foreach (var r in _renderers) {
                r.sharedMaterial.SetVector("_ScreenSize", new Vector2(Screen.width, Screen.height));
            }
        }
    }
}