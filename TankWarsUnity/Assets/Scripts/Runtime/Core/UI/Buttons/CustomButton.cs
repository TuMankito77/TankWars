namespace TankWars.Runtime.Core.UI.Buttons
{
    using System;
    using System.Collections.Generic;
    using TankWars.Runtime.Core.UI.Helpers;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class CustomButton : MonoBehaviour
    {
        public event Action onButtonPressed = null;

        [SerializeField]
        protected ButtonId buttonId = ButtonId.NONE;

        [SerializeField]
        private Button buttonComponent = null;
        
        [SerializeField]
        protected EventTrigger eventTrigger = null;

        protected EventTriggerController eventTriggerController = null;

        public virtual bool IsInteractable => buttonComponent.interactable;

        #region Unity Methods

        protected virtual void Awake()
        {
            GetButtonComponent();
            buttonComponent.onClick.AddListener(OnButtonClicked);
            GetEventTriggerComponent();
            eventTriggerController = new EventTriggerController(eventTrigger);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerEnter, OnPointerEnter);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerExit, OnPointerExit);
        }

        protected virtual void OnDestroy()
        {
            buttonComponent.onClick.RemoveListener(OnButtonClicked);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerEnter, OnPointerEnter);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerExit, OnPointerExit);
        }

        #endregion

        public virtual void SetButtonInteractable(bool isInteractable)
        {
            buttonComponent.interactable = isInteractable;
        }

        public virtual void SetButtonAsSelected()
        {
            buttonComponent.Select();
        }

        protected virtual void OnButtonClicked()
        {
            onButtonPressed?.Invoke();
        }

        protected virtual void GetButtonComponent()
        {
            if (buttonComponent != null) return;
            buttonComponent = GetComponent<Button>();
            if (buttonComponent != null) return;
            buttonComponent = gameObject.AddComponent<Button>();
        }

        protected void GetEventTriggerComponent()
        {
            if (eventTrigger != null) return;
            eventTrigger = GetComponent<EventTrigger>();
            if (eventTrigger != null) return;
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }

        private void OnPointerEnter(BaseEventData baseEventData)
        {
            SetButtonAsSelected();
        }

        private void OnPointerExit(BaseEventData baseEventData)
        {

        }
    }
}
