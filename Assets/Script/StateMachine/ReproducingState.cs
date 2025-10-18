using UnityEngine;

public class ReproducingState : State
{
    public override string name => "Reproducing";
    float reproducingDuration = 4f;
    float reproducingStartTime;
    ParticleSystem heartParticle;

    public ReproducingState(Entity owner) : base(owner)
    {

    }

    public override void Enter()
    {
        reproducingStartTime = Time.time;

        ReproducingEntityBase o = (ReproducingEntityBase)owner;
        if (o.isFemale)
        {
            heartParticle = Object.Instantiate(GameManager.instance.heartParticle, owner.transform.position + new Vector3(0,2), Quaternion.identity).GetComponent<ParticleSystem>();
            heartParticle.Play();
        }
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
        if (o.isFemale)
        {
            heartParticle.Stop();
            GameObject.Destroy(heartParticle.gameObject);
        }
        o.isTaken = false;
        o.isFemale = false;

    }
}
