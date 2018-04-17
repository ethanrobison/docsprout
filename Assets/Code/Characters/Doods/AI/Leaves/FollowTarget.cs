using Code.Utils;
using UnityEngine;

namespace Code.Characters.Doods.AI
{
    public class FollowTarget<T> : BehaviorTreeNode where T : MonoBehaviour
    {
        private readonly int _mask;

        // we stay between far and close thresh if we can
        private readonly float _farThresh;
        private readonly float _closeThresh;

        private Transform _target;

        /// <summary>
        /// Follows targets of type T that come within thresh of the dood.
        /// </summary>
        public FollowTarget (Dood dood, int layermask, float near = 5, float far = 10f) : base(dood) {
            _mask = layermask;
            _closeThresh = near;
            _farThresh = far;
        }

        private static readonly Collider[] Results = new Collider[10]; // you can safely ignore this warning

        public override void OnInitialize () {
            base.OnInitialize();
            Logging.Assert(_target == null, "Already have target!");

            var count = Physics.OverlapSphereNonAlloc(Dood.transform.position, _farThresh, Results, _mask);
            if (count <= 0) { return; }

            var go = Results[0].gameObject;
            if (go.GetComponent<T>() == null) { return; }

            if (Vector3.Distance(go.transform.position, Dood.transform.position) < _closeThresh) { return; }

            _target = go.transform;
        }

        public override void OnTerminate (Status result) {
            base.OnTerminate(result);
            _target = null;
            Dood.StopMoving();
        }

        protected override Status Update () {
            if (_target == null) { return Status.Failure; }

            return Dood.MoveTowards(_target.position, _closeThresh) ? Status.Success : Status.Running;
        }
    }
}