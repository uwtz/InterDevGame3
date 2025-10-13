using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public abstract class Entity : Consumable
{
    private void Start()
    {
        StateStart();
        AIStart();
        eatingParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        HungerUpdate();
        stateMachine.Update();
    }


    [HideInInspector] public ParticleSystem eatingParticle;



    public StateMachine     stateMachine        { get; private set; }

    public IdleState        idleState           { get; private set; }
    public MovingState      movingState         { get; private set; }
    public WanderingState   wanderingState      { get; private set; }
    public GoingToFoodState goingToFoodState    { get; private set; }
    public EatingState      eatingState         { get; private set; }

    public string currentState;
    public bool debugState = false;

    private void StateStart()
    {
        stateMachine = new StateMachine(this);

        idleState = new IdleState(this);
        movingState = new MovingState(this);
        wanderingState = new WanderingState(this);
        goingToFoodState = new GoingToFoodState(this);
        eatingState = new EatingState(this);


        stateMachine.SetStartingState(idleState);
    }

    [HideInInspector]
    public  NavMeshAgent agent;

    private void AIStart()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }



    // Range 0 (hungry) to 1 (full)
    public float hunger = .7f;
    float lookForFoodThreshold = .6f;
    public bool needFood = false;
    public GameObject food;

    private void HungerUpdate()
    {
        if (hunger > 0)
        {
            hunger -= .01f * Time.deltaTime;
        }
        if (hunger <= 0)
        {
            Debug.Log(gameObject.name + " died of hunger");
            hunger = 0;
            Destroy(gameObject);
        }

        needFood = hunger <= lookForFoodThreshold ? true : false;
    }

    public void AddHunger(float h)
    {
        hunger += h;
        if (hunger > 1) hunger = 1;
    }


    public abstract string[] foods { get; }

    public GameObject FindFood()
    {
        food = GetNearestConsumable();
        return food;
    }
    public GameObject GetFood()
    {
        return food;
    }

    private GameObject GetNearestConsumable()
    {

        List<GameObject> objs = new List<GameObject>();
        for (int i=0; i<foods.Length; i++)
        {
            objs.AddRange(GameObject.FindGameObjectsWithTag(foods[i]));
        }
        
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject obj in objs)
        {
            Consumable c = obj.GetComponent<Consumable>();
            if (c == null) continue;

            float dist = Vector3.Distance(obj.transform.position, transform.position);
            if (!c.HasPredator() && dist < minDist)
            {
                nearest = obj;
                minDist = dist;
            }
        }
        return nearest;
    }

    public bool reachedFood = false;
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == food) reachedFood = true;
    }
    protected void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == food) reachedFood = false;
    }
}

/*
public class EntityOld
{
    [Header("Info")]
    public int maxHealth;
    private int health;

    // Range 0 (hungry) to 1 (full)
    public float hunger = 0;
    public float lookForFoodThreshold = .6f;

    public bool canMove = false;

    protected Vector2 target;
    protected GameObject food;

    //protected IAstarAI ai;

    NavMeshAgent agent;

    public StateMachine stateMachine { get; private set; }

    public virtual void Start()
    {
        //ai = GetComponent<IAstarAI>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public virtual void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

        //ai.canMove = canMove;
        //if (target != null) ai.destination = target;
        if (target != null)
        {
            if (canMove) agent.destination = target;
            else agent.destination = transform.position;
        }

        if (hunger > 0)
        {
            hunger -= .01f * Time.deltaTime;
            if (hunger <= 0)
            {
                Debug.Log(gameObject.name + " died of hunger");
                hunger = 0;
                Destroy(gameObject);
            }
        }
    }

    protected void TakeDamage(int d)
    {
        health -= d;
    }
    public GameObject GetNearestConsumableByTag(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject obj in objs)
        {
            Consumable c = obj.GetComponent<Consumable>();
            if (c == null) continue;

            float dist = Vector3.Distance(obj.transform.position, transform.position);
            if (!c.HasPredator() && dist < minDist)
            {
                nearest = obj;
                minDist = dist;
            }
        }
        return nearest;
    }

    public void SetTarget(Vector2 t)
    { target = t; }

    public void SetTarget(GameObject go)
    { SetTarget(go.transform.position); }

    public void StartMoving()
    { canMove = true; }

    public void StopMoving()
    { canMove = false; }


    private float destinationThreshold = .01f;
    public bool ReachedDestination()
    {
        bool r = (Vector2.Distance(transform.position, target) < destinationThreshold) || (food != null && reachedFood);
        //if (r) food = null;
        return r;
    }

    private bool reachedFood = false;
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == food) reachedFood = true;
    }
    protected void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == food) reachedFood = false;
    }


    
    public bool SetRandomTarget()
    {
        Vector2 fx = GameManager.instance.floorXBound;
        Vector2 fy = GameManager.instance.floorYBound;
        Vector2 center = new Vector2(Random.Range(fx[0], fx[1]), Random.Range(fy[0], fy[1]));
        Debug.Log(center);


        float range = 10.0f;
        Vector3 result;
        if (RandomPointOnNavMesh(center, range, out result))
        {
            Vector2 t = new Vector2(result.x, result.y);
            Debug.Log(t);
            SetTarget(t);
            return true;
        }

        return false;
    }

    
    // https://docs.unity3d.com/6000.2/Documentation/ScriptReference/AI.NavMesh.SamplePosition.html
    private bool RandomPointOnNavMesh(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
*/