namespace InputSystem
{
    using System;
    using UnityEngine;
    using UnityEngine.UI; 
    using TankWars.Runtime.Core.Events;
    
    public class InputManager : IEventListener
    {
        private InputActions inputActions = null; 

        public InputManager()
        {
            inputActions = new InputActions();
            inputActions.PauseMenuControl.Enable(); 
        }

        private void OnUpArrowPressed()
        {
            
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
