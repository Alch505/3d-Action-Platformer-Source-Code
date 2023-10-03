using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Health : MonoBehaviour
{
    int _curHealth;
    [SerializeField] int _maxHealth;

    public int CurHealth { get { return _curHealth; } }
    public int MaxHealth { get { return _maxHealth; } }

    float _curInvuln;
    [SerializeField] float _invulnTime;

    public delegate void OnDamage();
    public event OnDamage OnWasDamaged;

    public delegate void OnDeath();
    public event OnDeath OnHasDied;

    [SerializeField] bool _randomizePitch;
    [SerializeField] AudioClip _tookDamage;
    [SerializeField] AudioClip _died;
    AudioSource _audioSource;


    void Awake()
    {
        //OnHasDied();

        _curHealth = _maxHealth;
        _audioSource = GetComponent<AudioSource>();
    }

    void Update() 
    {
        if (_curInvuln > 0)
        {
            _curInvuln -= Time.deltaTime;
        }
        else 
        {
            _curInvuln = 0;
        }

        //Temp falling Death
        if (transform.position.y <= -50 && _curHealth > 0) 
        {
            TakeDamage(9999999);
        }
    }

    public void TakeDamage(int dmg) 
    {
        if (GameManager.Instance.State is not GS_InPlay) return;

        if (_curInvuln > 0) return;

        _curHealth -= dmg;

        if (_randomizePitch) _audioSource.pitch = Random.Range(0.8f, 1.2f); ;

        if (_curHealth <= 0)
        {
            _curHealth = 0;

            Die();
        }
        else _audioSource.PlayOneShot(_tookDamage);

        if (OnWasDamaged != null) OnWasDamaged();
    }

    void Die() 
    {
        //Currently wont play for enemies since they get destroyed before it plays out
        _audioSource.PlayOneShot(_died);

        //TESTING REMOVE LATER
        OnHasDied();
    }
}
