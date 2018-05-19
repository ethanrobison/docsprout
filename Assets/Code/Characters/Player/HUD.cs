using Code.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Characters.Player
{
    public class HUDManager : IContextManager
    {
        public HUD HUD { get; private set; }

        public void Initialize () {
            HUD = new HUD();
            Game.Ctx.Economy.Stats.SpendFroot(0);
        }

        public void ShutDown () { }
    }

    public class HUD
    {
        private readonly Text _info;
        private Transform _buttons;

        public HUD () {
            var go = UIUtils.MakeUIPrefab(UIPrefab.HUD);
            _info = UIUtils.FindUICompOfType<Text>(go, "Info/Text");
            _buttons = go.transform.Find("Buttons");
            Game.Ctx.Economy.Stats.OnFrootChanged += OnFrootChanged;
        }

        private void OnFrootChanged (int froot) { _info.text = string.Format("{0}", froot); }
    }
}