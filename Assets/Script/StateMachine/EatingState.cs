using UnityEngine;

public class EatingState : State
{
    public override string name => "Eating";
    float eatingDuration = 2f;
    float eatingStartTime;


    public EatingState(Entity owner) : base(owner)
    {

    }

    public override void Enter()
    {
        eatingStartTime = Time.time;
        owner.eatingParticle.Play();
        
        if (owner.food.TryGetComponent<Entity>(out Entity entity))
        {
            entity.stateMachine.ChangeState(entity.beingEatenstate);
        }
    }

    public override void Update()
    {
        if (owner.food == null)
        {
            owner.stateMachine.ChangeState(owner.idleState);
            return;
        }

        if (Time.time - eatingStartTime > eatingDuration)
        {
            Object.Destroy(owner.food);
            owner.AddHunger(.3f);
            owner.stateMachine.ChangeState(owner.idleState);
        }
    }

    public override void Exit()
    {
        owner.eatingParticle.Stop();
    }
}
