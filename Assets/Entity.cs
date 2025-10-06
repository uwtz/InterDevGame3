using UnityEngine;
using Pathfinding;
using static UnityEngine.GraphicsBuffer;

public class Entity : MonoBehaviour
{
    [Header("Info")]
    public int maxHealth;
    int health;

    public enum State { Idle, Moving };
    public State state = State.Moving;

    public Transform target;

    IAstarAI ai;

    private void Start()
    {
        ai = GetComponent<IAstarAI>();
    }


    private void Update()
    {
        switch(state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Moving:
                Moving();
                break;
        }
    }

    private void Idle()
    {
    }
    private void Moving()
    {
        if (target != null && ai != null) ai.destination = target.position;
    }

    protected void TakeDamage(int d)
    {
        health -= d;
    }
}