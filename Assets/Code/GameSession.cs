using Code.UI.Menus;

namespace Code
{
    // Stub class for now. Will hold preferences, etc. later.
    public class GameSession
    {
        private MainMenu _mainMenu;

        public void Initialize () {
            _mainMenu = new MainMenu();
            _mainMenu.CreateGameObject();
        }

        private void StartGame () {
            var ctx = new GameContext();
            Game.SetContext(this, ctx);

            ctx.StartGame();
        }
    }
}