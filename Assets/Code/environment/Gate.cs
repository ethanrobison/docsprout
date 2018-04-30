using Code.Utils;
using UnityEngine;

namespace Code.Environment
{
    public class Gate : MonoBehaviour
    {
        public AudioClip OpenSound;
        public AudioClip CloseSound;
        private AudioSource _audioSource;
        private const float OPEN_ANGLE = 160f;
        private const float CLOSE_ANGLE = 0f;
        private const float DURATION = 1f;
        private bool _isOpen = false;
        private Transform _leftDoor;
        private Transform _rightDoor;
        private InteractionHighlight _highlight;

        private void Start () {
            _audioSource = gameObject.GetRequiredComponent<AudioSource>();
            _highlight = gameObject.GetRequiredComponent<InteractionHighlight>();
            _leftDoor = transform.Find("Left Door");
            _rightDoor = transform.Find("Right Door");
        }

        private void FixedUpdate () {
            if (_isAnimating) Animate(Time.fixedDeltaTime);
        }

        private float _timeAnimating;
        private bool _isAnimating;

        private void Animate (float deltaT) {
            if (Mathf.Approximately(_timeAnimating, 0f)) {
                _audioSource.clip = _isOpen ? CloseSound : OpenSound;
                _audioSource.Play();
            }

            _timeAnimating += deltaT / DURATION;
            if (_timeAnimating >= 1f) {
                _timeAnimating = 0f;
                _isAnimating = false;
                _isOpen = !_isOpen;
                return;
            }

            float a = _isOpen ? OPEN_ANGLE : CLOSE_ANGLE;
            float b = !_isOpen ? OPEN_ANGLE : CLOSE_ANGLE;
            float angle = Mathf.SmoothStep(a, b, _timeAnimating);
            if (_leftDoor != null) {
                _leftDoor.localRotation = Quaternion.Euler(0f, angle, 0f);
            }

            if (_rightDoor != null) {
                _rightDoor.localRotation = Quaternion.Euler(0f, -angle, 0f);
            }
        }

        public void Interact () { _isAnimating = true; }

        public void OnApproach () { _highlight.IsHighlighted = true; }

        public void OnDepart () { _highlight.IsHighlighted = false; }
    }
}