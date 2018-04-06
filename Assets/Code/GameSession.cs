using Code.Session;
using Code.UI.Menus;

namespace Code
{
    // Stub class for now. Will hold preferences, etc. later.
    public class GameSession
    {
        public PreferencesManager Prefs { get; private set; }

        private MainMenu _mainMenu;


        public void Initialize () {
            Prefs = new PreferencesManager();

            Prefs.Initialize();

            _mainMenu = new MainMenu();
            _mainMenu.CreateGameObject();
        }


        // todo this isn't very sophisticated in fact I dislike it
        public void StartGame (int scene) {
            var ctx = new GameContext();
            Game.SetContext(this, ctx);

            _mainMenu.RemoveGameObject();

            ctx.StartGame(scene);
        }
    }

    public interface ISessionManager
    {
        void Initialize ();
        void ShutDown ();
    }
}