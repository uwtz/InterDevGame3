using UnityEngine;


// a helper class so ReproducingState can cast the owner entity to this class and call Reproduce()
public abstract class ReproducingEntityBase : Entity
{
    public GoingToPartnerState goingToPartnerState { get; private set; }
    public ReproducingState reproducingState { get; private set; }

    public override void StateStart()
    {
        base.StateStart();
        goingToPartnerState = new GoingToPartnerState(this);
        reproducingState = new ReproducingState(this);
    }

    [HideInInspector] public bool isTaken = false;
    public bool isFemale = false;
    public virtual void Reproduce() { }
}

public abstract class ReproducingEntity<R> : ReproducingEntityBase where R : ReproducingEntity<R> // crtp
{


    private R partner = null;

    // find nearest partner of the same class that isnt taken
    public bool FindNearestPartner(out R partner)
    {
        partner = null;

        R[] partners = GameObject.FindObjectsByType<R>(FindObjectsSortMode.None);

        float minDist = Mathf.Infinity;

        foreach (R p in partners)
        {
            R potentialPartner = p;// p.GetComponent<P>();
            if (potentialPartner == null || potentialPartner == this) continue;

            float dist = Vector3.Distance(p.transform.position, transform.position);
            if (!potentialPartner.isTaken && potentialPartner.canReproduce && dist < minDist)
            {
                partner = p;
                minDist = dist;
            }
        }

        if (partner != null) { return true; }
        return false;
    }



    public override void PostIdleStateSelector()
    {
        if (canReproduce && !isTaken)
        {
            if (FindNearestPartner(out R partner))
            {
                this.partner = partner;
                partner.SetTarget(gameObject);
                SetTarget(this.partner.gameObject);

                isFemale = true;
                isTaken = true;
                partner.isTaken = true;

                stateMachine.ChangeState(goingToPartnerState);
                partner.stateMachine.ChangeState(partner.goingToPartnerState);
                return;
            }
        }
        base.PostIdleStateSelector();
    }

    public override void Reproduce()
    {
        if (isFemale)
        {
            GameObject child = Object.Instantiate(GetChildPrefab(), transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0), Quaternion.identity);
            child.GetComponent<Entity>().timeLastReproduced = Time.time;
        }

        AddHunger(-reproductionHungerCost);
        timeLastReproduced = Time.time;
        stateMachine.ChangeState(idleState);
    }

    public virtual GameObject GetChildPrefab() { return null; }
}
