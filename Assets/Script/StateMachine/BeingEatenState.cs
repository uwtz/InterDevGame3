using UnityEngine;

public class BeingEatenState : State
{
    public override string name => "BeingEaten";
    public BeingEatenState(Entity owner) : base(owner)
    {

    }
    public override void Enter()
    {
    }

    public override void Update()
    {
        // leave beingEaten state if predator dies before finish eating
        if (!owner.HasPredator()) owner.stateMachine.ChangeState(owner.idleState);
    }

    public override void Exit()
    {
    }
}
