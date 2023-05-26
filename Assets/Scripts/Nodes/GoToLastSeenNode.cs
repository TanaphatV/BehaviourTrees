using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GoToLastSeenNode : Node
{
    EnemyAI ai;
    NavMeshAgent agent;
    public GoToLastSeenNode(EnemyAI ai, NavMeshAgent agent)
    {
        this.ai = ai;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        if (ai.playerLastPos == Vector3.zero)
            return NodeState.FAILURE;
        ai.SetColor(Color.magenta);
        float distance = Vector3.Distance(ai.playerLastPos,ai.gameObject.transform.position);
        if (distance > 1.2f)
        {
            agent.isStopped = false;
            agent.SetDestination(ai.playerLastPos);
            MonoBehaviour.print("DIS"+distance);
            return NodeState.RUNNING;
        }
        else
        {
            agent.isStopped = true;
            return NodeState.SUCCESS;
        }
    }
}
