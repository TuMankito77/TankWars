namespace TankWars.Runtime.Core.UI
{
    using TankWars.Runtime.Core.UI.Buttons; 
    using TankWars.Runtime.Gameplay.Levels; 
    using TankWars.Runtime.Core.Events;
    using UnityEngine;

    public class LoadLevelButton : CustomButton
    {
        [SerializeField]
        private LevelId levelId = LevelId.NONE;

        public LevelId LevelId => levelId; 

        protected override void OnButtonClicked()
        {
            base.OnButtonClicked();
            EventManager.Instance.Dispatch(buttonId, this); 
        }
    }
}
