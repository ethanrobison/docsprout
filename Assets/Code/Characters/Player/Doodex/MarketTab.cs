using System.Collections.Generic;
using Code.Characters.Doods.LifeCycle;
using Code.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Code.Characters.Player.Doodex
{
    public class MarketTab : DoodexTab
    {
        private Transform _contents, _description;

        private readonly List<SeedInfo> _seeds = new List<SeedInfo>();

        public MarketTab (Transform tr, Transform tabbar) : base(tr, tabbar) { }

        public override void OnInitialize () {
            _contents = GO.transform.Find("Scroll View/Viewport/Content");
            _description = GO.transform.Find("Description");

            SetupSeedInfo();
            foreach (var seed in _seeds) { MakeSeedButton(seed); }
        }

        public override void OnShutdown () { }

        public override void Show () {
            base.Show();
            var first = _contents.GetComponentInChildren<Button>();
            first.Select();
            SelectInfo(first.GetComponent<SeedPurchaseContext>());
        }


        //
        // non-overridden crap

        private void SetupSeedInfo () {
            for (var s = Species.Vine; s <= Species.Cactus; s++) { _seeds.Add(new SeedInfo(s)); }
        }

        private void MakeSeedButton (SeedInfo info) {
            var slot = UIUtils.MakeUIPrefab(UIPrefab.SeedSlot, _contents);

            var button = slot.transform.Find("Button").gameObject;
            var ctx = button.AddComponent<SeedPurchaseContext>();
            ctx.Info = info;
            ctx.Tab = this;
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
        public readonly Species Species;

        public SeedInfo (Species species) {
            Species = species;
            Name = Species.ToString();
        }
    }

    public abstract class PurchaseContext : MonoBehaviour, ISelectHandler
    {
        public abstract void OnSelect (BaseEventData eventData);
        protected abstract void Buy ();

        private void Start () {
            var button = GetComponent<Button>();
            button.onClick.AddListener(Buy);
        }
    }

    public class SeedPurchaseContext : PurchaseContext
    {
        public SeedInfo Info;
        public MarketTab Tab;

        public float SlotPosition {
            get { return transform.parent.position.y; }
        }

        public float SlotHeight {
            get { return GetComponent<RectTransform>().rect.height; }
        }

        public override void OnSelect (BaseEventData eventData) { Tab.SelectInfo(this); }
        protected override void Buy () { Game.Ctx.Doods.PurchaseSeed(Info); }
    }

    public class ItemPurchaseContext : PurchaseContext
    {
        public override void OnSelect (BaseEventData eventData) { }
        protected override void Buy () { }
    }
}