using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AsexualReproducingState : State
{
    public override string name => "AsexualReproducing";
    float reproducingDuration = 3f;
    float reproducingStartTime;



    public AsexualReproducingState(Entity owner) : base(owner)
    {

    }
    public override void Enter()
    {
        reproducingStartTime = Time.time;
    }

    public override void Update()
    {
        if (Time.time - reproducingStartTime > reproducingDuration)
        {
            Object.Instantiate(GameManager.instance.beef, owner.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), Quaternion.identity);
            //Object.Instantiate(GameManager.instance.beef, owner.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), Quaternion.identity);
            //Object.Destroy(owner.gameObject);
            owner.AddHunger(-.5f);

            owner.stateMachine.ChangeState(owner.idleState);
        }
    }

    public override void Exit()
    {
    }
}
