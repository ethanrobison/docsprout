using UnityEngine;

namespace Code.Characters.Doods.AI
{
    public class LookAtPlayer : BehaviorTreeNode
    {
        public LookAtPlayer (Dood dood) : base(dood) { }

        protected override Status Update () {
            var playerPos = Game.Ctx.Player.transform.position;
            playerPos.y = Dood.transform.position.y;
            var goalrotation = Quaternion.LookRotation(playerPos - Dood.transform.position);
            Dood.transform.rotation = Quaternion.Slerp(Dood.transform.rotation, goalrotation, 0.1f);
            
            Dood.StopMoving();
            return Status.Success;
        }
    }
}