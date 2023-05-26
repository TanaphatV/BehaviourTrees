using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class LookAroundNode : Node
{
    private float lookDuration;
    private float elapsedTime;
    private Transform transform;
    EnemyAI ai;
    public LookAroundNode(float lookDuration, Transform transform, EnemyAI ai)
    {
        this.lookDuration = lookDuration;
        this.transform = transform;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {

            elapsedTime += Time.deltaTime;
        ai.SetColor(Color.black);

        if (elapsedTime < lookDuration)
        {
            transform.Rotate(0, Random.Range(0, 360) * Time.deltaTime, 0);
            return NodeState.RUNNING;
        }

        elapsedTime = 0;
        return NodeState.SUCCESS;
    }
}