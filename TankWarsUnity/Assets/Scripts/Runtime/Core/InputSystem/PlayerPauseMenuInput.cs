namespace InputSystem
{
    using System;
    using UnityEngine;
    using TankWars.Runtime.Core.Events;
    using TankWars.Runtime.Core.UI;
    using TankWars.Runtime.Gameplay;
    using TankWars.Runtime.Gameplay.Visuals;

    public class PlayerPauseMenuInput : IEventListener
    {
        private InputActions inputActions = null; 

        public PlayerPauseMenuInput()
        {
            inputActions = new InputActions();
        }

        public void Enable()
        {
            inputActions.PauseMenuControl.Enable();
            inputActions.PauseMenuControl.Unpause.performed += UnpauseButtonPressed;
        }

        public void Disable()
        {
            inputActions.PauseMenuControl.Disable();
            inputActions.PauseMenuControl.Unpause.performed -= UnpauseButtonPressed;
        }

        private void UnpauseButtonPressed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            EventManager.Instance.Dispatch(GameplayEvent.OnGameUnpaused, null);
            EventManager.Instance.Dispatch(UIEvent.ShowMenu, typeof(HeadsUpDisplay));
        }

        public void OnEventReceived(IComparable eventType, object data)
        {
            switch(eventType)
            {
                default:
                    {
                        Debug.LogError($"{GetType()}:{EventManager.UNHANDLED_EVENT_TYPE_ERROR}"); 
                        break; 
                    }
            }
        }
    }
}
