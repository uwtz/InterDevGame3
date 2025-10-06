using UnityEngine;

public class Steve : Entity
{
    public enum State { Idling, Wandering, ToEating, Eating };
    public State state = State.Idling;

    //private ParticleSystem eatingParticle;

    public override void Start()
    {
        base.Start();

        //eatingParticle = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idling:
                Idling();
                break;
            case State.Wandering:
                Wandering();
                break;
            case State.ToEating:
                ToGrazing();
                break;
            case State.Eating:
                Grazing();
                break;
        }

        if (food > 0)
        {
            food -= .01f * Time.deltaTime;
            if (food < 0) food = 0;
        }
    }


    float idlingStartTime;
    float idlingDuration = 5f;
    private void Idling()
    {
        if (Time.time - idlingStartTime > idlingDuration)
        {
            if (food < .5)
            {
                target = GetNearestByTag("Beef");
                if (target != null)
                {
                    ToToGrazingState();
                    return;
                }
            }
            ToWanderingState();
        }
    }
    private void ToIdlingState()
    {
        state = State.Idling;
        idlingStartTime = Time.time;
    }

    private void Wandering()
    {
        if (ai.reachedDestination)
        {
            ToIdlingState();
        }
    }
    private void ToWanderingState()
    {
        state = State.Wandering;
        SetDestination(new Vector2(Random.Range(-10, 10), Random.Range(-10, 10)));
    }

    float eatingStartTime;
    float eatingDuration = 2f;
    private void ToGrazing()
    {
        if (ai.reachedDestination)
        {
            ToGrazingState();
            target.GetComponent<Beef>().ToBeingEatenState();
        }
    }
    private void ToToGrazingState()
    {
        state = State.ToEating;
        SetDestination(target);
    }
    private void Grazing()
    {
        if (Time.time - eatingStartTime > eatingDuration)
        {
            Destroy(target);
            food += .3f;
            //eatingParticle.Stop();
            ToIdlingState();
        }
    }
    private void ToGrazingState()
    {
        state = State.Eating;
        eatingStartTime = Time.time;
        //grazeParticle.Play();
    }
}
