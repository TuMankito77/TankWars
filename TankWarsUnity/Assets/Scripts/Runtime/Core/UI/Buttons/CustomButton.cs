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

        [SerializeField, Min(0)]
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
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.Select, OnSelected);
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.Deselect, OnDeselect);
            defaultScale = transform.localScale;
        }

        protected virtual void OnDestroy()
        {
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerEnter, OnPointerEnter);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerExit, OnPointerExit);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.PointerClick, OnPointerClick);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.Submit, OnPointerClick);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.Select, OnSelected);
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.Deselect, OnDeselect);
        }

        #endregion

        public virtual void SetButtonInteractable(bool isInteractable)
        {
            buttonComponent.interactable = isInteractable;
        }

        public virtual void SetButtonAsSelected()
        {
            if (!buttonComponent.interactable) return;
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

        protected virtual void GetEventTriggerComponent()
        {
            if (eventTrigger != null) return;
            eventTrigger = GetComponent<EventTrigger>();
            if (eventTrigger != null) return;
            eventTrigger = gameObject.AddComponent<EventTrigger>();
        }

        protected virtual void OnPointerEnter(BaseEventData baseEventData)
        {
            SetButtonAsSelected(); 
        }

        protected virtual void OnPointerExit(BaseEventData baseEventData)
        {
            
        }

        protected virtual void OnSelected(BaseEventData baseEventData)
        {
            Vector3 increaseScale = new Vector3(defaultScale.x + scaleIncrease, defaultScale.y + scaleIncrease, defaultScale.z + scaleIncrease);
            transform.DOScale(increaseScale, scaleIncreaseDuration).SetEase(scaleEase).SetUpdate(true);
        }

        protected virtual void OnDeselect(BaseEventData baseEventData)
        {
            transform.DOScale(defaultScale, scaleIncreaseDuration).SetEase(scaleEase).SetUpdate(true);
        }

        private void OnPointerClick(BaseEventData baseEventData)
        {
            if (!buttonComponent.interactable) return;
            OnButtonClicked();
        }
    }
}
