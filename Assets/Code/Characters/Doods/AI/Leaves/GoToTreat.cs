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
            var goal = Dood.Comps.Status.TreatLocations[0];
            if (goal == null) { return Status.Failure; }

            return Dood.MoveTowards(goal.position, 3f) ? Status.Success : Status.Running;
        }
    }
}