using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private float lowHealthThreshold;
    [SerializeField] private float healthRestoreRate;

    [SerializeField] private float chasingRange;
    [SerializeField] private float shootingRange;

  
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Cover[] avaliableCovers;

    public Vector3 playerLastPos = Vector3.zero;
    public float playerLastSeenTime = Mathf.Infinity;
    public Vector3 randomPatrolPos = Vector3.zero;
   

    private Material material;
    private Transform bestCoverSpot;
    private NavMeshAgent agent;

    private Node topNode;

    //public GameObject marker;

    private float _currentHealth;
	public float currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        material = GetComponentInChildren<MeshRenderer>().material;
    }

    private void Start()
    {
        _currentHealth = startingHealth;
        ConstructBehahaviourTree();
    }

    private void ConstructBehahaviourTree()
    {
        IsCovereAvaliableNode coverAvaliableNode = new IsCovereAvaliableNode(avaliableCovers, playerTransform, this);
        GoToCoverNode goToCoverNode = new GoToCoverNode(agent, this);
        HealthNode healthNode = new HealthNode(this, lowHealthThreshold);
        IsCoveredNode isCoveredNode = new IsCoveredNode(playerTransform, transform);
        ChaseNode chaseNode = new ChaseNode(playerTransform, agent, this);
        RangeNode chasingRangeNode = new RangeNode(chasingRange, playerTransform, transform);
        RangeNode shootingRangeNode = new RangeNode(shootingRange, playerTransform, transform);
        ShootNode shootNode = new ShootNode(agent, this, playerTransform);
        PlayerVisibleNode visibleNode = new PlayerVisibleNode(this,playerTransform, transform, chasingRange);

        Sequence chaseSequence = new Sequence(new List<Node> { chasingRangeNode, chaseNode });
        Sequence shootSequence = new Sequence(new List<Node> { shootingRangeNode, shootNode });

        Sequence goToCoverSequence = new Sequence(new List<Node> { coverAvaliableNode, goToCoverNode });
        Selector findCoverSelector = new Selector(new List<Node> { goToCoverSequence, chaseSequence });
        Selector tryToTakeCoverSelector = new Selector(new List<Node> { isCoveredNode, findCoverSelector });
        Sequence mainCoverSequence = new Sequence(new List<Node> { healthNode, tryToTakeCoverSelector });

        Selector shootSelector = new Selector(new List<Node> { shootSequence, chaseSequence });
        Sequence PlayerVisible = new Sequence(new List<Node> { visibleNode, shootSelector});

        RandomPatrolNode randomNode = new RandomPatrolNode(agent, this,2.0f);
        LookAroundNode lookAroundNode = new LookAroundNode(5.0f, transform,this);

        GoToLastSeenNode goToLastSeenNode = new GoToLastSeenNode(this,agent);
        RecentlySeenNode recentNode = new RecentlySeenNode(this,10.0f);
        Sequence RecentlySeenSequence = new Sequence(new List<Node> { recentNode,goToLastSeenNode,lookAroundNode/* goto location patrol*/});
        Sequence NotSeenSequence = new Sequence(new List<Node> {/* new Inverter(recentNode),*/ randomNode,lookAroundNode});
        //Selector PlayerNotVisible = new Selector(new List<Node> { });

        topNode = new Selector(new List<Node> { mainCoverSequence, PlayerVisible, RecentlySeenSequence,NotSeenSequence });


    }

    private void Update()
    {
        topNode.Evaluate();
        if(topNode.nodeState == NodeState.FAILURE)
        {
            SetColor(Color.red);
            agent.isStopped = true;
        }
        currentHealth += Time.deltaTime * healthRestoreRate;
        //marker.transform.position = randomPatrolPos;

    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }

    public void SetBestCoverSpot(Transform bestCoverSpot)
    {
        this.bestCoverSpot = bestCoverSpot;
    }


    public Transform GetBestCoverSpot()
    {
        return bestCoverSpot;
    }


}
