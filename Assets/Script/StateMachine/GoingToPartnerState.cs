using UnityEngine;

public class GoingToPartnerState : State
{
    public override string name => "GoingToPartner";
    public GoingToPartnerState(Entity owner) : base(owner) { }

    public override void Enter()
    {
        Steve o = (Steve)owner;
        o.movingState.SetTarget(o.target);
        o.movingState.onArrive = () => o.stateMachine.ChangeState(o.reproducingState);

        o.stateMachine.ChangeState(o.movingState);
    }

    public override void Update() { }

    public override void Exit() { }
}
