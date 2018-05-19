using System.Collections.Generic;
using Code.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Characters.Player.Doodex
{
    public class MarketTab : DoodexTab
    {
        private Transform _contents, _description;

        private readonly List<SeedInfo> _seeds = new List<SeedInfo> {
            new SeedInfo("Moste Basiq Dood"),
            new SeedInfo("Moste Basiq Dood 2"),
            new SeedInfo("Moste Basiq Dood 3"),
            new SeedInfo("Moste Basiq Dood 4")
        };

        public MarketTab (Transform tr, Transform tabbar) : base(tr, tabbar) { }

        public override void OnInitialize () {
            _contents = GO.transform.Find("Scroll View/Viewport/Content");
            _description = GO.transform.Find("Description");
            foreach (var seed in _seeds) { MakeSeedButton(seed); }
        }

        public override void OnShutdown () { }

        public override void Show () {
            base.Show();
            var first = _contents.GetComponentInChildren<Button>();
            first.Select();
            SelectInfo(first.GetComponent<SeedPurchaseContext>());
        }

        private void MakeSeedButton (SeedInfo info) {
            var slot = UIUtils.MakeUIPrefab(UIPrefab.SeedSlot, _contents);

            var button = slot.transform.Find("Button").gameObject;
            var ctx = button.AddComponent<SeedPurchaseContext>();
            ctx.Info = info;
            ctx.Tab = this;

            button.GetComponent<Button>().onClick.AddListener(ctx.Buy);
        }


        public void SelectInfo (SeedPurchaseContext ctx) {
            UIUtils.FindUICompOfType<Text>(_description, "Info").text = ctx.Info.Name;
            MoveDescriptionBox(ctx);
        }

        private void MoveDescriptionBox (SeedPurchaseContext ctx) {
            var pos = _description.position;
            pos.y = ctx.SlotPosition + ctx.SlotHeight / 2f;
            _description.position = pos;
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

        public float SlotPosition {
            get { return transform.parent.position.y; }
        }

        public float SlotHeight {
            get { return GetComponent<RectTransform>().rect.height; }
        }

        public void OnSelect (BaseEventData eventData) { Tab.SelectInfo(this); }

        public void Buy () { Game.Ctx.Doods.PurchaseSeed(Info); }
    }
}