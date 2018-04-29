using UnityEngine;

namespace Code.Characters.Doods
{
    public class DoodColor : MonoBehaviour
    {
        [SerializeField] private float _happiness;
        [SerializeField] private Texture _texture;
        [SerializeField] private Color _color;
        [SerializeField] private Color _selectionColor;
        [SerializeField] private Color _interactColor;
        private Color _highlightColor;

        private bool _isSelected, _isInteracted;
        private MaterialPropertyBlock _propertyBlock;
        private Renderer _renderer;


        public Color Color {
            get { return _color; }
            set {
                _color = value;
                _propertyBlock.SetColor("_Color", _color);
                _renderer.SetPropertyBlock(_propertyBlock);
            }
        }

        public float Happiness {
            get { return _happiness; }
            set {
                _happiness = value;
                _propertyBlock.SetFloat("_Happiness", _happiness);
                _renderer.SetPropertyBlock(_propertyBlock);
            }
        }

        public Texture Texture {
            get { return _texture; }
            set {
                _texture = value;
                _propertyBlock.SetTexture("_MainTex", _texture);
                _renderer.SetPropertyBlock(_propertyBlock);
            }
        }

        private Color HighlightColor {
            get { return _highlightColor; }
            set {
                _highlightColor = value;
                _propertyBlock.SetColor("_HighlightColor", _highlightColor);
                _renderer.SetPropertyBlock(_propertyBlock);
            }
        }

        public bool IsSelected {
            get { return _isSelected; }
            set {
                _isSelected = value;
                _propertyBlock.SetFloat("_HighlightSize",
                    _isSelected ? _renderer.material.GetFloat("_HighlightSize") : 0f);
            }
        }

        public bool IsInteracted {
            get { return _isInteracted; }
            set {
                _isInteracted = value;
                HighlightColor = _isInteracted ? _interactColor : _selectionColor;
                _propertyBlock.SetFloat("_HighlightSize",
                    _isInteracted || _isSelected ? _renderer.material.GetFloat("_HighlightSize") : 0f);
            }
        }

        private void Awake () {
            _propertyBlock = new MaterialPropertyBlock();
            _propertyBlock.SetColor("_Color", _color);
            _propertyBlock.SetFloat("_Happiness", _happiness);
            if (_texture) {
                _propertyBlock.SetTexture("_MainTex", _texture);
            }

            _propertyBlock.SetColor("_HighlightColor", _highlightColor);
            _propertyBlock.SetFloat("_HighlightSize", 0f);

            _renderer = GetComponent<Renderer>();
            _renderer.SetPropertyBlock(_propertyBlock);

            HighlightColor = _selectionColor;
        }
    }
}