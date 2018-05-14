using System.Collections.Generic;
using Code.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Characters.Doods.Doodex
{
    public class MarketTab : DoodexTab
    {
        private Transform _contents, _description;

        private readonly List<SeedInfo> _seeds = new List<SeedInfo> { new SeedInfo("Moste Basiq Dood") };

        public MarketTab (Transform tr, Transform tabbar) : base(tr, tabbar) { }

        public override void OnInitialize () {
            _contents = GO.transform.Find("Right/Scroll View/Viewport/Content");
            _description = GO.transform.Find("Left/Description");
            foreach (var seed in _seeds) { MakeSeedButton(seed); }
        }

        public override void OnShutdown () { }

        public override void Show () {
            base.Show();
            var first = _contents.GetComponentInChildren<Button>();
            first.Select();
        }

        private void MakeSeedButton (SeedInfo info) {
            var slot = UIUtils.MakeUIPrefab(UIPrefab.SeedSlot, _contents);

            var button = slot.transform.Find("Button").gameObject;
            var ctx = button.AddComponent<SeedPurchaseContext>();
            ctx.Info = info;
            ctx.Tab = this;

            button.GetComponent<Button>().onClick.AddListener(ctx.Buy);
        }


        public void SelectInfo (SeedInfo info) {
            UIUtils.FindUICompOfType<Text>(_description, "Name").text = info.Name;
            UIUtils.FindUICompOfType<Text>(_description, "Info").text = info.Name;
        }
    }

    public class SeedInfo
    {
        public readonly string Name;

        public SeedInfo (string name) { Name = name; }
    }

    public class SeedPurchaseContext : MonoBehaviour, ISelectHandler
    {
        public SeedInfo Info;
        public MarketTab Tab;

        public void OnSelect (BaseEventData eventData) { Tab.SelectInfo(Info); }

        public void Buy () { Game.Ctx.Doods.PurchaseSeed(Info); }
    }
}