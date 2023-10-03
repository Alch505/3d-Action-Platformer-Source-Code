using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] EnemyState _state;

    [SerializeField] EnemyState _startingState;

    Health _health;
    [SerializeField] GameObject _deathFx;

    NavMeshAgent _agent;

    public NavMeshAgent Agent { get { return _agent; } }

    void Start() 
    {
        _health = GetComponent<Health>();

        _health.OnHasDied += Die;

        if (_startingState != null) ChangeState(_startingState);
        else 
        {
            Debug.LogWarning($"{this.gameObject} has no starting state set! Disabling");
            this.gameObject.SetActive(false);
        }

        GameManager.Instance.AddEnemyToCount();

        _agent = GetComponent<NavMeshAgent>();
    }

    void Update() 
    {
        _state.UpdateState();
    }

    void FixedUpdate()
    {
        _state.FixedUpdateState();
    }

    public void ChangeState(EnemyState newState) 
    {
        _state?.ExitState();

        _state = newState;

        _state.EnterState(this);
    }

    void Die() 
    {
        //Prototype code
        if (_deathFx != null) Instantiate(_deathFx, transform.position, transform.rotation);
        Destroy(this.gameObject);

        GameManager.Instance.AddToEnemiesKilled();
    }
}
