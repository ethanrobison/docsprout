using UnityEngine;

namespace Code.Environment
{
    public class InteractionHighlight : MonoBehaviour
    {
        private const float HIGHLIGHT_THICKNESS = .1f;
        [SerializeField] private bool _isHighlighted;
        private Renderer[] _renderers;
        private MaterialPropertyBlock _props;

        public bool IsHighlighted {
            get { return _isHighlighted; }
            set {
                if (value == _isHighlighted) return;
                _isHighlighted = value;
                _props.SetFloat("_OutlineSize", _isHighlighted ? HIGHLIGHT_THICKNESS : 0f);
                foreach (var r in _renderers) {
                    r.SetPropertyBlock(_props);
                }
            }
        }

        private void Start () {
            _renderers = GetComponentsInChildren<Renderer>();
            _props = new MaterialPropertyBlock();
            _props.SetFloat("_OutlineSize", _isHighlighted ? HIGHLIGHT_THICKNESS : 0f);
            foreach (var r in _renderers) {
                r.SetPropertyBlock(_props);
            }
        }
    }
}