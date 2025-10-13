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
            if (owner.needFood)
            {
                owner.FindFood();
                if (owner.GetFood() != null)
                {
                    owner.food.GetComponent<Consumable>().ClaimConsumable(owner.gameObject);
                    owner.stateMachine.ChangeState(owner.goingToFoodState);
                    return;
                }
            }

            owner.stateMachine.ChangeState(owner.wanderingState);


            // default: go wander
            // if class is Steve: wander or eat
            // if class if Beef: reproduce or wander or eat

            // or? use owner.
        }    
    }
    public override void Exit() { }
}
