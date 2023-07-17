namespace TankWars.Runtime.Core.UI.Buttons
{
    using TankWars.Runtime.Core.Databases;
    using TankWars.Runtime.Core.Events;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI; 

    public class SliderButton : CustomButton
    {
        private Slider sliderComponent = null;
        private EventTrigger eventTrigger = null;

        public ButtonId ButtonId => buttonId;
        public float sliderValue => sliderComponent.value; 

        #region Unity Methods

        protected override void Awake()
        {
            sliderComponent = GetComponent<Slider>();
            eventTrigger = GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.EndDrag;
            entry.callback.AddListener(OnSliderValueChange);
            eventTrigger.triggers.Add(entry); 
        }

        //We are overriding the OnDestroy method as the one that the CustomButton class has tries to 
        //unsubscribe from the button component which is never assigned a hense throughs a 
        //null reference exeption. 
        protected override void OnDestroy()
        {
            
        }

        #endregion

        public void SetSliderValue(float value)
        {
            sliderComponent.value = Mathf.Clamp(value, sliderComponent.minValue, sliderComponent.maxValue); 
        }
        
        private void OnSliderValueChange(BaseEventData baseEventData)
        {
            EventManager.Instance.Dispatch(buttonId, this); 
        }
    }
}
