using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyState : MonoBehaviour
{
    protected EnemyMovement _myMovement;

    public virtual void EnterState(EnemyMovement enemyMovement) 
    {
        if (_myMovement == null) _myMovement = enemyMovement;
    }

    public virtual void UpdateState() 
    {

    }

    public virtual void FixedUpdateState() 
    {

    }

    public virtual void ExitState() 
    {

    }
}
