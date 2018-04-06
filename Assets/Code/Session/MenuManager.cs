using Code.UI.Menus;
using Code.Utils;

namespace Code.Session
{
    public class MenuManager : ISessionManager
    {
        private readonly SmartStack<Menu> _menus = new SmartStack<Menu>();

        public void Initialize () { }

        public void ShutDown () { }

        public void PushMenu (Menu menu) {
            _menus.Push(menu);
        }

        public void CloseAll () {
            _menus.Clear();
        }

        public void CloseToMe (Menu menu) {
            var element = _menus.Pop();
            while (element != menu) { element = _menus.Pop(true); }
        }
    }
}