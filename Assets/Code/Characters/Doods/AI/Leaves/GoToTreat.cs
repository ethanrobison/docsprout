using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Characters.Doods.AI
{
    public class GoToTreat : BehaviorTreeNode
    {
        public GoToTreat (Dood dood) : base(dood) { }

        public override void OnTerminate (Status result) {
            base.OnTerminate(result);
            Dood.StopMoving();
        }

        protected override Status Update () {
            var goal = TryGetGoal();
            if (goal == null) { return Status.Failure; }

            return Dood.MoveTowards(goal.position) ? Status.Success : Status.Running;
        }

        private Transform TryGetGoal () {
            var treat = Dood.Comps.Status.Advertisers.Find(a => a.IsTreat);
            return treat != null ? treat.transform : null;
        }
    }
}