using UnityEngine;
using Pathfinding;
using static UnityEngine.GraphicsBuffer;

public class Entity : MonoBehaviour
{
    [Header("Info")]
    public int maxHealth;
    private int health;

    // Range 0 (hungry) to 1 (full)
    public float food = 0;

    public GameObject target;

    protected IAstarAI ai;

    public virtual void Start()
    {
        ai = GetComponent<IAstarAI>();
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
    public void SetDestination(Vector2 d)
    {
        if (d != null)
        {
            ai.destination = d;
        }
    }
    public void SetDestination(GameObject go)
    {
        SetDestination(go.transform.position);
    }
}