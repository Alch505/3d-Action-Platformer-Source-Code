using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cam;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        public PMState PMState { get; private set; }
        Rigidbody _rb;

        [Header("Ground Checking")]
        [SerializeField] Transform _groundCheck;

        [SerializeField] float _checkRadius = 1f;
        [SerializeField] float _checkDistance = 0.1f;

        RaycastHit _raycastHit;

        [SerializeField] LayerMask _groundLayers;

        [SerializeField] float _coyoteTime;
        float _curCoyoteTime;
        
        [Header("Movement")]
        [SerializeField] float _moveSpeed = 100f;
        [SerializeField] float _airMod = 0.5f;

        [SerializeField] float _airVelocityClamp = 9.5f;

        [SerializeField] float _groundedDrag = 1;

        [SerializeField] bool _aiming;

        Transform _camTransform;

        [Header("Jumping")]
        [SerializeField] float _jumpForce = 3f;
        [SerializeField] float _doubleJumpForce = 1f;
        [SerializeField] float _jumpBufferTime = 0.5f;
        float _curJumpBuffer;

        [Header("Model")]
        [SerializeField] GameObject _model;

        [SerializeField] float _turnVelocity;
        [SerializeField] float _turnSmoothing;

        #region Movement State Instances
        public PMS_Grounded PMS_Grounded { get; private set; }

        public PMS_Airborne PMS_Airborne { get; private set; }
        #endregion

        #region Public Access
        public Rigidbody RB { get { return _rb; } }

        //Ground Checking
        public Transform GroundCheck { get { return _groundCheck; } }
        public RaycastHit RaycastHit { get { return _raycastHit; } }

        //Movement
        public Transform CamTransform { get { return _camTransform; } }
        public float MoveSpeed { get { return _moveSpeed; } }
        public float AirMod { get { return _airMod; } }
        public float GroundedDrag { get { return _groundedDrag; } }
        public bool Aiming { get { return _aiming; } }

        //Jumping
        public float JumpForce { get { return _jumpForce; } }
        public float DoubleJumpForce { get { return _doubleJumpForce; } }

        //Model
        public GameObject Model { get { return _model; } }
        #endregion

        private void OnEnable()
        {
            PlayerManager.PControls.Gameplay.Jump.started += Jump;
            PlayerManager.PControls.Gameplay.Jump.canceled += JumpCancel;

            PlayerManager.PControls.Gameplay.Aim.started += StartAim;
            PlayerManager.PControls.Gameplay.Aim.canceled += EndAim;
        }
        private void OnDisable()
        {
            PlayerManager.PControls.Gameplay.Jump.started -= Jump;
            PlayerManager.PControls.Gameplay.Jump.started -= JumpCancel;

            PlayerManager.PControls.Gameplay.Aim.started -= StartAim;
            PlayerManager.PControls.Gameplay.Aim.canceled -= EndAim;
        }

        void Start()
        {
            _camTransform = CamController.Instance.transform;

            if (PlayerManager.Instance.PlayerState is PS_InPlay)
            {
                //Instance Movement States
                PMS_Grounded = new PMS_Grounded();
                PMS_Airborne = new PMS_Airborne();

                _rb = GetComponent<Rigidbody>();

                //Start in Idle State
                ChangeState(PMS_Grounded);
            }
            else //Start disabled since it'll be activated on switching to PS_InPlay
                enabled = false;
        }

        void Update() 
        {
            PMState?.UpdateState();
        }

        void FixedUpdate()
        {
            JumpBufferTime();

            PMState?.FixedUpdateState();
        }

        public void ChangeState(PMState newState)
        {
            if (PMState != null)
            {
                PMState.ExitState();
            }

            PMState = newState;

            PMState.StartState(this, _rb);
        }

        //GROUND LOGIC--------------------------------------------------------------------------
        public bool CheckForGround() 
        {
            if (Physics.SphereCast(_groundCheck.position, _checkRadius, -_groundCheck.up, out _raycastHit, _checkDistance, _groundLayers))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //PLAYER INPUT -------------------------------------------------------------------------
        public Vector3 MoveVector() 
        {
            Vector2 input = PlayerManager.PControls.Gameplay.Move.ReadValue<Vector2>();

            Vector3 adjustedInput = new Vector3(input.x, 0, input.y);

            float targetAngle = Mathf.Atan2(adjustedInput.x, adjustedInput.z) * Mathf.Rad2Deg + CamTransform.eulerAngles.y;

            Vector3 camDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward * adjustedInput.magnitude;

            ModelTurning(input, targetAngle);

            if (CheckForGround())
            {
                Vector3 slopeMoveDirection = Vector3.ProjectOnPlane(camDir, _raycastHit.normal);

                return slopeMoveDirection;
            }
            else return camDir;
        }

        public Vector3 LookVector() 
        {
            Vector2 cInput = PlayerManager.PControls.Gameplay.ControllerLook.ReadValue<Vector2>();
            Vector2 mInput = PlayerManager.PControls.Gameplay.MouseLook.ReadValue<Vector2>();
            return new Vector3(cInput.x + mInput.x, 0, cInput.y + mInput.y); 
        }

        void StartAim(InputAction.CallbackContext ctx)
        {
            _aiming = true;
        }

        void EndAim(InputAction.CallbackContext ctx)
        {
            _aiming = false;
        }

        //JUMPING ------------------------------------------------------------------------------
        public void ClearCoyoteTime() { _curCoyoteTime = 0; }
        public void StopCoyoteTime() { _curCoyoteTime = _coyoteTime; }

        public bool CheckCoyoteTime() 
        {
            if (_curCoyoteTime < _coyoteTime)
            {
                _curCoyoteTime += Time.deltaTime;
                return false;
            }
            else 
            {
                _curCoyoteTime = _coyoteTime;
                return true;
            }
        }

        void Jump(InputAction.CallbackContext ctx) 
        {
            _curJumpBuffer = _jumpBufferTime;
        }

        void JumpCancel(InputAction.CallbackContext ctx)
        {
            if (!CheckForGround() && _rb.velocity.y > 0)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, Mathf.Lerp(_rb.velocity.y, -1f, 0.4f), _rb.velocity.z);
            }
        }

        void JumpBufferTime() 
        {
            if (_curJumpBuffer > 0) _curJumpBuffer -= Time.deltaTime;
            else _curJumpBuffer = 0;
        }

        public void ClearJumpBuffer() 
        {
            _curJumpBuffer = 0;
        }

        public bool CheckJumpBuffer
        {
            get
            {
                if (_curJumpBuffer > 0)
                {
                    return true;
                }
                else return false;
            }
        }

        //Clamp the player's velocity while in the air to keep movement consistent with the ground
        public void VelocityClamping()
        {
            if (_rb.velocity.magnitude > _airVelocityClamp) 
            {
                Vector3 reducedRbVelocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
                Vector3 adjustedVelocity = Vector3.ClampMagnitude(reducedRbVelocity, _airVelocityClamp);

                _rb.velocity = new Vector3(adjustedVelocity.x, _rb.velocity.y, adjustedVelocity.z);
            }
        }

        //MODEL--------------------------------------------------------------------------------------
        void ModelTurning(Vector2 input, float targetAngle) 
        {
            if (!_aiming)
            {
                if (input.magnitude <= 0) return;

                float angle = Mathf.SmoothDampAngle(_model.transform.eulerAngles.y, targetAngle, ref _turnVelocity, _turnSmoothing);
                _model.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
            else 
            {
                float angle = Mathf.SmoothDampAngle(_model.transform.eulerAngles.y, _camTransform.rotation.eulerAngles.y, ref _turnVelocity, _turnSmoothing);
                _model.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
        }
    } 
}
