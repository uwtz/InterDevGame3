using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public abstract class Entity : Consumable
{
    private void Start()
    {
        StateStart();
        AIStart();

        agent.speed = speed;
        eatingParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (!isDead)
        {
            stateMachine.Update();
            HungerUpdate();
            ReproductionUpdate();
        }
    }

    [Header("misc")]
    public float speed = 3f;

    [HideInInspector] public ParticleSystem eatingParticle;




    public StateMachine     stateMachine        { get; private set; }

    public IdleState        idleState           { get; private set; }
    public MovingState      movingState         { get; private set; }
    public WanderingState   wanderingState      { get; private set; }
    public GoingToFoodState goingToFoodState    { get; private set; }
    public EatingState      eatingState         { get; private set; }
    public BeingEatenState  beingEatenstate     { get; private set; }


    [Header("State Machine")]
    public string currentState;
    public bool debugState = false;

    public virtual void StateStart()
    {
        stateMachine = new StateMachine(this);

        idleState = new IdleState(this);
        movingState = new MovingState(this);
        wanderingState = new WanderingState(this);
        goingToFoodState = new GoingToFoodState(this);
        eatingState = new EatingState(this);
        beingEatenstate = new BeingEatenState(this);


        stateMachine.SetStartingState(idleState);
    }

    [HideInInspector]
    public NavMeshAgent agent;

    private void AIStart()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }


    [Header("Target")]
    public GameObject target;// { get; private set; }
    public void SetTarget(GameObject target)
    {
        this.target = target;
        reachedTarget = false;
    }
    public bool reachedTarget = false;


    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject == target) reachedTarget = true;
    }
    protected void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject == target) reachedTarget = true;
    }
    protected void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject == target) reachedTarget = false;
    }



    [Header("Hunger (0-hungry to 1-full)")]

    // Range 0 (hungry) to 1 (full)
    public float hunger = .5f;
    public float hungerLossRate = .005f;
    public float EatHungerThreshold = .6f;
    public bool needFood = false;

    private void HungerUpdate()
    {
        if (hunger > 0)
        {
            hunger -= hungerLossRate * Time.deltaTime;
        }
        if (hunger <= 0)
        {
            Debug.Log(gameObject.name + " died of hunger");
            //if (debugState) { Debug.Log(gameObject.name + " died of hunger"); }
            hunger = 0;
            Kill();
        }

        needFood = hunger <= EatHungerThreshold ? true : false;
    }

    public void AddHunger(float h)
    {
        hunger += h;
        if (hunger > 1) hunger = 1;
    }


    public abstract string[] foods { get; }

    public GameObject FindFood()
    {
        return GetNearestConsumable(foods);
    }

    protected GameObject GetNearestConsumable(string[] s)
    {

        List<GameObject> objs = new List<GameObject>();
        for (int i=0; i<s.Length; i++)
        {
            objs.AddRange(GameObject.FindGameObjectsWithTag(s[i]));
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

    protected GameObject GetNearestConsumable(string s)
    {
        return GetNearestConsumable(new string[] { s });
    }



    [Header("Reproduction")]

    public float reproductionCooldown = 0f; // cooldown is applied to both parent and child
    [HideInInspector] public float timeLastReproduced;
    public float reproductionHungerThreshold = .7f;
    public float reproductionHungerCost = .5f;
    public bool canReproduce = false;


    public virtual Vector2 WanderingPositionSelector()
    {
        return GameManager.GetRandomPointOnNavMesh();
    }

    private void ReproductionUpdate()
    {
        canReproduce = reproductionHungerThreshold <= hunger && Time.time-timeLastReproduced >= reproductionCooldown ? true : false;
    }



    public virtual void PostIdleStateSelector()
    {
        if (needFood)
        {
            GameObject f = FindFood();
            if (f != null)
            {
                SetTarget(f);
                target.GetComponent<Consumable>().ClaimConsumable(gameObject);
                stateMachine.ChangeState(goingToFoodState);
                return;
            }
        }
        stateMachine.ChangeState(wanderingState);
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