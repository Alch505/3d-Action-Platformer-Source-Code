using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cam
{
    public class CamPoint : MonoBehaviour
    {
        [Header("CamPoint Settings")]
        [Tooltip("Is this the starting point of the scene?")]
        [SerializeField] protected bool _initialPoint;

        [Tooltip("The camera's target position")]
        [SerializeField] protected Transform _anchor;
        [Tooltip("How long til the camera will shift to it's new anchor position (set to 0 for instant)")]
        [SerializeField] protected float _shiftPositionTime;

        [Tooltip("What the camera will aim at (if null the camera will use anchor's rotation)")]
        [SerializeField] protected Transform _target;
        [Tooltip("How long til the camera will shift to it's new target rotation (set to 0 for instant)")]
        [SerializeField] protected float _shiftRotationTime;

        #region Public Access
        public Transform Anchor { get { return _anchor; } }
        public float ShiftPositionTime { get { return _shiftPositionTime; } }
        public Transform Target { get { return _target; } }
        public float ShiftRotationTime { get { return _shiftRotationTime; } }
        #endregion

        protected virtual void Start()
        {
            if (_initialPoint) CamController.Instance.ChangeCamPoint(this);
        }

        public virtual void StartPoint() 
        {

        }

        public virtual void UpdatePoint() 
        {

        }

        public virtual void EndPoint() 
        {

        }
    }
}
