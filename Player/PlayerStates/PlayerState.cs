using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerState
    {
        public virtual void StartState()
        {

        }

        public virtual void UpdateState()
        {

        }

        public virtual void ExitState()
        {

        }
    }

    public class PS_InPlay : PlayerState
    {

        public override void StartState()
        {
            base.StartState();

            PlayerManager.Instance.GetComponent<PlayerMovement>().enabled = true;

            PlayerManager.PControls.Enable();
        }

        public override void UpdateState()
        {
            base.UpdateState();
        }

        public override void ExitState()
        {
            base.ExitState();

            PlayerManager.Instance.GetComponent<PlayerMovement>().enabled = false;
        }
    }

    public class PS_Died : PlayerState
    {
        float _torqueMin = -5;
        float _torqueMax = 5;

        float _deathTimer = 3f;

        bool _timerDone;

        public override void StartState()
        {
            base.StartState();

            PlayerManager.PControls.Disable();


            PlayerManager.Instance.GetComponent<Rigidbody>().freezeRotation = false;

            PlayerManager.Instance.GetComponent<Rigidbody>().AddTorque(
                new Vector3(Random.Range(_torqueMin, _torqueMax), Random.Range(_torqueMin, _torqueMax), Random.Range(_torqueMin, _torqueMax)));

            _timerDone = false;
        }

        public override void UpdateState()
        {
            base.UpdateState();

            if (_deathTimer > 0) _deathTimer -= Time.deltaTime;
            else 
            {
                if (!_timerDone) 
                {
                    _timerDone = true;
                    _deathTimer = 0;

                    Reset();
                }
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        void Reset() 
        {
            Scene curScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(curScene.name);
        }
    }
}
