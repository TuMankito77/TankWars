namespace TankWars.Runtime.Core.UI.Menus
{
    using InputSystem;
    using TankWars.Runtime.Core.Events;
    using TankWars.Runtime.Core.ManagerSystem;
    using TankWars.Runtime.Core.UI.Buttons;
    using TankWars.Runtime.Gameplay;
    using UnityEngine;

    public class PauseMenu : BaseMenu
    {
        [SerializeField]
        private ShowMenuButton continueButton = null;

        [SerializeField]
        private ShowMenuButton restartButton = null;

        [SerializeField]
        private ShowMenuButton quitButton = null; 

        [SerializeField]
        private SliderButton musicVolume = null;

        [SerializeField]
        private SliderButton soundEffectsVolume = null;

        private PlayerPauseMenuInput playerPauseMenuInput = null;

        private GameManager GameManager => CoreManagers.Instance.GetManager<GameManager>();

        #region Unity Methods

        private void Start()
        {
            continueButton.onButtonPressed += OnContinueButtonPressed;
            restartButton.onButtonPressed += OnRestartButtonPressed;
            quitButton.onButtonPressed += OnQuitButtonPressed;
            playerPauseMenuInput = new PlayerPauseMenuInput();
        }

        private void OnDestroy()
        {
            continueButton.onButtonPressed -= OnContinueButtonPressed;
            restartButton.onButtonPressed -= OnRestartButtonPressed;
            quitButton.onButtonPressed -= OnQuitButtonPressed; 
        }

        #endregion

        public override void InitializeMenu()
        {
            base.InitializeMenu();
            musicVolume.SetSliderValue(GameManager.GameInformation.MusicVolume);
            soundEffectsVolume.SetSliderValue(GameManager.GameInformation.SoundEffectsVolume);
            playerPauseMenuInput.Enable();
        }

        public override void TerminateMenu()
        {
            base.TerminateMenu();
            playerPauseMenuInput.Disable();
        }

        private void OnContinueButtonPressed()
        {
            EventManager.Instance.Dispatch(GameplayEvent.OnGameUnpaused, null); 
        }

        private void OnRestartButtonPressed()
        {
            EventManager.Instance.Dispatch(GameplayEvent.OnLevelRestarted, null); 
        }

        private void OnQuitButtonPressed()
        {
            EventManager.Instance.Dispatch(GameplayEvent.OnGameQuit, null);
        }
    }
}
