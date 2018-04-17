using System;
using Code.Doods.AI;
using UnityEngine;

namespace Code.Characters.Doods.AI
{
    [RequireComponent(typeof(Dood))]
    public class BehaviorTree : MonoBehaviour
    {
        public Root Root { get; private set; }

        private void Start () {
            var dood = GetComponent<Dood>();
            var sel = new Selector(dood);

            var close = new PlayerDistance(dood, -1f, 5f);
            close.AddToEnd(new Idle(dood));

            var medium = new PlayerDistance(dood, 5f, 15f);
            medium.AddToEnd(new FollowPlayer(dood));

            var far = new PlayerDistance(dood, 15f, float.PositiveInfinity);
            far.AddToEnd(new Idle(dood));

            sel.AddToEnd(close);
            sel.AddToEnd(medium);
            sel.AddToEnd(far);

            Root = new Root(dood);
            Root.SetChild(sel);
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