using UnityEngine;
using Pathfinding;
using static UnityEngine.GraphicsBuffer;

public class Entity : MonoBehaviour
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
            if (hunger < 0) hunger = 0;
        }

        ai.canMove = canMove;
        if (target != null) ai.destination = target;
    }

    protected void TakeDamage(int d)
    {
        health -= d;
    }
    public GameObject GetNearestByTag(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject obj in objs)
        {
            float dist = Vector3.Distance(obj.transform.position, transform.position);
            if (dist < minDist)
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