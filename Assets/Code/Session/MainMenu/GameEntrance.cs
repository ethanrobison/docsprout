using Code.Characters.Doods;
using Code.Characters.Player;
using Code.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Code.Session.MainMenu
{
    public class GameEntrance : MonoBehaviour
    {
        private const float FADE_TIME = 1f;
        private float _timer;
        private Image _fadeImage;

        private void Start () {
            _fadeImage = UIUtils.GetCanvas().transform.Find("Fade Panel").gameObject.GetRequiredComponent<Image>();
        }

        private void OnTriggerEnter (Collider other) {
            var dood = other.gameObject.GetComponent<Dood>();
            if (dood == null) return;
            if (dood.IsSelected) {
                StartFade();
            }
        }

        private void OnTriggerExit (Collider other) {
            var player = other.gameObject.GetComponent<Player>();
            if (player == null) { return; }

            StopFade();
        }

        private void Update () {
            if (_timer < 0f) { return; }

            _timer -= Time.deltaTime;
            if (_timer < 0f) {
                Game.Sesh.StartGame(SceneIndex.Demo);
                SceneManager.sceneLoaded += OnSceneLoad;
                return;
            }

            var fadeColor = _fadeImage.color;
            fadeColor.a = 1 - _timer / FADE_TIME;
            _fadeImage.color = fadeColor;
        }

        private void StartFade () { _timer = FADE_TIME; }

        private void StopFade () {
            var fadeColor = _fadeImage.color;
            fadeColor.a = 0f;
            _fadeImage.color = fadeColor;
            _timer = 0f;
        }

        private void OnSceneLoad (Scene scene, LoadSceneMode mode) {
            var fadeColor = _fadeImage.color;
            fadeColor.a = 0f;
            _fadeImage.color = fadeColor;
            SceneManager.sceneLoaded -= OnSceneLoad;
        }
    }
}