using UnityEngine;

namespace Code.Doods.AI {
	public class FollowTarget<T> : BehaviorTreeNode where T : MonoBehaviour {
		int _mask;
		float _thresh;

		Transform _target;

		/// <summary>
		/// Follows targets of type T that come within thresh of the dood.
		/// </summary>
		public FollowTarget (Dood dood, int layermask, float thresh = 10f) : base (dood)
		{
			_mask = layermask;
			_thresh = thresh;
		}

		static Collider [] _results = new Collider [4]; // you can safely ignore this warning
		public override void OnInitialize ()
		{
			base.OnInitialize ();
			Debug.Assert (_target == null, "Already have target!");

			_results = Physics.OverlapSphere (_dood.transform.position, _thresh, _mask);
			if (_results.Length > 0) {
				var go = _results [0].gameObject;
				if (go == null || go.GetComponent<T> () == null) {
					_target = null;
					return;
				}
				_target = go.transform;
			}
		}

		public override void OnTerminate (Status result)
		{
			base.OnTerminate (result);
			_target = null;
		}

		protected override Status Update ()
		{
			if (_target == null) { return Status.Failure; }
			if (_dood.MoveTowards (_target.position)) { return Status.Success; }
			return Status.Running;
		}
	}
}