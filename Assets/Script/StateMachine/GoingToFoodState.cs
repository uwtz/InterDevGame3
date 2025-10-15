using UnityEngine;

public class GoingToFoodState : State
{
    public override string name => "GoingToFood";
    public GoingToFoodState(Entity owner) : base(owner)
    {

    }

    public override void Enter()
    {
        owner.movingState.SetTarget(owner.target);
        owner.movingState.onArrive = () => owner.stateMachine.ChangeState(owner.eatingState);

        owner.stateMachine.ChangeState(owner.movingState);
    }

    public override void Update() { }

    public override void Exit() { }
}
