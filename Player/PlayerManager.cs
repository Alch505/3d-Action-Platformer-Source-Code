using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }

        public static Transform PlayerTransform { get; private set; }

        public static PlayerControls PControls { get; private set; }
        public PlayerState PlayerState { get; private set; }

        Health _health;
        public Health Health { get { return _health; } }

        private void OnEnable()
        {
            PControls.Enable();
        }
        private void OnDisable()
        {
            PControls.Disable();
        }

        void Awake()
        {
            //Singleton
            if (Instance == null) Instance = this;
            else Destroy(this);

            _health = GetComponent<Health>();
            _health.OnWasDamaged += Damaged;
            _health.OnHasDied += Die;

            PlayerTransform = transform;

            PControls = new PlayerControls();
        }

        // Start is called before the first frame update
        void Start()
        {
            ChangeState(new PS_InPlay());
        }

        void Update() 
        {
            PlayerState.UpdateState();
        }

        public void ChangeState(PlayerState newState) 
        {
            if (PlayerState != null) 
            {
                PlayerState.ExitState();
            }

            PlayerState = newState;

            PlayerState.StartState();
        }

        void Damaged() 
        {
            GameManager.Instance.PlayerWasDamaged();
        }

        void Die() 
        {
            if (PlayerState is not PS_Died) 
            {
                ChangeState(new PS_Died());
            }
        }
    } 
}
