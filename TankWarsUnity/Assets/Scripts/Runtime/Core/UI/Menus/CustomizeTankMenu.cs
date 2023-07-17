namespace TankWars.Runtime.Core.UI.Menus
{
    using System;
    using TankWars.Runtime.Core.Databases;
    using TankWars.Runtime.Core.Events;
    using TankWars.Runtime.Core.ManagerSystem;
    using TankWars.Runtime.Core.StorageSystem;
    using TankWars.Runtime.Gameplay;
    using TankWars.Runtime.Gameplay.Unlockables;
    using TankWars.Runtime.Gameplay.Visuals;
    using TankWars.Runtime.Core.UI.Buttons; 
    using UnityEngine;
    using UnityEngine.UI;

    public class CustomizeTankMenu : BaseMenu, IEventListener
    {
        [SerializeField]
        private Text tankNameText = null;

        [SerializeField]
        private Button nextButton = null;

        [SerializeField]
        private Button previousButton = null;

        [SerializeField]
        private Button selectTankButton = null;

        [SerializeField]
        private CustomizeTankView customizeTankView = null;

        private GameManager GameManager => CoreManagers.Instance.GetManager<GameManager>(); 
        private PlayerInformation playerInformation => GameManager.PlayerInformation; 
        private TankInfoContainer[] tankInfoContainers => DatabaseManager.GetDatabase<TankInfoContainerDatabase>().TankInfoContainers;
        private TankInfoContainer tankInfoContainerSelected = null;
        private int tankInfoContainerSelectedIndex = 0;

        private DatabaseManager DatabaseManager => CoreManagers.Instance.GetManager<DatabaseManager>(); 
        
        #region Unity Methods 

        private void Awake()
        {
            nextButton.onClick.AddListener(OnNextButtonPressed);
            previousButton.onClick.AddListener(OnPreviousButtonPressed);
            selectTankButton.onClick.AddListener(OnSelectTankButtonPressed);
            EventManager.Instance.Register(this, typeof(ButtonId)); 
        }

        private void OnDestroy()
        {
            nextButton.onClick.RemoveListener(OnNextButtonPressed);
            previousButton.onClick.RemoveListener(OnPreviousButtonPressed);
            selectTankButton.onClick.RemoveListener(OnSelectTankButtonPressed);
            EventManager.Instance.Unregister(this, typeof(ButtonId)); 
        }

        #endregion

        public override void InitializeMenu()
        {
            base.InitializeMenu();
            tankInfoContainerSelectedIndex = playerInformation.TankInfoContaierIndex;
            tankInfoContainerSelected = tankInfoContainers[tankInfoContainerSelectedIndex];
            tankNameText.text = tankInfoContainerSelected.Name;
            customizeTankView.UpdateTankPlaceHolder(tankInfoContainerSelected);
            customizeTankView.UpdateTankPlaceHolderColor(playerInformation.TankColor); 
        }

        public override void TransitionIn()
        {
            base.TransitionIn();
            customizeTankView.Show();
        }

        public override void TransitionOut()
        {
            base.TransitionOut();
            customizeTankView.Hide();
        }

        private void OnNextButtonPressed()
        {
            tankInfoContainerSelectedIndex++;
            UpdateTankChoice(ref tankInfoContainerSelectedIndex); 
        }

        private void OnPreviousButtonPressed()
        {
            tankInfoContainerSelectedIndex--;
            UpdateTankChoice(ref tankInfoContainerSelectedIndex); 
        }

        private void UpdateTankChoice(ref int newIndex)
        {
            newIndex = newIndex % tankInfoContainers.Length;
            //We use Mathf.Abs since the MOD operator returns a negative number if one of the variables being processed is negative.
            tankInfoContainerSelected = tankInfoContainers[Mathf.Abs(newIndex)];
            tankNameText.text = tankInfoContainerSelected.Name;
            customizeTankView.UpdateTankPlaceHolder(tankInfoContainerSelected);
            selectTankButton.interactable = tankInfoContainerSelected.IsObjectUnlocked; 
        }

        private void OnSelectTankButtonPressed()
        {
            TankInfoContainer tankInfoContainerSelected = tankInfoContainers[tankInfoContainerSelectedIndex];
            tankInfoContainerSelected.Material.SetColor(CustomizeTankView.TANK_SHADER_COLOR_PROPERTY_NAME, customizeTankView.TankColor); 

            PlayerInformationParameters playerInformationParameters = new PlayerInformationParameters()
            {
                tankIndexSelected = tankInfoContainerSelectedIndex,
                tankColorSelected = customizeTankView.TankColor,
                points = playerInformation.Points
            };

            EventManager.Instance.Dispatch(GameplayEvent.NewTankSelected, playerInformationParameters);
        }

        #region IEventListener

        public void OnEventReceived(IComparable eventType, object data)
        {
            switch (eventType)
            {
                case ButtonId buttonId:
                    {
                        HandleUIEvents(buttonId, data);
                        break;
                    }
                default:
                    { 
                        break;
                    }
            }
        }

        private void HandleUIEvents(ButtonId uiEvent, object data)
        {
            switch (uiEvent)
            {
                case ButtonId.TANK_COLOR:
                    {
                        Color color = (Color)data;
                        customizeTankView.UpdateTankPlaceHolderColor(color);
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }

        #endregion
    }
}
