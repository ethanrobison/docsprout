using Code.Session;
using Code.Session.UI;

namespace Code
{
    // Stub class for now. Will hold preferences, etc. later.
    public class GameSession
    {
        public PreferencesManager Prefs { get; private set; }
        public MenuManager Menus { get; private set; }

        private MainMenu _mainMenu;


        public void Initialize () {
            Prefs = new PreferencesManager();
            Menus = new MenuManager();

            Prefs.Initialize();
            Menus.Initialize();

            _mainMenu = new MainMenu();
            Menus.PushMenu(_mainMenu);
        }

//        private void Shutdown () {
//            Dialogs.ShutDown();
//            Prefs.ShutDown();
//
//            Dialogs = null;
//            Prefs = null;
//        }


        // todo this isn't very sophisticated in fact I dislike it
        public void StartGame (int scene) {
            var ctx = new GameContext();
            Game.SetContext(this, ctx);

            Menus.CloseAll();

            ctx.StartGame(scene);
        }
    }

    public interface ISessionManager
    {
        void Initialize ();
        void ShutDown ();
    }
}