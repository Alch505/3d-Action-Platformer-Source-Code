using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Player;

public class ES_Chase : EnemyState
{
    [SerializeField] int _collisionDmg;

    [SerializeField] float _chaseRadius;

    [SerializeField] EnemyState _disengageState;

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

        _myMovement.Agent.SetDestination(PlayerManager.PlayerTransform.position);

        CheckForPlayer();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    void CheckForPlayer() 
    {
        if (!Physics.CheckSphere(transform.position, _chaseRadius, LayerMask.GetMask("Player")))
        {
            _myMovement.ChangeState(_disengageState);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player") 
        {
            collision.transform.GetComponent<Health>().TakeDamage(_collisionDmg);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _chaseRadius);
    }
}
