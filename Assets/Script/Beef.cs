using Unity.VisualScripting;
using UnityEngine;

public class Beef : Entity
{
    public enum State { Idling, Wandering, ToEating, Eating, BeingEaten, Reproducing };
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
            case State.BeingEaten:
                BeingEaten();
                break;
            case State.Reproducing:
                Reproducing();
                break;
        }
    }


    float idlingStartTime;
    float idlingDuration = 5f;
    private void Idling()
    {
        if (Time.time - idlingStartTime > idlingDuration)
        {
            if (hunger < .5f)
            {
                GameObject b = GetNearestByTag("Bone");
                if (b != null)
                {
                    food = b;
                    ToToEatingState();
                    return;
                }
            }
            else if (hunger > .8f)
            {
                if (Random.Range(0f, 1f) > .9f)
                {
                    ToReproducingState();
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
        SetTarget(new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f)));
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

    private void BeingEaten()
    {

    }
    public void ToBeingEatenState()
    {
        state = State.BeingEaten;
        StopMoving();
        eatingParticle.Stop();
    }

    private void Reproducing()
    {
        Instantiate(GameManager.instance.beef, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), Quaternion.identity);
        Instantiate(GameManager.instance.beef, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), Quaternion.identity);
        Destroy(gameObject);
    }
    private void ToReproducingState()
    {
        state = State.Reproducing;
    }
}
