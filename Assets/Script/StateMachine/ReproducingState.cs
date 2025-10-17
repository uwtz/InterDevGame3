using UnityEngine;

public class ReproducingState : State
{
    public override string name => "Reproducing";
    float reproducingDuration = 4f;
    float reproducingStartTime;

    public ReproducingState(Entity owner) : base(owner)
    {

    }

    public override void Enter()
    {
        reproducingStartTime = Time.time;
    }

    public override void Update()
    {
        if (Time.time - reproducingStartTime >= reproducingDuration)
        {
            ReproducingEntityBase o = (ReproducingEntityBase)owner;
            o.Reproduce();
        }
    }

    public override void Exit()
    {
        ReproducingEntityBase o = (ReproducingEntityBase)owner;
        o.isTaken = false;
        o.isFemale = false;
    }
}
