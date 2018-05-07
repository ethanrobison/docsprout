using System;
using System.Linq;
using Code.Characters.Doods.Needs;
using UnityEngine;

namespace Code.Characters.Doods.AI
{
    public class BehaviorTree : MonoBehaviour
    {
        public Root Root;

        private Sequence _passive;
        private Selector _active;
        private Need[] _needs;


        private void Start () {
            var dood = GetComponentInParent<Dood>();

            InitializeTree(dood);

//            var close = new PlayerDistance(dood, -1f, 5f);
//            close.AddToEnd(new Idle(dood));
//            close.AddToEnd(new LookAtPlayer(dood));
            var selected = new SelectedFilter(dood);
            var selectedSelector = new Selector(dood);
            var selectedClose = new PlayerDistance(dood, -1f, 5f);
            selectedClose.AddToEnd(new LookAtPlayer(dood));
            selectedSelector.AddToEnd(selectedClose);
            selectedSelector.AddToEnd(new FollowPlayer(dood));
            selected.AddToEnd(selectedSelector);

            var far = new PlayerDistance(dood, 20f, float.PositiveInfinity);
            far.AddToEnd(new Wander(dood));

            var lookAtPlayer = new LookAtPlayer(dood);


//            AddActiveNode(dood, close);
            AddActiveNode(dood, selected);

            AddPassiveNeed(dood, NeedType.Water);
            AddPassiveNeed(dood, NeedType.Sun);
//            AddActiveNeed(dood, NeedType.Fun);
            AddActiveNeed(dood, NeedType.Food);

            var need = _needs.First(n => n.Type == NeedType.Fun);
            var needAdvertiser = new AdvertiserNear(dood, NeedType.Fun);
            var needSatisfaction = new NeedSatisfiable(dood, NeedType.Fun);
            needSatisfaction.AddToEnd(new InteractWithSatisfier(dood, NeedType.Fun));
            needAdvertiser.AddToEnd(needSatisfaction);

            var goToNeed = new AlwaysSucceed(dood);
            goToNeed.SetChild(new GoToAdvertiser(dood, NeedType.Fun));
            needAdvertiser.AddToEnd(goToNeed);
            AddActiveNode(dood, needAdvertiser);

            AddActiveNode(dood, far);

            AddActiveNode(dood, lookAtPlayer);

            FinishBuildingTree(dood);
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


        private void InitializeTree (Dood dood) {
            _needs = dood.GetComponentsInChildren<Need>();
            _passive = new Sequence(dood);
            _active = new Selector(dood);
        }

        private void FinishBuildingTree (Dood dood) {
            var activeAlwaysSucceed = new AlwaysSucceed(dood);
            activeAlwaysSucceed.SetChild(_active);

            _passive.AddToEnd(activeAlwaysSucceed);

            Root = new Root(dood);
            Root.SetChild(_passive);
        }

        private void AddPassiveNode (Dood dood, BehaviorTreeNode node) {
            var alwaysSucceed = new AlwaysSucceed(dood);
            alwaysSucceed.SetChild(node);
            _passive.AddToEnd(alwaysSucceed);
        }

        private void AddActiveNode (Dood dood, BehaviorTreeNode node) { _active.AddToEnd(node); }


        private void AddPassiveNeed (Dood dood, NeedType type) {
            var need = _needs.First(n => n.Type == type);
            var needSatisfaction = new NeedSatisfiable(dood, type);
            needSatisfaction.AddToEnd(new InteractWithSatisfier(dood, type));
            AddPassiveNode(dood, needSatisfaction);

            var needNode = new NeedLevel(dood, need);
            var needAdvertiser = new AdvertiserNear(dood, type);
            needSatisfaction = new NeedSatisfiable(dood, type);
            needSatisfaction.AddToEnd(new Idle(dood));
            needAdvertiser.AddToEnd(needSatisfaction);

            var goToNeed = new AlwaysSucceed(dood);
            goToNeed.SetChild(new GoToAdvertiser(dood, type));
            needAdvertiser.AddToEnd(goToNeed);
            needNode.AddToEnd(needAdvertiser);
            AddActiveNode(dood, needNode);
        }

        private void AddActiveNeed (Dood dood, NeedType type) {
            var need = _needs.First(n => n.Type == type);
            var needNode = new NeedLevel(dood, need);
            var needAdvertiser = new AdvertiserNear(dood, type);
            var needSatisfaction = new NeedSatisfiable(dood, type);
            needSatisfaction.AddToEnd(new InteractWithSatisfier(dood, type));
            needAdvertiser.AddToEnd(needSatisfaction);

            var goToNeed = new AlwaysSucceed(dood);
            goToNeed.SetChild(new GoToAdvertiser(dood, type));
            needAdvertiser.AddToEnd(goToNeed);
            needNode.AddToEnd(needAdvertiser);
            AddActiveNode(dood, needNode);
        }
    }
}