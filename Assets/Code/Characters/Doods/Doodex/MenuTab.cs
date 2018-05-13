using Code.Session;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Characters.Doods.Doodex
{
    public class MenuTab : DoodexTab
    {
        private Button _mainMenu, _options;

        public MenuTab (Transform tr, Transform tabbar) : base(tr, tabbar) { }

        public override void Show () {
            base.Show();
            _options.Select();
        }

        public override void OnInitialize () {
            _mainMenu = GO.transform.Find("Main Menu").GetComponent<Button>();
            _mainMenu.onClick.AddListener(() => {
                Game.Ctx.Doods.Doodex.Hide();
                Game.Sesh.ReturnToMenu();
            });
            _options = GO.transform.Find("Options").GetComponent<Button>();
        }

        public override void OnShutdown () { }
    }
}