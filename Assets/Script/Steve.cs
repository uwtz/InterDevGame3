using UnityEngine;
using UnityEngine.AI;

public class Steve : Entity
{
    public override string[] foods => new string[] { "Beef" };
}

/*
public class SteveOld
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
                GameObject b = GetNearestConsumableByTag("Beef");
                if (b != null)
                {
                    food = b;
                    food.GetComponent<Consumable>().ClaimConsumable(gameObject);
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
        if (ReachedDestination())
        {
            StopMoving();
            ToIdlingState();
        }
    }
    private void ToWanderingState()
    {
        state = State.Wandering;
        SetTarget(GameManager.GetRandomPointOnNavMesh());
        //SetTarget(new Vector2(Random.Range(-10f, 10f), Random.Range(-10, 10)));
        StartMoving();
    }

    float eatingStartTime;
    float eatingDuration = 2f;
    private void ToEating()
    {
        if (food == null)
        {
            StopMoving();
            ToIdlingState();
            return;
        }

        SetTarget(food);
        if (ReachedDestination())
        {
            StopMoving();
            food.GetComponent<Beef>().ToBeingEatenState();
            ToEatingState();
        }
    }
    private void ToToEatingState()
    {
        state = State.ToEating;
        SetTarget(food);
        StartMoving();
    }
    private void Eating()
    {
        if (food == null)
        {
            StopMoving();
            eatingParticle.Stop();
            ToIdlingState();
            return;
        }

        if (Time.time - eatingStartTime > eatingDuration)
        {
            //Debug.Log(gameObject.name + " Ate " + food.name);
            Destroy(food);
            hunger += .3f;
            if (hunger > 1) hunger = 1;
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
*/