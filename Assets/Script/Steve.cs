using UnityEngine;

public class Steve : Entity
{
    public enum State { Idling, Wandering, ToEating, Eating };
    public State state = State.Idling;

    private ParticleSystem eatingParticle;

    public override void Start()
    {
        base.Start();

        eatingParticle = GetComponentInChildren<ParticleSystem>();
    }

    public override void Update()
    {
        base.Update();

        switch (state)
        {
            case State.Idling:
                Idling();
                break;
            case State.Wandering:
                Wandering();
                break;
            case State.ToEating:
                ToEating();
                break;
            case State.Eating:
                Eating();
                break;
        }
    }


    float idlingStartTime;
    float idlingDuration = 5f;
    private void Idling()
    {
        if (Time.time - idlingStartTime > idlingDuration)
        {
            if (hunger < .5)
            {
                GameObject b = GetNearestByTag("Beef");
                if (b != null)
                {
                    food = b;
                    ToToEatingState();
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
            StopMoving();
            ToIdlingState();
        }
    }
    private void ToWanderingState()
    {
        state = State.Wandering;
        SetTarget(new Vector2(Random.Range(-10f, 10f), Random.Range(-10, 10)));
        StartMoving();
    }

    float eatingStartTime;
    float eatingDuration = 2f;
    private void ToEating()
    {
        SetTarget(food);
        if (ai.reachedDestination)
        {
            StopMoving();
            food.GetComponent<Beef>().ToBeingEatenState();
            ToEatingState();
        }
    }
    private void ToToEatingState()
    {
        state = State.ToEating;
        StartMoving();
    }
    private void Eating()
    {
        if (Time.time - eatingStartTime > eatingDuration)
        {
            Debug.Log(gameObject.name + " Ate " + food.name);
            Destroy(food);
            hunger += .3f;
            eatingParticle.Stop();
            ToIdlingState();
        }
    }
    private void ToEatingState()
    {
        state = State.Eating;
        eatingStartTime = Time.time;
        eatingParticle.Play();
    }
}
