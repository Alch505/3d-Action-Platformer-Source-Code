using UnityEngine;

namespace Player.Weapons
{
    public class B_GrenadeL1 : Bullet
    {
        [SerializeField] float _explosionRadius;

        [SerializeField] LayerMask _layerMask;

        [SerializeField] GameObject _fx;

        protected override void Update()
        {
            base.Update();
        }

        public override void InitializeBullet()
        {
            base.InitializeBullet();
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

        void OnDrawGizmosSelected()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}
