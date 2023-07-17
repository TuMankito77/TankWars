namespace TankWars.Runtime.Core.UI.Menus
{
    using TankWars.Runtime.Core.Events;
    using TankWars.Runtime.Core.ManagerSystem;
    using TankWars.Runtime.Core.UI.Buttons;
    using UnityEngine;

    public class SettingsMenu : BaseMenu
    {
        [SerializeField]
        private DifficultyButton[] difficultyButtons = null;

        [SerializeField]
        private SliderButton musicVolume = null;

        [SerializeField]
        private SliderButton soundEffectsVolume = null;

        private GameManager GameManager => CoreManagers.Instance.GetManager<GameManager>(); 

        #region Unity Methods

        private void Awake()
        {
            foreach(DifficultyButton difficultyButton in difficultyButtons)
            {
                difficultyButton.onButtonPressed += OnDifficultyButtonPressed; 
            }   
        }

        private void OnDestroy()
        {
            foreach (DifficultyButton difficultyButton in difficultyButtons)
            {
                difficultyButton.onButtonPressed -= OnDifficultyButtonPressed;
            }
        }

        #endregion

        public override void InitializeMenu()
        {
            base.InitializeMenu();

            foreach(DifficultyButton difficultyButton in difficultyButtons)
            {
                difficultyButton.SetButtonInteractable(!(GameManager.GameInformation.Difficulty == difficultyButton.Difficulty)); 
            }

            musicVolume.SetSliderValue(GameManager.GameInformation.MusicVolume);
            soundEffectsVolume.SetSliderValue(GameManager.GameInformation.SoundEffectsVolume);
        }

        private void OnDifficultyButtonPressed(DifficultyButton sourceDifficultyButton)
        {
            foreach (DifficultyButton difficultyButton in difficultyButtons)
            {
                bool isButtonInteractable = !(difficultyButton.Difficulty == sourceDifficultyButton.Difficulty);
                difficultyButton.SetButtonInteractable(isButtonInteractable); 
            }
        }
    }
}
