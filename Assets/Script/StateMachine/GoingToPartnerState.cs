using UnityEngine;

public class GoingToPartnerState : State
{
    public override string name => "GoingToPartner";
    public GoingToPartnerState(Entity owner) : base(owner) { }

    public override void Enter()
    {
        ReproducingEntityBase o = (ReproducingEntityBase)owner;
        o.movingState.SetTarget(o.target);
        o.movingState.onArrive = () => o.stateMachine.ChangeState(o.reproducingState);
        o.movingState.onFail = () =>
        {
            o.isTaken = false;
            o.isFemale = false;
        };

        o.stateMachine.ChangeState(o.movingState);
    }

    public override void Update() { }

    public override void Exit() { }
}
