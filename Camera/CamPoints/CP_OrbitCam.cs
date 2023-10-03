using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Cam
{
    public class CP_OrbitCam : CamPoint
    {
        [SerializeField] Transform _targetPosition;

        [Header("Movement")]
        [SerializeField] float _xZFollowRate = 0.05f;
        Vector3 _xZVelocity;

        [SerializeField] float _yFollowRate = 0.1f;
        Vector3 _yVelocity;

        PlayerMovement _pMovement;

        [Header("Rotation")]
        [SerializeField] float _controllerRotationSpeed = 100f;
        [SerializeField] float _mouseRotationSpeed = .2f;

        Vector3 _currentRotation;

        [SerializeField] Vector2 _vertClamp = new Vector2(-40, 85);

        [SerializeField] float _rotationSmoothTime = .12f;
        Vector3 _rotationSmoothVelocity;

        float _horiInput;
        float _vertInput;

        protected override void Start() 
        {
            base.Start();
        }

        public override void StartPoint()
        {
            base.StartPoint();

            _pMovement = PlayerManager.Instance.GetComponent<PlayerMovement>();

            _currentRotation = transform.rotation.eulerAngles;
            _horiInput = _currentRotation.y;
            _vertInput = _currentRotation.x;

            transform.position = _targetPosition.position;
        }

        public override void UpdatePoint()
        {
            base.UpdatePoint();

            SetXZPosition();
            SetYPosition();
            Orbit();
        }

        public override void EndPoint()
        {
            base.EndPoint();
        }

        void SetXZPosition() 
        {
            Vector3 targetXZ = Vector3.SmoothDamp(transform.position, _targetPosition.position, ref _xZVelocity, _xZFollowRate);

            transform.position = new Vector3(targetXZ.x, transform.position.y, targetXZ.z);
        }

        void SetYPosition() 
        {
            if (_pMovement.CheckForGround() || _targetPosition.position.y < transform.position.y) 
            {
                Vector3 targetY = Vector3.SmoothDamp(transform.position, _targetPosition.position, ref _yVelocity, _yFollowRate);

                transform.position = new Vector3(transform.position.x, targetY.y, transform.position.z);
            }
        }

        void Orbit() 
        {
            //Controller
            Vector2 rStick = PlayerManager.PControls.Gameplay.ControllerLook.ReadValue<Vector2>();

            //Mouse
            Vector2 mouse = PlayerManager.PControls.Gameplay.MouseLook.ReadValue<Vector2>();

            //Maps inputs
            _horiInput += (rStick.x * _controllerRotationSpeed * Time.deltaTime) + (mouse.x * _mouseRotationSpeed);
            _vertInput -= (rStick.y * _controllerRotationSpeed * Time.deltaTime) + (mouse.y * _mouseRotationSpeed);

            //Clamps _vertInput
            _vertInput = Mathf.Clamp(_vertInput, _vertClamp.x, _vertClamp.y);

            _currentRotation = Vector3.SmoothDamp(_currentRotation, new Vector3(_vertInput, _horiInput), ref _rotationSmoothVelocity, _rotationSmoothTime);

            transform.eulerAngles = _currentRotation;
        }
    }
}
