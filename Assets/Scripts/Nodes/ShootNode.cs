using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootNode : Node
{
    private NavMeshAgent agent;
    private EnemyAI ai;
    private Transform target;

    private Vector3 currentVelocity;
    private float smoothDamp;
    bool shooting = false;
    int shotCount = 0;
    float timer = 0;

    public ShootNode(NavMeshAgent agent, EnemyAI ai, Transform target)
    {
        this.agent = agent;
        this.ai = ai;
        this.target = target;
        smoothDamp = 1f;
    }

    public override NodeState Evaluate()
    {
        agent.isStopped = true;
        ai.SetColor(Color.green);
        Vector3 direction = target.position - ai.transform.position;
        Vector3 currentDirection = Vector3.SmoothDamp(ai.transform.forward, direction, ref currentVelocity, smoothDamp);
        Quaternion rotation = Quaternion.LookRotation(currentDirection, Vector3.up);
        ai.transform.rotation = rotation;

        if(shotCount < 3 && timer == 0)
        {
            Debug.Log("Shoot");
            shotCount++;
        }
        
        timer += Time.deltaTime;
        if (timer >= 0.6f)
            timer = 0;
        if (shotCount >= 3)
            return NodeState.SUCCESS;

        return NodeState.RUNNING;
    }

    IEnumerator Shoot()
    {
        for(int i = 0; i < 3; i++)
        {
            Debug.Log("Shoot");
            yield return new WaitForSeconds(0.5f);
        }
    }

}
