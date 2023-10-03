using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Weapons
{
    public class PlayerWeapons : MonoBehaviour
    {
        Weapon _curWeapon;
        Weapon _lastWeapon;

        [SerializeField] List<Weapon> _equippedWeapons;

        private void OnEnable()
        {
            if (PlayerManager.PControls != null) 
            {
                PlayerManager.PControls.Gameplay.WeaponSelect.started += QuickWeaponSwap;
            }
        }
        private void OnDisable()
        {
            PlayerManager.PControls.Gameplay.WeaponSelect.started -= QuickWeaponSwap;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (_equippedWeapons.Count > 0) ChangeEquippedWeapon(0);

            //PlayerManager.PControls.Gameplay.WeaponSelect.started += QuickWeaponSwap;

            //For Prototype
            _lastWeapon = _equippedWeapons[1];
        }

        // Update is called once per frame
        void Update()
        {

        }

        void QuickWeaponSwap(InputAction.CallbackContext ctx) 
        {
            //Prototype implementation
            for (int i = 0; i < _equippedWeapons.Count; i++) 
            {
                if (_equippedWeapons[i] == _lastWeapon) 
                {
                    ChangeEquippedWeapon(i);
                    break;
                }
            }
        }

        public void ChangeEquippedWeapon(int newSlot) 
        {
            if (_curWeapon != null) _lastWeapon = _curWeapon;

            _curWeapon = _equippedWeapons[newSlot];

            InitializeEquippedWeapon();
        }

        void InitializeEquippedWeapon() 
        {
            if (_lastWeapon != null && _lastWeapon != _curWeapon) 
            {
                PlayerManager.PControls.Gameplay.Shoot.started -= _lastWeapon.Fire;
                _lastWeapon.RenderMesh(false);
            }
                
            PlayerManager.PControls.Gameplay.Shoot.started += _curWeapon.Fire;
            _curWeapon.RenderMesh(true);
        }
    }
}
