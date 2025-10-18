using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AsexualReproducingState : State
{
    public override string name => "AsexualReproducing";
    float reproducingDuration = 1f;
    float reproducingStartTime;
    ParticleSystem heartParticle;


    public AsexualReproducingState(Entity owner) : base(owner)
    {

    }
    public override void Enter()
    {
        reproducingStartTime = Time.time;
        heartParticle = Object.Instantiate(GameManager.instance.heartParticle, owner.transform.position + new Vector3(0, 2), Quaternion.identity).GetComponent<ParticleSystem>();
        heartParticle.Play();
    }

    public override void Update()
    {
        if (Time.time - reproducingStartTime > reproducingDuration)
        {
            GameObject child = Object.Instantiate(GameManager.instance.beef, owner.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), Quaternion.identity);
            //GameObject child2 = Object.Instantiate(GameManager.instance.beef, owner.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), Quaternion.identity);
            //Object.Destroy(owner.gameObject);

            owner.AddHunger(-owner.reproductionHungerCost);
            owner.timeLastReproduced = Time.time;
            child.GetComponent<Entity>().timeLastReproduced = Time.time;
            //child2.GetComponent<Entity>().timeLastReproduced = Time.time;
            owner.stateMachine.ChangeState(owner.idleState);
        }
    }

    public override void Exit()
    {
        heartParticle.Stop();
        GameObject.Destroy(heartParticle.gameObject);
    }
}
