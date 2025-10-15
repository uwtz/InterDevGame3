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
            Steve o = (Steve)owner;
            if (o.isFemale)
            {
                GameObject child = Object.Instantiate(GameManager.instance.steve, owner.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), Quaternion.identity);
                child.GetComponent<Entity>().timeLastReproduced = Time.time;
            }

            owner.AddHunger(-owner.reproductionHungerCost);
            owner.timeLastReproduced = Time.time;
            owner.stateMachine.ChangeState(owner.idleState);
        }
    }

    public override void Exit()
    {
        Steve o = (Steve)owner;
        o.isTaken = false;
        o.isFemale = false;
    }
}
