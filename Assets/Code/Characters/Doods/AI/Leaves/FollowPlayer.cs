using UnityEngine;

namespace Code.Characters.Doods.AI
{
    public class FollowPlayer : BehaviorTreeNode
    {
        private readonly float _goalDistance;
        private readonly Player.Player _player;

        public FollowPlayer (Dood dood, float distance = 5f) : base(dood) {
            _goalDistance = distance;
            _player = Game.Ctx.Player;
        }

        public override void OnTerminate (Status result) {
            base.OnTerminate(result);
            Dood.StopMoving();
        }

        protected override Status Update () {
            var dist = Vector3.Distance(Dood.transform.position, _player.transform.position);
            if (dist < _goalDistance) { return Status.Success; }

            Dood.MoveTowards(_player.transform.position, 5f);
            return Status.Running;
        }
    }
}