using UnityEngine;

namespace Code.Doods.AI {
	public class FollowTarget<T> : BehaviorTreeNode where T : MonoBehaviour {
		int _mask;

		// we stay between far and close thresh if we can
		readonly float _farThresh;
		readonly float _closeThresh;

		Transform _target;

		/// <summary>
		/// Follows targets of type T that come within thresh of the dood.
		/// </summary>
		public FollowTarget (Dood dood, int layermask, float near = 5, float far = 10f) : base (dood)
		{
			_mask = layermask;
			_closeThresh = near;
			_farThresh = far;
		}

		static readonly Collider [] _results = new Collider[10]; // you can safely ignore this warning
		public override void OnInitialize ()
		{
			base.OnInitialize ();
			Debug.Assert (_target == null, "Already have target!");

			int count = Physics.OverlapSphereNonAlloc (_dood.transform.position, _farThresh, _results, _mask);
			if (count > 0) {
				var go = _results [0].gameObject;
				if (go == null || go.GetComponent<T> () == null) {
					return;
				}
				if (Vector3.Distance (go.transform.position, _dood.transform.position) < _closeThresh) { return; }
				_target = go.transform;
			}
		}

		public override void OnTerminate (Status result)
		{
			base.OnTerminate (result);
			_target = null;
			_dood.StopMoving ();
		}

		protected override Status Update ()
		{
			if (_target == null) { return Status.Failure; }
			if (_dood.MoveTowards (_target.position, _closeThresh)) { return Status.Success; }
			return Status.Running;
		}
	}
}