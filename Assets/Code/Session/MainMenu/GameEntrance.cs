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
        private float timer = 0f;
        Image fadeImage;

        private void Start () {
            fadeImage = UIUtils.GetCanvas().transform.Find("Fade Panel").gameObject.GetRequiredComponent<Image>();
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
            if (player == null) return;
            StopFade();
        }

        private void Update () {
            if (timer <= 0f) return;
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                Game.Sesh.StartGame((int) SceneIdx.Demo);
                SceneManager.sceneLoaded += OnSceneLoad;
                return;
            }

            var fadeColor = fadeImage.color;
            fadeColor.a = 1 - timer / FADE_TIME;
            fadeImage.color = fadeColor;
        }

        private void StartFade () { timer = FADE_TIME; }

        private void StopFade () {
            var fadeColor = fadeImage.color;
            fadeColor.a = 0f;
            fadeImage.color = fadeColor;
            timer = 0f;
        }

        private void OnSceneLoad (Scene scene, LoadSceneMode mode) {
            var fadeColor = fadeImage.color;
            fadeColor.a = 0f;
            fadeImage.color = fadeColor;
            SceneManager.sceneLoaded -= OnSceneLoad;
        }
    }
}