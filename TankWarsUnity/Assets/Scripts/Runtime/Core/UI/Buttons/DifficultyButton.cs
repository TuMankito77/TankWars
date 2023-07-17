namespace TankWars.Runtime.Core.UI.Buttons
{
    using System;
    using UnityEngine; 
    using TankWars.Runtime.Gameplay.Levels;
    using TankWars.Runtime.Core.Databases;
    using TankWars.Runtime.Core.Events;

    public class DifficultyButton : CustomButton
    {
        public new event Action<DifficultyButton> onButtonPressed = null;

        [SerializeField]
        private GameDifficulty difficulty = GameDifficulty.Easy;

        public GameDifficulty Difficulty => difficulty; 

        protected override void OnButtonClicked()
        {
            base.OnButtonClicked();
            onButtonPressed?.Invoke(this);
            EventManager.Instance.Dispatch(buttonId, this);
        }
    }
}
