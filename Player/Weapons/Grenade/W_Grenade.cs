using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Weapons
{
    public class W_Grenade : Weapon
    {
        [Header("Grenade Lobbing Stats")]
        [Tooltip("Should only edit Y and Z values as X will make it fly off to the side")]
        [SerializeField] Vector3 _lobForce;
        [SerializeField] float _lobMod;

        [Tooltip("A random torque is added when throwing")]
        [SerializeField] float _torqueMin;
        [SerializeField] float _torqueMax;

        [SerializeField] GameObject _grenadeL1;
        [SerializeField] GameObject _grenadeL2;

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

            Rigidbody curShot;

            if (_exp < _neededExp) curShot = Instantiate(_grenadeL1, transform.position, transform.rotation).GetComponent<Rigidbody>();
            else curShot = Instantiate(_grenadeL2, transform.position, transform.rotation).GetComponent<Rigidbody>();

            curShot.GetComponent<Bullet>().InitializeBullet();

            curShot.AddForce(new Vector3(_lobForce.z * curShot.transform.forward.x, _lobForce.y * curShot.transform.up.y, _lobForce.z * curShot.transform.forward.z) * _lobMod, ForceMode.Impulse);
            curShot.AddTorque(new Vector3(Random.Range(_torqueMin, _torqueMax), Random.Range(_torqueMin, _torqueMax), Random.Range(_torqueMin, _torqueMax)));
        }

        protected override void Cooldown()
        {
            base.Cooldown();
        }
    }
}
