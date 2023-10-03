using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cam
{
    public class CamController : MonoBehaviour
    {
        public static CamController Instance;

        public CamPoint CurPoint { get; private set; }

        Vector3 _shiftStartPos;
        Quaternion _shiftStartRot;

        float _elapsedShiftPosTime;
        float _elapsedShiftRotTime;

        public void ChangeCamPoint(CamPoint newPoint) 
        {
            if (CurPoint != null) 
            {
                CurPoint.EndPoint();

                _shiftStartPos = CurPoint.transform.position;
                _shiftStartRot = CurPoint.transform.rotation;
            }

            CurPoint = newPoint;

            _elapsedShiftPosTime = Mathf.Epsilon;
            _elapsedShiftRotTime = Mathf.Epsilon;

            CurPoint.StartPoint();
        }

        private void Awake()
        {
            //Singleton
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        private void LateUpdate()
        {
            ShiftPosition();
            ShiftRotation();

            CurPoint?.UpdatePoint();
        }

        void ShiftPosition() 
        {
            Vector3 targetPosition;

            if (CurPoint.ShiftPositionTime > 0 && _elapsedShiftPosTime < CurPoint.ShiftPositionTime)
            {
                //SmoothStep equation
                float t = _elapsedShiftPosTime / CurPoint.ShiftPositionTime;
                t = t * t * (3f - 2f * t);

                if (CurPoint.Anchor != null) targetPosition = Vector3.Lerp(_shiftStartPos, CurPoint.Anchor.position, t);
                else targetPosition = Vector3.Lerp(_shiftStartPos, CurPoint.transform.position, t);

                _elapsedShiftPosTime += Time.deltaTime;
            }
            else 
            {
                if (CurPoint.Anchor != null) targetPosition = CurPoint.Anchor.position;
                else targetPosition = CurPoint.transform.position;
            }

            transform.position = targetPosition;
        }

        void ShiftRotation()
        {
            Quaternion targetRotation;

            if (CurPoint.ShiftRotationTime > 0 && _elapsedShiftRotTime < CurPoint.ShiftRotationTime)
            {
                //SmoothStep equation
                float t = _elapsedShiftRotTime / CurPoint.ShiftRotationTime;
                t = t * t * (3f - 2f * t);

                if (CurPoint.Target != null) targetRotation = Quaternion.Slerp(_shiftStartRot, Quaternion.LookRotation(CurPoint.Target.position - transform.position), t);
                else targetRotation = Quaternion.Slerp(_shiftStartRot, CurPoint.transform.rotation, t);

                _elapsedShiftRotTime += Time.deltaTime;
            }
            else
            {
                if (CurPoint.Target != null) targetRotation = Quaternion.LookRotation(CurPoint.Target.position - transform.position);
                else targetRotation = CurPoint.transform.rotation;
            }

            transform.rotation = targetRotation;
        }
    } 
}
