using UnityEngine;

public class WanderingState : State
{
    public override string name => "Wandering";
    public WanderingState(Entity owner) : base(owner)
    {

    }

    public override void Enter()
    {
        owner.movingState.SetTarget(owner.WanderingPositionSelector());
        owner.movingState.onArrive = () => owner.stateMachine.ChangeState(owner.idleState);

        owner.stateMachine.ChangeState(owner.movingState);
    }

    public override void Update() { }

    public override void Exit() { }
}

