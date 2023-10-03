using UnityEngine;
using Player;
using Player.Weapons;

public class ES_Shoot : EnemyState
{
    [Header("Firing")]
    float _curFireRate;
    [SerializeField] float _fireRate = 0.5f;

    int _roundsleft;
    [SerializeField] int _roundsToFire = 1;

    [SerializeField] Transform _bulletSpawner;
    [SerializeField] GameObject _bullet;

    [Header("Cooldown")]
    [SerializeField] bool _stayInStateAfterShooting;

    float _curCooldown;
    [SerializeField] float _cooldown = 2.5f;

    Quaternion _playerDirection;

    public override void EnterState(EnemyMovement enemyMovement)
    {
        base.EnterState(enemyMovement);

        _curCooldown = _cooldown;
    }

    public override void FixedUpdateState()
    {
        base.FixedUpdateState();
    }

    public override void UpdateState()
    {
        base.UpdateState();

        Aim();

        Cooldown();
        Firing();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    void Aim() 
    {
        _playerDirection = Quaternion.LookRotation(PlayerManager.PlayerTransform.position - transform.position, Vector3.up);

        transform.rotation = _playerDirection;

        //_bulletSpawner.eulerAngles = new Vector3(_playerDirection.x, _playerDirection.y, 0);
    }

    //Reload rounds after cooldown
    void Cooldown() 
    {
        if (_curCooldown > 0) 
        {
            _curCooldown -= Time.deltaTime;
        }
        else if (_curCooldown <= 0 && _roundsleft <= 0) 
        {
            _roundsleft = _roundsToFire;
        }
    }

    //Call Shoot while considering fire rate
    void Firing() 
    {
        if (_roundsleft <= 0) return;

        if (_curFireRate > 0)
        {
            _curFireRate -= Time.deltaTime;
        }
        else 
        {
            _roundsleft -= 1;

            if (_roundsleft == 0) _curFireRate = 0;
            else _curFireRate = _fireRate;

            Shoot();
        }
    }

    void Shoot() 
    {
        Bullet bullet = Instantiate(_bullet, _bulletSpawner.position, _bulletSpawner.rotation).GetComponent<Bullet>();
        bullet.InitializeBullet();

        if (_roundsleft <= 0) 
        {
            _curCooldown = _cooldown;
        }
    }
}
