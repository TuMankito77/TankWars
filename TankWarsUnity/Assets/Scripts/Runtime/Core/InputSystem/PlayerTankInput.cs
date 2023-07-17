namespace InputSystem
{
    using System;
    using UnityEngine;
    using TankWars.Runtime.Gameplay;
    using TankWars.Runtime.Core.Events;
    using UnityEngine.InputSystem;
    using TankWars.Runtime.Gameplay.Player;
    using TankWars.Runtime.Gameplay.Vehicles;

    public class PlayerTankInput : MonoBehaviour, IEventListener
    {
        public const string TANK_FIELD_NAME = nameof(tank);

        [SerializeField]
        private Tank tank = null;

        private InputActions playerInput = null;
        private Vector2 moveDirection = Vector2.zero; 

        #region Unity Methods

        private void Awake()
        {
            playerInput = new InputActions(); 
            playerInput.VehicleControl.Enable();
            playerInput.VehicleControl.Fire.performed += OnPlayerInputFire;
            EventManager.Instance.Register(this, typeof(PlayerGameplayEvents), typeof(GameplayEvent));
        }

        private void OnDestroy()
        {
            playerInput.VehicleControl.Fire.performed -= OnPlayerInputFire;
            EventManager.Instance.Unregister(this, typeof(PlayerGameplayEvents), typeof(GameplayEvent)); 
        }

        private void Update()
        {
            moveDirection = playerInput.VehicleControl.Movement.ReadValue<Vector2>();
            Vector2 rotationDirection = playerInput.VehicleControl.Aiming.ReadValue<Vector2>();
            
            if (rotationDirection.magnitude != 0)
            {
                tank.AimTurret(rotationDirection, true);
            }
        }

        private void FixedUpdate()
        {
            //We perform the tank movement in the fixed update since it is handled using the RigidBody component. 
            if(moveDirection.magnitude != 0)
            {
                tank.MoveTowards(moveDirection, true); 
            }
        }

        #endregion

        private void OnPlayerInputFire(InputAction.CallbackContext context)
        {
            tank.Fire(); 
        }

        #region IEventListener

        public void OnEventReceived(IComparable eventType, object data)
        {
            switch(eventType)
            {
                case PlayerGameplayEvents playerGameplayEvent:
                    {
                        HandlePlayerGameplayEvents(playerGameplayEvent, data);
                        break; 
                    }

                case GameplayEvent gameplayEvent:
                    {
                        HandleGameplayEvents(gameplayEvent, data); 
                        break; 
                    }

                default:
                    {
                        Debug.LogError($"{gameObject.name}-{GetType().Name}:{EventManager.UNHANDLED_EVENT_TYPE_ERROR}"); 
                        break; 
                    }
            }
        }

        private void HandlePlayerGameplayEvents(PlayerGameplayEvents playerGameplayEvent, object data)
        {
            switch(playerGameplayEvent)
            {
                case PlayerGameplayEvents.HasDied:
                    {
                        playerInput.VehicleControl.Disable(); 
                        break; 
                    }
            }
        }

        private void HandleGameplayEvents(GameplayEvent gameplayEvent, object data)
        {
            switch(gameplayEvent)
            {
                case GameplayEvent.OnGamePaused:
                    {
                        playerInput.VehicleControl.Disable(); 
                        break; 
                    }

                case GameplayEvent.OnGameUnpaused:
                    {
                        playerInput.VehicleControl.Enable(); 
                        break; 
                    }
            }
        }
        #endregion
    }
}
