using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PMState
    {
        protected PlayerMovement _playerMovement;
        protected Rigidbody _rb;

        public virtual void StartState(PlayerMovement pm, Rigidbody rb)
        {
            _playerMovement = pm;
            _rb = rb;
        }

        public virtual void UpdateState() 
        {

        }

        public virtual void FixedUpdateState()
        {
            
        }

        public virtual void ExitState()
        {

        }

        protected void StillGroundedCheck()
        {
            //check if player is no longer grounded. If so begin Coyote time check
            if (!_playerMovement.CheckForGround())
            {
                if (_playerMovement.CheckCoyoteTime())
                {
                    _playerMovement.ChangeState(_playerMovement.PMS_Airborne);
                }
            }
        }

        protected void StillAirborne()
        {
            if (_playerMovement.CheckForGround())
            {
                _playerMovement.ChangeState(_playerMovement.PMS_Grounded);
            }
        }
    }

    public class PMS_Grounded : PMState
    {
        bool _hasJumped;

        Vector3 _lastPlatVelocity;

        public override void StartState(PlayerMovement pm, Rigidbody rb)
        {
            base.StartState(pm, rb);

            _lastPlatVelocity = new Vector3();
            _playerMovement.ClearCoyoteTime();
            _hasJumped = false;
            _rb.drag = _playerMovement.GroundedDrag;
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();

            if (!_hasJumped) 
            {
                StillGroundedCheck();
                Jump();
            }

            Move();
        }

        public override void ExitState()
        {
            base.ExitState();

            _lastPlatVelocity = new Vector3(0, 0, 0);
        }

        void Move()
        {
            if (!_playerMovement.CheckForGround()) return;

            Vector3 desiredMove = _playerMovement.MoveVector() * _playerMovement.MoveSpeed;

            //Add force remade (for clarity)
            _rb.velocity += (desiredMove / _rb.mass) * Time.fixedDeltaTime;

            //Manually apply drag (for clarity)
            _rb.velocity *= (1 - Time.deltaTime * 5) * _playerMovement.MoveVector().magnitude;

            //project velocity on grounds normal
            _rb.velocity = Vector3.ProjectOnPlane(_rb.velocity, _playerMovement.RaycastHit.normal);
        }

        void Jump() 
        {
            if (!_playerMovement.CheckJumpBuffer) return;

            _hasJumped = true;

            _playerMovement.ClearJumpBuffer();
            _playerMovement.StopCoyoteTime();
            _rb.AddForce(Vector3.up * _playerMovement.JumpForce, ForceMode.Impulse);
            _playerMovement.ChangeState(_playerMovement.PMS_Airborne);
        }
    }

    public class PMS_Airborne : PMState
    {
        bool _hasDoubleJumped;

        public override void StartState(PlayerMovement pm, Rigidbody rb)
        {
            base.StartState(pm, rb);

            _rb.drag = 0;
            _hasDoubleJumped = false;
        }

        public override void UpdateState() 
        {
            _playerMovement.VelocityClamping();
        }

        public override void FixedUpdateState()
        {
            base.FixedUpdateState();

            Move();

            if (_rb.velocity.y <= 0) StillAirborne();
            DoubleJump();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        void Move()
        {
            _rb.AddForce(_playerMovement.MoveVector() * _playerMovement.MoveSpeed * _playerMovement.AirMod, ForceMode.Force);
        }

        void DoubleJump()
        {
            if (!_playerMovement.CheckJumpBuffer) return;

            if (_hasDoubleJumped) return;

            _hasDoubleJumped = true;

            _rb.velocity = new Vector3(_rb.velocity.x, 0 , _rb.velocity.z);

            _playerMovement.ClearJumpBuffer();
            _rb.AddForce(Vector3.up * _playerMovement.DoubleJumpForce, ForceMode.Impulse);
        }
    }
}
