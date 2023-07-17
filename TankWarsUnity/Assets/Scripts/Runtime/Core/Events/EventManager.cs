namespace TankWars.Runtime.Core.Events
{
    using System;
    using System.Collections.Generic;

    public class EventManager : Singleton<EventManager>
    {
        private enum InstructionType
        {
            Register,
            Unregister,
            Dispatch
        }

        private class Listener
        {
            public Type[] eventTypes;
            public IEventListener eventListener; 
        }

        private class Event
        {
            public IComparable eventType;
            public object data; 
        }

        private class Instruction
        {
            public InstructionType instructionType;
            public object instructionData; 
        }

        public const string UNHANDLED_EVENT_TYPE_ERROR = "An unhandled type of event has been received."; 

        private Dictionary<Type, List<IEventListener>> listenersByType = new Dictionary<Type, List<IEventListener>>();
        private Queue<Instruction> instructionsQueued = new Queue<Instruction>(); 
        private bool isExecutingInstruction = false; 

        public void Register(IEventListener listener, params Type[] eventTypes)
        {
            if(isExecutingInstruction)
            {
                Listener newListener = new Listener 
                { 
                    eventTypes = eventTypes, 
                    eventListener = listener 
                };

                Instruction instruction = new Instruction
                {
                    instructionType = InstructionType.Register,
                    instructionData = newListener
                };

                instructionsQueued.Enqueue(instruction);
                return;
            }

            isExecutingInstruction = true; 

            foreach(Type eventType in eventTypes)
            {
                if(!listenersByType.ContainsKey(eventType))
                {
                    listenersByType.Add(eventType, new List<IEventListener>());
                    listenersByType[eventType].Add(listener);
                    continue; 
                }

                listenersByType[eventType].Add(listener); 
            }

            isExecutingInstruction = false;

            CheckForQueuedInstructions(); 
        }

        public void Unregister(IEventListener listener, params Type[] eventTypes)
        {
            if (isExecutingInstruction)
            {
                Listener oldListener = new Listener 
                { 
                    eventTypes = eventTypes, 
                    eventListener = listener 
                };

                Instruction instruction = new Instruction
                {
                    instructionType = InstructionType.Unregister,
                    instructionData = oldListener
                };

                instructionsQueued.Enqueue(instruction);
                return; 
            }

            isExecutingInstruction = true; 

            foreach (Type eventType in eventTypes)
            {
                if(listenersByType.TryGetValue(eventType, out List<IEventListener> listeners))
                {
                    listeners.Remove(listener); 
                }
            }

            isExecutingInstruction = false;

            CheckForQueuedInstructions(); 
        }

        public void Dispatch(IComparable eventType, object data)
        {
            if(!listenersByType.ContainsKey(eventType.GetType()))
            {
                return;
            }

            if(isExecutingInstruction)
            {
                Event newEvent = new Event
                {
                    eventType = eventType, 
                    data = data
                };

                Instruction instruction = new Instruction
                {
                    instructionType = InstructionType.Dispatch,
                    instructionData = newEvent
                };

                instructionsQueued.Enqueue(instruction); 
                return; 
            }

            isExecutingInstruction = true; 

            List<IEventListener> invalidListeners = new List<IEventListener>(); 

            foreach(IEventListener listener in listenersByType[eventType.GetType()])
            {
                if(listener == null)
                {
                    invalidListeners.Add(listener);
                    continue; 
                }

                listener.OnEventReceived(eventType, data); 
            }

            foreach(IEventListener invalidListener in invalidListeners)
            {
                Unregister(invalidListener, eventType.GetType());
            }

            isExecutingInstruction = false;

            CheckForQueuedInstructions(); 
        }

        private void CheckForQueuedInstructions()
        {
            if(instructionsQueued.Count > 0)
            {
                Instruction instruction = instructionsQueued.Dequeue(); 

                switch(instruction.instructionType)
                {
                    case InstructionType.Register:
                        {
                            Listener newListener = (Listener)instruction.instructionData;
                            Register(newListener.eventListener, newListener.eventTypes); 
                            break;
                        }
                    case InstructionType.Unregister:
                        {
                            Listener oldListner = (Listener)instruction.instructionData;
                            Unregister(oldListner.eventListener, oldListner.eventTypes);
                            break;
                        }
                    case InstructionType.Dispatch:
                        {
                            Event newEvent = (Event)instruction.instructionData; 
                            Dispatch(newEvent.eventType, newEvent.data); 
                            break; 
                        }
                }
            }
        }
    }
}
