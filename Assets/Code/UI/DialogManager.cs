using Code.Utils;

namespace Code.UI
{
    public class DialogManager : IContextManager
    {
        private readonly SmartStack<BaseDialog> _dialogs = new SmartStack<BaseDialog>();

        public void Initialize () { }

        public void ShutDown () { }

        public void PushDialog (BaseDialog dialog) { _dialogs.Push(dialog); }

        public void CloseToMe (BaseDialog dialog) {
            var element = _dialogs.Pop();
            while (element != dialog) { element = _dialogs.Pop(); }
        }

        public void CloseAll () { _dialogs.Clear(); }
    }
}