using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Weapons
{
    public class B_PistolL1 : Bullet
    {
        Rigidbody _rb;

        [SerializeField] float _moveSpeed;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        protected override void Update()
        {
            base.Update();
        }

        private void FixedUpdate()
        {
            _rb.AddForce(transform.forward * _moveSpeed, ForceMode.VelocityChange);
        }

        public override void InitializeBullet()
        {
            base.InitializeBullet();
        }

        public override void KillBullet()
        {
            base.KillBullet();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.GetComponent<Health>()) collision.transform.GetComponent<Health>().TakeDamage(_damage);
            KillBullet();
        }
    }
}
