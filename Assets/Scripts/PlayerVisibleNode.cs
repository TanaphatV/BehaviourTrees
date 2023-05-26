using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisibleNode : Node
{
    private Transform target;
    private Transform origin;
    float range;
    EnemyAI ai;

    public PlayerVisibleNode(EnemyAI ai, Transform target, Transform origin, float range)
    {
        this.ai = ai;
        this.target = target;
        this.origin = origin;
        this.range = range;
    }

    public override NodeState Evaluate()
    {
        RaycastHit hit;
        if (!Physics.Raycast(origin.position, target.position - origin.position, out hit,range,LayerMask.GetMask("Cover")))
        {
            if(Vector3.Distance(origin.position,target.position) <= range)
            {
                ai.playerLastPos = target.position;
                ai.playerLastSeenTime = Time.time;
                return NodeState.SUCCESS;
            }
           

        }
        return NodeState.FAILURE;
    }
}
