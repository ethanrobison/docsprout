using UnityEngine;

namespace Code.Characters.Doods.AI
{
    public class Wander : BehaviorTreeNode
    {
        private Vector2 _goal;

        public Wander (Dood dood) : base(dood) { }

        private const float DIST = 40f;

        public override void OnInitialize () {
            base.OnInitialize();
            var x = Random.Range(-DIST, DIST);
            var y = Random.Range(-DIST, DIST);
            _goal = new Vector3(x, 0f, y);
        }

        protected override Status Update () { return Dood.MoveTowards(_goal) ? Status.Success : Status.Running; }
    }
}