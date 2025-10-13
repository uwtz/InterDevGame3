using UnityEngine;

public abstract class State
{
    public abstract string name { get; }
    protected Entity owner;

    public State(Entity owner)
    {
        this.owner = owner;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
