namespace TankWars.Runtime.Core.UI.Helpers
{
    using System.Collections.Generic;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;

    public class EventTriggerController
    {
        private EventTrigger eventTrigger = null;
        private Dictionary<EventTriggerType, EventTrigger.Entry> triggerEntries = null;

        public EventTriggerController(EventTrigger eventTrigger)
        {
            this.eventTrigger = eventTrigger;
            triggerEntries = new Dictionary<EventTriggerType, EventTrigger.Entry>();
        }

        public void SubscribeToTiggerEvent(EventTriggerType eventTriggerType, UnityAction<BaseEventData> onEventTriggered)
        {
            if(triggerEntries.TryGetValue(eventTriggerType, out EventTrigger.Entry entry))
            {
                entry.callback.AddListener(onEventTriggered);
                return;
            }

            triggerEntries.Add(eventTriggerType, new EventTrigger.Entry());
            triggerEntries[eventTriggerType].eventID = eventTriggerType;
            triggerEntries[eventTriggerType].callback.AddListener(onEventTriggered);
        }

        public void UnsubscribeToTriggerEvent(EventTriggerType eventTriggerType, UnityAction<BaseEventData> onEventTriggered)
        {
            if (triggerEntries.TryGetValue(eventTriggerType, out EventTrigger.Entry entry))
            {
                entry.callback.RemoveListener(onEventTriggered);
            }
        }
    }
}
