using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ES_Perch : EnemyState
{
    [Tooltip("If left null, this will be set to starting postion")]
    [SerializeField] Transform _waitPosition;

    [Tooltip("If left null, this will be set to _waitPosition")]
    [SerializeField] Transform _perch;

    [SerializeField] EnemyState _perchedState;

    public override void EnterState(EnemyMovement enemyMovement)
    {
        base.EnterState(enemyMovement);

        if (_waitPosition == null) 
        {
            _waitPosition = Instantiate(new GameObject($"{this.gameObject}WaitPos"), transform.position, transform.rotation).transform;
        }

        if (_perch == null) _perch = _waitPosition;
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (Vector3.Distance(transform.position, _perch.position) >= 1f)
        {
            _myMovement.Agent.SetDestination(_perch.position);
        }
        else _myMovement.ChangeState(_perchedState);
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}
