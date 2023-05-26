using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomPatrolNode : Node
{
    private NavMeshAgent agent;
    private EnemyAI ai;
    private float stoppingDistance;

    public RandomPatrolNode(NavMeshAgent agent, EnemyAI ai, float stoppingDistance)
    {
        this.agent = agent;
        this.ai = ai;
        this.stoppingDistance = stoppingDistance;
    }

    public override NodeState Evaluate()
    {
        ai.SetColor(Color.black);
        if (ai.randomPatrolPos == Vector3.zero)
        {
            float radius = 35.0f;
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
            randomDirection.y = 0;
            randomDirection += ai.transform.position;
           
            ai.randomPatrolPos = randomDirection;
            
        }
        agent.isStopped = false;
        agent.SetDestination(ai.randomPatrolPos);

        if (Vector3.Distance(ai.randomPatrolPos,ai.transform.position) < 2.0f)
        {
            agent.isStopped = true;
            ai.randomPatrolPos = Vector3.zero;
            return NodeState.SUCCESS;
        }

        

        return NodeState.RUNNING;
    }
}
