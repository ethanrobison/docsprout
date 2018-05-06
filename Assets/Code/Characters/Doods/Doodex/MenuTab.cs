using UnityEngine;

namespace Code.Characters.Doods.Doodex
{
    public class MenuTab : DoodexTab
    {
        public MenuTab (Transform tr, Transform tabbar) : base(tr, tabbar) { }
        public override void OnInitialize () { }
        public override void OnShutdown () { }
    }
}