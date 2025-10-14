using UnityEngine;
using System;

public class MovingState : State
{
    public override string name => "Moving";
    private Vector2 targetPos;
    private GameObject targetGameObject;
    private bool isTargetMoving;
    
    public Action onArrive;

    public MovingState(Entity owner) : base(owner) { }

    public override void Enter()
    {
        if (owner.agent.isOnNavMesh)
        {
            owner.agent.SetDestination(targetPos);
            StartMoving();
        }
    }

    public override void Update()
    {
        if (isTargetMoving && targetGameObject == null)
        {
            owner.stateMachine.ChangeState(owner.idleState);
        }

        if (isTargetMoving && owner.agent.isOnNavMesh)
        {
            owner.agent.SetDestination(targetGameObject.transform.position);
        }

        if (Vector2.Distance(owner.transform.position, targetPos) <= .01
            || (isTargetMoving && owner.reachedFood))
        {
            onArrive.Invoke();
        }
    }

    public override void Exit()
    {
        targetPos = new Vector2(Mathf.Infinity, Mathf.Infinity);
        targetGameObject = null;
        isTargetMoving = false;

        onArrive = null;
        StopMoving();
    }

    public void SetTarget(Vector2 targetPos)
    {
        this.targetPos = targetPos;
    }

    public void SetTarget(GameObject targetGameObject)
    {
        SetTarget(targetGameObject.transform.position);
        this.targetGameObject = targetGameObject;
        isTargetMoving = CheckIsTargetMoving(targetGameObject);
    }

    private bool CheckIsTargetMoving(GameObject go)
    {
        if (go.TryGetComponent<Entity>(out Entity e))
        {
            return true;
        }
        return false;
    }

    private void StartMoving()
    {
        if (owner.agent.isOnNavMesh) owner.agent.isStopped = false;
    }

    private void StopMoving()
    {
        if (owner.agent.isOnNavMesh) owner.agent.isStopped = true;
    }
}

