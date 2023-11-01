namespace TankWars.Runtime.Core.UI.Buttons
{
    using TankWars.Runtime.Core.UI.Helpers;
    using TankWars.Runtime.Core.Events;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI; 

    public class SliderButton : CustomButton
    {
        private Slider sliderComponent = null;

        public ButtonId ButtonId => buttonId;
        public float sliderValue => sliderComponent.value;
        public override bool IsInteractable => sliderComponent.interactable;

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            eventTriggerController.SubscribeToTiggerEvent(EventTriggerType.EndDrag, OnSliderValueChange);
            sliderComponent = GetComponent<Slider>();
        }
 
        protected override void OnDestroy()
        {
            base.Awake();
            eventTriggerController.UnsubscribeToTriggerEvent(EventTriggerType.EndDrag, OnSliderValueChange);
        }

        #endregion

        public void SetSliderValue(float value)
        {
            sliderComponent.value = Mathf.Clamp(value, sliderComponent.minValue, sliderComponent.maxValue); 
        }

        public override void SetButtonInteractable(bool isInteractable)
        {
            sliderComponent.interactable = isInteractable; 
        }

        public override void SetButtonAsSelected()
        {
            sliderComponent.Select(); 
        }

        //We are overriding the GetButtonComponent method as the one that the CustomButton class has tries to 
        //unsubscribe from the button component which is never assigned and ,hense ,throughs a 
        //null reference exeption.
        protected override void GetButtonComponent()
        {

        }

        private void OnSliderValueChange(BaseEventData baseEventData)
        {
            EventManager.Instance.Dispatch(buttonId, this); 
        }
    }
}
