using UnityEngine;
using UnityEngine.AI;

public class ES_Patrol : EnemyState
{
    float _curTimer;
    [SerializeField] float _idleTimer;
    [Tooltip("Idle Timer has this added to it when creating a random value to wait")]
    [SerializeField] float _idleVariance;

    [SerializeField] float _wanderRadius;

    Vector3 _wanderTarget;
    bool _needsNewTarget;

    [SerializeField] Transform _detectionPoint;
    [SerializeField] float _detectionRadius;

    [SerializeField] EnemyState _detectionState;

    public override void EnterState(EnemyMovement enemyMovement)
    {
        base.EnterState(enemyMovement);
    }
    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        DetectPlayer();
        IdleTimer();
        SetWanderTarget();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    void DetectPlayer() 
    {
        if (Physics.CheckSphere(_detectionPoint.position, _detectionRadius, LayerMask.GetMask("Player"))) 
        {
            _myMovement.ChangeState(_detectionState);
        }
    }

    void IdleTimer() 
    {
        if (Vector3.Distance(transform.position, _wanderTarget) > 1f && _curTimer > 0)
        {
            _curTimer -= Time.deltaTime;
        }
        else 
        {
            _curTimer = 0;
            if (!_needsNewTarget) _needsNewTarget = true;
        }
    }

    void SetWanderTarget() 
    {
        if (!_needsNewTarget) return;

        _needsNewTarget = false;
        _curTimer = Random.Range(_idleTimer, _idleTimer + _idleVariance);

        _wanderTarget = Random.insideUnitSphere * _wanderRadius;

        _wanderTarget += transform.position;

        _myMovement.Agent.SetDestination(_wanderTarget);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
