using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Weapons
{
    public class Weapon : MonoBehaviour 
    {
        [Header("Base Weapon Stats")]
        [SerializeField] protected int _exp;
        [SerializeField] protected int _neededExp;

        [SerializeField] protected int _curAmmo = 8;
        [SerializeField] protected int _maxAmmo = 8;

        [SerializeField] protected bool _holdFire;

        [SerializeField] protected float _fireRate = 1f;
        protected float _curCooldown;

        [SerializeField] protected GameObject _mesh; 

        protected virtual void Start() 
        {
            _curAmmo = _maxAmmo;
        }

        protected virtual void Update() 
        {
            Cooldown();
        }

        public virtual void RenderMesh(bool render) 
        {
            if (_mesh == null) return;

            _mesh.SetActive(render);
        }

        public virtual void Fire(InputAction.CallbackContext ctx) 
        {
            if (_curAmmo <= 0 || _curCooldown > 0) return;

            _curCooldown = _fireRate;

            _curAmmo -= 1;
        }

        protected virtual void Cooldown() 
        {
            if (_curCooldown > 0)
            {
                _curCooldown -= Time.deltaTime;
            }
            else _curCooldown = 0;
        }
    }
}
