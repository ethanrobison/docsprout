using System.Collections;
using System.Collections.Generic;
using Code;
using Code.Characters.Doods;
using Code.Characters.Doods.AI;
using UnityEngine;

public class LookAtPlayer : BehaviorTreeNode
{
    public LookAtPlayer (Dood dood) : base(dood) { }

    protected override Status Update () {
        Vector3 playerPos = Game.Ctx.Player.transform.position;
        playerPos.y = Dood.transform.position.y;
        Dood.transform.LookAt(playerPos);
        Dood.StopMoving();
        return Status.Success;
    }
}