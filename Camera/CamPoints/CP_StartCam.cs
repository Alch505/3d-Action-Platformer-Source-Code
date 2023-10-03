using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Player;

namespace Cam
{
    public class CP_StartCam : CamPoint
    {
        [SerializeField] CamPoint _nextPoint;

        protected override void Start()
        {
            base.Start();
        }

        public override void StartPoint()
        {
            base.StartPoint();

            InputSystem.onAnyButtonPress.CallOnce(ChangeToNextPoint);
        }

        public override void UpdatePoint()
        {
            base.UpdatePoint();

            if (CamController.Instance.CurPoint != this) return;

            //Messy but checks if the player is moving or looking around and changes to the next point if so
            if (PlayerManager.PControls.Gameplay.ControllerLook.ReadValue<Vector2>().magnitude > 0
                || PlayerManager.PControls.Gameplay.MouseLook.ReadValue<Vector2>().magnitude > 0
                || PlayerManager.PControls.Gameplay.Move.ReadValue<Vector2>().magnitude > 0)
                CamController.Instance.ChangeCamPoint(_nextPoint);
        }

        public override void EndPoint()
        {
            base.EndPoint();

            if (_nextPoint is CP_OrbitCam) 
            {
                Quaternion newAngle = Quaternion.LookRotation(_nextPoint.transform.position - transform.position);

                newAngle.eulerAngles = new Vector3(0, newAngle.eulerAngles.y, 0);

                _nextPoint.transform.rotation = newAngle;
            }
        }

        void ChangeToNextPoint(InputControl button) 
        {
            if (CamController.Instance.CurPoint != this) return;
            CamController.Instance.ChangeCamPoint(_nextPoint);
        }
    } 
}
