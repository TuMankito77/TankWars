namespace TankWars.Runtime.Core.UI.Buttons
{
    using System;
    using TankWars.Runtime.Core.UI.Helpers;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using DG.Tweening;

    public class CustomButton : MonoBehaviour
    {
        public event Action onButtonPressed = null;

        [SerializeField]
        protected ButtonId buttonId = ButtonId.NONE;

        [SerializeField]
        private Button buttonComponent = null;
        
        [SerializeField]
        protected EventTrigger eventTrigger = null;

        [Header("Button Effects Settings")]

        [SerializeField]
        private float scaleIncrease = 0.2f;

        [SerializeField]
        private float scaleIncreaseDuration = 0.1f;

        [SerializeField]
        private Ease scaleEase = Ease.InOutSine;

        private Vector3 defaultScale = default(Vector3);
        protected EventTriggerController eventTriggerController = null;

        public virtual bool IsInteractable => buttonComponent.interactable;

        #region Unity Methods

        protected virtual void Awake()
        {
            GetButtonComponent();
            GetEventTriggerComponent();
            eventTriggerController = new EventTriggerController(eventTrigger);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerEnter, OnPointerEnter);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerExit, OnPointerExit);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.PointerClick, OnPointerClick);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.Submit, OnPointerClick);
            defaultScale = transform.localScale;
        }

        protected virtual void OnDestroy()
        {
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerEnter, OnPointerEnter);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerExit, OnPointerExit);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerClick, OnPointerClick);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.Submit, OnPointerClick);
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
            Vector3 increaseScale = new Vector3(defaultScale.x + scaleIncrease, defaultScale.y + scaleIncrease, defaultScale.z + scaleIncrease);
            transform.DOScale(increaseScale, scaleIncreaseDuration).SetEase(scaleEase); 
        }

        private void OnPointerExit(BaseEventData baseEventData)
        {
            transform.DOScale(defaultScale, scaleIncreaseDuration).SetEase(scaleEase);
        }

        private void OnPointerClick(BaseEventData baseEventData)
        {
            OnButtonClicked();
        }
    }
}
