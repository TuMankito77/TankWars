namespace TankWars.Runtime.Core.Tools.Time
{
    using System; 
    using TankWars.Runtime.Core.ManagerSystem;
    using UnityEngine;

    public class TimeManager : BaseManager
    {
        public event Action<float> onTimerTick = null;

        #region Unity Methods

        private void Update()
        {
            onTimerTick?.Invoke(Time.deltaTime); 
        }

        #endregion
    }
}
