namespace TankWars.Runtime.Core.Tools.Time
{ 
    using System;
    using TankWars.Runtime.Core.ManagerSystem;
    using UnityEngine;

    public class Timer 
    {
        public event Action onTimerTick = null;
        public event Action onTimerCompleted = null; 

        private bool isRunning = false;

        public float Duration { get; private set; } = 0;
        public bool IsCountdown { get; private set; } = false;
        public float ElapsedTime { get; private set; } = 0; 

        private float StartValue => IsCountdown ? Duration : 0;
        private float EndValue => IsCountdown ? 0 : Duration;
        private bool IsRunning => isRunning && Duration > 0; 
        private bool IsCompleted => Mathf.Approximately(ElapsedTime, EndValue);
        private TimeManager TimeManager => CoreManagers.Instance.GetManager<TimeManager>(); 

        public Timer(float duration, bool isCountdown)
        {
            Duration = duration;
            IsCountdown = isCountdown;
            TimeManager.onTimerTick += TimerTick;
            ElapsedTime = StartValue; 
        }

        public void Start()
        {
            if(!isRunning && !IsCompleted)
            {
                isRunning = true; 
            }
        }

        public void Restart()
        {
            ElapsedTime = StartValue;
            Start(); 
        }

        public void Stop()
        {
            isRunning = false; 
        }

        private void TimerTick(float deltaTime)
        {
            if(!IsRunning)
            {
                return;
            }

            if(IsCountdown)
            {
                ElapsedTime = Mathf.Max(ElapsedTime - deltaTime, EndValue); 
            }
            else
            {
                ElapsedTime = Mathf.Min(ElapsedTime + deltaTime, EndValue);
            }

            onTimerTick?.Invoke(); 

            if(IsCompleted)
            {
                onTimerCompleted?.Invoke(); 
                isRunning = false;
            }
        }
    }
}
