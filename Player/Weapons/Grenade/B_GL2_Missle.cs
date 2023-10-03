using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class B_GL2_Missle : Bullet
    {
        Rigidbody _rb;

        [SerializeField] LayerMask _layerMask;
        [SerializeField] float _targetCheckRadius;
        Transform _target;

        [SerializeField] float _explosionRadius;

        [SerializeField] float _moveSpeed = 5;
        [SerializeField] float _turnRate = 0.1f;

        [SerializeField] GameObject _mesh;
        [SerializeField] GameObject _fx;

        float _xVelocity;
        float _yVelocity;
        float _zVelocity;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.useGravity = false;
        }

        protected override void Update()
        {
            base.Update();

            if (_target == null) return;

            Vector3 direction = _target.position - transform.position;
            //Vector3 meshTarget = Vector3.SmoothDamp(_mesh.transform.eulerAngles, direction.normalized, ref _turnVelocity, _turnRate);
            //_mesh.transform.rotation = Quaternion.Euler(meshTarget);

            float x = Mathf.SmoothDampAngle(_mesh.transform.eulerAngles.x, direction.normalized.x, ref _yVelocity, _turnRate);
            float y = Mathf.SmoothDampAngle(_mesh.transform.eulerAngles.y, direction.normalized.y, ref _yVelocity, _turnRate);
            float z = Mathf.SmoothDampAngle(_mesh.transform.eulerAngles.z, direction.normalized.z, ref _yVelocity, _turnRate);
            _mesh.transform.rotation = Quaternion.Euler(x, y, z);
        }

        private void FixedUpdate()
        {
            if (_target != null)
            {
                Vector3 direction = _target.position - transform.position;
                _rb.AddForce(direction.normalized * _moveSpeed, ForceMode.Force);
            }
            else
            {
                FindTarget();
                _rb.AddForce(transform.forward + transform.up * _moveSpeed, ForceMode.Force);
            }

            //_mesh.transform.rotation = Quaternion.Euler(Vector3.SmoothDamp(transform.eulerAngles, _rb.velocity.normalized, ref _turnVelocity, _turnRate));
        }

        public override void InitializeBullet()
        {
            base.InitializeBullet();

            FindTarget();
        }

        void FindTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _targetCheckRadius, _layerMask);

            if (colliders.Length > 0) _target = colliders[0].transform;
        }

        public override void KillBullet()
        {
            Collider[] damageTargets = Physics.OverlapSphere(transform.position, _explosionRadius, _layerMask);

            foreach (Collider cur in damageTargets)
            {
                if (cur.GetComponent<Health>())
                {
                    cur.GetComponent<Health>().TakeDamage(_damage);
                }
            }

            GameObject newFx = Instantiate(_fx, transform.position, new Quaternion());

            newFx.transform.localScale = new Vector3(_explosionRadius * 2, _explosionRadius * 2, _explosionRadius * 2);

            base.KillBullet();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform == _target) KillBullet();
        }
    }
}
