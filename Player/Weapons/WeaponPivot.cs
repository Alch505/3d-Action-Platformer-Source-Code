using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cam;

namespace Player
{
    public class Weaponpivot : MonoBehaviour
    {
        [SerializeField] LayerMask _layerMask;

        //bool _aiming;

        PlayerMovement _playerMovement;
        CamController _camController;
        Transform _player;

        RaycastHit _rayHit;

        void Start()
        {
            _playerMovement = PlayerManager.Instance.GetComponent<PlayerMovement>();
            _camController = CamController.Instance;
            _player = PlayerManager.Instance.transform;

            PlayerManager.Instance.Health.OnHasDied += PlayerDied;
        }

        void Update()
        {
            AimLogic();

            transform.position = new Vector3(_player.position.x, _player.position.y + 1, _player.position.z);
        }

        void AimLogic()
        {
            if (_playerMovement.Aiming)
            {
                if (Physics.Raycast(_camController.transform.position, _camController.transform.forward, out _rayHit, 100000f, _layerMask))
                {
                    transform.LookAt(_rayHit.point);
                }
                else
                {
                    transform.LookAt(_camController.transform.forward * 100000);
                }
            }
            else 
            {
                transform.rotation = _playerMovement.Model.transform.rotation;
            }
        }

        void PlayerDied() 
        {
            //this.gameObject.SetActive(false);
        }
    }
}

