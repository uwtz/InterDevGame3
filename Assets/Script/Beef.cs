using Unity.VisualScripting;
using UnityEngine;

public class Beef : Entity
{
    public enum State { Idling, Wandering, ToGrazing, Grazing, BeingEaten };
    public State state = State.Idling;

    private ParticleSystem grazeParticle;

    public override void Start()
    {
        base.Start();

        grazeParticle = GetComponentInChildren<ParticleSystem>();
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
            case State.ToGrazing:
                ToGrazing();
                break;
            case State.Grazing:
                Grazing();
                break;
            case State.BeingEaten:
                BeingEaten();
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
        if (Time.time -  idlingStartTime > idlingDuration)
        {
            if (food < .5)
            {
                target = GetNearestByTag("Bone");
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

    float grazingStartTime;
    float grazingDuration = 2f;
    private void ToGrazing()
    {
        if (ai.reachedDestination)
        {
            ToGrazingState();
        }
    }
    private void ToToGrazingState()
    {
        state = State.ToGrazing;
        SetDestination(target);
    }
    private void Grazing()
    {
        if (Time.time - grazingStartTime > grazingDuration)
        {
            Destroy(target);
            food += .3f;
            grazeParticle.Stop();
            ToIdlingState();
        }
    }
    private void ToGrazingState()
    {
        state = State.Grazing;
        grazingStartTime = Time.time;
        grazeParticle.Play();
    }

    private void BeingEaten()
    {
        SetDestination(transform.position);
    }
    public void ToBeingEatenState()
    {
        state = State.BeingEaten;
    }
}
