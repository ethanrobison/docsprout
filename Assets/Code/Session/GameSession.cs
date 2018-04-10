using Code.Session.UI;

namespace Code.Session {
	// Stub class for now. Will hold preferences, etc. later.
	public class GameSession {
		//        public PlatformManager Platform { get; private set; }
		public PreferencesManager Prefs { get; private set; }
		public MenuManager Menus { get; private set; }
		public InputManager Input { get; private set; }

		private MainMenu _mainMenu;


		public void Initialize ()
		{
			//            Platform = new PlatformManager();
			Prefs = new PreferencesManager ();
			Menus = new MenuManager ();
			Input = new InputManager();

			//            Platform.Initialize();
			Prefs.Initialize ();
			Menus.Initialize ();
			Input.Initialize ();

			_mainMenu = new MainMenu ();
			//Menus.PushMenu (_mainMenu);
		}

		//        private void Shutdown () {
		//            Dialogs.ShutDown();
		//            Prefs.ShutDown();
		//
		//            Dialogs = null;
		//            Prefs = null;
		//        }


		// todo this isn't very sophisticated in fact I dislike it
		public void StartGame (int scene)
		{
			var ctx = new GameContext ();
			Game.SetContext (this, ctx);

			Menus.CloseAll ();

			ctx.StartGame (scene);
		}
	}

	public interface ISessionManager {
		void Initialize ();
		void ShutDown ();
	}
}