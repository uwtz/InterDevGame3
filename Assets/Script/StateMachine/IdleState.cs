using UnityEngine;

public class IdleState : State
{
    public override string name => "Idle";
    float idleDuration = 2f;
    float idleStartTime;


    public IdleState(Entity owner) : base(owner) { }
    public IdleState(Entity owner, float idleDuration) : base(owner)
    { this.idleDuration = idleDuration; }


    public override void Enter()
    { idleStartTime = Time.time; }

    public override void Update()
    {
        if (Time.time - idleStartTime >= idleDuration)
        {
            owner.PostIdleStateSelector();
        }    
    }
    public override void Exit() { }
}
