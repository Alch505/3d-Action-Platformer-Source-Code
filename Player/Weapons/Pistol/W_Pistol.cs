using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Weapons
{
    public class W_Pistol : Weapon
    {
        [SerializeField] GameObject _bullet;

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void Fire(InputAction.CallbackContext ctx)
        {
            if (_curAmmo <= 0 || _curCooldown > 0) return;

            base.Fire(ctx);

            Bullet bullet = Instantiate(_bullet, transform.position, transform.rotation).GetComponent<Bullet>();
            bullet.InitializeBullet();
        }

        protected override void Cooldown()
        {
            base.Cooldown();
        }

    } 
}
