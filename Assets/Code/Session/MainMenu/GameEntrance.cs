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
        private float _timer = -1f;
        private Image _fadeImage;

        private void Start () { _fadeImage = UIUtils.FindUICompOfType<Image>(UIUtils.GetCanvas(), "Fade Panel"); }

        private void OnTriggerEnter (Collider other) {
            var dood = other.gameObject.GetComponent<Dood>();
            if (dood != null && dood.IsSelected) { StartFade(); }
        }

        private void OnTriggerExit (Collider other) {
            if (other.gameObject.GetComponent<Player>() != null) { StopFade(); }
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