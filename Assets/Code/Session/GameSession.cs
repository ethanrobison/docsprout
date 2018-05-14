using Code.Characters.Doods.LifeCycle;
using UnityEngine.SceneManagement;

namespace Code.Session
{
    public enum SceneIndex
    {
        Current = -1,
        MainMenu = 0,
        Demo = 1,
        Ethan = 2,
        Kyle = 3,
        Alyssa = 4
    }

    public class GameSession
    {
        public PreferencesManager Prefs { get; private set; }
        public InputManager Input { get; private set; }

        public void Initialize () {
            Prefs = new PreferencesManager();
            Input = new InputManager();

            Prefs.Initialize();
            Input.Initialize();
            Doodopedia.LoadSpecies();

            StartGame(SceneIndex.MainMenu);
        }


        // todo this isn't very sophisticated in fact I dislike it
        public void StartGame (SceneIndex index) {
            ResetContext();
            if (index == SceneIndex.MainMenu) {
                Input.OnGameStop();
                Prefs.OnGameStop();
            }
            else {
                Input.OnGameStart();
                Prefs.OnGameStart();
            }

            Game.Ctx.StartGame(index);
        }

        public void ReturnToMenu () { ResetContext(); }

        private void ResetContext () { Game.SetContext(this, new GameContext()); }
    }

    public interface ISessionManager
    {
        void Initialize ();
        void ShutDown ();
        void OnGameStart ();
        void OnGameStop ();
    }
}