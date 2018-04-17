using UnityEngine;

namespace Code.Characters.Doods
{
    public class DoodColor : MonoBehaviour
    {
        [SerializeField] private Color _color;
        [SerializeField] private float _happiness;
        [SerializeField] private Texture _texture;
        private MaterialPropertyBlock _propertyBlock;
        private Renderer _renderer;

        public float Happiness {
            set {
                _happiness = value;
                _propertyBlock.SetFloat("_Happiness", _happiness);
                _renderer.SetPropertyBlock(_propertyBlock);
            }
        }

        private void Awake () {
            _propertyBlock = new MaterialPropertyBlock();
            _propertyBlock.SetColor("_Color", _color);
            _propertyBlock.SetFloat("_Happiness", _happiness);
            if (_texture) {
                _propertyBlock.SetTexture("_MainTex", _texture);
            }

            _renderer = GetComponent<Renderer>();
            _renderer.SetPropertyBlock(_propertyBlock);
        }
    }
}