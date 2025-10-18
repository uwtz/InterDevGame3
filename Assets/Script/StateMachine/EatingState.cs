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

        if (owner.target == null)
        {
            owner.stateMachine.ChangeState(owner.idleState);
            return;
        }

        if (owner.target.TryGetComponent<Entity>(out Entity entity))
        {
            entity.stateMachine.ChangeState(entity.beingEatenstate);
        }

        if (owner.animator != null)
        { owner.animator.SetBool("isEating", true); }
    }

    public override void Update()
    {
        if (owner.target == null)
        {
            owner.stateMachine.ChangeState(owner.idleState);
            return;
        }

        if (Time.time - eatingStartTime > eatingDuration)
        {
            //Debug.Log(owner.name + " ate " + owner.target.name);
            owner.target.GetComponent<Consumable>().Kill();
            owner.AddHunger(.4f);
            owner.stateMachine.ChangeState(owner.idleState);
        }
    }

    public override void Exit()
    {
        owner.eatingParticle.Stop();

        if (owner.animator != null)
        { owner.animator.SetBool("isEating", false); }
    }
}
