using UnityEngine;
using Code.Environment.Advertising;
using Code.Characters.Doods;

namespace Code.environment
{
    public class Treat : MonoBehaviour
    {
        private const float DECAY_RATE = 5f;
        private float _lifetime = 100f;
        private Satisfier _satisfier;

        private void Start () {
            _satisfier = GetComponentInChildren<Satisfier>();
            _satisfier.OnInteract += Snacking;
        }

        private void Snacking (Dood dood) {
            _lifetime -= DECAY_RATE * Time.deltaTime;
            if (_lifetime <= 0f) { Destroy(gameObject); }
        }
    }
}