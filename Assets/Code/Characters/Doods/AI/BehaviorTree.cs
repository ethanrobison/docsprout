using System;
using Code.Characters.Doods.Needs;
using UnityEngine;

namespace Code.Characters.Doods.AI
{
    public class BehaviorTree : MonoBehaviour
    {
        public Root Root;

        private void Start () {
            var dood = GetComponentInParent<Dood>();
            var sel = new Selector(dood);

            var close = new PlayerDistance(dood, -1f, 5f);
            close.AddToEnd(new Idle(dood));

            var medium = new PlayerDistance(dood, 5f, 20f);
            medium.AddToEnd(new FollowPlayer(dood));

            var far = new PlayerDistance(dood, 20f, float.PositiveInfinity);
            far.AddToEnd(new Wander(dood));

            sel.AddToEnd(close);
            sel.AddToEnd(medium);
            sel.AddToEnd(far);

            // todo this is shite
            var water = new NeedLevel<Waterable>(dood, dood.GetComponent<Waterable>());
            var wateradvertiser = new AdvertiserNear(dood, NeedType.Water);
            wateradvertiser.AddToEnd(new InteractWithAdvertiser(dood, NeedType.Water));
            water.AddToEnd(wateradvertiser);

            var top = new Selector(dood);
            top.AddToEnd(water);
            top.AddToEnd(sel);

            Root = new Root(dood);
            Root.SetChild(top);
        }

        private void Update () {
            var status = Root.Tick();
            switch (status) {
                case Status.Invalid:
                    break;
                case Status.Running:
                    break;
                case Status.Success:
                    break;
                case Status.Failure:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}