using UnityEngine;

public class StateMachine
{
    public Entity owner;
    public State currentState { get; private set; }

    public StateMachine(Entity owner) { this.owner = owner; }

    public void ChangeState(State state)
    {
        if (owner.debugState) { Debug.Log(owner.name + " from " + currentState.name + " to " + state.name); }
        owner.currentState = state.name;
        currentState.Exit();
        currentState = state;
        currentState.Enter();

    }

    public void Update()
    { currentState.Update(); }

    public void SetStartingState(State state)
    {
        if (currentState != null) return;
        currentState = state;
        owner.currentState = state.name;
        if (owner.debugState) { Debug.Log(owner.name + " starting state: " + state.name); }
    }
}
