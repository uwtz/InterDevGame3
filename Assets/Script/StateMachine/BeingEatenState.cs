using UnityEngine;

public class BeingEatenState : State
{
    public override string name => "BeingEaten";
    public BeingEatenState(Entity owner) : base(owner)
    {

    }
    public override void Enter()
    {
        owner.agent.isStopped = true;
    }

    public override void Update()
    {

    }

    public override void Exit() { }
}
