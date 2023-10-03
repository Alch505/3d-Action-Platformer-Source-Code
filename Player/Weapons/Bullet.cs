using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Weapons
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] protected int _damage;

        protected float _curLife;
        [SerializeField] protected float _lifeTime = 5f;

        protected virtual void Update()
        {
            if (_curLife > 0) _curLife -= Time.deltaTime;
            else
            {
                _curLife = 0;
                KillBullet();
            }
        }

        public virtual void InitializeBullet()
        {
            _curLife = _lifeTime;
        }

        public virtual void KillBullet()
        {
            Destroy(this.gameObject);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Enemy") 
            {
                KillBullet();
            }
        }
    }
}
