using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecentlySeenNode : Node
{
    EnemyAI ai;
    float recentTime;
    public RecentlySeenNode(EnemyAI ai,float recentTime)
    {
        this.ai = ai;
        this.recentTime = recentTime;
    }

    public override NodeState Evaluate()
    {
        if (Time.time - ai.playerLastSeenTime < recentTime)
            return NodeState.SUCCESS;
        return NodeState.FAILURE;
    }
}
