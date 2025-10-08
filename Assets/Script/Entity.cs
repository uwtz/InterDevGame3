using UnityEngine;
using Pathfinding;
using UnityEngine.AI;

public class Entity : Consumable
{
    [Header("Info")]
    public int maxHealth;
    private int health;

    // Range 0 (hungry) to 1 (full)
    public float hunger = 0;

    public bool canMove = false;

    protected Vector2 target;
    protected GameObject food;

    //protected IAstarAI ai;

    NavMeshAgent agent;

    public virtual void Start()
    {
        //ai = GetComponent<IAstarAI>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public virtual void Update()
    {
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
    { return Vector2.Distance(transform.position, target) < destinationThreshold; }

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