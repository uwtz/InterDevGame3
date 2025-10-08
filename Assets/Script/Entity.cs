using UnityEngine;
using Pathfinding;

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

    protected IAstarAI ai;

    public virtual void Start()
    {
        ai = GetComponent<IAstarAI>();
    }

    public virtual void Update()
    {
        if (hunger > 0)
        {
            hunger -= .01f * Time.deltaTime;
            if (hunger < 0)
            {
                Debug.Log(gameObject.name + "died of hunger");
                hunger = 0;
                Destroy(gameObject);
            }
        }

        ai.canMove = canMove;
        if (target != null) ai.destination = target;
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
    { target = go.transform.position; }

    public void StartMoving()
    { canMove = true; }

    public void StopMoving()
    { canMove = false; }
}