namespace TankWars.Runtime.Core.Events
{
    using System; 

    public interface IEventListener 
    {
        public void OnEventReceived(IComparable eventType, object data); 
    }
}
