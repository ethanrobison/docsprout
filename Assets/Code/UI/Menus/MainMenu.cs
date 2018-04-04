namespace Code.UI.Menus
{
    public class MainMenu : Menu
    {
        public override void CreateGameObject () {
            GO = UIUtils.MakeUIPrefab(UIPrefab.MainMenu);
        }

        public override void RemoveGameObject () { }
    }
}